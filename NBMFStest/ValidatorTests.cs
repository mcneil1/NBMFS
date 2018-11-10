using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NBMFStest
{

    [TestClass]
    public class ValidatorTests
    {
        //SMSvalidatorTests
        #region
        //test that SMSValidator works as expected when given the correct input
        [TestMethod]
        public void SMStest1()
        {
            string header = "S123456789";
            string sender = "07990123456";
            bool expectedResult = true;
            bool actualResult = NBMFS.SMS.SMSValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //test that SMSValidator can catch an incorrect header
        [TestMethod]
        public void SMStest2()
        {
            string header = "S12345678";
            string sender = "07990123456";
            bool expectedResult = false;
            bool actualResult = NBMFS.SMS.SMSValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //test that SMSValidator can catch an incorrect sender
        [TestMethod]
        public void SMStest3()
        {
            string header = "S12345678";
            string sender = "079901234561";
            bool expectedResult = false;
            bool actualResult = NBMFS.SMS.SMSValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }
#endregion

        //TweetValidatorTests
        #region
        //test that TweetValidator works as expected when given the correct input
        [TestMethod]
        public void TweetTest1()
        {
            string header = "T123456789";
            string sender = "@twitteruser";
            bool expectedResult = true;
            bool actualResult = NBMFS.Tweet.TweetValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //test that TweetValidator can catch an incorrect header
        [TestMethod]
        public void TweetTest2()
        {
            string header = "T1234567891";
            string sender = "@twitteruser";
            bool expectedResult = false;
            bool actualResult = NBMFS.Tweet.TweetValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //test that TweetValidator can catch an incorrect sender
        [TestMethod]
        public void TweetTest()
        {
            string header = "T1234567891";
            string sender = "twitteruser";
            bool expectedResult = false;
            bool actualResult = NBMFS.Tweet.TweetValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }
#endregion

        //EmailValidatorTests
        #region
        //test that EmailValidator works as expected when given the correct input
        [TestMethod]
        public void EmailTest1()
        {
            string header = "E123456789";
            string sender = "example@example.com";
            bool expectedResult = true;
            bool actualResult = NBMFS.Email.EmailValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //test that SMSValidator can catch an incorrect header
        [TestMethod]
        public void EmailTest2()
        {
            string header = "E12345678";
            string sender = "example@example.com";
            bool expectedResult = false;
            bool actualResult = NBMFS.Email.EmailValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //test that SMSValidator can catch an incorrect sender
        [TestMethod]
        public void EmailTest3()
        {
            string header = "E12345678";
            string sender = "exampleexample.com";
            bool expectedResult = false;
            bool actualResult = NBMFS.Email.EmailValidator(header, sender);

            Assert.AreEqual(expectedResult, actualResult);
        }
#endregion

        //SIRvalidatorTests
        #region 
        //test that SIRValidator works as expected when given the correct input
        [TestMethod]
        public void SIRTest1()
        {
            string subject = "SIR 01/11/2018";
            string body = "Sort Code: 12-34-56\r\nNature of Incident: Terrorism\r\nhey ho";
            bool expectedResult = true;
            bool actualResult = NBMFS.SIR.SIRValidator(subject, body);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //test that SMSValidator can catch an incorrect header
        [TestMethod]
        public void SIRTest2()
        {
            string subject = "SIR 32/11/2018";
            string body = "Sort Code: 12-34-56\r\nNature of Incident: Terrorism\r\nhey ho";
            bool expectedResult = false;
            bool actualResult = NBMFS.SIR.SIRValidator(subject, body);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //test that SMSValidator can catch an incorrect body
        [TestMethod]
        public void SIRTest3()
        {
            string subject = "SIR 08/11/2018";
            string body = "Sort Code: 12-34-56\r\nNature of Incident: Terrorist\r\nhey ho";
            bool expectedResult = false;
            bool actualResult = NBMFS.SIR.SIRValidator(subject, body);

            Assert.AreEqual(expectedResult, actualResult);
        }
        #endregion

    }
}
