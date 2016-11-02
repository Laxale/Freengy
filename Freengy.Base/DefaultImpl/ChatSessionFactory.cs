// Created by Laxale 02.11.2016
//
//


namespace Freengy.Base.DefaultImpl
{
    using System;

    using Freengy.Base.Interfaces;


    public class ChatSessionFactory : IChatSessionFactory 
    {
        #region Singleton

        private static ChatSessionFactory instance;

        private ChatSessionFactory() 
        {

        }

        public static IChatSessionFactory Instance => 
            ChatSessionFactory.instance ?? (ChatSessionFactory.instance = new ChatSessionFactory());

        #endregion Singleton


        public IChatSession CreateInstance() 
        {
            return new ChatSession(Guid.NewGuid());
        }
    }
}