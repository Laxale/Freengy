// Created by Laxale 17.04.2018
//
//


namespace Freengy.Common.ErrorReason 
{
    /// <summary>
    /// Непредвиденная ошибка.
    /// </summary>
    public sealed class UnexpectedErrorReason : ErrorReason 
    {
        private readonly string message;


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public UnexpectedErrorReason(string message)
        {
            this.message = message;
        }


        /// <summary>
        /// Получить сообщение.
        /// </summary>
        /// <returns>Сообщение об ошибке.</returns>
        protected override string GetMessage()
        {
            return message;
        }
    }
}