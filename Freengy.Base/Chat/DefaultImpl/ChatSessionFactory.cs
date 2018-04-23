// Created by Laxale 02.11.2016
//
//

using System;

using Freengy.Base.Chat.Interfaces;


namespace Freengy.Base.Chat.DefaultImpl 
{
    internal class ChatSessionFactory : IChatSessionFactory 
    {
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
                new ChatSession(Guid.NewGuid())
                {
                    Name = name, 
                    DisplayedName = displayedName
                };

            return newSession;
        }
    }
}