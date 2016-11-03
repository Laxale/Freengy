// Created by Laxale 02.11.2016
//
//


namespace Base.Tests.Chat 
{
    using System;
    using System.Linq;

    using Freengy.Base.Chat.Interfaces;
    using Freengy.Base.Chat.DefaultImpl;
    
    using NUnit.Framework;


    [TestFixture]
    public class ChatSessionFactoryTests
    {
        private readonly string name = "wub";
        private readonly string displayedName = "wubwub";

        private IChatSession session;
        private readonly IChatSessionFactory factory = ChatSessionFactory.Instance;


        [SetUp]
        public void Setup() 
        {
            this.session = this.factory.CreateInstance(this.name, this.displayedName);
        }

        [Test]
        public void CreateInstance_TestNotNull() 
        {
            // wow so short
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

        [Test]
        public void CreateInstance_TestNamesEquals() 
        {
            bool areNamesEqual = this.session.Name == this.name && this.session.DisplayedName == this.displayedName;

            Assert.IsTrue(areNamesEqual);
        }
    }
}