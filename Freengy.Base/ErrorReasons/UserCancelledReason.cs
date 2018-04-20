// Created by Laxale 20.04.2018
//
//

using Freengy.Common.Helpers.ErrorReason;


namespace Freengy.Base.ErrorReasons 
{
    /// <summary>
    /// Error reason about that user cancelled some operation.
    /// </summary>
    public class UserCancelledReason : ErrorReason 
    {
        /// <summary>
        /// Получить сообщение об ошибке.
        /// </summary>
        /// <returns>Сообщение об ошибке.</returns>
        protected override string GetMessage() 
        {
            return "Cancelled by user";
        }
    }
}