using System;
using System.Threading.Tasks;

namespace RawBot.Runtime.Timers
{
    public interface ITimer
    {
        string Name { get; }

        TimeSpan Interval { get; }

        Task Poll(Context context);
    }
}