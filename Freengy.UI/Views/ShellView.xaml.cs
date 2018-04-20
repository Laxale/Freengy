// Created by Laxale 20.10.2016
//
//

using System.Windows;
using System.Windows.Controls;
using Catel.IoC;
using Catel.Messaging;

using Freengy.Base.Messages;
using Freengy.Chatter.Views;
using Freengy.Base.Messages.Collapse;

using CatelControl = Catel.Windows.Controls.UserControl;


namespace Freengy.UI.Views 
{
    /// <summary>
    /// Main view for a program for logged in user.
    /// </summary>
    public partial class ShellView : CatelControl 
    {
        private readonly IMessageMediator mediator = MessageMediator.Default;

        private GridLength previousChatHeight;
        private RowDefinition chatterRowDefinition;


        public ShellView() 
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty IsChatCollapsedProperty =
            DependencyProperty.Register(nameof(IsChatCollapsed), typeof(bool), typeof(ShellView));


        /// <summary>
        /// Gets or sets collapsed state of the chatter view.
        /// </summary>
        public bool IsChatCollapsed 
        {
            get => (bool)GetValue(IsChatCollapsedProperty);
            set
            {
                if (value)
                {
                    previousChatHeight = chatterRowDefinition.Height;
                    chatterRowDefinition.Height = new GridLength(60);
                }
                else
                {
                    chatterRowDefinition.Height = previousChatHeight;
                }

                SetValue(IsChatCollapsedProperty, value);
            }
        }


        private void ChatCollapserButton_OnClick(object sender, RoutedEventArgs e) 
        {
            IsChatCollapsed = !IsChatCollapsed;

            mediator.SendMessage(new MessageCollapseChatRequest(IsChatCollapsed));
        }

        private void ChatterRegionDefinition_OnLoaded(object sender, RoutedEventArgs e) 
        {
            chatterRowDefinition = (RowDefinition)sender;
        }
    }
}