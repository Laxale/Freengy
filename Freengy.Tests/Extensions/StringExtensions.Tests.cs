// Created 21.10.2016
//
//


namespace Base.Tests.Extensions 
{
    using Freengy.Base.Extensions;

    using NUnit.Framework;


    [TestFixture]
    public class StringExtensionsTests 
    {
        [TestCase(null,         Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase, 
                                ExpectedResult = null, Description = "Pass null - get null")]

        [TestCase("",           Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase,
                                ExpectedResult = "", Description = "Pass epmty - get empty")]

        [TestCase("tp",         Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase,
                                ExpectedResult = "tp", Description = "Pass no special symbols - get string back")]

        [TestCase("t\np",       Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase,
                                ExpectedResult = "tp", Description = "Pass single newline - get filtered string")]

        [TestCase("\nt\rp",     Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase,
                                ExpectedResult = "tp", Description = "Pass two newlines - get filtered string")]

        [TestCase("\nt\r\rp\n", Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryBase,
                                ExpectedResult = "tp", Description = "Pass holy bunch of newlines - get filtered string")]
        public string FilterNewLineSymbols_Cases(string inputString) 
        {
            // Assert

            // Act
            string result = inputString.FilterNewLineSymbols();

            // Assert
            return result;
        }
    }
}