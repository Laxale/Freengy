// Created by Laxale 02.11.2016
//
//


namespace Base.Tests.DefaultImpl 
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    using Freengy.Base.Interfaces;
    using Freengy.Base.DefaultImpl;


    [TestFixture]
    public class ChatSessionFactoryTests 
    {
        private IChatSession session;
        private readonly IChatSessionFactory factory = ChatSessionFactory.Instance;


        [SetUp]
        public void Setup()
        {
            this.session = this.factory.CreateInstance();
        }

        [Test]
        public void CreateInstance_TestNotNull() 
        {
            Assert.NotNull(this.session);
        }

        [Test]
        public void CreateInstance_TestNotEmptyId() 
        {
            Assert.AreNotEqual(this.session.Id, Guid.Empty);
        }

        [Test]
        public void CreateInstance_TestNoMessages() 
        {
            var allMessages = this.session.GetMessages(null);

            Assert.IsFalse(allMessages.Any());
        }

    }
}