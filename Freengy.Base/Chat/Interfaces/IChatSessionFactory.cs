// Created by Laxale 02.11.2016
//
//


namespace Freengy.Base.Chat.Interfaces 
{
    /// <summary>
    /// Interface for the <see cref="IChatSession"/> factory.
    /// </summary>
    public interface IChatSessionFactory 
    {
        /// <summary>
        /// Create a new <see cref="IChatSession"/> instance.
        /// </summary>
        /// <param name="name">Name of the new chat session.</param>
        /// <param name="displayedName">Displayed name of the new chat session.</param>
        /// <returns><see cref="IChatSession"/> instance.</returns>
        IChatSession CreateInstance(string name, string displayedName);
    }
}