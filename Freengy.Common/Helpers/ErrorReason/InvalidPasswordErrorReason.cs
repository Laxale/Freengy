// Created by Laxale 06.05.2018
//
//


namespace Freengy.Common.Helpers.ErrorReason 
{
    /// <summary>
    /// Ошибка о неправильном пароле.
    /// </summary>
    public class InvalidPasswordErrorReason : ErrorReason 
    {
        /// <summary>
        /// Получить сообщение об ошибке.
        /// </summary>
        /// <returns>Сообщение об ошибке.</returns>
        protected override string GetMessage()
        {
            return "Invalid password";
        }
    }
}