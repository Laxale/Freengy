// Created by Laxale 12.11.2016
//
//


namespace Freengy.UI.Views 
{
    using System.Windows;
    using System.Windows.Controls;

    using CatelControl = Catel.Windows.Controls.UserControl;


    public partial class RegistrationView : CatelControl 
    {
        public RegistrationView() 
        {
            this.InitializeComponent();
        }

        private void FocusBox_Loaded(object sender, RoutedEventArgs e) 
        {
            (sender as TextBox)?.Focus();
        }
    }
}