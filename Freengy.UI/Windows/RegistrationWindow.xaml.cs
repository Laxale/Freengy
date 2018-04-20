// Created by Laxale 10.11.2016
//
//

using System.Windows;
using System.Windows.Input;

using Freengy.Base.Helpers;


namespace Freengy.UI.Windows 
{
    /// <summary>
    /// Window for new user registration.
    /// </summary>
    public partial class RegistrationWindow 
    {
        public RegistrationWindow() 
        {
            InitializeComponent();

            Owner = Application.Current.MainWindow;
        }


        private void Window_KeyDown(object sender, KeyEventArgs e) 
        {
            new KeyHandler(e).ExecuteOnEscapePressed(Close);
        }
    }
}