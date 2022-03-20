using System.Threading;

namespace RawBot.State
{
    public class Events
    {
        public AutoResetEvent QuestsLoad = new(false);
    }
}
