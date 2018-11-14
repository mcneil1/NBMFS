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
    public class SMS : Message     //SMS inherits it's attributes from Message 
    {


        //SMSValidator checks that the header and sender are in the correct formats
        public static bool SMSValidator(string header, string sender)
        {
            //SMSheader returns headers that start with S and are followed by 9 digits
            string SMSheader = @"^[S]{1}[0-9]{9}";
            MatchCollection matchHeader = Regex.Matches(header, SMSheader, RegexOptions.IgnoreCase);
            //SMSsender returns 11 digits (a UK mobile phone number)
            string SMSsender = @"^([0-9]+)";
            MatchCollection matchSender = Regex.Matches(sender, SMSsender, RegexOptions.IgnoreCase);

            //initialize a new string builder 
            StringBuilder sb = new StringBuilder();
            foreach (var m in matchHeader)
            {
                sb.Append(m + " ");     //add SMSheader match to string builder
            }
            foreach (var m in matchSender)
            {
                sb.Append(m + "");     //then add SMSsender match to string builder
            }

            //if the new string sb is equal to header + sender and is equal to 22 characters the message is an SMS
            if ((sb.ToString() == header + " " + sender) && sb.Length == 22)
            {
                return true;     
            }
            //if the string is over 22 characters or not equal to header + sender the message is not SMS
            else     
            {
                return false;
            }
        }

        //SMSconstructor takes the header, sender and body and returns a SMS object
        public static SMS SMSconstructor(string header, string sender, string body)
        {
            //make a new SMS
            SMS sms = new SMS();

            //this returns the current path which we can use to access the textwords.csv file
            string thisPath = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));

            //use StreamReader to read from textwords.csv
            StreamReader fileReader = new StreamReader(File.OpenRead(thisPath + "\\textwords.csv"));

            //while the reader is not finished
            while (!fileReader.EndOfStream)
            {
                string[] cvsWords = fileReader.ReadLine().Split(',');    //cvsWords is an array of all words in textwords.cvs
                string slang = @"\b" + cvsWords[0] + @"\b";     //string slang is equal to a word in the first column 

                body = Regex.Replace(body, slang, delegate (Match m) //in body add expanded textspeak next to the slang
                      {
                          return m.Value + "<" + cvsWords[1] + ">";

                      }, RegexOptions.IgnoreCase);
            }

            sms.Header = header;     //make the SMS header = header
            sms.Body = sender + "\n" + body;     //make ths SMS body = sender + body (with expanded textspeak)
            return sms;     
        }
    }
}
