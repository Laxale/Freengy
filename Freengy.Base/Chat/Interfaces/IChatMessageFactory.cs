﻿// Created by Laxale 03.11.2016
//
//


namespace Freengy.Base.Chat.Interfaces 
{
    using Freengy.Base.Interfaces;
    

    public interface IChatMessageFactory 
    {
        IUserAccount Author { get; set; }

        IChatMessage CreateMessage(string text);
    }
}