// Created by Laxale 03.11.2016
//
//

using System;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;

using Freengy.Base.ViewModels;
using Freengy.Base.Chat.Interfaces;
using Freengy.Base.Helpers.Commands;
using Freengy.Common.Models;
using Freengy.Networking.Interfaces;


namespace Freengy.Chatter.ViewModels 
{
    public class ChatSessionViewModel : WaitableViewModel 
    {
        private readonly ILoginController loginController;
        private readonly IChatMessageFactory chatMessageFactory;
        private readonly ObservableCollection<IChatMessageDecorator> sessionMessages = new ObservableCollection<IChatMessageDecorator>();

        private string messageText;

        /// <summary>
        /// Event is fired to scroll the message list to end.
        /// </summary>
        internal event Action<IChatMessageDecorator> MessageAdded = decoratpr => { };



        public ChatSessionViewModel(IChatSession session) 
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
            Session.MessageAdded += OnMessageAdded;

            loginController = ServiceLocator.Resolve<ILoginController>();
            chatMessageFactory = ServiceLocator.Resolve<IChatMessageFactory>();
            chatMessageFactory.Author = loginController.MyAccountState.Account;

            SessionMessages = CollectionViewSource.GetDefaultView(sessionMessages);

            // this viewmodel is not created by catel. need to init manually
            CommandSendMessage = new MyCommand(CommandSendMessageImpl, CanSendMessage);
        }

        
        /// <summary>
        /// Command to send current message.
        /// </summary>
        public MyCommand CommandSendMessage { get; }


        /// <summary>
        /// Gets current chat session object.
        /// </summary>
        public IChatSession Session { get; }

        /// <summary>
        /// Get the collection of messages for current session.
        /// </summary>
        public ICollectionView SessionMessages { get; }


        /// <summary>
        /// Gets or sets current user message ready to post to session.
        /// </summary>
        public string MessageText 
        {
            get => messageText;

            set
            {
                if (messageText == value) return;

                messageText = value;

                OnPropertyChanged();
            }
        }


        public override string ToString() 
        {
            return Session.Name;
        }


        private bool CanSendMessage() 
        {
            //return true;
            return !string.IsNullOrWhiteSpace(MessageText);
        }

        private void CommandSendMessageImpl() 
        {
            IChatMessageDecorator processedMessage;

            var newMessage = chatMessageFactory.CreateMessage(MessageText);

            Session.SendMessage(newMessage, out processedMessage);

            MessageText = string.Empty;
        }

        private void OnMessageAdded(object sender, IChatMessageDecorator addedMessage) 
        {
            GUIDispatcher.InvokeOnGuiThread(() =>
            {
                sessionMessages.Add(addedMessage);
                SessionMessages.MoveCurrentToLast();
                MessageAdded.Invoke(addedMessage);
            });
        }
    }
}