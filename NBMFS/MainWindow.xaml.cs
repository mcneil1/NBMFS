using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NBMFS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        //use lists to store values that need to be printed to listviews
        //messages -> MessageList
        public List<Message> messages = new List<Message>();

        //Reportlist -> SIRlist
        public List<SIR> Reportlist = new List<SIR>();

        //hashtagList -> TrendingList
        public IDictionary<string, int> hashtagList = new Dictionary<string, int>();

        //mentions -> MentionsList
        public List<Tweet> mentions = new List<Tweet>();

        //click clear button to set all textboxes to an empty string
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        { 
      
            HeaderTextBox.Text = string.Empty;
            SubjectTextBox.Text = string.Empty;
            SenderTextBox.Text = string.Empty;
            BodyTextBox.Text = string.Empty;
        }

        //when send is clicked the inputted textboxes are validated before they can be categorized
        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {

            //if the header, sender or body textboxes are empty or white space an error
            //is shown and the message is not put into the system
            if (string.IsNullOrWhiteSpace(HeaderTextBox.Text) || string.IsNullOrWhiteSpace(BodyTextBox.Text) || string.IsNullOrWhiteSpace(SenderTextBox.Text))
            {
                MessageBox.Show("Messages must have a sender, header and a body");
                return;
            }



            //if SMSValidator returns true and the body is <= 140 characters then then message is an SMS 
            else if (SMS.SMSValidator(HeaderTextBox.Text, SenderTextBox.Text) && BodyTextBox.Text.Length <= 140)
            {
                //use SMSconstructor to make the message a SMS object
                SMS sms = SMS.SMSconstructor(HeaderTextBox.Text, SenderTextBox.Text, BodyTextBox.Text);

                //if the file path does not exist then create it
                string path = @"c:\NBMFS\sms.json";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }

                //DataContractJsonSerializer allows us to convert the SMS to JSON
                //use FileStream to write the JSON object to the path
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SMS));
                using (FileStream fs = new FileStream(path, FileMode.Append))
                { 
                    js.WriteObject(fs, sms);
                    //let the user know the SMS has been serialized and written to a file
                    MessageBox.Show($"New SMS added to c:\\NBMFS\\sms.json:\n\nHeader: {sms.Header}\nBody: {sms.Body}");
                    //close FileStream
                    fs.Close();
                }

                //add the message to messages so it may be added to MessageList
                messages.Add(new Message() { Header = sms.Header, Body = sms.Body });

                //Refresh the listViews to add new messages 
                Refresh();


            }



            //if TweetValidator returns true and the body is <= 140 characters then then message is a Tweet
            else if (Tweet.TweetValidator(HeaderTextBox.Text, SenderTextBox.Text.ToString()) && BodyTextBox.Text.Length <= 140)
            {
                //use TweetConstructor to turn the message into a Tweet object
                Tweet tweet = Tweet.TweetConstructor(HeaderTextBox.Text, SenderTextBox.Text, BodyTextBox.Text);

                //hashtagRegex returns any hashtags in the tweet body
                string hashtagRegex = @"#[A-z0-9-_]+";
                MatchCollection matchHashtags = Regex.Matches(tweet.Body, hashtagRegex, RegexOptions.IgnoreCase);

                //foreach hashtag found in the tweet body
                foreach (var m in matchHashtags)
                {
                    //if hashtag is already in hashtagList increase value by one
                    if (hashtagList.ContainsKey(m.ToString()))
                    {
                        hashtagList[m.ToString()]++;
                    }
                    //if hashtag is not in hashtagList add it to list and make value = 1
                    else
                    {
                        hashtagList.Add(m.ToString(), 1);
                    }
                }

                //mentionRegex returns any twitter usernames
                string mentionRegex = @"@[A-z0-9_]{1,15}";

                //split body into lines and loop through all lines except the first
                //as the first line is the sender's twitter handle
                string[] splitBody = new string[] { "\n" };
                string[] lines = tweet.Body.Split(splitBody, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 1; i < lines.Length; i++)
                {
                    MatchCollection matchMentions = Regex.Matches(lines[i], mentionRegex, RegexOptions.IgnoreCase);
                    foreach (var m in matchMentions)
                    {
                        //for each mention check if they are already included in mentions
                        //if not add the mention to the list 
                        if (mentions.Contains(m) == false)
                        {
                            mentions.Add(new Tweet() { Mention = m.ToString()});
                        }
                    }
                }

                //if file path does not exist then create it 
                string path = @"c:\NBMFS\tweet.json";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }

                //DataContractJsonSerializer allows us to convert the Tweet to JSON
                //use FileStream to write the JSON object to the path
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Tweet));
                using (FileStream fs = new FileStream(path, FileMode.Append))
                {
                    js.WriteObject(fs, tweet);
                    //let user know message has been serialized and added to file and close FileStream
                    MessageBox.Show($"New Tweet added to c:\\NBMFS\\tweet.json:\n\nHeader: {tweet.Header}\nBody: {tweet.Body}");
                    fs.Close();
                }

                //add mesage to messages list
                messages.Add(new Message() { Header = tweet.Header, Body = tweet.Body });

                //use refresh to update listViews
                Refresh();
            }





            //if EmailValidator returns true and the body is <= 1028 characters then then message is an Email
            else if (Email.EmailValidator(HeaderTextBox.Text, SenderTextBox.Text) && BodyTextBox.Text.Length <= 1028)
            {
                //Email must have a subject, if SubjectTextBox is empty return  
                if (string.IsNullOrWhiteSpace(SubjectTextBox.ToString()))
                {
                    MessageBox.Show("Emails must contain a subject");
                    return;
                }




                //if SIRValidator returns true then the Email is a serious incident report
                if (SIR.SIRValidator(SubjectTextBox.Text.ToString(), BodyTextBox.Text) == true)
                {
                    //use SIRConstructor to create a SIR Object from the message 
                    SIR sir = SIR.SIRConstructor(HeaderTextBox.Text, SenderTextBox.Text, SubjectTextBox.Text, BodyTextBox.Text);

                    //create file path if it does not exist
                    string path = @"c:\NBMFS\sir.json";
                    if (!File.Exists(path))
                    {
                        File.Create(path);
                    }


                    //DataContractJsonSerializer will turn our SIR object into a JSON object
                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SIR));
                    using (FileStream fs = new FileStream(path, FileMode.Append))
                    {
                        //write JSON object to the file path using FileStream, let users know this has happened
                        //then close FileStream
                        js.WriteObject(fs, sir);
                        MessageBox.Show($"New SIR added to c:\\NBMFS\\sir.json:\n\nHeader: {sir.Header}\nBody: {sir.Body}");
                        fs.Close();
                    }

                    //add the message to messages, add the SIR code and incident to ReportList
                    messages.Add(new Message() { Header = sir.Header, Body = sir.Body });
                    Reportlist.Add(new SIR() { Code = sir.Code, Incident = sir.Incident });

                    //update lists
                    Refresh();


                }







                //if SIRValidator is false then the Email is just a normal email
                else
                {
                    //make an Email object using EmailConstructor 
                    Email email = Email.EmailConstructor(HeaderTextBox.Text, SenderTextBox.Text, SubjectTextBox.Text, BodyTextBox.Text);

                    //create path if it doesn't exist
                    string path = @"c:\NBMFS\email.json";
                    if (!File.Exists(path))
                    {
                        File.Create(path);
                    }

                    //use DataContractJsonSerializer to turn Email into JSON object 
                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Email));
                    using (FileStream fs = new FileStream(path, FileMode.Append))
                    {
                        //write JSON object to file using FileStream, let users know the message has been written to file
                        //then close FileStream
                        js.WriteObject(fs, email);
                        MessageBox.Show($"New Email added to c:\\NBMFS\\email.json:\n\nHeader: {email.Header}\nBody: {email.Body}");
                        fs.Close();
                    }

                    //add email to messages 
                    messages.Add(new Message() { Header = email.Header, Body = email.Body });

                    //update listViews
                    Refresh();

                }
            }




            //if no validators return true then the message cannot be categorized
            else
            {
                //let user know that the message is invalid
                MessageBox.Show("Message format invalid");
                return;
            }

        }




        //Refresh() updates the ListViews everytime a message is added
        private void Refresh()
        {
            //first start by clearing all items from all listViews
            SIRlist.Items.Clear();
            MessageList.Items.Clear();
            MentionList.Items.Clear();
            TrendingList.Items.Clear();

            //then for each message in each list add the message to its respective listView
            for (int i = 0; i < messages.Count; i++)
            {
                MessageList.Items.Add(messages[i]);
            }

            for (int i = 0; i < Reportlist.Count; i++)
            {
                SIRlist.Items.Add(Reportlist[i]);
            }

            for (int i = 0; i < mentions.Count; i++)
            {
                MentionList.Items.Add(mentions[i].Mention);
            }

            //make hashtagList order by decending so the hashtag with the most mentions is at the top and the least at the bottom
            IEnumerable<KeyValuePair<string, int>> result = hashtagList.OrderByDescending(i => i.Value);
            foreach (KeyValuePair<string, int> kvp in result)
            {
                TrendingList.Items.Add(kvp.Key);
            }
        }

        //the following buttons make thier respective listView visable and all others hidden
        //this way users can choose which list they want to view 
        private void TrendingBtn_Click(object sender, RoutedEventArgs e)
        {
            MentionList.Visibility = Visibility.Hidden;
            MessageList.Visibility = Visibility.Hidden;
            SIRlist.Visibility = Visibility.Hidden;
            TrendingList.Visibility = Visibility.Visible;
        }

        private void MessageBtn_Click(object sender, RoutedEventArgs e)
        {
            MentionList.Visibility = Visibility.Hidden;
            TrendingList.Visibility = Visibility.Hidden;
            SIRlist.Visibility = Visibility.Hidden;
            MessageList.Visibility = Visibility.Visible;
        }

        private void sirBtn_Click(object sender, RoutedEventArgs e)
        {
            MentionList.Visibility = Visibility.Hidden;
            MessageList.Visibility = Visibility.Hidden;
            TrendingList.Visibility = Visibility.Hidden;
            SIRlist.Visibility = Visibility.Visible;
        }

        private void MentionBtn_Click(object sender, RoutedEventArgs e)
        {
            TrendingList.Visibility = Visibility.Hidden;
            MessageList.Visibility = Visibility.Hidden;
            SIRlist.Visibility = Visibility.Hidden;
            MentionList.Visibility = Visibility.Visible;
        }
    }

}
