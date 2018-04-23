// Created by Laxale 23.04.2018
//
//

using System;


namespace Freengy.Base.Chat.Interfaces 
{
    /// <summary>
    /// Interface of a centralized chat sessions hub.
    /// </summary>
    public interface IChatHub
    {
        /// <summary>
        /// Get a chat session by identifier.
        /// </summary>
        /// <param name="sessionId">Chat session identifier.</param>
        /// <returns>Chat session or null.</returns>
        IChatSession GetSession(Guid sessionId);

        /// <summary>
        /// Add new session.
        /// </summary>
        /// <param name="session">Chat session to add.</param>
        void AddSession(IChatSession session);

        /// <summary>
        /// Remove a session.
        /// </summary>
        /// <param name="session">Chat session to remove.</param>
        void RemoveSession(IChatSession session);
    }
}