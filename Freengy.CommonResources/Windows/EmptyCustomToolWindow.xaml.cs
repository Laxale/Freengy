// Created by Laxale 21.04.2018
//
//

using System.Windows;


namespace Freengy.CommonResources.Windows 
{
    /// <summary>
    /// Interaction logic for EmptyCustomToolWindow.xaml
    /// </summary>
    public partial class EmptyCustomToolWindow : Window 
    {
        public EmptyCustomToolWindow() 
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty MainContentProperty = 
            DependencyProperty.Register(nameof(MainContent), typeof(object), typeof(EmptyCustomToolWindow));


        /// <summary>
        /// Main content of the window.
        /// </summary>
        public object MainContent 
        {
            get => (object)GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }
    }
}