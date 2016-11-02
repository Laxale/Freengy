// Created by Laxale 02.11.2016
//
//


namespace Freengy.Base.DefaultImpl 
{
    using System;

    using Freengy.Base.Helpers;
    using Freengy.Base.Interfaces;
    

    internal class ChatMessageDecorator : IChatMessageDecorator 
    {
        internal ChatMessageDecorator(IChatMessage originalMessage, IChatSession session)
        {
            Common.ThrowIfArgumentsHasNull(originalMessage, session);

            this.Id = Guid.NewGuid();
            this.ChatSession = session;
            this.TimeStamp = DateTime.UtcNow;
            this.OriginalMessage = originalMessage;
        }

        public Guid Id { get; }
        public DateTime TimeStamp { get; }
        public IChatSession ChatSession { get; }
        public IChatMessage OriginalMessage { get; }
    }
}