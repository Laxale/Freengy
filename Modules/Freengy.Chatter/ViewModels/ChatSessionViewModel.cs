// Created by Laxale 03.11.2016
//
//


namespace Freengy.Chatter.ViewModels 
{
    using System;
    using System.Windows.Data;
    using System.ComponentModel;
    using System.Collections.ObjectModel;

    using Base.ViewModels;
    using Base.Chat.Interfaces;

    using Catel.IoC;
    using Catel.Data;
    using Catel.MVVM;


    public class ChatSessionViewModel : WaitableViewModel 
    {
        private readonly IChatMessageFactory chatMessageFactory;
        private readonly ObservableCollection<IChatMessageDecorator> sessionMessages = new ObservableCollection<IChatMessageDecorator>();

        
        public ChatSessionViewModel(IChatSession session) 
        {
            if (session == null) throw new ArgumentNullException(nameof(session));
            
            Session = session;
            Session.MessageAdded += OnMessageAdded;

            chatMessageFactory = serviceLocator.ResolveType<IChatMessageFactory>();

            SessionMessages = CollectionViewSource.GetDefaultView(sessionMessages);

            // this viewmodel is not created by catel. need to init manually
            CommandSendMessage = new Command(CommandSendMessageImpl, CanSendMessage);
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
        public Command CommandSendMessage { get; private set; }

        #endregion commands
        

        public IChatSession Session { get; }

        public ICollectionView SessionMessages { get; private set; }

        public string MessageText 
        {
            get { return (string) GetValue(MessageTextProperty); }

            set { SetValue(MessageTextProperty, value); }
        }
        public static readonly PropertyData MessageTextProperty = RegisterProperty(nameof(MessageText), typeof(string), () => string.Empty);


        private void CommandSendMessageImpl() 
        {
            IChatMessageDecorator processedMessage;

            var newMessage = chatMessageFactory.CreateMessage(MessageText);

            Session.SendMessage(newMessage, out processedMessage);

            MessageText = string.Empty;
        }
        private bool CanSendMessage() 
        {
            //return true;
            return !string.IsNullOrWhiteSpace(MessageText);
        }

        private void OnMessageAdded(object sender, IChatMessageDecorator addedMessage) 
        {
            sessionMessages.Add(addedMessage);
        }
    }
}