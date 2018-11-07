using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
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

        public List<Message> messages = new List<Message>();

        public List<string> SIRlist = new List<string>();

        public void AddSIR(string report)
        {
            SIRlist.Add(report);
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            HeaderTextBox.Text = "";
            SubjectTextBox.Text = "";
            SenderTextBox.Text = "";
            BodyTextBox.Text = "";
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(HeaderTextBox.Text) || string.IsNullOrWhiteSpace(BodyTextBox.Text) || string.IsNullOrWhiteSpace(SenderTextBox.Text))
            {
                MessageBox.Show("Messages must have a sender, header and a body");
                return;
            }




            else if (SMS.SMSValidator(HeaderTextBox.Text, SenderTextBox.Text) && BodyTextBox.Text.Length <= 140)
            {
                SMS sms = new SMS();
                sms.Header = HeaderTextBox.Text;
                sms.Body = SenderTextBox.Text + " " + BodyTextBox.Text;

                string path = @"c:\NBMFS\sms.json";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }


                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SMS));
                using (FileStream fs = new FileStream(path, FileMode.Append))
                {
                    js.WriteObject(fs, sms);
                    MessageBox.Show($"New SMS added to c:\\NBMFS\\sms.json:\n\nHeader: {sms.Header}\nBody: {sms.Body}");
                    fs.Close();
                }


                messages.Add(new Message() { Header = sms.Header, Body = sms.Body });
                Refresh();


            }




            else if (Tweet.TweetValidator(HeaderTextBox.Text, SenderTextBox.Text.ToString()) && BodyTextBox.Text.Length <= 140)
            {
                Tweet tweet = new Tweet();
                tweet.Header = HeaderTextBox.Text;
                tweet.Body = SenderTextBox.Text + " " + BodyTextBox.Text;

                string path = @"c:\NBMFS\tweet.json";
                if (!File.Exists(path))
                {
                    File.Create(path);
                }


                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Tweet));
                using (FileStream fs = new FileStream(path, FileMode.Append))
                {
                    js.WriteObject(fs, tweet);
                    MessageBox.Show($"New Tweet added to c:\\NBMFS\\tweet.json:\n\nHeader: {tweet.Header}\nBody: {tweet.Body}");
                    fs.Close();
                }


                messages.Add(new Message() { Header = tweet.Header, Body = tweet.Body });
                Refresh();

            }




            else if (Email.EmailValidator(HeaderTextBox.Text, SenderTextBox.Text) && BodyTextBox.Text.Length <= 1028)
            {
                if (string.IsNullOrWhiteSpace(SubjectTextBox.ToString()))
                {
                    MessageBox.Show("Emails must contain a subject");
                    return;
                }





                if (SIR.SIRValidator(SubjectTextBox.Text.ToString(), BodyTextBox.Text) == true)
                {
                    SIR sir = SIR.SIRConstructor(HeaderTextBox.Text, SenderTextBox.Text, SubjectTextBox.Text, BodyTextBox.Text);

                    string path = @"c:\NBMFS\sir.json";
                    if (!File.Exists(path))
                    {
                        File.Create(path);
                    }



                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SIR));
                    using (FileStream fs = new FileStream(path, FileMode.Append))
                    {
                        js.WriteObject(fs, sir);
                        MessageBox.Show($"New SIR added to c:\\NBMFS\\sir.json:\n\nHeader: {sir.Header}\nBody: {sir.Body}");
                        fs.Close();
                    }

                    messages.Add(new Message() { Header = sir.Header, Body = sir.Body });
                    Refresh();

                }





                else
                {
                    Email email = Email.EmailConstructor(HeaderTextBox.Text, SenderTextBox.Text, SubjectTextBox.Text, BodyTextBox.Text);

                    string path = @"c:\NBMFS\email.json";
                    if (!File.Exists(path))
                    {
                        File.Create(path);
                    }


                    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(Email));
                    using (FileStream fs = new FileStream(path, FileMode.Append))
                    {
                        js.WriteObject(fs, email);
                        MessageBox.Show($"New Email added to c:\\NBMFS\\email.json:\n\nHeader: {email.Header}\nBody: {email.Body}");
                        fs.Close();
                    }


                    messages.Add(new Message() { Header = email.Header, Body = email.Body });
                    Refresh();
                }
            }


            else
            {
                MessageBox.Show("Message format invalid");
                return;
            }

        }

        private void Refresh()
        {
            MessageList.Items.Clear();
            for(int i = 0; i < messages.Count; i++)
            {
                MessageList.Items.Add(messages[i]);
            }

        }
    }

}
