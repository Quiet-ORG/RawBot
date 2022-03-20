using RawBot.State.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace RawBot.Runtime.Timers
{
    public class TimerManager : ListManager<ITimer, string>
    {
        private const int MinSleep = 5;

        private readonly Context _context;
        private readonly Stopwatch _stopwatch = new();
        private readonly Dictionary<string, TimeSpan> _lastRun = new();

        private Thread _thread;
        private volatile bool _running;
        private volatile bool _exit;

        public TimeSpan Elapsed => _stopwatch.Elapsed;
        public double Time => _stopwatch.Elapsed.TotalSeconds;

        public TimerManager(Context context)
        {
            _context = context;
        }

        public void Start()
        {
            if (_running)
            {
                throw new InvalidOperationException("Timer manager already started.");
            }

            _exit = false;
            _thread = new Thread(Poll);
            _thread.Start();
        }

        public void Stop()
        {
            if (!_running)
            {
                throw new InvalidOperationException("Timer manager not running.");
            }

            _exit = true;
        }

        private void Poll()
        {
            _running = true;
            _stopwatch.Start();
            while (!_exit)
            {
                var earliestNextRun = _stopwatch.Elapsed + TimeSpan.FromDays(1);
                foreach (var timer in Items)
                {
                    var now = _stopwatch.Elapsed;
                    var nextRun = _lastRun.GetValueOrDefault(timer.Name, TimeSpan.Zero) + timer.Interval;
                    if (nextRun <= now)
                    {
                        _lastRun[timer.Name] = now;
                        timer.Poll(_context);
                        nextRun = _stopwatch.Elapsed + timer.Interval;
                    }

                    if (nextRun < earliestNextRun)
                    {
                        earliestNextRun = nextRun;
                    }
                }

                var sleep = (int)(earliestNextRun.TotalMilliseconds - _stopwatch.Elapsed.TotalMilliseconds);
                Thread.Sleep(Math.Max(MinSleep, sleep));
            }

            _lastRun.Clear();
            _stopwatch.Reset();
            _running = false;
        }

        protected override string GetKey(ITimer timer)
        {
            return timer.Name;
        }
    }
}
