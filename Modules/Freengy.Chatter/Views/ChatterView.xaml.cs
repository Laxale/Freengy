// Created by Laxale 01.11.2016
//
//

using System.Windows;

using Freengy.Base.Messages;
using Freengy.Base.Messages.Collapse;
using Freengy.Base.Attributes;
using Freengy.Chatter.ViewModels;

using Catel.Messaging;


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

            MessageMediator.Default.Register<MessageCollapseChatRequest>(this, OnCollapseRequest);
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
    }
}