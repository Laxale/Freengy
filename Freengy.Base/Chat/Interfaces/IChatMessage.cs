// Created by Laxale 02.11.2016
//
//

using Freengy.Base.Interfaces;
using Freengy.Common.Models;


namespace Freengy.Base.Chat.Interfaces 
{
    public interface IChatMessage 
    {
        string Text { get; }

        UserAccount Author { get; }
    }
}