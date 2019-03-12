using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace SCADA_Core
{
    [DataContract]
    public class Measurement
    {
        [DataMember]
        public string TagId { get; set; }

        [DataMember]
        public double Value { get; set; }

        [DataMember]
        public SqlDateTime Time { get; set; }
    }
}