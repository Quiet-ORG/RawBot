using System.Runtime.Loader;

namespace RawBot.Runtime.Scripts
{
    public class CollectibleAssemblyLoadContext : AssemblyLoadContext
    {
        public CollectibleAssemblyLoadContext() : base(true)
        {

        }
    }
}