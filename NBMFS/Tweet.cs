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
    public class Tweet : Message
    {


        public static bool TweetValidator(string header, string sender)
        {
            string TweetHeader = @"[T]{1}[0-9]{9}";
            MatchCollection matchHeader = Regex.Matches(header, TweetHeader, RegexOptions.IgnoreCase);
            string TweetSender = @"^@?([\w]+)$";
            MatchCollection matchSender = Regex.Matches(sender, TweetSender, RegexOptions.IgnoreCase);

            StringBuilder sb = new StringBuilder();
            foreach (var m in matchHeader)
            {
                sb.Append(m + " ");
            }
            foreach (var m in matchSender)
            {
                sb.Append(m + "");
            }
            if ((sb.ToString() == header + " " + sender) && sb.Length <= 26)
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
