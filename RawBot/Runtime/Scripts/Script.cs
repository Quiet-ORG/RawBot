using RawBot.Logging;
using RawBot.Runtime.Botting;
using RawBot.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace RawBot.Runtime.Scripts
{
    public class Script : IDisposable
    {
        private readonly string _source;
        private AssemblyLoadContext _context;
        private Type _type;
        private MethodInfo _entry;
        private CancellationTokenSource _cts;

        public Script(string source)
        {
            _source = source;
        }

        public void Compile()
        {
            var (assembly, context) = Compiler.Compile(_source);
            _context = context;
            Type type = null;
            MethodInfo entry = null;
            foreach (var method in assembly.GetTypes().SelectMany(t => t.GetMethods()))
            {
                if (method.GetCustomAttributes<EntryPointAttribute>().Any())
                {
                    type = method.DeclaringType;
                    entry = method;
                    break;
                }
            }

            if (entry is null || type is null)
            {
                throw new ScriptException("Failed to find script entry point. Ensure the method has the EntryPointAttribute.");
            }

            _type = type;
            _entry = entry;
        }

        public Task Invoke(Context context)
        {
            if (_type is null || _entry is null)
            {
                throw new InvalidOperationException("Script has not been successfully compiled.");
            }

            _cts?.Dispose();
            var injectable = CreateInjectableObjects(context);
            injectable[typeof(CancellationTokenSource)] = _cts = new CancellationTokenSource();

            var parameters = _entry.GetParameters().Select(p => injectable.TryGetValue(p.ParameterType, out var inject)
                ? inject
                : throw new ScriptException($"Failed to inject parameter of type {p.ParameterType.Name}.")).ToArray();
            var script = Activator.CreateInstance(_type);
            var task = _entry.Invoke(script, parameters);
            return task as Task ?? Task.CompletedTask;
        }

        public void Cancel()
        {
            if (_type is null || _entry is null || _cts is null)
            {
                throw new InvalidOperationException("Script has not been invoked.");
            }

            _cts.Cancel();
        }

        public void Dispose()
        {
            _cts?.Dispose();
            _context?.Unload();
        }

        private static Dictionary<Type, object> CreateInjectableObjects(Context context) => new()
        {
            { typeof(Context), context },
            { typeof(Game), context.Game },
            { typeof(World), context.World },
            { typeof(BotEngine), context.Bot },
            { typeof(ILogger), context.Game.Log }
        };
    }
}
