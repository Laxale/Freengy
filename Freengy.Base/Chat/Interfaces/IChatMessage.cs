// Created by Laxale 02.11.2016
//
//

using Freengy.Base.Interfaces;
using Freengy.Base.Models.Readonly;
using Freengy.Common.Models;


namespace Freengy.Base.Chat.Interfaces 
{
    /// <summary>
    /// Interface of a chat message.
    /// </summary>
    public interface IChatMessage 
    {
        /// <summary>
        /// Gets message text.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets message author account.
        /// </summary>
        UserAccount Author { get; }
    }
}