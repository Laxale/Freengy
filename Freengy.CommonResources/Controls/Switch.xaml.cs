// Created by Laxale 25.04.2018
//
//

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Freengy.CommonResources.Controls 
{
    /// <summary>
    /// Простой контрол - переключатель.
    /// </summary>
    public partial class Switch : UserControl 
    {
        /// <summary>
        /// Конструктор <see cref="Switch"/>.
        /// </summary>
        public Switch() 
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty IsOnProperty = 
            DependencyProperty.Register(nameof(IsOn), typeof(bool), typeof(Switch));

        public static readonly DependencyProperty CommandProperty = 
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(Switch));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(Switch));


        /// <summary>
        /// Команда, выполняемая переключателем.
        /// </summary>
        public ICommand Command 
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Аргумент команды, выполняемой переключателем.
        /// </summary>
        public object CommandParameter 
        {
            get => (ICommand)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Возвращает или задаёт значение - включён ли переключатель.
        /// </summary>
        public bool IsOn 
        {
            get => (bool)GetValue(IsOnProperty);
            set => SetValue(IsOnProperty, value);
        }
    }
}