// Created by Laxale 12.11.2016
//
//


namespace Base.Tests.Helpers 
{
    using System;

    using Freengy.Base.Helpers;

    using NUnit.Framework;


    [TestFixture]
    public class CommonTests 
    {
        [TestCase(null, null, null, Description = "Check method throws exception if any null argument")]
        [TestCase(null, null, "")]
        [TestCase(null, "", 100500)]
        [TestCase(null, "", null)]
        [TestCase(9000, "", null)]
        [TestCase(9000, null, null)]
        public void ThrowIfArgumentsHasNull_AnyNullCases(params object[] arguments) 
        {
            Action testMethod = () => Common.ThrowIfArgumentsHasNull(arguments);

            Assert.Throws<ArgumentNullException>(() => testMethod());
        }

        [TestCase("", "", "", Description = "Check method doesnt throw exception if no any null argument")]
        [TestCase(9000, "", 100500)]
        [TestCase("wow", 1, 2.0)]
        [TestCase(9000, "such", 0xFF)]
        [TestCase(0x55, "hex", 0)]
        public void ThrowIfArgumentsHasNull_NoNullCases(params object[] arguments) 
        {
            Common.ThrowIfArgumentsHasNull(arguments);
        }


        [TestCase(null,  false, Description = "Test null case - get false")]
        [TestCase("",    false, Description = "Test empty case - get false")]
        [TestCase("wow", false, Description = "Test normal case - get false")]
        [TestCase("!such!", false, Description = "Test normal case - get false")]
        [TestCase("\\f",  true,  Description = "Test bad case - get true")]
        [TestCase("*f",  true,  Description = "Test bad case - get true")]
        [TestCase("/testing",  true,  Description = "Test bad case - get true")]
        public void HasInvalidSymbols_Cases(string argument, bool hasBadSymbols)
        {
            bool foundBadSymbols = Common.HasInvalidSymbols(argument);

            Assert.AreEqual(hasBadSymbols, foundBadSymbols);
        }
    }
}