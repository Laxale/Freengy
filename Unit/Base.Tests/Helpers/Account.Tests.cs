// Created by Laxale 13.11.2016
//
//

using Freengy.Base.Helpers;

using NUnit.Framework;


namespace Base.Tests.Helpers 
{
    /// <summary>
    /// This test class is a documented requirement for passwords.
    /// Rigth now I dont care about complexity. Just length
    /// </summary>
    [TestFixture]
    public class AccountTests 
    {
        [TestCase("much@good.pass", true,  Description = "Pass valid email - get true")]
        [TestCase(null,             false, Description = "Pass null email  - get false")]
        [TestCase("",               false, Description = "Pass empty email - get false")]
        [TestCase("@",              false, Description = "Pass wrong email - get false")]
        [TestCase(" @",             false, Description = "Pass wrong email - get false")]
        [TestCase(" @ wow",         false, Description = "Pass wrong email - get false")]
        [TestCase("such @ wrong",   false, Description = "Pass wrong email - get false")]
        public void IsValidEmail_Cases(string email, bool isValidEmail) 
        {
            bool isValidChecked = Account.IsValidEmail(email);

            Assert.AreEqual(isValidEmail, isValidChecked);
        }
    }
}