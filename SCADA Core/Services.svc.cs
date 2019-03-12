using SCADA_Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using static SCADACore.Utility;
using static SCADACore.Vars;

namespace SCADACore
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Services : IServiceRealTimeUnit, IServiceDatabaseManager, IServiceAlarmDisplay, IServiceTrending
    {       

        private delegate void RaiseAlarmDelegate(string alarm);
        private static RaiseAlarmDelegate RaiseAlarmHandler = null;
        private delegate void UpdateTrendingDelegate(string tagId, double value);
        private static UpdateTrendingDelegate UpdateTrendingHandler = null;
        private IServiceCallback ServiceCallback = null;

        public static void WriteAlarm(string alarm)
        {
            if (RaiseAlarmHandler != null)
            {
                RaiseAlarmHandler(alarm);
            }
        }

        public static void UpdateTrending(string tagId, double value)
        {
            if (UpdateTrendingHandler != null)
            {
                UpdateTrendingHandler(tagId, value);
            }
        }

        public void AlarmDisplayInit()
        {
            ServiceCallback = OperationContext.Current.GetCallbackChannel<IServiceCallback>();
            RaiseAlarmHandler = new RaiseAlarmDelegate(WriteAlarmHandler);
        }

        public void TrendingInit()
        {
            ServiceCallback = OperationContext.Current.GetCallbackChannel<IServiceCallback>();
            UpdateTrendingHandler = new UpdateTrendingDelegate(SendToTrendingHandler);
        }

        public void WriteAlarmHandler(string alarm)
        {
            ServiceCallback.RaiseAlarm(alarm);
        }

        public void SendToTrendingHandler(string tagId, double value)
        {
            ServiceCallback.SendToTrending(tagId, value);
        }

        public bool InitRTU(string RTUid)
        {
            foreach (RTU rtu in rtus)
            {
                if (rtu.rtuID.Equals(RTUid))
                {
                    Thread newThread = new Thread(rtu.function);
                    rtuThreads[RTUid] = newThread;
                    rtuThreads[RTUid].Start();

                    return true;
                }
            }

            return false;
        }

        //public static bool SendData(string RTUid, string address, string message, byte[] signature)
        //{
        //    // If RTU is not connected on an input tag, break the operation
        //    if (connectedRTUs[RTUid] == null)
        //    {
        //        return false;
        //    }

        //    // Read public key
        //    CspParameters csp = new CspParameters();
        //    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp);

        //    try
        //    {
        //        rsa.FromXmlString(RTUkeys[RTUid]);
        //    } catch
        //    {
        //        return false;
        //    }

        //    rsa.PersistKeyInCsp = true;

        //    // Compute hash value
        //    byte[] hashValue = ComputeHashValue(message);

        //    if (VerifyMessage(hashValue, signature, rsa))
        //    {
        //        RealTimeDriver.addresses[address] = double.Parse(message);
        //        return true;
        //    }

        //    return false;

        //}

        public string ApplyOnSystem(string id)
        {
            csp.KeyContainerName = "KeyContainerName";
            csp.Flags = CspProviderFlags.UseMachineKeyStore;
            rsa = new RSACryptoServiceProvider(csp);
            rsa.PersistKeyInCsp = true;

            string publicKey = rsa.ToXmlString(false);

            string retVal = "";
            if (!RTUkeys.ContainsKey(id))
            {
                if (RealTimeDriver.addresses.ContainsValue(-1))
                {
                    foreach(KeyValuePair<string, double> pair in RealTimeDriver.addresses)
                    {
                        if (pair.Value == -1)
                        {
                            retVal = pair.Key;
                            RealTimeDriver.addresses[pair.Key] = 0;
                            break;
                        }
                    }

                    RTU rtu = new RTU();
                    rtu.rtuID = id;
                    rtu.publicKey = publicKey;
                    rtu.address = retVal;
                    if (id.StartsWith("DI"))
                    {
                        rtu.type = true;
                    } else
                    {
                        rtu.type = false;
                    }
                    rtus.Add(rtu);

                    RTUkeys[id] = publicKey;
                    connectedRTUs[id] = null;
                    RTUIdAddressMap[id] = retVal;
                    return retVal;
                } else
                {
                    retVal = "-1";
                    return retVal;
                }
            } else
            {
                retVal = "0";
                return retVal;
            }
        }

        public string GetSimAddress(bool type)
        {
            string retVal = null;

            if (type)
            {
                string[] keys = {"1", "2", "3", "4", "5"};
                foreach (KeyValuePair<string, double> pair in SimulationDriver.addresses)
                {
                    if (keys.Contains(pair.Key) && pair.Value == -1)
                    {
                        retVal = pair.Key;
                        return retVal;
                    }
                }
            }
            else
            {
                string[] keys = { "6", "7", "8", "9", "10" };
                foreach (KeyValuePair<string, double> pair in SimulationDriver.addresses)
                {
                    if (keys.Contains(pair.Key) && pair.Value == -1)
                    {
                        retVal = pair.Key;
                        return retVal;
                    }
                }
            }

            return retVal;
        }

        public List<string> GetRTUs(bool type)
        {
            List<string> retVal = new List<string>();

            if (type)
            {
                foreach (string rtuId in connectedRTUs.Keys)
                {
                    if (rtuId.StartsWith("DI") && connectedRTUs[rtuId] == null)
                    {
                        retVal.Add(rtuId);
                    }
                }
            } else
            {
                foreach (string rtuId in connectedRTUs.Keys)
                {
                    if (rtuId.StartsWith("AN") && connectedRTUs[rtuId] == null)
                    {
                        retVal.Add(rtuId);
                    }
                }
            }

            return retVal;

        }

        public bool AddDITag(string id, string desc, int scanTime, bool realTime, string rtuId)
        {
            foreach (Tag iter in tags)
            {
                if (iter.ID.Equals(id))
                {
                    return false;
                }
            }

            DigitalInput tag = new DigitalInput();
            tag.ID = id;
            tag.Description = desc;
            tag.ScanTime = scanTime;
            tag.Alarms = new List<string>();
            tag.AutoManual = true;
            tag.OnOffScan = true;

            if (realTime)
            {
                tag.Driver = true;
                tag.IOAddress = RTUIdAddressMap[rtuId];
                connectedRTUs[rtuId] = id;
            } else
            {
                tag.Driver = false;
                tag.IOAddress = rtuId;
                InitSimulation("rectangle", rtuId);
                //tags.Add(tag);
                //return true;
            }

            tags.Add(tag);
            Thread thread = new Thread(tag.Scan);
            threads[tag.ID] = thread;
            threads[tag.ID].Start();

            return true;

        }

        public bool AddAITag(string id, string desc, int scanTime, bool realTime, string rtuId, double lowLimit, double highLimit, string units, string simType)
        {
            foreach (Tag iter in tags)
            {
                if (iter.ID.Equals(id))
                {
                    return false;
                }
            }

            AnalogInput tag = new AnalogInput();
            tag.ID = id;
            tag.Description = desc;
            tag.ScanTime = scanTime;
            tag.Alarms = new List<string>();
            tag.AutoManual = true;
            tag.OnOffScan = true;
            tag.LowLimit = lowLimit;
            tag.HighLimit = highLimit;
            tag.Units = units;

            if (realTime)
            {
                tag.Driver = true;
                tag.IOAddress = RTUIdAddressMap[rtuId];
                connectedRTUs[rtuId] = id;
            }
            else
            {
                tag.Driver = false;
                tag.IOAddress = rtuId;
                InitSimulation(simType, rtuId);
                //tags.Add(tag);
                //return true;
            }

            tags.Add(tag);
            Thread thread = new Thread(tag.Scan);
            threads[tag.ID] = thread;
            threads[tag.ID].Start();

            return true;
        }

        public bool AddDOTag(string id, string desc, double initValue, string rtuId)
        {
            foreach (Tag iter in tags)
            {
                if (iter.ID.Equals(id))
                {
                    return false;
                }
            }

            DigitalOutput tag = new DigitalOutput();
            tag.ID = id;
            tag.Description = desc;
            tag.TagValue = initValue;
            tag.Driver = false;
            tag.IOAddress = rtuId;

            tags.Add(tag);
            //Thread thread = new Thread(new ParameterizedThreadStart(helper));
            //simThreads[tag.ID] = thread;
            //object[] arr = { id, initValue };
            //simThreads[tag.ID].Start(arr);
            return true;
        }

        public bool AddAOTag(string id, string desc, double initValue, string rtuId, double lowLimit, double highLimit, string units)
        {
            foreach (Tag iter in tags)
            {
                if (iter.ID.Equals(id))
                {
                    return false;
                }
            }

            AnalogOutput tag = new AnalogOutput();
            tag.ID = id;
            tag.Description = desc;
            tag.LowLimit = lowLimit;
            tag.HighLimit = highLimit;
            tag.Units = units;
            tag.Driver = false;
            tag.IOAddress = rtuId;
            tag.TagValue = initValue;

            tags.Add(tag);
            //Thread thread = new Thread(new ParameterizedThreadStart(helper));
            //simThreads[tag.ID] = thread;
            //object[] arr = { id, initValue };
            //simThreads[tag.ID].Start(arr);
            return true;

        }

        private void InitSimulation(string type, string address)
        {
            Thread thread = null;
            if (type.Equals("sine"))
            {
                thread = new Thread(new ParameterizedThreadStart(SimulationDriver.Sine));
            } else if (type.Equals("cosine"))
            {
                thread = new Thread(new ParameterizedThreadStart(SimulationDriver.Cosine));
            }
            else if (type.Equals("rectangle"))
            {
                thread = new Thread(new ParameterizedThreadStart(SimulationDriver.Rectangle));
            }
            else if (type.Equals("triangle"))
            {
                thread = new Thread(new ParameterizedThreadStart(SimulationDriver.Triangle));
            }
            else if (type.Equals("ramp"))
            {
                thread = new Thread(new ParameterizedThreadStart(SimulationDriver.Ramp));
            }

            simThreads[address] = thread;
            simThreads[address].Start(address);

            SimulationItem simItem = new SimulationItem() { type = type, address = address };
            simulations.Add(simItem);
        }
        
        public string GetTagValue(string name)
        {
            foreach (Tag iter in tags)
            {
                if (iter.ID.Equals(name))
                {
                    return (iter.TagValue).ToString();
                }
            }

            return null;
        }

        public bool SetTagValue(string tagId, double value)
        {
            foreach (Tag iter in tags)
            {
                if (iter.ID.Equals(tagId))
                {
                    iter.TagValue = value;
                    return true;
                }
            }
            return false;
        }

        public void helper(object obj)
        {
            object[] arr = (object[])obj;
            string id = (string)arr[0];
            double value = (double)arr[1];

            UpdateTrending(id, value);
        }

        public List<string> GetTags()
        {
            List<string> retVal = new List<string>();
            if (tags.Count == 0)
            {
                return null;
            } else
            {
                foreach (Tag tag in tags)
                {
                    retVal.Add(tag.ID);
                }
            }

            return retVal;
        }

        public Dictionary<string, string> GetTagInfo(string tagId)
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();

            foreach (Tag tag in tags)
            {
                if (tag.ID.Equals(tagId))
                {
                    if (tag.GetType() == typeof(DigitalInput))
                    {
                        DigitalInput temp = (DigitalInput)tag;
                        retVal["Name"] = temp.ID;
                        retVal["Type"] = "DigitalInput";
                        retVal["Description"] = temp.Description;
                        retVal["Driver"] = temp.Driver ? "Real-Time" : "Simulation";
                        retVal["I/O Address"] = temp.IOAddress;
                        retVal["Scan time"] = temp.ScanTime.ToString();

                        string alarms = "";
                        foreach (string s in temp.Alarms)
                        {
                            alarms += s + "\n";
                        }

                        retVal["Alarms"] = alarms;
                        retVal["Scan"] = temp.OnOffScan ? "ON" : "OFF";
                        retVal["Auto/Manual"] = temp.AutoManual ? "AUTO" : "MANUAL";

                        return retVal;
                    }
                    else if (tag.GetType() == typeof(AnalogInput))
                    {
                        AnalogInput temp = (AnalogInput)tag;
                        retVal["Name"] = temp.ID;
                        retVal["Type"] = "AnalogInput";
                        retVal["Description"] = temp.Description;
                        retVal["Driver"] = temp.Driver ? "Real-Time" : "Simulation";
                        retVal["I/O Address"] = temp.IOAddress;
                        retVal["Scan time"] = temp.ScanTime.ToString();

                        string alarms = "";
                        foreach (string s in temp.Alarms)
                        {
                            alarms += s + "\n";
                        }

                        retVal["Alarms"] = alarms;
                        retVal["Scan"] = temp.OnOffScan ? "ON" : "OFF";
                        retVal["Auto/Manual"] = temp.AutoManual ? "AUTO" : "MANUAL";
                        retVal["Low limit"] = temp.LowLimit.ToString();
                        retVal["High limit"] = temp.HighLimit.ToString();
                        retVal["Units"] = temp.Units;

                        return retVal;
                    }
                    else if (tag.GetType() == typeof(DigitalOutput))
                    {
                        DigitalOutput temp = (DigitalOutput)tag;
                        retVal["Name"] = temp.ID;
                        retVal["Type"] = "DigitalOutput";
                        retVal["Description"] = temp.Description;
                        retVal["Driver"] = temp.Driver ? "Real-Time" : "Simulation";
                        retVal["I/O Address"] = temp.IOAddress;
                        retVal["Initial value"] = temp.InitialValue.ToString();

                        return retVal;
                    }
                    else
                    {
                        AnalogOutput temp = (AnalogOutput)tag;
                        retVal["Name"] = temp.ID;
                        retVal["Type"] = "AnalogOutput";
                        retVal["Description"] = temp.Description;
                        retVal["Driver"] = temp.Driver ? "Real-Time" : "Simulation";
                        retVal["I/O Address"] = temp.IOAddress;
                        retVal["Low limit"] = temp.LowLimit.ToString();
                        retVal["High limit"] = temp.HighLimit.ToString();
                        retVal["Units"] = temp.Units;
                        retVal["Initial value"] = temp.InitialValue.ToString();

                        return retVal;
                    }
                }
            }

            return null;
        }

        public bool RemoveTag(string tagId)
        {
            foreach (Tag tag in tags)
            {
                if (tag.ID.Equals(tagId))
                {
                    tags.Remove(tag);
                    if (simThreads.ContainsKey(tag.IOAddress))
                    {
                        simThreads[tag.IOAddress].Abort();
                        simThreads.Remove(tag.IOAddress);
                        SimulationDriver.addresses[tag.IOAddress] = -1;
                        DeleteFromSimulationItems(tag.IOAddress);
                    } 
                    if (threads.ContainsKey(tagId))
                    {
                        threads[tagId].Abort();
                        threads.Remove(tagId);
                    }
                    if (connectedRTUs.ContainsValue(tagId))
                    {
                        foreach (KeyValuePair<string, string> pair in connectedRTUs)
                        {
                            if (pair.Value.Equals(tagId))
                            {
                                connectedRTUs[pair.Key] = null;
                                break;
                            }
                        }
                    }
                    UpdateTrending(tagId, 0);

                    Serialize();
                    return true;
                }
            }

            return false;
        }

        public bool SwitchScan(string tagId)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].ID.Equals(tagId))
                {
                    if (tags[i].GetType() == typeof(DigitalInput))
                    {
                        DigitalInput temp = (DigitalInput)tags[i];
                        temp.OnOffScan = !temp.OnOffScan;
                        tags[i] = temp;

                        Serialize();
                        return true;
                    }
                    else if (tags[i].GetType() == typeof(AnalogInput))
                    {
                        AnalogInput temp = (AnalogInput)tags[i];
                        temp.OnOffScan = !temp.OnOffScan;
                        tags[i] = temp;

                        Serialize();
                        return true;
                    }
                }
            }

            return false;
        }

        public bool SwitchAutoManual(string tagId)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].ID.Equals(tagId))
                {
                    if (tags[i].GetType() == typeof(DigitalInput))
                    {
                        DigitalInput temp = (DigitalInput)tags[i];
                        temp.AutoManual = !temp.AutoManual;
                        tags[i] = temp;

                        Serialize();
                        return true;
                    }
                    else if (tags[i].GetType() == typeof(AnalogInput))
                    {
                        AnalogInput temp = (AnalogInput)tags[i];
                        temp.AutoManual = !temp.AutoManual;
                        tags[i] = temp;

                        Serialize();
                        return true;
                    }
                }
            }

            return false;
        }

        public bool SwitchDriver(string tagId, string address, string simType)
        {
            int i;
            bool check = false;
            for (i = 0; i < tags.Count; i++)
            {
                if (tags[i].ID.Equals(tagId)) {
                    check = true;
                    break;
                }
            }

            if (!check) return false;

            if (simType == null) {
                foreach (KeyValuePair<string, string> pair in connectedRTUs)
                {
                    if (pair.Value != null)
                    {
                        if (pair.Value.Equals(tags[i].ID))
                        {
                            connectedRTUs[pair.Key] = null;
                            break;
                        }
                    }
                    
                }
                if (simThreads.ContainsKey(tags[i].IOAddress))
                {
                    simThreads[tags[i].IOAddress].Abort();
                    simThreads[tags[i].IOAddress] = null;
                    SimulationDriver.addresses[tags[i].IOAddress] = -1;
                    DeleteFromSimulationItems(tags[i].IOAddress);
                }

                tags[i].Driver = true;
                tags[i].IOAddress = RTUIdAddressMap[address];
                connectedRTUs[address] = tags[i].ID;

                Serialize();
                return true;
            } else
            {
                foreach (KeyValuePair<string, string> pair in connectedRTUs)
                {
                    if (pair.Value != null)
                    {
                        if (pair.Value.Equals(tags[i].ID))
                        {
                            connectedRTUs[pair.Key] = null;
                            break;
                        }
                    }
                    
                }
                if (simThreads.ContainsKey(tags[i].IOAddress))
                {
                    simThreads[tags[i].IOAddress].Abort();
                    simThreads[tags[i].IOAddress] = null;
                    SimulationDriver.addresses[tags[i].IOAddress] = -1;
                    DeleteFromSimulationItems(tags[i].IOAddress);
                }

                tags[i].Driver = false;
                tags[i].IOAddress = address;
                InitSimulation(simType, address);

                Serialize();
                return true;
            }
            
        }

        public bool ChangeInfo(string tagId, Tuple<string, string> newVal)
        {
            int i;
            Tag tag = null;
            bool check = false;
            for (i = 0; i < tags.Count; i++)
            {
                if (tags[i].ID.Equals(tagId))
                {
                    check = true;
                    tag = tags[i];
                    break;
                }
            }

            if (!check) return false;

            if (!newVal.Item2.Equals(""))
            {
                if (newVal.Item1.Equals("Name"))
                {
                    tag.ID = newVal.Item2;
                }
                else if (newVal.Item1.Equals("Description"))
                {
                    tag.Description = newVal.Item2;
                }
                else if (newVal.Item1.Equals("Units"))
                {
                    if (tag.GetType() == typeof(AnalogInput))
                    {
                        AnalogInput temp = (AnalogInput)tag;
                        temp.Units = newVal.Item2;
                        tag = temp;
                    } else
                    {
                        AnalogOutput temp = (AnalogOutput)tag;
                        temp.Units = newVal.Item2;
                        tag = temp;
                    }
                }
                else if (newVal.Item1.Equals("Low limit"))
                {
                    if (tag.GetType() == typeof(AnalogInput))
                    {
                        AnalogInput temp = (AnalogInput)tag;
                        temp.LowLimit = double.Parse(newVal.Item2);
                        tag = temp;
                    }
                    else
                    {
                        AnalogOutput temp = (AnalogOutput)tag;
                        temp.LowLimit = double.Parse(newVal.Item2);
                        tag = temp;
                    }
                }
                else if (newVal.Item1.Equals("High limit"))
                {
                    if (tag.GetType() == typeof(AnalogInput))
                    {
                        AnalogInput temp = (AnalogInput)tag;
                        temp.HighLimit = double.Parse(newVal.Item2);
                        tag = temp;
                    }
                    else
                    {
                        AnalogOutput temp = (AnalogOutput)tag;
                        temp.HighLimit = double.Parse(newVal.Item2);
                        tag = temp;
                    }
                }
                else if (newVal.Item1.Equals("Scan time"))
                {
                    if (tag.GetType() == typeof(AnalogInput))
                    {
                        AnalogInput temp = (AnalogInput)tag;
                        temp.ScanTime = int.Parse(newVal.Item2);
                        tag = temp;
                    }
                    else
                    {
                        DigitalInput temp = (DigitalInput)tag;
                        temp.ScanTime = int.Parse(newVal.Item2);
                        tag = temp;
                    }
                }
                else // Inital value
                {
                    if (tag.GetType() == typeof(AnalogInput))
                    {
                        DigitalOutput temp = (DigitalOutput)tag;
                        temp.InitialValue = double.Parse(newVal.Item2);
                        tag = temp;
                    }
                    else
                    {
                        AnalogOutput temp = (AnalogOutput)tag;
                        temp.InitialValue = double.Parse(newVal.Item2);
                        tag = temp;
                    }
                }
            }

            tags[i] = tag;
            Serialize();
            return true;
        }

        private bool DeleteFromSimulationItems(string address)
        {
            int i;
            for (i = 0; i < simulations.Count; i++)
            {
                if (simulations[i].address.Equals(address))
                {
                    simulations.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public bool LoadLastState()
        {
            List<Measurement> measurements = dbManager.Select();

            if (measurements == null)
            {
                return false;
            } else if (measurements.Count == 0)
            {
                return false;
            }

            VarsSerialization temp = Deserialize();
            if (temp == null)
            {
                return false;
            }

            tags = temp.Tags;
            rtus = temp.RTUs;
            simulations = temp.SimulationItems;

            foreach (RTU rtu in rtus)
            {
                RTUkeys[rtu.rtuID] = rtu.publicKey;
                RTUIdAddressMap[rtu.rtuID] = rtu.address;
                connectedRTUs[rtu.rtuID] = null;
                Thread thread = new Thread(rtu.function);
                rtuThreads[rtu.rtuID] = thread;
                rtuThreads[rtu.rtuID].Start();
            }

            foreach (Tag tag in tags)
            {
                if (tag.Driver)
                {
                    foreach (KeyValuePair<string, string> pair in RTUIdAddressMap)
                    {
                        if (pair.Value.Equals(tag.IOAddress))
                        {
                            connectedRTUs[pair.Key] = tag.ID;
                            Thread thread = new Thread(tag.Scan);
                            threads[tag.ID] = thread;
                            threads[tag.ID].Start();
                            break;
                        }
                    }
                } else
                {
                    foreach (SimulationItem item in simulations)
                    {
                        if (item.address.Equals(tag.IOAddress))
                        {
                            InitSimulation(item.type, item.address);
                            Thread thread = new Thread(tag.Scan);
                            threads[tag.ID] = thread;
                            threads[tag.ID].Start();
                            break;
                        }
                    }
                }
                
            }

            return true;
        }

        public void ClearDatabase()
        {
            dbManager.Clear();
        }

        public bool DeleteRTU(string RTUid)
        {
            foreach (RTU rtu in rtus)
            {
                if (rtu.rtuID.Equals(RTUid))
                {

                    rtuThreads[rtu.rtuID].Abort();
                    rtuThreads.Remove(rtu.rtuID);
                    RTUkeys.Remove(rtu.rtuID);
                    RTUIdAddressMap.Remove(rtu.rtuID);
                    connectedRTUs.Remove(rtu.rtuID);
                    RealTimeDriver.addresses[rtu.address] = -1;
                    rtus.Remove(rtu);

                    return true;
                }
            }

            return false;
        }

        public List<string> GetRTUs()
        {
            List<string> retVal = new List<string>();
            foreach (RTU rtu in rtus)
            {
                retVal.Add(rtu.rtuID);
            }

            return retVal;
        }

    }
}
