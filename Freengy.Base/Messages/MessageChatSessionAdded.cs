// Created by Laxale 23.04.2018
//
//

using System;

using Freengy.Base.Chat.Interfaces;


namespace Freengy.Base.Messages 
{
    /// <summary>
    /// A message to signal about new created chat session.
    /// </summary>
    public class MessageChatSessionAdded : MessageBase 
    {
        /// <summary>
        /// Creates a new <see cref="MessageChatSessionAdded"/> with a given session argument.
        /// </summary>
        /// <param name="session">A new created chat session.</param>
        public MessageChatSessionAdded(IChatSession session) 
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
        }


        /// <summary>
        /// A new created chat session.
        /// </summary>
        public IChatSession Session { get; }
    }
}