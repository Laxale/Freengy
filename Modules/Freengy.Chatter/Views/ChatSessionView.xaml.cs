// Created by Laxale 03.11.2016
//
//

using System;
using System.Windows;
using System.Windows.Controls;

using Freengy.Base.Chat.Interfaces;
using Freengy.Chatter.Helpers;
using Freengy.Chatter.ViewModels;


namespace Freengy.Chatter.Views 
{
    /// <summary>
    /// The view of a single chat session.
    /// </summary>
    public partial class ChatSessionView : UserControl 
    {
        private ChatSessionViewModel currentContext;


        public ChatSessionView() 
        {
            InitializeComponent();

            Unloaded += OnUnloaded;

            DataContextChanged += OnDataContextChanged;
        }


        private void OnMessageAdded(DistinguishedChatMessage addedMessage) 
        {
            var moved = MessageList.Items.MoveCurrentTo(addedMessage);

            if (moved)
            {
                MessageList.ScrollIntoView(MessageList.Items.CurrentItem);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs) 
        {
            Unloaded -= OnUnloaded;
            DataContextChanged -= OnDataContextChanged;
            
            currentContext.MessageAdded -= OnMessageAdded;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs args) 
        {
            if (args.OldValue is ChatSessionViewModel previousContext)
            {
                previousContext.MessageAdded -= OnMessageAdded;
            }

            currentContext = (ChatSessionViewModel)args.NewValue;
            currentContext.MessageAdded += OnMessageAdded;
        }
    }
}