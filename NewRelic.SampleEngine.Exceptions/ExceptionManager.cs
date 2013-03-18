using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NewRelic.SampleEngine.Logging;


namespace NewRelic.SampleEngine.Exceptions
{
    public class ExceptionManager
    {
        private static ExceptionManager _exceptionManager;

        private ExceptionManager()
        {

        }

        public static void HandleException(Exception e, string policyName)
        {
            var exceptionManager = GetInstance();

            exceptionManager.HandleExceptionSync(e, policyName);
        }

        public static ExceptionManager GetInstance()
        {
            _exceptionManager = new ExceptionManager();
            return _exceptionManager;
        }

        public void HandleExceptionSync(Exception e, string policyName)
        {
            Action handleException = () =>
                                         {
                                             IExceptionHandler exceptionHandler = new ExceptionHandler(null);
                                             exceptionHandler.HandleException(e, Guid.NewGuid());
                                         };
            Task.Factory.StartNew(handleException);
        }
    }
}