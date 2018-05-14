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
        public ChatSessionView() 
        {
            InitializeComponent();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }


        private void OnMessageAdded(DistinguishedChatMessage addedMessage) 
        {
            MessageList.ScrollIntoView(addedMessage);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) 
        {
            ((ChatSessionViewModel)DataContext).MessageAdded += OnMessageAdded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs) 
        {
            Loaded -= OnLoaded;
            Unloaded -= OnUnloaded;
            ((ChatSessionViewModel)DataContext).MessageAdded -= OnMessageAdded;
        }
    }
}