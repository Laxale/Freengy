// Created by Laxale 02.11.2016
//
//

using System;
using System.Collections.Generic;

using Freengy.Base.Interfaces;
using Freengy.Common.Interfaces;
using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Chat.Interfaces 
{
    /// <summary>
    /// Interface for the chat session.
    /// </summary>
    public interface IChatSession : IObjectWithId, INamedObject 
    {
        event EventHandler<IChatMessageDecorator> MessageAdded;


        /// <summary>
        /// Add user to chat session.
        /// </summary>
        /// <param name="account">User account to add to session.</param>
        void AddToChat(AccountState account);

        /// <summary>
        /// Send a message to the chat session.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <param name="processedMesage">Processed message decorator.</param>
        /// <returns></returns>
        bool SendMessage(IChatMessage message, out IChatMessageDecorator processedMesage);

        /// <summary>
        /// Gets messages from this session by specified criteria
        /// </summary>
        /// <param name="predicate">Predicate to filter messages</param>
        /// <returns>Filtered (or not) collection of session messages</returns>
        IEnumerable<IChatMessageDecorator> GetMessages(Func<IChatMessageDecorator, bool> predicate);
    }
}