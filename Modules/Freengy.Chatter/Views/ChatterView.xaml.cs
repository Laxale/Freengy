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
using Freengy.Base.Interfaces;
using Freengy.Chatter.ViewModels;
using Freengy.Common.Helpers.Result;


namespace Freengy.Chatter.Views 
{
    /// <summary>
    /// View of a main chatter module panel.
    /// </summary>
    [HasViewModel(typeof(ChatterViewModel))]
    public partial class ChatterView : IUserActivity 
    {
        public ChatterView() 
        {
            InitializeComponent();

            this.Subscribe<MessageCollapseChatRequest>(OnCollapseRequest);
            this.Subscribe<MessageShowChatSession>(OnShowChatSessionRequest);

            this.Publish(new MessageActivityChanged(this, true));
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

        /// <summary>
        /// Возвращает значение - можно ли остановить данную активити без ведома юзера.
        /// </summary>
        public bool CanCancelInSilent { get; } = true;

        /// <summary>
        /// Возвращает описание активности в контексте её остановки.
        /// </summary>
        public string CancelDescription { get; } = string.Empty;


        /// <summary>
        /// Cancel activity.
        /// </summary>
        /// <returns>Result of a cancel attempt.</returns>
        public Result Cancel() 
        {
            this.Unsubscribe();

            return Result.Ok();
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