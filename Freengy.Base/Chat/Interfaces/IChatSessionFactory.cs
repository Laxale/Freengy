// Created by Laxale 02.11.2016
//
//


namespace Freengy.Base.Chat.Interfaces 
{
    public interface IChatSessionFactory 
    {
        IChatSession CreateInstance(string name, string displayedName);
    }
}