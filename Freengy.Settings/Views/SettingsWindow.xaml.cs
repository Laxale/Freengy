// Created by Laxale 27.11.2016
//
//

using System.Windows;
using System.Windows.Input;

using Freengy.Base.Helpers;


namespace Freengy.Settings.Views 
{
    public partial class SettingsWindow : Window 
    {
        public SettingsWindow() 
        {
            InitializeComponent();
        }


        private void SettingsWindow_OnKeyDown(object sender, KeyEventArgs e) 
        {
            new KeyHandler(e).ExecuteOnEscapePressed(Close);
        }
    }
}