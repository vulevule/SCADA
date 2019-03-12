using SCADA_Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Xml.Serialization;

namespace SCADACore
{
    public class Vars
    {
        public static CspParameters csp = new CspParameters();
        public static RSACryptoServiceProvider rsa = null;

        public static DBManager dbManager = new DBManager();
        public static string ConfigFile = @"C:\Users\Vule\SNUS\SCADA Core\configScada.xml";
        public static VarsSerialization serVars = new VarsSerialization();
        //public List<string> addresses = new List<string>() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        public static List<RTU> rtus = new List<RTU>();
        public static List<SimulationItem> simulations = new List<SimulationItem>();
        public static List<Tag> tags = new List<Tag>();
        //public static List<DigitalInput> DItags = new List<DigitalInput>();
        //public static List<AnalogInput> AItags = new List<AnalogInput>();
        //public static List<DigitalOutput> DOtags = new List<DigitalOutput>();
        //public static List<AnalogOutput> AOtags = new List<AnalogOutput>();

        // Collection of <simulation address, thread> - used to store simulation threads
        public static Dictionary<string, Thread> simThreads = new Dictionary<string, Thread>();

        // Collection of <tag ID, thread> - used for tag's Scan() method
        public static Dictionary<string, Thread> threads = new Dictionary<string, Thread>();

        // Collection of <RTU ID, thread> - used to store RTU threads
        public static Dictionary<string, Thread> rtuThreads = new Dictionary<string, Thread>();

        // Collection of <RTU ID, RTU public key> - used for encryption
        public static Dictionary<string, string> RTUkeys = new Dictionary<string, string>();

        // Collection of <RTU ID, RTU address> - used to map RTUs and addresses where they send data
        public static Dictionary<string, string> RTUIdAddressMap = new Dictionary<string, string>();

        // Collection of <RTU ID, tag ID> - used to check if RTU is connected on a tag
        public static Dictionary<string, string> connectedRTUs = new Dictionary<string, string>();

        public static void Serialize()
        {
            try
            {
                serVars.Tags = tags;
                serVars.RTUs = rtus;
                serVars.SimulationItems = simulations;

                XmlSerializer serializer = new XmlSerializer(typeof(VarsSerialization));
                using (TextWriter tw = new StreamWriter(ConfigFile))
                {
                    serializer.Serialize(tw, serVars);
                }
            }
            catch { }
        }

        public static VarsSerialization Deserialize()
        {
            VarsSerialization result;
            XmlSerializer serializer = new XmlSerializer(typeof(VarsSerialization));
            using (TextReader tr = new StreamReader(ConfigFile))
            {
                result = (VarsSerialization)serializer.Deserialize(tr);
            }
            return result;
        }
    }
}