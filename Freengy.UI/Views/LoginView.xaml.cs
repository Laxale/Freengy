// Created by Laxale 19.10.2016
//
//


using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Freengy.Base.Attributes;
using Freengy.UI.ViewModels;


namespace Freengy.UI.Views 
{
    /// <summary>
    /// Login process view.
    /// </summary>
    [HasViewModel(typeof(LoginViewModel))]
    public partial class LoginView 
    {
        public LoginView() 
        {
            InitializeComponent();
        }


        private void PsswordBox_OnLoaded(object sender, RoutedEventArgs e) 
        {
            Keyboard.Focus((PasswordBox) sender);
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var t = e;
        }
    }
}