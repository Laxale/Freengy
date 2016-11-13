// Created by Laxale 19.10.2016
//
//


namespace Freengy.UI.Windows 
{
    using System.Windows;


    public partial class MainWindow : Window 
    {
        public MainWindow() 
        {
            this.InitializeComponent();

            var asmVersion = typeof(MainWindow).Assembly.GetName().Version;

            this.Title = $"{ this.Title } | { asmVersion }";
        }
    }
}