// Created by Laxale 10.05.2018
//
//

using System;

using Freengy.Base.Messages.Base;


namespace Freengy.Base.Messages 
{
    /// <summary>
    /// Сообщение-запрос на перемещение фокуса к указанной чат-сессии.
    /// </summary>
    public class MessageShowChatSession : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageShowChatSession"/> с заданным идентификатором сессии.
        /// </summary>
        /// <param name="sessionId">Идентификатор желаемой для показа сессии.</param>
        public MessageShowChatSession(Guid sessionId)
        {
            SessionId = sessionId;
        }


        /// <summary>
        /// Идентификатор желаемой для показа сессии.
        /// </summary>
        public Guid SessionId { get; }
    }
}