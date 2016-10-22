// Created by Laxale 21.10.2016
//
//


namespace Base.Tests.Extensions 
{
    using System;

    using Freengy.Base.Extensions;

    using NUnit.Framework;


    [TestFixture]
    public class ExceptionExtensionsTests 
    {
        [Test(Author = TestAuthors.LaxaleAuthor, Description = "Pass in null exception - get ArgumentNullException")]
        public void GetReallyRootException_NullCase() 
        {
            // Arrange
            Exception ex = null;

            // Act

            TestDelegate testDelegate =
                () =>
                {
                    Exception rootEx = ex.GetReallyRootException();
                };
            

            // Assert
            Assert.Throws<ArgumentNullException>(testDelegate);
        }

        [Test(Author = TestAuthors.LaxaleAuthor, Description = "Pass in one exception - get itself")]
        public void GetReallyRootException_OneExCase() 
        {
            // Arrange
            Exception ex = new DivideByZeroException();

            // Act
            Exception rootEx = ex.GetReallyRootException();

            // Assert
            Assert.IsInstanceOf<DivideByZeroException>(rootEx);
        }

        [Test(Author = TestAuthors.LaxaleAuthor, Description = "Pass in two exceptions - get root")]
        public void GetReallyRootException_TwoExCase() 
        {
            // Arrange
            Exception ex = new DivideByZeroException(null, new DllNotFoundException());

            // Act
            Exception rootEx = ex.GetReallyRootException();
            
            // Assert
            Assert.IsInstanceOf<DllNotFoundException>(rootEx);
        }

        [Test(Author = TestAuthors.LaxaleAuthor, Description = "Pass in three exceptions - get root")]
        public void GetReallyRootException_ThreeExCase() 
        {
            // Arrange
            Exception ex = new DivideByZeroException(null, new DllNotFoundException(null, new FieldAccessException()));

            // Act
            Exception rootEx = ex.GetReallyRootException();

            // Assert
            Assert.IsInstanceOf<FieldAccessException>(rootEx);
        }
    }
}