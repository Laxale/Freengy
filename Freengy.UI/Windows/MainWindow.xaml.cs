// Created by Laxale 19.10.2016
//
//

using System;
using System.Windows;
using System.Windows.Controls;

using Freengy.UI.Views;
using Freengy.Base.Helpers;
using Freengy.Base.Messages;
using Freengy.Common.Interfaces;
using Freengy.CommonResources.Styles;

using Catel.Messaging;


namespace Freengy.UI.Windows 
{
    /// <summary>
    /// Main application window.
    /// </summary>
    public partial class MainWindow : Window, IObjectWithId
    {
        private bool isToolbarLoaded;


        public MainWindow() 
        {
            InitializeComponent();

            var asmVersion = typeof(MainWindow).Assembly.GetName().Version;

            Title = $"{ Title } | { asmVersion }";

            Id = KnownCurtainedIds.MainWindowId;

            StylishWindowStyle.ToolbarRegionLoadedEvent += OnHeaderToolbarHostLoaded;

            MessageMediator.Default.Register<MessageCurtainRequest>(this, OnCurtainRequest);
        }


        /// <summary>
        /// Returns unique identifier of an implementer object.
        /// </summary>
        public Guid Id { get; }


        private void OnCurtainRequest(MessageCurtainRequest request) 
        {
            if (request.AcceptorId == Id)
            {
                SwitchVisibility(CurtainBorder);
            }
        }


        private void SwitchVisibility(UIElement element) 
        {
            Dispatcher.Invoke(() =>
            {
                element.Visibility =
                    element.Visibility == Visibility.Visible ?
                        Visibility.Collapsed :
                        Visibility.Visible;
            });
        }

        private void OnHeaderToolbarHostLoaded(ContentControl contentControl) 
        {
            if (isToolbarLoaded) return;

            contentControl.Content = new HeaderToolbarView();

            isToolbarLoaded = true;
        }
    }
}