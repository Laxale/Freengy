// Created by Laxale 03.11.2016
//
//


namespace Freengy.Chatter.ViewModels 
{
    using System;
    using System.Windows.Data;
    using System.ComponentModel;
    using System.Collections.ObjectModel;

    using Freengy.Base.ViewModels;
    using Freengy.Base.Chat.Interfaces;

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
            
            this.Session = session;
            this.Session.MessageAdded += this.OnMessageAdded;

            this.chatMessageFactory = base.serviceLocator.ResolveType<IChatMessageFactory>();

            this.SessionMessages = CollectionViewSource.GetDefaultView(this.sessionMessages);

            // this viewmodel is not created by catel. need to init manually
            this.CommandSendMessage = new Command(this.CommandSendMessageImpl, this.CanSendMessage);
        }


        #region override

        protected override void SetupCommands()
        {

        }

        #endregion override


        #region commands
        public Command CommandSendMessage { get; private set; }

        #endregion commands
        

        public IChatSession Session { get; }
        public ICollectionView SessionMessages { get; private set; }

        public string MessageText 
        {
            get { return (string) base.GetValue(MessageTextProperty); }

            set { base.SetValue(MessageTextProperty, value); }
        }
        public static readonly PropertyData MessageTextProperty = RegisterProperty(nameof(MessageText), typeof(string), () => string.Empty);


        private void CommandSendMessageImpl() 
        {
            IChatMessageDecorator processedMessage;

            var newMessage = this.chatMessageFactory.CreateMessage(this.MessageText);

            this.Session.SendMessage(newMessage, out processedMessage);

            this.MessageText = string.Empty;
        }
        private bool CanSendMessage() 
        {
            //return true;
            return !string.IsNullOrWhiteSpace(this.MessageText);
        }

        private void OnMessageAdded(object sender, IChatMessageDecorator addedMessage) 
        {
            this.sessionMessages.Add(addedMessage);
        }
    }
}