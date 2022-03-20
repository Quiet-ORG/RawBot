using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RawBot.State.Model.Bindings
{
    public static class Bindings
    {
        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> BindingCache = new();

        public static void UpdateWith(this object o, JToken updates)
        {
            if (o is null)
            {
                return;
            }

            var other = updates.ToObject(o.GetProxiedType());
            if (updates is JObject jObject)
            {
                o.CopyFrom(other, jObject.Properties().Select(p => p.Name));
            }
            else
            {
                o.CopyFrom(other);
            }
        }

        public static void CopyFrom(this object o, object other)
        {
            CopyFrom(o, other, Enumerable.Empty<string>());
        }

        public static void CopyFrom(this object o, object other, IEnumerable<string> names)
        {
            var propertyMap = o.BuildBindings();
            var srcPropertyMap = other.BuildBindings();

            foreach (var (name, property) in srcPropertyMap.Where(kvp => propertyMap.ContainsKey(kvp.Key) && names.Contains(kvp.Key)))
            {
                if (property.GetValue(other) is { } val)
                {
                    propertyMap[name].SetValue(o, val);
                }
            }
        }

        private static Dictionary<string, PropertyInfo> BuildBindings(this object o)
        {
            var type = o.GetType();
            if (!BindingCache.TryGetValue(type, out var bindings))
            {
                bindings = new Dictionary<string, PropertyInfo>();
                foreach (var propertyInfo in o.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    foreach (var binding in propertyInfo.GetCustomAttributes<JsonPropertyAttribute>().Where(a => a.PropertyName is not null))
                    {
                        bindings[binding.PropertyName!] = propertyInfo;
                    }
                }

                BindingCache[type] = bindings;
            }

            return bindings;
        }

        private static Type GetProxiedType(this object o)
        {
            var type = o.GetType();
            if (type.BaseType is { IsGenericType: true } && type.BaseType.GetGenericTypeDefinition() == typeof(StateProxy<>))
            {
                type = (Type)type.BaseType.GetProperty(nameof(StateProxy<object>.InstanceType)).GetValue(o);
            }

            return type;
        }
    }
}
