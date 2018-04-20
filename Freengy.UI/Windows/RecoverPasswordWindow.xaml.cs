// Created by Laxale 19.10.2016
//
//

using System;
using System.Windows;
using System.Windows.Input;

using Freengy.Base.Helpers;


namespace Freengy.UI.Windows 
{
    /// <summary>
    /// Interaction logic for RecoverPasswordWindow.xaml
    /// </summary>
    public partial class RecoverPasswordWindow : Window 
    {
        public RecoverPasswordWindow() 
        {
            InitializeComponent();

            Owner = Application.Current?.MainWindow;
        }


        private void RecoverPasswordWindow_OnKeyDown(object sender, KeyEventArgs e) 
        {
            new KeyHandler(e).ExecuteOnEscapePressed(Close);
        }
    }
}