// Created by Laxale 02.11.2016
//
//


namespace Freengy.Base.Interfaces 
{
    using System;
    using System.Collections.Generic;
    

    public interface IChatSession : IObjectWithId
    {
        /// <summary>
        /// Gets messages from this session by specified criteria
        /// </summary>
        /// <param name="predicate">Predicate to filter messages</param>
        /// <returns>Filtered (or not) collection of session messages</returns>
        IEnumerable<IChatMessageDecorator> GetMessages(Func<IChatMessageDecorator, bool> predicate);

        bool SendMessage(IChatMessage message, out IChatMessageDecorator processedMesage);
    }
}
