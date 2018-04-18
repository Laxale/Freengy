// Created by Laxale 02.11.2016
//
//


namespace Freengy.Base.Chat.DefaultImpl 
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    
    using Freengy.Base.Chat.Interfaces;


    internal sealed class ChatMessageComparer : IComparer<IChatMessageDecorator> 
    {
        public int Compare(IChatMessageDecorator first, IChatMessageDecorator second) 
        {
            if (first.TimeStamp > second.TimeStamp) return 1;

            if (first.TimeStamp == second.TimeStamp) return 0;

            return -1;
        }
    }


    internal class ChatSession : IChatSession 
    {
        #region vars

        private static readonly object Locker = new object();
        // for unknown reason SortedSet sometimes does not pass add-message->check-message-exists unit test
        //private readonly ISet<IChatMessageDecorator> sessionMessages = new SortedSet<IChatMessageDecorator>(new ChatMessageComparer());
        private readonly IList<IChatMessageDecorator> sessionMessages = new List<IChatMessageDecorator>();

        #endregion vars


        internal ChatSession(Guid id) 
        {
            this.UniqueId = id;
        }


        public Guid UniqueId { get; }
        public string Name { get; internal set; }
        public string DisplayedName { get; internal set; }


        public event EventHandler<IChatMessageDecorator> MessageAdded;


        public IEnumerable<IChatMessageDecorator> GetMessages(Func<IChatMessageDecorator, bool> predicate) 
        {
            lock (ChatSession.Locker)
            {
                IEnumerable<IChatMessageDecorator> result = 
                    predicate == null ? 
                        this.sessionMessages.ToArray() : 
                        this.sessionMessages.Where(predicate);

                return result;
            }
        }
        
        public bool SendMessage(IChatMessage message, out IChatMessageDecorator processedMesage) 
        {
            this.ThrowIfInvalidMessage(message);
            
            processedMesage = new ChatMessageDecorator(message, this);

            lock (ChatSession.Locker)
            {
                this.sessionMessages.Add(processedMesage);
            }

            this.MessageAdded?.Invoke(this, processedMesage);
            
            return true;
        }


        private void ThrowIfInvalidMessage(IChatMessage message) 
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (message.Author == null) throw new ArgumentNullException(nameof(message.Author));
            if (string.IsNullOrWhiteSpace(message.Text)) throw new ArgumentNullException(nameof(message.Text));
        }
    }
}