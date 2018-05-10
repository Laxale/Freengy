// Created by Laxale 10.05.2018
//
//


using Freengy.Base.Messages.Base;
using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Messages 
{
    /// <summary>
    /// Сообщение об изменении состояния моего аккаунта.
    /// </summary>
    public class MessageMyAccountUpdated : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageMyAccountUpdated"/> с новым состоянием моего аккаунта.
        /// </summary>
        /// <param name="myAccountState">Новое состояние моего аккаунта.</param>
        public MessageMyAccountUpdated(AccountState myAccountState) 
        {
            MyAccountState = myAccountState;
        }


        /// <summary>
        /// Возвращает новое состояние моего аккаунта.
        /// </summary>
        public AccountState MyAccountState { get; }
    }
}