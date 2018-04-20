// Created by Laxale 27.11.2016
//
//

using System.Windows;
using System.Windows.Input;

using Freengy.Base.Helpers;
using Freengy.Base.Attributes;
using Freengy.Settings.ViewModels;


namespace Freengy.Settings.Views 
{
    [HasViewModel(typeof(SettingsViewModel))]
    public partial class SettingsWindow : Window 
    {
        public SettingsWindow() 
        {
            InitializeComponent();

            Owner = Application.Current?.MainWindow;
        }


        private void SettingsWindow_OnKeyDown(object sender, KeyEventArgs e) 
        {
            new KeyHandler(e).ExecuteOnEscapePressed(Close);
        }
    }
}