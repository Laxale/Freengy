// Created by Laxale 02.11.2016
//
//

using System;

using Freengy.Base.Interfaces;
using Freengy.Common.Interfaces;


namespace Freengy.Base.Chat.Interfaces 
{
    /// <summary>
    /// Represents processed chat message - one that has been marked with id and timestamp.
    /// </summary>
    public interface IChatMessageDecorator : IObjectWithId 
    {
        /// <summary>
        /// Message creation timestamp.
        /// </summary>
        DateTime TimeStamp { get; }

        /// <summary>
        /// Parent chat session.
        /// </summary>
        IChatSession ChatSession { get; }

        /// <summary>
        /// Message itself.
        /// </summary>
        IChatMessage OriginalMessage { get; }
    }
}