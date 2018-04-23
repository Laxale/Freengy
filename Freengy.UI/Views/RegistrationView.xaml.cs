// Created by Laxale 12.11.2016
//
//

using System.Windows;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;

using Freengy.Base.Helpers;
using Freengy.Base.Attributes;
using Freengy.UI.ViewModels;

using TextBox = System.Windows.Controls.TextBox;


namespace Freengy.UI.Views 
{
    [HasViewModel(typeof(RegistrationViewModel))]
    public partial class RegistrationView 
    {
        public RegistrationView() 
        {
            InitializeComponent();

            new ViewModelWierer().Wire(this);
        }


        private void FocusBox_Loaded(object sender, RoutedEventArgs e) 
        {
            (sender as TextBox)?.Focus();
        }

        private void RegisterButton_OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // simulate click to update window state. for unknown reason it hangs after registration
            RegistrationView_OnMouseDown(this, new MouseButtonEventArgs(Mouse.PrimaryDevice, 10, MouseButton.Left));
        }

        private void RegistrationView_OnMouseDown(object sender, MouseButtonEventArgs e) 
        {
            SendKeys.SendWait("{ENTER}");
        }
    }
}