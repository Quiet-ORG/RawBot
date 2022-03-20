using RawBot.Runtime;
using System.Threading.Tasks;

namespace RawBot.Plugins
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        string Author { get; }

        Context Context { set; }

        Task LoadAsync();
        Task UnloadAsync();
    }
}