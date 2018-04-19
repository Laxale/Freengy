// Created by Laxale 19.04.2018
//
//

using Freengy.Common.Enums;
using Freengy.Common.Database;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// A friend request model.
    /// </summary>
    public class FriendRequest : DbObject 
    {
        /// <summary>
        /// Account that user wants to be friends with.
        /// </summary>
        public UserAccount TargetAccount { get; set; }

        /// <summary>
        /// User account that wants to be friends.
        /// </summary>
        public UserAccount RequesterAccount { get; set; }

        /// <summary>
        /// State of this request. Is set by server.
        /// </summary>
        public FriendRequestState RequestState { get; set; }
    }
}