// Created by Laxale 03.11.2016
//
//

using System;
using System.Linq;
using System.Collections.Generic;

using Freengy.Base.Interfaces;
using Freengy.Base.Chat.Interfaces;
using Freengy.Base.Chat.DefaultImpl;
using Freengy.Common.Models;

using NUnit.Framework;

using Res = Freengy.CommonResources.StringResources;


namespace Base.Tests.Chat 
{
    [TestFixture]
    public class ChatMessageFactoryTests 
    {
        private IChatMessageFactory messageFactory;
        private readonly UserAccount author = new UserAccount(new UserAccountModel { Name = "Awesome Author", UniqueId = Guid.NewGuid() });


        [SetUp]
        public void Setup() 
        {
            this.messageFactory = new ChatMessageFactory(this.author);
        }

        [Test]
        public void Ctor_TestThrowsIfNullAuthor() 
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var factory = new ChatMessageFactory(null);
            });
        }

        [Test]
        public void Author_TestThrowsIfNullAuthor() 
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                this.messageFactory.Author = null;
            });
           
        }

        [Test]
        public void Author_TestEqualsToArgument()
        {
            var newAuthor = new UserAccount(new UserAccountModel { Name = "test name" });
            this.messageFactory.Author = newAuthor;

            Assert.AreEqual(this.messageFactory.Author, newAuthor);
        }

        [TestCase("",   Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryChat)]
        [TestCase(null, Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryChat)]
        public void CreateMessage_TestEmptyTextConversion(string text) 
        {
            var message = this.messageFactory.CreateMessage(text);

            Assert.AreEqual(Res.EmptyPlaceHolder, message.Text);
        }
    }
}