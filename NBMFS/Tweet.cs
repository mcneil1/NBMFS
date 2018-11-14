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
    public class Tweet : Message     //Tweet inherits header and body from Message
    {

        public string Mention { get; set; }     //Mention is used to store any mentions in the tweet body (no need to serialize)

        //TweetValidator checks a header and sender are in the correct format
        public static bool TweetValidator(string header, string sender)
        {
            string TweetHeader = @"[T]{1}[0-9]{9}";     //TweetHeader matches "T" + 9 digits 
            MatchCollection matchHeader = Regex.Matches(header, TweetHeader, RegexOptions.IgnoreCase);
            string TweetSender = @"^@?([\w]+)${1,15}";     //TweetSender matches a twitter username 
            MatchCollection matchSender = Regex.Matches(sender, TweetSender, RegexOptions.IgnoreCase);

            StringBuilder sb = new StringBuilder();     //initialize a StringBuilder sb
            foreach (var m in matchHeader)
            {
                sb.Append(m + " ");     //add matched header to sb
            }
            foreach (var m in matchSender)
            {
                sb.Append(m + "");     //add matched sender to sb
            }
            if ((sb.ToString() == header + " " + sender) && sb.Length <= 26)
            {
                return true;     //if sb == header + sender and is under the max characters of 26 then message is Tweet
            }
            else
            {
                return false;     //if sb != header + sender or is greater than 26 characters then message is not Tweet
            }
        }


        //TweetConstructor takes a header, sender and body and returns a Tweet object
        public static Tweet TweetConstructor(string header, string sender, string body)
        {
            Tweet tweet = new Tweet();     //make new Tweet object

            //this returns the current path which we can use to access the textwords.csv file
            string thisPath = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));

            //use StreamReader to read from textwords.csv
            StreamReader fileReader = new StreamReader(File.OpenRead(thisPath + "\\textwords.csv"));

            while (!fileReader.EndOfStream)     //while the StreamReader is not at the end of file
            {
                string[] cvsWords = fileReader.ReadLine().Split(',');     //array cvsWords separates words with commas 
                string slang = @"\b" + cvsWords[0] + @"\b";     //slang is equal to the textspeak

                body = Regex.Replace(body, slang, delegate (Match m)     //if any word in body matches slang
                {
                    return m.Value + "<" + cvsWords[1] + ">";     //add the expanded textspeak next to the slang in between < >

                }, RegexOptions.IgnoreCase);
            }


            tweet.Header = header;     //make tweet header = header
            tweet.Body = sender + "\n" + body;     //make tweet body = sender + body (with expanded textspeak)
            return tweet;
        }

    }
}
