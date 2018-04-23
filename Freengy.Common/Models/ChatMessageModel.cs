﻿// Created by Laxale 23.04.2018
//
//

using System;

using Freengy.Common.Database;
using Freengy.Common.Models.Readonly;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Data model of a chat message.
    /// </summary>
    public class ChatMessageModel : DbObject 
    {
        /// <summary>
        /// Message session identifier.
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Text of a message.
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// Message creation timestamp.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Account of a message author.
        /// </summary>
        public UserAccount AuthorAccount { get; set; }
    }
}