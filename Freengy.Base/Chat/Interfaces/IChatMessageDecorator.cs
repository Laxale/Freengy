// Created by Laxale 02.11.2016
//
//


namespace Freengy.Base.Chat.Interfaces 
{
    using System;

    using Freengy.SharedWebTypes.Interfaces;


    /// <summary>
    /// Represents processed chat message - one that has been marked with id and timestamp
    /// </summary>
    public interface IChatMessageDecorator : IObjectWithId 
    {
        DateTime TimeStamp { get; }

        IChatSession ChatSession { get; }

        IChatMessage OriginalMessage { get; }
    }
}