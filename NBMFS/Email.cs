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
    public class Email : Message
    {


        public List<string> URLquarantine = new List<string>();

        public void AddURL(string url)
        {
            URLquarantine.Add(url);
        }

        public static bool EmailValidator(string header, string sender)
        {
            string Emailheader = @"[E]{1}[0-9]{9}";
            MatchCollection matchHeader = Regex.Matches(header, Emailheader, RegexOptions.IgnoreCase);

            string EmailSender = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            MatchCollection matchSender = Regex.Matches(sender, EmailSender, RegexOptions.IgnoreCase);

            StringBuilder sb = new StringBuilder();
            foreach (var m in matchHeader)
            {
                sb.Append(m + " ");
            }
            foreach (var m in matchSender)
            {
                sb.Append(m + "");
            }
            if (sb.ToString() == header + " " + sender)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Email EmailConstructor(string header, string sender, string subject, string body)
        {
            Email email = new Email();
            string url = @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
            MatchCollection match = Regex.Matches(body, url, RegexOptions.IgnoreCase);

            foreach (var m in match)
            {
                email.AddURL(m.ToString());
            }

            Regex rgxUrls = new Regex(url);
            string result = rgxUrls.Replace(body, "<URL Quarantined>");

            email.Header = header;
            email.Body = sender + " " + subject + " " + result;

            return email;

        }

    }
}
