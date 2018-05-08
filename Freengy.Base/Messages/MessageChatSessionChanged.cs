// Created by Laxale 23.04.2018
//
//

using System;

using Freengy.Base.Chat.Interfaces;
using Freengy.Base.Messages.Base;


namespace Freengy.Base.Messages 
{
    /// <summary>
    /// A message to signal about created or removed chat session.
    /// </summary>
    public class MessageChatSessionChanged : MessageBase 
    {
        /// <summary>
        /// Creates a new <see cref="MessageChatSessionChanged"/> with a given session argument.
        /// </summary>
        /// <param name="session">A new created chat session.</param>
        /// <param name="isCreated">Is session created (or removed).</param>
        public MessageChatSessionChanged(IChatSession session, bool isCreated)
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
            IsCreated = isCreated;
        }


        /// <summary>
        /// Is session created (or removed).
        /// </summary>
        public bool IsCreated { get; }

        /// <summary>
        /// A new created chat session.
        /// </summary>
        public IChatSession Session { get; }
    }
}