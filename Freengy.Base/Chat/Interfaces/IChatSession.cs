// Created by Laxale 02.11.2016
//
//


namespace Freengy.Base.Chat.Interfaces 
{
    using System;
    using System.Collections.Generic;

    using Freengy.Base.Interfaces;


    public interface IChatSession : IObjectWithId, INamedObject 
    {
        event EventHandler<IChatMessageDecorator> MessageAdded;

        bool SendMessage(IChatMessage message, out IChatMessageDecorator processedMesage);

        /// <summary>
        /// Gets messages from this session by specified criteria
        /// </summary>
        /// <param name="predicate">Predicate to filter messages</param>
        /// <returns>Filtered (or not) collection of session messages</returns>
        IEnumerable<IChatMessageDecorator> GetMessages(Func<IChatMessageDecorator, bool> predicate);
    }
}