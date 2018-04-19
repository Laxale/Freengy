// Created by Laxale 19.10.2016
//
//


using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using CatelControl = Catel.Windows.Controls.UserControl;


namespace Freengy.UI.Views 
{
    public partial class LoginView : CatelControl 
    {
        public LoginView() 
        {
            InitializeComponent();
        }


        private void PsswordBox_OnLoaded(object sender, RoutedEventArgs e) 
        {
            Keyboard.Focus((PasswordBox) sender);
        }
    }
}