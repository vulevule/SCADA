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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServices" in both code and config file together.
    [ServiceContract]
    public interface IServiceRealTimeUnit
    {
        [OperationContract]
        bool InitRTU(string RTUid);

        [OperationContract]
        List<string> GetRTUs();

        [OperationContract]
        string ApplyOnSystem(string id);

        [OperationContract]
        bool DeleteRTU(string RTUid);
    }

    [ServiceContract]
    public interface IServiceDatabaseManager
    {
        [OperationContract]
        bool AddDITag(string id, string desc, int scanTime, bool realTime, string rtuId);

        [OperationContract]
        bool AddAITag(string id, string desc, int scanTime, bool realTime, string rtuId, double lowLimit, double highLimit, string units, string simType);

        [OperationContract]
        bool AddDOTag(string id, string desc, double initValue, string rtuId);

        [OperationContract]
        bool AddAOTag(string id, string desc, double initValue, string rtuId, double lowLimit, double highLimit, string units);

        [OperationContract]
        List<string> GetRTUs(bool type);

        [OperationContract]
        string GetSimAddress(bool type);

        [OperationContract]
        string GetTagValue(string name);

        [OperationContract]
        bool SetTagValue(string tagId, double value);

        [OperationContract]
        List<string> GetTags();

        [OperationContract]
        Dictionary<string, string> GetTagInfo(string tagId);

        [OperationContract]
        bool RemoveTag(string tagId);

        [OperationContract]
        bool SwitchScan(string tagId);

        [OperationContract]
        bool SwitchAutoManual(string tagId);

        [OperationContract]
        bool SwitchDriver(string tagId, string address, string simType);

        [OperationContract]
        bool ChangeInfo(string tagId, Tuple<string, string> newVal);

        [OperationContract]
        bool LoadLastState();

        [OperationContract]
        void ClearDatabase();
    }

    [ServiceContract(CallbackContract = typeof(IServiceCallback))]
    public interface IServiceAlarmDisplay
    {
        //[OperationContract]
        //void WriteAlarm(string alarm);

        [OperationContract]
        void AlarmDisplayInit();
    }

    [ServiceContract(CallbackContract = typeof(IServiceCallback))]
    public interface IServiceTrending
    {
        //[OperationContract]
        //void WriteToTrending(string tagId, double value);

        [OperationContract]
        void TrendingInit();
    }

    

   

}
