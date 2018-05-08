// Created by Laxale 04.05.2018
//
//

using System.Windows;
using System.Windows.Input;
using Freengy.Base.Messages.Base;


namespace Freengy.Base.Messages 
{
    /// <summary>
    /// Сообщение о том, что в неком окне была нажата кнопка. Может быть полезно для глубоко вложенных контролов.
    /// </summary>
    public class MessageParentWindowKeyDown : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageParentWindowKeyDown"/> с заданным окном-источником и аргументами события.
        /// </summary>
        /// <param name="window">Окно-источник события нажатия кнопки.</param>
        /// <param name="args">Аргументы события нажатия кнопки.</param>
        public MessageParentWindowKeyDown(Window window, KeyEventArgs args) 
        {
            Window = window;
            Args = args;
        }


        /// <summary>
        /// Окно-источник события нажатия кнопки.
        /// </summary>
        public Window Window { get; }

        /// <summary>
        /// Аргументы события нажатия кнопки.
        /// </summary>
        public KeyEventArgs Args { get; }
    }
}