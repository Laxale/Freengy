// Created by Laxale 15.11.2016
//
//


using System.Windows;
using System.Windows.Input;
using Freengy.Base.Helpers;


namespace Freengy.Diagnostics.Views 
{
    public partial class DiagnosticsWindow : Window 
    {
        public DiagnosticsWindow() 
        {
            this.InitializeComponent();
        }


        private void DiagnosticsWindow_OnKeyDown(object sender, KeyEventArgs e) 
        {
            new KeyHandler(e).ExecuteOnEscapePressed(Close);
        }
    }
}