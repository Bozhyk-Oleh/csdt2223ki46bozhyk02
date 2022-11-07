using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Client.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DataRow("abcdefghij")] // 10 letters
        public void TestMessageLimit_Good(string i)//expected input  
        {
            string excpectedResult = i;

            var check = new CheckInput();
            string actualResult = check.checkmessage(i);

            Assert.AreEqual(excpectedResult, actualResult);
        }

        [TestMethod]
        [DataRow("abcdefghijabcdefghijabcdefghija")] //unexpected input(message limit) 
        public void TestMessageLimit_Error(string i)
        {
            string excpectedResult = "Message must be less than 31 characters";

            var check = new CheckInput();
            string actualResult = check.checkmessage(i);

            Assert.AreEqual(excpectedResult, actualResult);
        }

        [TestMethod]
        [DataRow("helloo")]
        public void TestMessageIsLatin_Good(string i)//expected input
        {
            string excpectedResult = i;

            var check = new CheckInput();
            string actualResult = check.checkmessage(i);

            Assert.AreEqual(excpectedResult, actualResult);
        }

        [TestMethod]
        [DataRow("hellфo")]
        public void TestMessageIsLatinOrNum_Error(string i)//unexpected characters
        {
            string excpectedResult = "Message must contain only latin characters";

            var check = new CheckInput();
            string actualResult = check.checkmessage(i);

            Assert.AreEqual(excpectedResult, actualResult);
        }
    }
}
