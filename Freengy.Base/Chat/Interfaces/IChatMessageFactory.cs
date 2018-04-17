// Created by Laxale 03.11.2016
//
//

using Freengy.Base.Interfaces;


namespace Freengy.Base.Chat.Interfaces 
{
    public interface IChatMessageFactory 
    {
        IUserAccount Author { get; set; }

        IChatMessage CreateMessage(string text);
    }
}