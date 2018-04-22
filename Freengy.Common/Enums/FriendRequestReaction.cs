// Created by Laxale 22.04.2018
//
//


namespace Freengy.Common.Enums 
{
    /// <summary>
    /// Contains user reply possibilities for a friend request.
    /// </summary>
    public enum FriendRequestReaction 
    {
        /// <summary>
        /// Undefined.
        /// </summary>
        None,

        /// <summary>
        /// Friendship accepted.
        /// </summary>
        Accept,

        /// <summary>
        /// Friendship declined.
        /// </summary>
        Decline,

        /// <summary>
        /// Friendship requester is banned by target user.
        /// </summary>
        Ban,

        /// <summary>
        /// Some error occured.
        /// </summary>
        Error
    }
}