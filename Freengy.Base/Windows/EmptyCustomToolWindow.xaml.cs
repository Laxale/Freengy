// Created by Laxale 21.04.2018
//
//

using System;
using System.Windows;
using System.Windows.Input;

using Freengy.Base.Helpers;


namespace Freengy.Base.Windows 
{
    /// <summary>
    /// Interaction logic for EmptyCustomToolWindow.xaml
    /// </summary>
    public partial class EmptyCustomToolWindow : Window 
    {
        public EmptyCustomToolWindow() 
        {
            InitializeComponent();

            Owner = Application.Current.MainWindow;
        }


        public static readonly DependencyProperty MainContentProperty = 
            DependencyProperty.Register(nameof(MainContent), typeof(object), typeof(EmptyCustomToolWindow));


        /// <summary>
        /// Main content of the window.
        /// </summary>
        public object MainContent 
        {
            get => (object)GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }


        private void EmptyCustomToolWindow_OnKeyDown(object sender, KeyEventArgs e) 
        {
            new KeyHandler(e).ExecuteOnEscapePressed(Close);
        }
    }
}