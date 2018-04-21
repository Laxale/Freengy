﻿// Created by Laxale 03.11.2016
//
//

using System;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;

using Freengy.Base.ViewModels;
using Freengy.Base.Chat.Interfaces;
using Freengy.Common.Models;
using Freengy.Networking.Interfaces;

using Catel.IoC;
using Catel.Data;
using Catel.MVVM;
using Freengy.Base.Helpers;


namespace Freengy.Chatter.ViewModels 
{
    using Freengy.Base.Helpers.Commands;


    public class ChatSessionViewModel : WaitableViewModel 
    {
        private readonly ILoginController loginController;
        private readonly IChatMessageFactory chatMessageFactory;
        private readonly ObservableCollection<IChatMessageDecorator> sessionMessages = new ObservableCollection<IChatMessageDecorator>();

        private string messageText;



        public ChatSessionViewModel(IChatSession session) 
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
            Session.MessageAdded += OnMessageAdded;

            loginController = ServiceLocatorProperty.ResolveType<ILoginController>();
            chatMessageFactory = ServiceLocatorProperty.ResolveTypeUsingParameters<IChatMessageFactory>(new object[]{ loginController.CurrentAccount });

            SessionMessages = CollectionViewSource.GetDefaultView(sessionMessages);

            // this viewmodel is not created by catel. need to init manually
            CommandSendMessage = new MyCommand(CommandSendMessageImpl, CanSendMessage);
        }


        #region override

        protected override void SetupCommands()
        {

        }

        public override string ToString() 
        {
            return Session.Name;
        }

        #endregion override


        #region commands
        public MyCommand CommandSendMessage { get; private set; }

        #endregion commands
        

        public IChatSession Session { get; }

        public ICollectionView SessionMessages { get; private set; }


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
            sessionMessages.Add(addedMessage);
        }
    }
}