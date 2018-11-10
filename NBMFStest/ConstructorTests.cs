using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBMFS;

namespace NBMFStest
{
    [TestClass]
    public class ConstructorTests
    {
        [TestMethod]
        public void SMSconstructorTest()
        {
            string header = "S123456789";
            string sender = "07990123456";
            string body = "lol what's up";

            SMS expectedSMS = new SMS();
            expectedSMS.Header = "S123456789";
            expectedSMS.Body = "07990123456\nlol<Laughing out loud> what's up";

            SMS actualSMS = SMS.SMSconstructor(header, sender, body);

            Assert.AreEqual(expectedSMS.Header, actualSMS.Header);
            Assert.AreEqual(expectedSMS.Body, actualSMS.Body);

        }

        [TestMethod]
        public void TweetConstructorTest()
        {
            string header = "T123456789";
            string sender = "@twitteruser";
            string body = "brb guys";

            Tweet expectedTweet = new Tweet();
            expectedTweet.Header = "T123456789";
            expectedTweet.Body = "@twitteruser\nbrb<Be right back> guys";

            Tweet actualTweet = Tweet.TweetConstructor(header, sender, body);

            Assert.AreEqual(expectedTweet.Header, actualTweet.Header);
            Assert.AreEqual(expectedTweet.Body, actualTweet.Body);
        }

        [TestMethod]
        public void EmailConstructorTest()
        {
            string header = "E123456789";
            string sender = "sender@test.com";
            string subject = "Check out this URL";
            string body = "hey, I thought you'd like this - http://www.google.com";

            Email expectedEmail = new Email();
            expectedEmail.Header = "E123456789";
            expectedEmail.Body = "sender@test.com\nCheck out this URL\nhey, I thought you'd like this - <URL Quarantined>";

            Email actualEmail = Email.EmailConstructor(header, sender, subject, body);

            Assert.AreEqual(expectedEmail.Header, actualEmail.Header);
            Assert.AreEqual(expectedEmail.Body, actualEmail.Body);
        }

        [TestMethod]
        public void SIRconstructorTest()
        {
            string header = "E123456789";
            string sender = "sender@test.com";
            string subject = "SIR 08/11/2018";
            string body = "Sort code: 12-34-56\nNature of Incident: Bomb Threat\nhttps://twitter.com";

            SIR expectedSIR = new SIR();
            expectedSIR.Header = "E123456789";
            expectedSIR.Body = "sender@test.com\nSIR 08/11/2018\nSort code: 12-34-56\nNature of Incident: Bomb Threat\n<URL Quarantined>";

            SIR actualSIR = SIR.SIRConstructor(header, sender, subject, body);

            Assert.AreEqual(expectedSIR.Header, actualSIR.Header);
            Assert.AreEqual(expectedSIR.Body, actualSIR.Body);
        }
    }
}
