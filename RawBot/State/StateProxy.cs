using RawBot.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RawBot.State
{
    public class StateProxy<T> : DispatchProxy, IDisposable
    {
        private readonly Dictionary<int, EventWaitHandle> _waitHandles = new();
        private readonly Dictionary<string, PropertyInfo> _properties = new();

        public T Target { get; set; }
        public Type InstanceType { get; set; }

        public StateProxy()
        {
            foreach (var property in typeof(T).GetProperties().Where(p => p.SetMethod is not null))
            {
                _waitHandles[property.SetMethod!.MetadataToken] = new EventWaitHandle(false, EventResetMode.AutoReset);
                _properties[property.Name] = property;
            }
        }

        public EventWaitHandle GetWaitHandle(string propertyName)
        {
            if (_properties.TryGetValue(propertyName, out var prop) && _waitHandles.TryGetValue(prop.MetadataToken, out var handle))
            {
                return handle;
            }

            return null;
        }

        public TProp GetPropertyValue<TProp>(string name)
        {
            if (_properties.TryGetValue(name, out var prop) && prop.GetValue(Target) is TProp value)
            {
                return value;
            }

            return default;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var result = targetMethod.Invoke(Target, args);
            if (_waitHandles.TryGetValue(targetMethod.MetadataToken, out var handle))
            {
                handle.Set();
            }

            return result;
        }

        public void Dispose()
        {
            foreach (var handle in _waitHandles.Values)
            {
                handle.Close();
            }
        }
    }

    public static class StateExtensions
    {
        public static Task Wait<T, TProp>(this T o, string propertyName, Predicate<TProp> predicate)
        {
            return Wait(o, propertyName, predicate, TimeSpan.FromTicks(-1));
        }

        public static async Task Wait<T, TProp>(this T o, string propertyName, Predicate<TProp> predicate, TimeSpan timeout)
        {
            if (o is not StateProxy<T> proxy)
            {
                throw new InvalidOperationException("Object must be a StateProxy.");
            }

            if (predicate(proxy.GetPropertyValue<TProp>(propertyName)))
            {
                return;
            }

            var handle = proxy.GetWaitHandle(propertyName);
            if (handle is not null)
            {
                handle.Reset();
                await handle.WaitAsTask(timeout);
                if (!predicate(proxy.GetPropertyValue<TProp>(propertyName)))
                {
                    await Wait(o, propertyName, predicate, timeout);
                }
            }
        }

        public static TInterface DecorateState<TInterface, TInstance>(this TInstance instance) where TInstance : TInterface
        {
            var proxy = DispatchProxy.Create<TInterface, StateProxy<TInterface>>();
            if (proxy is StateProxy<TInterface> state)
            {
                state.Target = instance;
                state.InstanceType = typeof(TInstance);
            }

            return proxy;
        }
    }
}
