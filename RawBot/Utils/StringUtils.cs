using System;

namespace RawBot.Utils
{
    public static class StringUtils
    {
        public static bool EqualsIgnoreCase(this string s0, string s1)
        {
            return s0 is not null && s1 is not null && s0.Equals(s1, StringComparison.OrdinalIgnoreCase);
        }
    }
}