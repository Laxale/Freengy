// Created by Laxale 19.04.2018
//
//

using System;
using System.Windows.Input;


namespace Freengy.Base.Helpers 
{
    /// <summary>
    /// Содержит вспомогательные методы для обработки нажатий кнопок.
    /// </summary>
    public class KeyHandler 
    {
        private readonly KeyEventArgs keyArgs;


        /// <summary>
        /// Конструирует <see cref="KeyHandler"/> для единичного события нажатой кнопки.
        /// </summary>
        /// <param name="keyArgs">Аргументы события нажатой кнопки.</param>
        public KeyHandler(KeyEventArgs keyArgs) 
        {
            this.keyArgs = keyArgs;
        }


        /// <summary>
        /// Небезопасно выполнить method (без try), если была нажата кнопка Escape.
        /// </summary>
        /// <param name="method">Метод для выполнения в случае нажатия кнопки Escape.</param>
        public KeyHandler ExecuteOnEscapePressed(Action method) 
        {
            if (keyArgs?.Key == Key.Escape)
            {
                method();
            }

            return this;
        }

        /// <summary>
        /// Выполнить метод, если была нажата кнопка F1 (вызов справки).
        /// </summary>
        /// <param name="method">Метод для выполнения по нажатой кнопке.</param>
        public KeyHandler ExecuteOnF1Pressed(Action method) 
        {
            if (keyArgs?.Key == Key.F1)
            {
                method();
            }

            return this;
        }

        /// <summary>
        /// Выполнить метод, если была нажата указанная кнопка.
        /// </summary>
        /// <param name="expectedKey">Кнопка, по нажатию которой нужно выполнить действие.</param>
        /// <param name="method">Метод для выполнения по нажатой кнопке.</param>
        public KeyHandler ExecuteOnKeyPressed(Key expectedKey, Action method) 
        {
            if (keyArgs?.Key == expectedKey)
            {
                method();
            }

            return this;
        }
    }
}