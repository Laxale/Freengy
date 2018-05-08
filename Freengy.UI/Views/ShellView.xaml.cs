// Created by Laxale 20.10.2016
//
//

using System.Windows;
using System.Windows.Controls;

using Freengy.Base.Attributes;
using Freengy.Base.DefaultImpl;
using Freengy.Base.Messages.Collapse;
using Freengy.UI.ViewModels;

using Prism.Regions;


namespace Freengy.UI.Views 
{
    /// <summary>
    /// Main view for a program for logged in user.
    /// </summary>
    [RegionMemberLifetime(KeepAlive = false)]
    [HasViewModel(typeof(ShellViewModel))]
    public partial class ShellView 
    {
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

            this.Publish(new MessageCollapseChatRequest(IsChatCollapsed));
        }

        private void ChatterRegionDefinition_OnLoaded(object sender, RoutedEventArgs e) 
        {
            chatterRowDefinition = (RowDefinition)sender;
        }
    }
}