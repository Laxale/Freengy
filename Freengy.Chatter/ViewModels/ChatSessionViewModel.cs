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

    using Catel.Data;
    using Catel.MVVM;


    public class ChatSessionViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<IChatMessageDecorator> sessionMessages = new ObservableCollection<IChatMessageDecorator>();


        public ChatSessionViewModel(IChatSession session) 
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            this.Session = session;
            this.Session.MessageAdded += this.OnMessageAdded;

            this.SessionMessages = CollectionViewSource.GetDefaultView(this.sessionMessages);
        }

        protected override void SetupCommands() 
        {
            this.CommandSendMessage = new Command(this.CommandSendMessageImpl, this.CanSendMessage);
        }


        public Command CommandSendMessage { get; private set; }


        public IChatSession Session { get; private set; }
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

            this.Session.SendMessage(null, out processedMessage);
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