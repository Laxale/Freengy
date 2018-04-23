// Created by Laxale 02.11.2016
//
//

using System;

using Freengy.Base.Chat.Interfaces;


namespace Freengy.Base.Chat.DefaultImpl 
{
    using Freengy.Common.Models.Readonly;


    internal class ChatSessionFactory : IChatSessionFactory
    {
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


        public IChatSession CreateInstance(string name, string displayedName) 
        {
            if (string.IsNullOrWhiteSpace(name)) name = "Unnamed session";
            if (string.IsNullOrWhiteSpace(displayedName)) displayedName = "Unnamed session";

            var newSession = 
                new ChatSession(Guid.NewGuid(), messageSender)
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