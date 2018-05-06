// Created by Laxale 17.04.2018
//
//

using System;
using System.Text;


namespace Freengy.Common.Helpers.ErrorReason 
{
    /// <summary>
    /// Причина ошибки.
    /// </summary>
    public abstract class ErrorReason 
    {
        private readonly StringBuilder developerMessage = new StringBuilder();

        /// <summary>
        /// Пустая ErrorReason.
        /// </summary>
        public static ErrorReason None => new EmptyErrorReason();

        /// <summary>
        /// Детали для разработчика.
        /// </summary>
        public string DeveloperDetails => developerMessage.ToString();

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string Message => GetMessage();

        /// <summary>
        /// Добавить сообщение для разработчика.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <returns>ErrorReason.</returns>
        public ErrorReason Add(string message)
        {
            developerMessage.AppendLine(message);
            return this;
        }

        /// <summary>
        /// Добавить сообщение для разработчика. 
        /// </summary>
        /// <param name="format">Формат.</param>
        /// <param name="values">Значения.</param>
        /// <returns>ErrorReason.</returns>
        public ErrorReason Add(string format, params object[] values)
        {
            Append(string.Format(format, values));
            return this;
        }

        /// <summary>
        /// Добавить сообщение для разработчика. 
        /// </summary>
        /// <param name="ex">Исключение.</param>
        /// <returns>ErrorReason.</returns>
        public ErrorReason Add(Exception ex) 
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            Append($"{{ Exception: { ex.Message } }}");

            if (ex.InnerException != null)
            {
                Append($"Exception: { ex.Message }");
            }

            return this;
        }

        /// <summary>
        /// Получить сообщение об ошибке.
        /// </summary>
        /// <returns>Сообщение об ошибке.</returns>
        protected abstract string GetMessage();

        private void Append(string message)
        {
            developerMessage.Append(message);
            developerMessage.Append(" ");
        }

        private sealed class EmptyErrorReason : ErrorReason
        {
            protected override string GetMessage()
            {
                return string.Empty;
            }
        }
    }
}