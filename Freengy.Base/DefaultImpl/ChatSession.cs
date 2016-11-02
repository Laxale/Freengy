// Created by Laxale 02.11.2016
//
//


namespace Freengy.Base.DefaultImpl 
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Freengy.Base.Helpers;
    using Freengy.Base.Interfaces;
    

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
        private static readonly object Locker = new object();
        private readonly ISet<IChatMessageDecorator> sessionMessages = new SortedSet<IChatMessageDecorator>(new ChatMessageComparer());


        internal ChatSession(Guid id) 
        {
            this.Id = id;
        }


        public Guid Id { get; }


        public IEnumerable<IChatMessageDecorator> GetMessages(Func<IChatMessageDecorator, bool> predicate) 
        {
            lock (Locker)
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

            lock (Locker)
            {
                this.sessionMessages.Add(processedMesage);
            }
//            
//            System
//                .IO
//                .File
//                .AppendAllText
//                (
//                    @"D:\ChatSession.txt", 
//                    $"[{System.Threading.Thread.CurrentThread.ManagedThreadId}] Added message; author = '{message.Author.Name}'\n"
//                );

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