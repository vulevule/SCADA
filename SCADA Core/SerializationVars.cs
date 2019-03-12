using SCADACore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml.Serialization;

namespace SCADA_Core
{
    public class VarsSerialization
    {

        public List<Tag> Tags { get; set; }
        public List<RTU> RTUs { get; set; }
        public List<SimulationItem> SimulationItems { get; set; }

        public VarsSerialization()
        {
            Tags = new List<Tag>();
            RTUs = new List<RTU>();
            SimulationItems = new List<SimulationItem>();
        }
      
    }
}