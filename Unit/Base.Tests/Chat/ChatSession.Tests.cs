// Created by Laxale 02.11.2016
//
//


namespace Base.Tests.Chat 
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Freengy.Base.Interfaces;
    using Freengy.Base.Chat.Interfaces;
    using Freengy.Base.Chat.DefaultImpl;
    
    using NUnit.Framework;


    internal class TestMessage : IChatMessage 
    {
        public string Text { get; set; }
        public IUserAccount Author { get; set; }
    }


    internal class TestAccount : IUserAccount 
    {
        public Guid Id { get; internal set; }
        public string Name { get; internal set; }
        public string DisplayedName { get; internal set; }
    }


    [TestFixture]
    public class ChatSessionTests 
    {
        private IChatSession chatSession;

        
        [SetUp]
        public void Setup() 
        {
            this.chatSession = ChatSessionFactory.Instance.CreateInstance("wub", "wubwub");
        }
        

        [Test]
        public void SendMessage_TestNull() 
        {
            Action sendMessageAction =
                () =>
                {
                    IChatMessageDecorator processedMessage;

                    bool sentMessage = this.chatSession.SendMessage(null, out processedMessage);
                };

            Assert.Throws<ArgumentNullException>(() => sendMessageAction());
        }

        [Test]
        public void SendMessage_TestNullAuthor() 
        {
            Action sendMessageAction =
                () =>
                {
                    IChatMessageDecorator processedMessage;

                    var testMessage = new TestMessage 
                    {
                        Author = null
                    };

                    bool sentMessage = this.chatSession.SendMessage(testMessage, out processedMessage);
                };

            Assert.Throws<ArgumentNullException>(() => sendMessageAction());
        }

        [TestCase(null, Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryChat)]
        [TestCase("",   Author = TestAuthors.LaxaleAuthor, Category = TestCategories.CategoryChat)]
        public void SendMessage_TestBadText(string text) 
        {
            Action sendMessageAction =
                () =>
                {
                    IChatMessageDecorator processedMessage;

                    var testMessage = new TestMessage
                    {
                        Author = new TestAccount(),
                        Text = text
                    };

                    bool sentMessage = this.chatSession.SendMessage(testMessage, out processedMessage);
                };

            Assert.Throws<ArgumentNullException>(() => sendMessageAction());
        }

        [Test]
        public void SendMessage_TestNormalMessageIsSent() 
        {
            IChatMessageDecorator processedMessage;

            var testMessage = new TestMessage
            {
                Author = new TestAccount(),
                Text = "wow such text much awesome"
            };

            bool sentMessage = this.chatSession.SendMessage(testMessage, out processedMessage);

            Assert.IsTrue(sentMessage);
        }

        [Test]
        public void SendMessage_TestProcessedMessageNotNull() 
        {
            IChatMessageDecorator processedMessage;

            var testMessage = new TestMessage
            {
                Author = new TestAccount(),
                Text = "wow such text much awesome"
            };

            bool sentMessage = this.chatSession.SendMessage(testMessage, out processedMessage);

            Assert.NotNull(processedMessage);
        }
        
        [Test]
        public void SendMessage_TestSessionsEqual() 
        {
            IChatMessageDecorator processedMessage;

            var testMessage = new TestMessage
            {
                Author = new TestAccount(),
                Text = "wow such text much awesome"
            };

            bool sentMessage = this.chatSession.SendMessage(testMessage, out processedMessage);

            Assert.AreEqual(this.chatSession, processedMessage.ChatSession);
        }

        [Test]
        public void SendMessage_TestProcessedNotEmptyId() 
        {
            IChatMessageDecorator processedMessage;

            var testMessage = new TestMessage
            {
                Author = new TestAccount(),
                Text = "wow such text much awesome"
            };

            bool sentMessage = this.chatSession.SendMessage(testMessage, out processedMessage);

            Assert.AreNotEqual(processedMessage.Id, Guid.Empty);
        }

        [Test]
        public void SendMessage_TestSameMessageAfterProcess() 
        {
            IChatMessageDecorator processedMessage;

            var testMessage = new TestMessage
            {
                Author = new TestAccount(),
                Text = "wow such text much awesome"
            };

            bool sentMessage = this.chatSession.SendMessage(testMessage, out processedMessage);

            Assert.AreEqual(testMessage, processedMessage.OriginalMessage);
        }

        [Test]
        public void SendMessage_TestNotFailTimeStamp() 
        {
            IChatMessageDecorator processedMessage;

            var testMessage = new TestMessage
            {
                Author = new TestAccount(),
                Text = "wow such text much awesome"
            };

            bool sentMessage = this.chatSession.SendMessage(testMessage, out processedMessage);

            bool stampIsOkay =
                processedMessage.TimeStamp < DateTime.Now &&
                processedMessage.TimeStamp != DateTime.MinValue &&
                processedMessage.TimeStamp != DateTime.MaxValue;

            Assert.IsTrue(stampIsOkay);
        }

        [Test]
        public void SendMessage_TestSessionHasNewMessage() 
        {
            IChatMessageDecorator processedMessage;

            var testMessage = new TestMessage
            {
                Author = new TestAccount(),
                Text = "wow such text much awesome"
            };

            bool sentMessage = this.chatSession.SendMessage(testMessage, out processedMessage);

            bool sessionHasNewMessage = 
                this
                .chatSession
                .GetMessages(message => message.Id == processedMessage.Id)
                .First() != null;

            Assert.IsTrue(sessionHasNewMessage);
        }

        [Test]
        public void GetMessages_TestEmtyIfNotSent() 
        {
            bool sessionHasMessages = this.chatSession.GetMessages(null).Any();

            Assert.IsFalse(sessionHasMessages);
        }

        [Test]
        public void GetMessages_TestHasAllMessages() 
        {
            string authorName = "AwesomeAuthor";
            IEnumerable<IChatMessage> testMessages = ChatSessionTests.CreateTestMessages(authorName);

            int sentCount = this.SendTestMessages(testMessages);

            int processedMessagesCount = this.chatSession.GetMessages(message => message.OriginalMessage.Author.Name == authorName).Count();

            Assert.AreEqual(sentCount, processedMessagesCount);
        }


        private int SendTestMessages(IEnumerable<IChatMessage> testMessages) 
        {
            int sentCount = 0;

            foreach (var message in testMessages)
            {
                IChatMessageDecorator decorator;

                bool sentMessage = this.chatSession.SendMessage(message, out decorator);

                sentCount++;
            }

            return sentCount;
        }

        private static IEnumerable<IChatMessage> CreateTestMessages(string authorName) 
        {
            var author = new TestAccount { Name = authorName };

            return new List<IChatMessage>
            {
                new TestMessage
                {
                    Author = author,
                    Text = "wow"
                },

                new TestMessage
                {
                    Author = author,
                    Text = "much"
                },

                new TestMessage
                {
                    Author = author,
                    Text = "tests"
                }
            };
        }
    }
}