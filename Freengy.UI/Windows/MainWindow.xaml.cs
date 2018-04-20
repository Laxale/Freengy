// Created by Laxale 19.10.2016
//
//

using System.Windows;


namespace Freengy.UI.Windows 
{
    /// <summary>
    /// Main application window.
    /// </summary>
    public partial class MainWindow : Window 
    {
        public MainWindow() 
        {
            InitializeComponent();

            var asmVersion = typeof(MainWindow).Assembly.GetName().Version;

            Title = $"{ this.Title } | { asmVersion }";
        }
    }
}