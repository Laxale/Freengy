// Created by Laxale 06.05.2018
//
//

using LocalizedRes = Freengy.Localization.StringResources;


namespace Freengy.Common.ErrorReason 
{
    /// <summary>
    /// Ошибка о неправильном пароле.
    /// </summary>
    public class NoServerConnectionErrorReason : ErrorReason 
    {
        /// <summary>
        /// Получить сообщение об ошибке.
        /// </summary>
        /// <returns>Сообщение об ошибке.</returns>
        protected override string GetMessage() 
        {
            return LocalizedRes.NoServerConnection;
        }
    }
}