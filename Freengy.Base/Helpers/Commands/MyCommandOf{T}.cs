// Created by Laxale 21.04.2018
//
//

using System;
using System.Windows.Input;


namespace Freengy.Base.Helpers.Commands 
{
    /// <summary>
    /// Generic <see cref="ICommand"/> implementer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyCommand<T> : ICommand 
    {
        private readonly Action<T> action;
        private readonly Func<T, bool> canExecute;


        /// <summary>
        /// Creates a new <see cref="MyCommand{T}"/> instance.
        /// </summary>
        /// <param name="action">Command action.</param>
        /// <param name="canExecute">Command execution possibility predicate.</param>
        public MyCommand(Action<T> action, Func<T, bool> canExecute = null) 
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));

            this.canExecute = canExecute;
        }


        /// <inheritdoc />
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute((T)parameter);
        }

        /// <inheritdoc />
        public void Execute(object parameter) 
        {
            action((T)parameter);
        }

        /// <summary>
        /// Вызвать вычисление функции CanExecute().
        /// </summary>
        public void RaiseCanExecuteChanged() 
        {
            CommandManager.InvalidateRequerySuggested();
        }


        /// <inheritdoc />
        public event EventHandler CanExecuteChanged 
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
