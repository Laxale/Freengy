// Created by Laxale 02.11.2016
//
//

using System;

using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Chat.Interfaces 
{
    /// <summary>
    /// Interface for the <see cref="IChatSession"/> factory.
    /// </summary>
    public interface IChatSessionFactory
    {
        /// <summary>
        /// Set the generated session identifier. Used for synchronizing sessions on clients.
        /// </summary>
        /// <param name="sessionId">Desired generated session id. If null, random id will be used.</param>
        /// <returns>this.</returns>
        IChatSessionFactory SetSessionId(Guid? sessionId);

        /// <summary>
        /// Create a new <see cref="IChatSession"/> instance.
        /// </summary>
        /// <param name="name">Name of the new chat session.</param>
        /// <param name="displayedName">Displayed name of the new chat session.</param>
        /// <returns><see cref="IChatSession"/> instance.</returns>
        IChatSession CreateInstance(string name, string displayedName);

        /// <summary>
        /// Set an implementation for sending message to a user.
        /// </summary>
        /// <param name="messageSender">Network sending message implementation.</param>
        void SetNetworkInterface(Action<IChatMessageDecorator, AccountState> messageSender);
    }
}