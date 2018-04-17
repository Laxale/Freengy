// Created by Laxale 03.11.2016
//
//

using System;

using Freengy.Base.Interfaces;
using Freengy.Base.Chat.Interfaces;

using Res = Freengy.CommonResources.StringResources;


namespace Freengy.Base.Chat.DefaultImpl 
{
    internal class ChatMessageFactory : IChatMessageFactory 
    {
        public ChatMessageFactory(IUserAccount author) 
        {
            if (author == null) throw new ArgumentNullException(nameof(author));

            this.Author = author;
        }


        private IUserAccount author;
        public IUserAccount Author 
        {
            get { return this.author; }

            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                
                this.author = value;
            }
        }
        
        public IChatMessage CreateMessage(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) text = Res.EmptyPlaceHolder;

            return new ChatMessage(this.Author) { Text = text };
        }
    }
}