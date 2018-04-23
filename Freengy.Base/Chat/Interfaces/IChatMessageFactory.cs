// Created by Laxale 03.11.2016
//
//

using Freengy.Base.Interfaces;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Chat.Interfaces 
{
    public interface IChatMessageFactory 
    {
        UserAccount Author { get; set; }

        IChatMessage CreateMessage(string text);
    }
}