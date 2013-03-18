using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace NewRelic.Agent.Api.SampleEngine
{
    [ServiceContract]
    public interface IEngine
    {
        [OperationContract]
        [WebGet]
        bool Execute();
    }
}
