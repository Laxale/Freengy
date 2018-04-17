// Created by Laxale 17.04.2018
//
//

using Freengy.Base.Helpers;


namespace Freengy.Database 
{
    /// <summary>
    /// Ошибка при работе с базой данных.
    /// </summary>
    public class DBStorageErrorReason : ErrorReason 
    {
        private readonly string message;


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="message">Сообщение об ошибке.</param>
        public DBStorageErrorReason(string message)
        {
            this.message = message;
        }


        /// <summary>
        /// Получить сообщение об ошибке.
        /// </summary>
        /// <returns>Сообщение об ошибке.</returns>
        protected override string GetMessage()
        {
            return message;
        }
    }
}