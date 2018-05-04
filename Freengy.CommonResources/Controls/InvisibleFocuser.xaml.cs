// Created by Laxale 04.05.2018
//
//

using System.Windows;
using System.Windows.Input;


namespace Freengy.CommonResources.Controls 
{
    /// <summary>
    /// Невидимая пустышка, по умолчанию принимающая на себя фокус клавиатуры. 
    /// Используется для передачи фокуса в родительский контрол при его загрузке.
    /// </summary>
    public partial class InvisibleFocuser : FrameworkElement 
    {
        /// <summary>
        /// Конструктор <see cref="InvisibleFocuser"/>.
        /// </summary>
        public InvisibleFocuser() 
        {
            InitializeComponent();
        }


        private void InvisibleFocuser_OnLoaded(object sender, RoutedEventArgs e) 
        {
            Keyboard.Focus(this);
        }
    }
}