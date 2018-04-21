// Created by Laxale 20.04.2018
//
//

using System;
using System.Windows.Input;


namespace Freengy.Base.Helpers.Commands 
{
    /// <summary>
    ///   Реализация команды через делегаты.
    /// </summary>
    public class MyCommand : ICommand 
    {
        /// <summary>
        /// Доступность команды.
        /// </summary>
        private readonly Func<bool> canExecute;

        /// <summary>
        /// Действие.
        /// </summary>
        private readonly Action execute;


        /// <summary>
        /// Создает новую команду.
        /// </summary>
        /// <param name="execute">Действие.</param>
        /// <param name="canExecute">Доступность команды.</param>
        public MyCommand(Action execute, Func<bool> canExecute = null) 
        {
            this.canExecute = canExecute;
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }


        /// <summary>
        /// Событие изменения доступности команды.
        /// </summary>
        public event EventHandler CanExecuteChanged 
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }


        /// <summary>
        ///   Проверка доступности команды.
        /// </summary>
        /// <returns> Доступность команды. </returns>
        public bool CanExecute(object arg) 
        {
            return canExecute == null || canExecute();
        }

        /// <summary>
        /// Выполнение команды.
        /// </summary>
        public void Execute(object arg) 
        {
            execute();
        }

        /// <summary>
        /// Вызвать вычисление функции CanExecute().
        /// </summary>
        public void RaiseCanExecuteChanged() 
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
