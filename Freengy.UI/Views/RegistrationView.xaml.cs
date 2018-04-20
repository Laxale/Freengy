// Created by Laxale 12.11.2016
//
//

using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;

using Freengy.Base.Helpers;
using Freengy.Base.Attributes;
using Freengy.UI.ViewModels;


namespace Freengy.UI.Views 
{
    [HasViewModel(typeof(RegistrationViewModel))]
    public partial class RegistrationView 
    {
        private Button finishButton;


        public RegistrationView() 
        {
            InitializeComponent();

            new ViewModelWierer().Wire(this);
        }


        private void FocusBox_Loaded(object sender, RoutedEventArgs e) 
        {
            (sender as TextBox)?.Focus();
        }


        private void FinishButton_OnLoaded(object sender, RoutedEventArgs e)
        {
            finishButton = (Button)sender;
        }

        private void RegisterButton_OnIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FocusFinishButton(e);
        }

        private void FocusFinishButton(DependencyPropertyChangedEventArgs e) 
        {
            if ((bool)e.NewValue == false)
            {
                finishButton?.Focus();
            }
        }
    }
}