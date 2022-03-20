using System;
using System.Collections.Generic;
using System.Linq;

namespace RawBot.Utils
{
    public static class CollectionUtils
    {
        public static void RemoveFirst<T>(this IList<T> list, Predicate<T> predicate)
        {
            var index = -1;
            for (var i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                list.RemoveAt(index);
            }
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default;
            rest = list.Skip(1).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default;
            second = list.Count > 1 ? list[1] : default;
            rest = list.Skip(2).ToList();
        }

        public static IEnumerable<T> Singleton<T>(this T obj)
        {
            return new[] { obj };
        }
    }
}
