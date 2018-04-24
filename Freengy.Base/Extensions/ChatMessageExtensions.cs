// Created by Laxale 23.04.2018
//
//

using Freengy.Common.Models;
using Freengy.Base.Chat.Interfaces;
using Freengy.Common.Extensions;


namespace Freengy.Base.Extensions 
{
    /// <summary>
    /// Contains extensions for chat message objects.
    /// </summary>
    public static class ChatMessageExtensions 
    {
        /// <summary>
        /// Convert <see cref="IChatMessageDecorator"/> to network message model.
        /// </summary>
        /// <param name="messageDecorator">Message decorator to convert.</param>
        /// <returns>Converted message model.</returns>
        public static ChatMessageModel ToModel(this IChatMessageDecorator messageDecorator) 
        {
            var model = new ChatMessageModel
            {
                Id = messageDecorator.Id,
                TimeStamp = messageDecorator.TimeStamp,
                SessionId = messageDecorator.ChatSession.Id,
                MessageText = messageDecorator.OriginalMessage.Text,
                AuthorAccount = messageDecorator.OriginalMessage.Author.ToModel()
            };

            return model;
        }
    }
}