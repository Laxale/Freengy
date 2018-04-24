// Created by Laxale 22.04.2018
//
//

using System;

using Freengy.Common.Enums;
using Freengy.Common.Database;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// User friend-request-reply model.
    /// </summary>
    public class FriendRequestReply : DbObject 
    {
        /// <summary>
        /// Friendship establishing timestamp is set by server.
        /// </summary>
        public DateTime EstablishedDate { get; set; }

        /// <summary>
        /// Incoming friend request to reply for.
        /// </summary>
        public FriendRequest Request { get; set; }

        /// <summary>
        /// User reaction for a request.
        /// </summary>
        public FriendRequestReaction Reaction { get; set; }
    }
}
