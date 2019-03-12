using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using SCADA_Core;
using System.Data.SqlTypes;
using static SCADACore.Vars;

namespace SCADACore
{
    [DataContract]
    [KnownType(typeof(DigitalInput))]
    [KnownType(typeof(DigitalOutput))]
    [KnownType(typeof(AnalogInput))]
    [KnownType(typeof(AnalogOutput))]
    [XmlInclude(typeof(DigitalInput))]
    [XmlInclude(typeof(DigitalOutput))]
    [XmlInclude(typeof(AnalogInput))]
    [XmlInclude(typeof(AnalogOutput))]
    public abstract class Tag
    {
        string id;
        string description;
        bool driver;
        string ioAddress;
        double tagValue;

        [DataMember]
        //[XmlElement]
        public string ID { get { return id; } set { id = value; } }
        [DataMember]
        //[XmlElement]
        public string Description { get { return description; } set { description = value; } }
        [DataMember]
        //[XmlElement]
        public string IOAddress { get { return ioAddress; } set { ioAddress = value; } }
        [DataMember]
        //[XmlElement]
        public bool Driver { get { return driver; } set { driver = value; } }
        [DataMember]
        //[XmlElement]
        public double TagValue { get { return tagValue; } set { tagValue = value; } }

        public abstract void Scan();
    }

    [DataContract]
    public class DigitalInput : Tag
    {
        int scanTime;
        List<string> alarms;
        bool onoffScan;
        bool automanual;

        [DataMember]
        //[XmlElement]
        public int ScanTime { get { return scanTime; } set { scanTime = value; } }
        [DataMember]
        //[XmlElement]
        public List<string> Alarms { get { return alarms; } set { alarms = value; } }
        [DataMember]
        //[XmlElement]
        public bool OnOffScan { get { return onoffScan; } set { onoffScan = value; } }
        [DataMember]
        //[XmlElement]
        public bool AutoManual { get { return automanual; } set { automanual = value; } }

        
        public override void Scan()
        {
            while (true)
            {
                if (onoffScan)
                {
                    Thread.Sleep(scanTime);

                    if (Driver)
                    {
                        TagValue = RealTimeDriver.addresses[IOAddress];
                    }
                    else
                    {
                        TagValue = SimulationDriver.addresses[IOAddress];
                    }

                    Services.UpdateTrending(ID, TagValue);

                    Serialize();

                    //DateTime myDateTime = DateTime.Now;
                    //string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.ff");
                    Measurement m = new Measurement() { TagId = ID, Value = TagValue, Time = new SqlDateTime(DateTime.Now)};
                    dbManager.Insert(m);
                }
                
            }
        }
    }

    [DataContract]
    public class DigitalOutput : Tag
    {
        double initialValue;

        [DataMember]
        //[XmlElement]
        public double InitialValue { get { return initialValue; } set { initialValue = value; } }

        public override void Scan()
        {
            while (true)
            {
                Services.UpdateTrending(ID, TagValue);

                Serialize();
            }
        }
    }

    [DataContract]
    public class AnalogInput : Tag
    {
        int scanTime;
        List<string> alarms;
        bool onoffScan;
        bool automanual;
        double lowLimit;
        double highLimit;
        string units;

        [DataMember]
        //[XmlElement]
        public int ScanTime { get { return scanTime; } set { scanTime = value; } }
        [DataMember]
        //[XmlElement]
        public List<string> Alarms { get { return alarms; } set { alarms = value; } }
        [DataMember]
        //[XmlElement]
        public bool OnOffScan { get { return onoffScan; } set { onoffScan = value; } }
        [DataMember]
        //[XmlElement]
        public bool AutoManual { get { return automanual; } set { automanual = value; } }
        [DataMember]
        //[XmlElement]
        public double LowLimit { get { return lowLimit; } set { lowLimit = value; } }
        [DataMember]
        //[XmlElement]
        public double HighLimit { get { return highLimit; } set { highLimit = value; } }
        [DataMember]
        //[XmlElement]
        public string Units { get { return units; } set { units = value; } }

        public override void Scan()
        {
            while (true)
            {
                if (onoffScan)
                {
                    if (automanual)
                    {
                        Thread.Sleep(scanTime);

                        if (Driver)
                        {
                            TagValue = RealTimeDriver.addresses[IOAddress];
                        }
                        else
                        {
                            TagValue = SimulationDriver.addresses[IOAddress];
                        }

                        if (TagValue > highLimit)
                        {
                            string alarm = "ALARM on " + DateTime.Now + " - Tag " + ID + " value: " + TagValue + " | High limit: " + highLimit;
                            alarms.Add(alarm);
                            Services.WriteAlarm(alarm);
                        }
                        else if (TagValue < lowLimit)
                        {
                            string alarm = "ALARM on " + DateTime.Now + " - Tag " + ID + " value: " + TagValue + " | Low limit: " + lowLimit;
                            alarms.Add(alarm);
                            Services.WriteAlarm(alarm);
                        }
                    }                    

                    Services.UpdateTrending(ID, TagValue);

                    Serialize();

                    Measurement m = new Measurement() { TagId = ID, Value = TagValue, Time = new SqlDateTime(DateTime.Now) };
                    dbManager.Insert(m);
                }
                
            }
        }
    }

    [DataContract]
    public class AnalogOutput : Tag
    {
        double initialValue;
        double lowLimit;
        double highLimit;
        string units;

        [DataMember]
        //[XmlElement]
        public double InitialValue { get { return initialValue; } set { initialValue = value; } }
        [DataMember]
        //[XmlElement]
        public double LowLimit { get { return lowLimit; } set { lowLimit = value; } }
        [DataMember]
        //[XmlElement]
        public double HighLimit { get { return highLimit; } set { highLimit = value; } }
        [DataMember]
        //[XmlElement]
        public string Units { get { return units; } set { units = value; } }

        public override void Scan()
        {
            while (true)
            {
                Services.UpdateTrending(ID, TagValue);

                Serialize();
            }
        }
    }
}