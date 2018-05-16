// Created by Laxale 02.11.2016
//
//

using System;

using Freengy.Base.Chat.Interfaces;
using Freengy.Base.Models.Readonly;


namespace Freengy.Base.Chat.DefaultImpl 
{
    /// <summary>
    /// <see cref="IChatSessionFactory"/> implementer.
    /// </summary>
    internal class ChatSessionFactory : IChatSessionFactory
    {
        private Guid? outerSessionId;
        private Action<IChatMessageDecorator, AccountState> messageSender;


        #region Singleton

        private static ChatSessionFactory instance;

        private ChatSessionFactory() 
        {

        }


        /// <summary>
        /// Единственный инстанс <see cref="ChatSessionFactory"/>.
        /// </summary>
        public static IChatSessionFactory Instance => instance ?? (instance = new ChatSessionFactory());

        #endregion Singleton


        /// <summary>
        /// Set the generated session identifier. Used for synchronizing sessions on clients.
        /// </summary>
        /// <param name="sessionId">Desired generated session id. If null, random id will be used.</param>
        /// <returns>this.</returns>
        public IChatSessionFactory SetSessionId(Guid? sessionId) 
        {
            outerSessionId = sessionId;

            return this;
        }

        public IChatSession CreateInstance(string name, string displayedName) 
        {
            if (string.IsNullOrWhiteSpace(name)) name = "Unnamed session";
            if (string.IsNullOrWhiteSpace(displayedName)) displayedName = "Unnamed session";

            Guid sessionId = outerSessionId ?? Guid.NewGuid();
            var newSession = 
                new ChatSession(sessionId, messageSender)
                {
                    Name = name, 
                    DisplayedName = displayedName
                };

            return newSession;
        }

        /// <inheritdoc />
        public void SetNetworkInterface(Action<IChatMessageDecorator, AccountState> senderImpl) 
        {
            messageSender = senderImpl ?? throw new ArgumentNullException(nameof(senderImpl));
        }
    }
}