// Created by Laxale 20.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Freengy.Base.Helpers 
{
    /// <summary>
    ///   Реализация команды через делегаты.
    /// </summary>
    [Serializable]
    public class MyCommand : ICommand 
    {
        /// <summary>
        /// Доступность команды.
        /// </summary>
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// Действие.
        /// </summary>
        private readonly Action<object> execute;


        /// <summary>
        ///   Создает новую команду.
        /// </summary>
        /// <param name="execute"> Действие. </param>
        public MyCommand(Action<object> execute) : this(execute, null) 
        {

        }

        /// <summary>
        /// Создает новую команду.
        /// </summary>
        /// <param name="execute"> Действие. </param>
        /// <param name="canExecute"> Доступность команды. </param>
        public MyCommand(Action<object> execute, Predicate<object> canExecute) 
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
        /// <param name="parameter"> Параметр операции. </param>
        /// <returns> Доступность команды. </returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        /// <summary>
        ///   Выполнение команды.
        /// </summary>
        /// <param name="parameter"> Параметр операции. </param>
        public void Execute(object parameter)
        {
            execute(parameter);
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
