using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace NBMFS
{
    [DataContract]
    public class Message
    {
        [DataMember]
        public string Header { get; set; }

        [DataMember]
        public string Body { get; set; }

    }
}
