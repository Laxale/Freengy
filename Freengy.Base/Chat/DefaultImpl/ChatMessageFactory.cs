// Created by Laxale 03.11.2016
//
//

using System;

using Freengy.Base.Interfaces;
using Freengy.Base.Chat.Interfaces;
using Freengy.Common.Models;

using Res = Freengy.CommonResources.StringResources;


namespace Freengy.Base.Chat.DefaultImpl 
{
    internal class ChatMessageFactory : IChatMessageFactory 
    {
        private UserAccount author;


        public ChatMessageFactory(UserAccount author) 
        {
            Author = author ?? throw new ArgumentNullException(nameof(author));
        }


        
        public UserAccount Author 
        {
            get => author;

            set => author = value ?? throw new ArgumentNullException(nameof(value));
        }
        
        public IChatMessage CreateMessage(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) text = Res.EmptyPlaceHolder;

            return new ChatMessage(Author) { Text = text };
        }
    }
}