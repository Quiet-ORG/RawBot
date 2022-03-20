using Newtonsoft.Json.Linq;
using RawBot.State;
using System.Collections.Generic;
using System.Linq;

namespace RawBot.Utils
{
    public static class JsonUtils
    {
        public static IEnumerable<T> Convert<T>(this object o)
        {
            if (o is not JArray array)
            {
                return Enumerable.Empty<T>();
            }

            return array.Select(i => i.ToObject<T>());
        }

        public static IEnumerable<TInterface> Convert<TInterface, TObject>(this object o, bool decorate = true) where TObject : TInterface
        {
            var converted = Convert<TObject>(o);
            return decorate ? converted.Select(c => c.DecorateState<TInterface, TObject>()) : converted.Cast<TInterface>();
        }
    }
}
