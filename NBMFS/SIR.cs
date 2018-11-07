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
    public class SIR : Email
    {
        public List<string> SIRlist = new List<string>();

        public void AddSIR(string report)
        {
            SIRlist.Add(report);
        }

        public static bool SIRValidator(string subject,string body)
        {
            string SIR = @"(SIR[\s])(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$";
            MatchCollection matchSubject = Regex.Matches(subject, SIR, RegexOptions.IgnoreCase);

            

            string sortcode = @"(Sort Code:(\s?)([0-9]{2}[-][0-9]{2}[-][0-9]{2}))";
            MatchCollection matchCode = Regex.Matches(body, sortcode, RegexOptions.IgnoreCase);
            string incident = @"(Nature of Incident:(\s ?)(Theft|Staff Attack|ATM Theft|Raid|Customer Attack|Staff Abuse|Bomb Threat|Terrorism|Suspicious Incident|Intelligence|Cash Loss))";
            MatchCollection matchIncident = Regex.Matches(body, incident, RegexOptions.IgnoreCase);

            string[] splitBody = new string[] { "\n" };
            string[] lines = body.Split(splitBody, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder sb = new StringBuilder();
            foreach (var m in matchSubject)
            {
                sb.Append(m + "\r\n");
            }
            foreach (var m in matchCode)
            {
                sb.Append(m + "\r\n");
            }
            foreach (var m in matchIncident)
            {
                sb.Append(m + "");
            }
            if (sb.ToString().Contains(subject) && sb.ToString().Contains(lines[0]) && sb.ToString().Contains(lines[1]))
            {
                return true;
            }
            else
            {
                return false;
            }


        }


        public static SIR SIRConstructor(string header, string sender, string subject, string body)
        {
            SIR sir = new SIR();

            string sortcode = @"(Sort Code:(\s?)([0-9]{2}[-][0-9]{2}[-][0-9]{2}))";
            MatchCollection matchCode = Regex.Matches(body, sortcode, RegexOptions.IgnoreCase);
            string incident = @"(Nature of Incident:(\s ?)(Theft|Staff Attack|ATM Theft|Raid|Customer Attack|Staff Abuse|Bomb Threat|Terrorism|Suspicious Incident|Intelligence|Cash Loss))";
            MatchCollection matchIncident = Regex.Matches(body, incident, RegexOptions.IgnoreCase);

            string[] splitBody = new string[] { "\n" };
            string[] lines = body.Split(splitBody, StringSplitOptions.RemoveEmptyEntries);

            sir.AddSIR(lines[0] + "\n" + lines[1]);

            string url = @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
            MatchCollection match = Regex.Matches(body, url, RegexOptions.IgnoreCase);

            foreach (var m in match)
            {
                sir.AddURL(m.ToString());
            }

            Regex rgxUrls = new Regex(url);
            string result = rgxUrls.Replace(body, "<URL Quarantined>");

            sir.Header = header;
            sir.Body = sender + " " + subject + " " + result;

            return sir;

        }
    }
}
