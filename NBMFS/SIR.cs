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
    [DataContract]
    public class SIR : Email     //SIR inherits body and header from Email (which inherits from Messages) 
    {

        public string Code { get; set; }     //Code is used to store any sort-codes (no need to be serialized)

        public string Incident { get; set; }     //Incident stores the nature of incident (no need to be serialized)

        //SIRValidator checks that the subject and body are in the correct format 
        public static bool SIRValidator(string subject,string body)
        {
            //SIR matches the letters "SIR" followed by a date
            string SIR = @"(SIR[\s])(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)\d\d$";
            MatchCollection matchSubject = Regex.Matches(subject, SIR, RegexOptions.IgnoreCase);

            //sortcode matches "Sort Code: " plus a sort code (dd-dd-dd) where d are digits 
            string sortcode = @"(Sort Code:(\s?)([0-9]{2}[-][0-9]{2}[-][0-9]{2}))";
            MatchCollection matchCode = Regex.Matches(body, sortcode, RegexOptions.IgnoreCase);
            //incident matches "Nature of Incident: " plus any of the valid incident types
            string incident = @"(Nature of Incident:(\s ?)(Theft|Staff Attack|ATM Theft|Raid|Customer Attack|Staff Abuse|Bomb Threat|Terrorism|Suspicious Incident|Intelligence|Cash Loss))";
            MatchCollection matchIncident = Regex.Matches(body, incident, RegexOptions.IgnoreCase);

            //split the body into lines, sort code should be line[0] and incident should be line[1]
            string[] splitBody = new string[] { "\r\n" };
            string[] lines = body.Split(splitBody, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder sb = new StringBuilder();     //initialize new StringBuilder sb
            foreach (var m in matchSubject)
            {
                sb.Append(m + "\r\n");      //add matching subject and new line to sb 
            }
            foreach (var m in matchCode)
            {
                sb.Append(m + "\r\n");     //add matching sort code and new line to sb
            }
            foreach (var m in matchIncident)
            {
                sb.Append(m + "");     //add matching incident to sb
            }

            
            if (sb.ToString().Contains(subject) && sb.ToString().Contains(lines[0]) && sb.ToString().Contains(lines[1]))
            {
                return true;     //if sb contains the subject, lines[0] (sort code) and lines[1] (incident) then message is a SIR
            }
            else
            {
                return false;     //if sb doesn't contain all these things then message is not a SIR
            }


        }


        //SIRConstructor takes a header, sender, subject and body and returns a SIR object
        public static SIR SIRConstructor(string header, string sender, string subject, string body)
        {
            SIR sir = new SIR();     //make new SIR object

            string[] splitBody = new string[] { "\n" };     //split body into lines
            string[] lines = body.Split(splitBody, StringSplitOptions.RemoveEmptyEntries);     //lines[0] = sortcode, lines[1] = incident

            //url matches any URLS in body 
            string url = @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
            MatchCollection match = Regex.Matches(body, url, RegexOptions.IgnoreCase);

            foreach (var m in match)
            {
                URLquarantine.Add(m.ToString());     //for any matched urls add url to URLquarantine using AddURL() 
            }

            Regex rgxUrls = new Regex(url);
            string result = rgxUrls.Replace(body, "<URL Quarantined>");     //replace any urls in body with <URL Quarantined>

            sir.Header = header;     //make SIR header = header
            sir.Body = sender + "\n" + subject + "\n" + result;     //make body = sender + subject + body with quarantined URLs

            string sirIncident = (lines[1]).ToString().Substring(20);     //sirIncident = lines[1] starting from the 20th character 
            string sirCode = (lines[0]).ToString().Substring(10);     //sirCode = lines[0] starting from the 10th character
            sir.Code = sirCode;     //SIR code = sirCode
            sir.Incident = sirIncident;     //SIR incident = sirIncident

            return sir;      

        }
    }
}
