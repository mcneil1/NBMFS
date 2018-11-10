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
    //[DataContract] allows the object to be serialized to JSON
    //[DataMember] allows the object attribute to be serialized to JSON

    [DataContract]
    public class Message
    {
        //every message has a header
        [DataMember]
        public string Header { get; set; }

        //every message has a body
        [DataMember]
        public string Body { get; set; }

    }
}
