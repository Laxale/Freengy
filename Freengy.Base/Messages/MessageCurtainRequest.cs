// Created by Laxale 03.05.2018
//
//

using System;


namespace Freengy.Base.Messages 
{
    /// <summary>
    /// Сообщение-запрос на включение или выключение занавески.
    /// </summary>
    public class MessageCurtainRequest 
    {
        /// <summary>
        /// Конструирует <see cref="MessageCurtainRequest"/> для данного идентификатора получателя запроса.
        /// </summary>
        /// <param name="acceptorId">Уникальный идентификатор получателя запроса на занавеску.</param>
        public MessageCurtainRequest(Guid acceptorId) 
        {
            AcceptorId = acceptorId;
        }


        /// <summary>
        /// Уникальный идентификатор получателя запроса на занавеску.
        /// </summary>
        public Guid AcceptorId { get; }
    }
}