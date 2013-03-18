using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NewRelic.Agent.Api.Pusher
{
    public class ExceptionPusher
    {
        public void Start()
        {
            Task.Factory.StartNew(StartPolling);
        }

        private void StartPolling()
        {
            while (true)
            {
                Thread.Sleep(100);
                var isExceptionInQueue = ExceptionQueue.PollQueue();
                if (!isExceptionInQueue) continue;
                Action action = () =>
                                    {
                                        foreach (
                                            var exception in ExceptionQueue.Dequeue())
                                        {
                                            NewRelic.Api.Agent.NewRelic.NoticeError(exception);
                                        }
                                    };
                Parallel.Invoke(action, action);
            }
        }
    }

    public class ExceptionQueue
    {
        private static ConcurrentQueue<Exception> _queue;

        static ExceptionQueue()
        {
            _queue = new ConcurrentQueue<Exception>();
        }

        public static void Enqueue(Exception ex)
        {
            _queue.Enqueue(ex);
        }

        internal static bool PollQueue()
        {
            Exception firstException;
            if (_queue.TryPeek(out firstException) && firstException != null)
            {
                return true;
            }
            return false;
        }

        internal static IEnumerable<Exception> Dequeue()
        {
            Exception exception;
            while (_queue.TryDequeue(out exception))
            {
                yield return exception;
            }
        }
    }
}
