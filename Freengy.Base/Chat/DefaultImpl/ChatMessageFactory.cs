// Created by Laxale 03.11.2016
//
//

using System;

using Freengy.Base.Interfaces;
using Freengy.Base.Chat.Interfaces;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;

using Res = Freengy.Localization.StringResources;


namespace Freengy.Base.Chat.DefaultImpl 
{
    internal class ChatMessageFactory : IChatMessageFactory 
    {
        private UserAccount author;

        
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