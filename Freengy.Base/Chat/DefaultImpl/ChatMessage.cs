// Created by Laxale 03.11.2016
//
//


namespace Freengy.Base.Chat.DefaultImpl 
{
    using System;

    using Freengy.Base.Interfaces;
    using Freengy.Base.Chat.Interfaces;


    internal class ChatMessage : IChatMessage 
    {
        public ChatMessage(IUserAccount author) 
        {
            if (author == null) throw new ArgumentNullException(nameof(author));

            this.Author = author;
        }


        public string Text { get; internal set; }

        public IUserAccount Author { get; internal set; }   
    }
}