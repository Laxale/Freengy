// Created by Laxale 19.10.2016
//
//

using System;
using System.Windows;

using Freengy.Base.Helpers;
using Freengy.Base.Messages;
using Freengy.Common.Interfaces;

using Catel.Messaging;


namespace Freengy.UI.Windows 
{
    /// <summary>
    /// Main application window.
    /// </summary>
    public partial class MainWindow : Window, IObjectWithId 
    {
        public MainWindow() 
        {
            InitializeComponent();

            var asmVersion = typeof(MainWindow).Assembly.GetName().Version;

            Title = $"{ Title } | { asmVersion }";

            Id = KnownCurtainedIds.MainWindowId;

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
    }
}