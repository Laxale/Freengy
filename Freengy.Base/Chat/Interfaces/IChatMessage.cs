// Created by Laxale 02.11.2016
//
//

using Freengy.Base.Interfaces;


namespace Freengy.Base.Chat.Interfaces 
{
    public interface IChatMessage  
    {
        string Text { get; }

        IUserAccount Author { get; }
    }
}