// Created by Laxale 03.11.2016
//
//

using Freengy.Base.Interfaces;
using Freengy.Base.Models.Readonly;
using Freengy.Common.Models;


namespace Freengy.Base.Chat.Interfaces 
{
    public interface IChatMessageFactory 
    {
        UserAccount Author { get; set; }

        IChatMessage CreateMessage(string text);
    }
}