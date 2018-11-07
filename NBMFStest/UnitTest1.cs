using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NBMFStest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SMStest1()
        {
            string header = "S123456789";
            string sender = "07990123456";
            bool expectedResult = true;
            bool actualResult = NBMFS.SMS.SMSValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void TweetTest1()
        {
            string header = "T123456789";
            string sender = "@twitteruser";
            bool expectedResult = true;
            bool actualResult = NBMFS.Tweet.TweetValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void EmailTest1()
        {
            string header = "E123456789";
            string sender = "example@example.com";
            bool expectedResult = true;
            bool actualResult = NBMFS.Email.EmailValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }

        [TestMethod]
        public void SIRTest1()
        {
            string subject = "SIR 01/11/2018";
            string body = "Sort Code: 12-34-56\nNature of Incident: Terrorism\nhey ho";
            bool expectedResult = true;
            bool actualResult = NBMFS.SIR.SIRValidator(subject, body);

            Assert.AreEqual(expectedResult, actualResult);
        }


    }
}
