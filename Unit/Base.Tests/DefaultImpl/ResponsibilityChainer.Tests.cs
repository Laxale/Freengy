// Created by Laxale 24.10.2016
//
//


namespace Base.Tests.DefaultImpl 
{
    using System;

    using NUnit.Framework;

    using Freengy.Base.Interfaces;
    using Freengy.Base.DefaultImpl;


    internal class TestObject 
    {
        
    }

    [TestFixture]
    public class ResponsibilityChainerTests
    {
        private IResponsibilityChainer<TestObject> testChain;

        [SetUp]
        public void Setup() 
        {
            this.testChain = new ResponsibilityChainer<TestObject>();
        }


        [Test(Description = "Add null handler, get exception")]
        public void AddHandler_NullCase() 
        {
            Assert.Throws<ArgumentNullException>(() => this.testChain.AddHandler(null));
        }

        [Test(Description = "Add handler, not crashing")]
        public void AddHandler_NormalCase() 
        {
            this.testChain.AddHandler(arg => true);
        }

        [Test(Description = "Add throwing handler, not crashing")]
        public void AddHandler_ThrowingCase() 
        {
            this.testChain.AddHandler(arg => { throw new ArgumentException(); });
        }

        [Test(Description = "Handle null, get exception")]
        public void HandleAsync_NullCase()
        {
            this.testChain.AddHandler(arg => true);

            Assert.Throws<ArgumentNullException>(() => this.testChain.HandleAsync(null));
        }

        [TestCase(true,  Description = "Add one handling handler - object is handled")]
        [TestCase(false, Description = "Add not handling handlers - object is not handled")]
        public void HandleAsync_NormalCase(bool atLeastOneHandling) 
        {
            var testObject = new TestObject();

            this.testChain.AddHandler(arg => false);
            this.testChain.AddHandler(arg => false);
            this.testChain.AddHandler(arg => atLeastOneHandling);
            this.testChain.AddHandler(arg => false);

            bool handled = this.testChain.HandleAsync(testObject).Result;

            Assert.True(handled == atLeastOneHandling);
        }
    }
}