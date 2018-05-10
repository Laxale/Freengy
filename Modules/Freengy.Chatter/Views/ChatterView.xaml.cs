// Created by Laxale 01.11.2016
//
//

using System;
using System.Linq;
using System.Windows;

using Freengy.Base.Messages;
using Freengy.Base.Messages.Collapse;
using Freengy.Base.Attributes;
using Freengy.Base.DefaultImpl;
using Freengy.Chatter.ViewModels;


namespace Freengy.Chatter.Views 
{
    /// <summary>
    /// View of a main chatter module panel.
    /// </summary>
    [HasViewModel(typeof(ChatterViewModel))]
    public partial class ChatterView 
    {
        public ChatterView() 
        {
            InitializeComponent();

            this.Subscribe<MessageShowChatSession>(OnShowChatSessionRequest);
            this.Subscribe<MessageCollapseChatRequest>(OnCollapseRequest);
        }

        
        public static readonly DependencyProperty IsCollapsedProperty =
            DependencyProperty.Register(nameof(IsCollapsed), typeof(bool), typeof(ChatterView));


        /// <summary>
        /// Gets or sets collapsed state of the chatter view.
        /// </summary>
        public bool IsCollapsed 
        {
            get => (bool)GetValue(IsCollapsedProperty);
            set => SetValue(IsCollapsedProperty, value);
        }


        private void OnCollapseRequest(MessageCollapseChatRequest collapseRequest) 
        {
            IsCollapsed = collapseRequest.BeCollapsed;
        }

        private void OnShowChatSessionRequest(MessageShowChatSession request) 
        {
            ChatSessionViewModel targetSession = 
                SessionsTab.Items
                    .OfType<ChatSessionViewModel>()
                    .FirstOrDefault(viewModel => viewModel.Session.Id == request.SessionId);

            if (targetSession == null)
            {
                MessageBox.Show($"Session {request.SessionId} is not found in chatter");
                return;
            }

            SessionsTab.Items.MoveCurrentTo(targetSession);
        }
    }
}