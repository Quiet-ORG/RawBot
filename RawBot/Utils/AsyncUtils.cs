using System;
using System.Threading;
using System.Threading.Tasks;

namespace RawBot.Utils
{
    public static class AsyncUtils
    {
        public static Task<bool> WaitAsTask(this WaitHandle handle)
        {
            return WaitAsTask(handle, TimeSpan.FromTicks(-1));
        }

        public static Task<bool> WaitAsTask(this WaitHandle handle, TimeSpan timeout)
        {
            var already = handle.WaitOne(0);
            if (already)
            {
                return Task.FromResult(true);
            }

            if (timeout == TimeSpan.Zero)
            {
                return Task.FromResult(false);
            }

            if (timeout < TimeSpan.Zero)
            {
                timeout = TimeSpan.FromTicks(-1);
            }

            var tcs = new TaskCompletionSource<bool>();
            var threadPoolRegistration = ThreadPool.RegisterWaitForSingleObject(handle,
                (state, timedOut) => ((TaskCompletionSource<bool>)state).TrySetResult(!timedOut),
                tcs, timeout, true);
            tcs.Task.ContinueWith(_ => { threadPoolRegistration.Unregister(handle); }, TaskScheduler.Default);
            return tcs.Task;
        }
    }
}
