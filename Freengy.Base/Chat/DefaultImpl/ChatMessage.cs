﻿// Created by Laxale 03.11.2016
//
//

using System;

using Freengy.Base.Interfaces;
using Freengy.Base.Chat.Interfaces;
using Freengy.Base.Models.Readonly;
using Freengy.Common.Models;


namespace Freengy.Base.Chat.DefaultImpl 
{
    internal class ChatMessage : IChatMessage 
    {
        public ChatMessage(UserAccount author) 
        {
            this.Author = author ?? throw new ArgumentNullException(nameof(author));
        }


        public string Text { get; internal set; }

        public UserAccount Author { get; internal set; }   
    }
}