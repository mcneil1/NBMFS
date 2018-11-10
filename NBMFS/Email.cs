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
    public class Email : Message     //Email inherits header and body from Message
    {

        //URLquarantine is a list for storing all of the URLs that may be included in an email
        public List<string> URLquarantine = new List<string>();

        //AddURL adds a url string to the URLquarantine list  
        public void AddURL(string url)
        {
            URLquarantine.Add(url);
        }

        //EmailValidator checks that the header and sender are correct
        public static bool EmailValidator(string header, string sender)
        {
            //Emailheader returns headers that start with E and are followed by 9 digits
            string Emailheader = @"[E]{1}[0-9]{9}";
            MatchCollection matchHeader = Regex.Matches(header, Emailheader, RegexOptions.IgnoreCase);

            //EmailSender returns an email address
            string EmailSender = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            MatchCollection matchSender = Regex.Matches(sender, EmailSender, RegexOptions.IgnoreCase);

            StringBuilder sb = new StringBuilder();     //initialize new StringBuilder sb
            foreach (var m in matchHeader)
            {
                sb.Append(m + " ");     //add any matching Emailheaders to sb
            }
            foreach (var m in matchSender)
            {
                sb.Append(m + "");     //then add and EmailSenders to sb
            }
            if (sb.ToString() == header + " " + sender)     //if sb == header + sender then the message is an Email
            {
                return true;
            }
            else     //if sb != header + sender then the message is not an Email
            {
                return false;
            }
        }

        //EmailConstructor takes header, sender, subject and body and returns an Email object
        public static Email EmailConstructor(string header, string sender, string subject, string body)
        {
            Email email = new Email();     //make a new Email object 
            //url matches any URLs
            string url = @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?";
            MatchCollection match = Regex.Matches(body, url, RegexOptions.IgnoreCase);

            foreach (var m in match)
            {
                email.AddURL(m.ToString());     //for each URL found add to URLquarantine usind AddURL() 
            }

            Regex rgxUrls = new Regex(url);
            string result = rgxUrls.Replace(body, "<URL Quarantined>");    //replace any urls in body with <URL Quarantined>

            email.Header = header;     //make the email header = header 
            //make email body = sender + subject +  the body with quarantined URLs
            email.Body = sender + "\n" + subject + "\n" + result;     

            return email;    

        }

    }
}
