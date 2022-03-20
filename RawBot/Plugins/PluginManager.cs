using RawBot.Logging;
using RawBot.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RawBot.Plugins
{
    public class PluginManager
    {
        private readonly Context _context;
        private readonly List<PluginContext> _plugins = new();

        public IEnumerable<IPlugin> Plugins => _plugins.Select(p => p.Plugin);

        public PluginManager(Context context)
        {
            _context = context;
        }

        public async Task LoadAsync(string path)
        {
            if (_plugins.Any(p => p.Path == path))
            {
                return;
            }

            var loadContext = new PluginLoadContext(Path.Combine(Environment.CurrentDirectory, path));
            try
            {
                var pluginAssembly = loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
                var pluginType = pluginAssembly.GetTypes().FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t));
                if (pluginType is null)
                {
                    loadContext.Unload();
                }
                else
                {
                    if (Activator.CreateInstance(pluginType) is not IPlugin plugin)
                    {
                        throw new Exception("Failed to instantiate plugin class.");
                    }

                    plugin.Context = _context;
                    await plugin.LoadAsync();
                    _plugins.Add(new PluginContext(loadContext, plugin, path));
                    _context.Log.Debug($"Loaded plugin {plugin.Name}.");
                }
            }
            catch (Exception e)
            {
                _context.Log.Error("Error loading plugin.", e);
                loadContext.Unload();
            }
        }

        public async Task UnloadAsync(PluginContext pluginContext)
        {
            try
            {
                _plugins.Remove(pluginContext);
                await pluginContext.Plugin.UnloadAsync();
                pluginContext.Plugin = null;
                pluginContext.Context.Unload();
            }
            catch (Exception e)
            {
                _context.Log.Error("Error unloading plugin.", e);
            }
        }
    }
}
