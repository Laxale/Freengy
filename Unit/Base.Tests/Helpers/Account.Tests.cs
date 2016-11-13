// Created by Laxale 13.11.2016
//
//


namespace Base.Tests.Helpers 
{
    using Freengy.Base.Helpers;

    using NUnit.Framework;


    /// <summary>
    /// This test class is a documented requirement for passwords.
    /// Rigth now I dont care about complexity. Just length
    /// </summary>
    [TestFixture]
    public class AccountTests 
    {
        [TestCase(null,         false, Description = "Pass null  password - get false")]
        [TestCase("",           false, Description = "Pass empty password - get false")]
        [TestCase("123",        false, Description = "Pass short password - get false")]
        [TestCase("1 23",       false, Description = "Pass spaceous password - get false")]
        [TestCase("123 ",       false, Description = "Pass spaceous password - get false")]
        [TestCase("!&$*(^!",    false, Description = "Pass symbolish short password - get false")]
        [TestCase("!&$*(^!1ds", true,  Description = "Pass good password  - get true. 10 symbols is minimum length")]
        public void IsGoodPassword_Cases(string password, bool isGoodPassword)
        {
            bool isGoodChecked = Account.IsGoodPassword(password);

            Assert.AreEqual(isGoodPassword, isGoodChecked);
        }

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