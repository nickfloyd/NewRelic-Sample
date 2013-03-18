using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using NewRelic.Agent.Api.Pusher;

namespace NewRelic.SampleEngine.Logging
{
    public class ExceptionHandler : IExceptionHandler
    {
        public ExceptionHandler(NameValueCollection attributes)
        {

        }

        public Exception HandleException(Exception exception, Guid handlingInstanceId)
        {
            ExceptionQueue.Enqueue(exception);
            return exception;
        }
    }

    public interface IExceptionHandler
    {
        Exception HandleException(Exception exception, Guid handlingInstanceId);
    }
}
