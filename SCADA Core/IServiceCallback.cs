using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;


namespace SCADACore
{
    interface IServiceCallback
    {
        [OperationContract]
        void RaiseAlarm(string alarm);

        [OperationContract]
        void SendToTrending(string tagId, double value);
    }
}
