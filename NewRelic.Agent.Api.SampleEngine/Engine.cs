using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NewRelic.SampleEngine.Exceptions;

namespace NewRelic.Agent.Api.SampleEngine
{
    public class Engine : IEngine
    {
        public bool Execute()
        {
            var taskList = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                taskList.Add(Task.Factory.StartNew(new SearchThread().Execute));
            }
            Task.WaitAll(taskList.ToArray());
            return true;
        }
    }

    public class SearchThread
    {
        public void Execute()
        {
            var taskList = new List<Task>();
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    taskList.Add(Task.Factory.StartNew(new DataThread().Execute));
                }
                Task.WaitAll(taskList.ToArray());
            }
            catch (AggregateException ex)
            {
                //
            }
            finally
            {
                foreach (var task in taskList)
                {
                    if (task.IsCompleted == false || task.Status != TaskStatus.RanToCompletion)
                    {
                        ExceptionManager.HandleException(task.Exception, "Log Only Policy");
                    }
                }
            }
        }
    }

    public class DataThread
    {
        public void Execute()
        {
            try
            {
                throw new ArgumentException();
            }
            catch (Exception e)
            {
                ExceptionManager.HandleException(e, "Log Only Policy");
            }
        }
    }
}