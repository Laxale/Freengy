// Created by Laxale 23.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Freengy.Base.Chat.Interfaces;


namespace Freengy.Base.Chat.DefaultImpl 
{
    /// <summary>
    /// An <see cref="IChatHub"/> implementer.
    /// </summary>
    internal class ChatHub : IChatHub 
    {
        private static readonly object Locker = new object();

        private static ChatHub instance;

        private readonly Dictionary<Guid, IChatSession> chatSessions = new Dictionary<Guid, IChatSession>();


        private ChatHub() 
        {

        }


        /// <summary>
        /// Единственный инстанс <see cref="ChatHub"/>.
        /// </summary>
        public static IChatHub Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new ChatHub());
                }
            }
        }


        /// <inheritdoc />
        /// <summary>
        /// Get a chat session by identifier.
        /// </summary>
        /// <param name="sessionId">Chat session identifier.</param>
        /// <returns>Chat session or null.</returns>
        public IChatSession GetSession(Guid sessionId) 
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Add new session.
        /// </summary>
        /// <param name="session">Chat session to add.</param>
        public void AddSession(IChatSession session) 
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Remove a session.
        /// </summary>
        /// <param name="session">Chat session to remove.</param>
        public void RemoveSession(IChatSession session) 
        {
            throw new NotImplementedException();
        }
    }
}