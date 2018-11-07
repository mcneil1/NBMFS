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
    public class SMS : Message
    {


        //SMS validator checks that the header and sender are in the correct formats
        public static bool SMSValidator(string header, string sender)
        {
            string SMSheader = @"^[S]{1}[0-9]{9}";
            MatchCollection matchHeader = Regex.Matches(header, SMSheader, RegexOptions.IgnoreCase);
            string SMSsender = @"^([0-9]+)";
            MatchCollection matchBody = Regex.Matches(sender, SMSsender, RegexOptions.IgnoreCase);

            StringBuilder sb = new StringBuilder();
            foreach (var m in matchHeader)
            {
                sb.Append(m + " ");
            }
            foreach (var m in matchBody)
            {
                sb.Append(m + "");
            }
            if ((sb.ToString() == header + " " + sender) && sb.Length == 22)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
