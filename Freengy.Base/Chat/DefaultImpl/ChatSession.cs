// Created by Laxale 02.11.2016
//
//

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

using Freengy.Base.Interfaces;
using Freengy.Base.Chat.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Base.Models.Readonly;


namespace Freengy.Base.Chat.DefaultImpl 
{
    internal sealed class ChatMessageComparer : IComparer<IChatMessageDecorator> 
    {
        public int Compare(IChatMessageDecorator first, IChatMessageDecorator second) 
        {
            if (first.TimeStamp > second.TimeStamp) return 1;

            if (first.TimeStamp == second.TimeStamp) return 0;

            return -1;
        }
    }


    /// <summary>
    /// <see cref="IChatSession"/> implementer.
    /// </summary>
    internal class ChatSession : IChatSession 
    {
        private static readonly object Locker = new object();
        private static readonly IGuiDispatcher guiDispatcher = MyServiceLocator.Instance.Resolve<IGuiDispatcher>();

        private readonly Action<IChatMessageDecorator, AccountState> messageSender;
        private readonly List<AccountState> sessionUsers = new List<AccountState>();
        
        // for unknown reason SortedSet sometimes does not pass add-message->check-message-exists unit test
        //private readonly ISet<IChatMessageDecorator> sessionMessages = new SortedSet<IChatMessageDecorator>(new ChatMessageComparer());
        private readonly IList<IChatMessageDecorator> sessionMessages = new List<IChatMessageDecorator>();


        public ChatSession(Guid id, Action<IChatMessageDecorator, AccountState> messageSender) 
        {
            this.messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
            Id = id;
        }


        /// <summary>
        /// Unique session identifier.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Session name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Displayed session name.
        /// </summary>
        public string DisplayedName { get; internal set; }


        public event EventHandler<IChatMessageDecorator> MessageAdded;


        /// <inheritdoc />
        public void AcceptMessage(IChatMessage message) 
        {
            lock (Locker)
            {
                var processedMesage = new ChatMessageDecorator(message, this);

                sessionMessages.Add(processedMesage);
                MessageAdded?.Invoke(this, processedMesage);
            }
        }

        public IEnumerable<IChatMessageDecorator> GetMessages(Func<IChatMessageDecorator, bool> predicate) 
        {
            lock (Locker)
            {
                IEnumerable<IChatMessageDecorator> result = 
                    predicate == null ? 
                        sessionMessages.ToArray() : 
                        sessionMessages.Where(predicate);

                return result;
            }
        }

        /// <inheritdoc />
        public void AddToChat(AccountState account) 
        {
            if (sessionUsers.Contains(account)) return;

            sessionUsers.Add(account);
        }

        public bool SendMessage(IChatMessage message, out IChatMessageDecorator processedMesage) 
        {
            ThrowIfInvalidMessage(message);
            
            processedMesage = new ChatMessageDecorator(message, this);

            lock (Locker)
            {
                sessionMessages.Add(processedMesage);

                foreach (AccountState sessionUser in sessionUsers)
                {
                    try
                    {
                        messageSender(processedMesage, sessionUser);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Oops. { ex.Message }");
                    }
                }
            }

            MessageAdded?.Invoke(this, processedMesage);
            
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