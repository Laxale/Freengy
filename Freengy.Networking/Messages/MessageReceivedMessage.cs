// Created by Laxale 10.05.2018
//
//

using System;

using Freengy.Base.Messages.Base;


namespace Freengy.Networking.Messages 
{
    /// <summary>
    /// Сообщение о том, что принято входящее сообщение.
    /// </summary>
    public class MessageReceivedMessage : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageReceivedMessage"/> с заданным идентификатором отправителя входящего сообщения.
        /// </summary>
        /// <param name="senderId">Идентификатор отправителя входящего сообщения.</param>
        /// <param name="sessionId">Идентификатор сессии, в которую добавлено сообщение.</param>
        public MessageReceivedMessage(Guid senderId, Guid sessionId)
        {
            SenderId = senderId;
            SessionId = sessionId;
        }


        /// <summary>
        /// Идентификатор отправителя входящего сообщения.
        /// </summary>
        public Guid SenderId { get; }

        /// <summary>
        /// Идентификатор сессии, в которую добавлено сообщение.
        /// </summary>
        public Guid SessionId { get; }
    }
}