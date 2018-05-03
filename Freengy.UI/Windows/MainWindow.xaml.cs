// Created by Laxale 19.10.2016
//
//

using System;
using System.Windows;
using Catel.Messaging;
using Freengy.Base.Helpers;
using Freengy.Common.Interfaces;


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

            MessageMediator.Default.Register(this, )
        }


        /// <summary>
        /// Returns unique identifier of an implementer object.
        /// </summary>
        public Guid Id { get; }
    }
}