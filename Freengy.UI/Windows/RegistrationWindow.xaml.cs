// Created by Laxale 10.11.2016
//
//


namespace Freengy.UI.Windows 
{
    using System.Windows;
    using System.Windows.Input;


    public partial class RegistrationWindow 
    {
        public RegistrationWindow() 
        {
            this.Owner = Application.Current.MainWindow;
            this.InitializeComponent();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e) 
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}