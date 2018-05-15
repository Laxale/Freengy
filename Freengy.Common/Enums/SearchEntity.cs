// Created by Laxale 18.04.2018
//
//


namespace Freengy.Common.Enums 
{
    /// <summary>
    /// Contains searchable entity types.
    /// </summary>
    public enum SearchEntity 
    {
        /// <summary>
        /// Undefined.
        /// </summary>
        None,

        /// <summary>
        /// Entity to search is user(s).
        /// </summary>
        Users,

        /// <summary>
        /// Entity to search is friend(s).
        /// </summary>
        Friends,

        /// <summary>
        /// Entity to search is game session(s).
        /// </summary>
        GameSessions,

        /// <summary>
        /// Entity to search is user avatar(s)
        /// </summary>
        UserAvatars,

        /// <summary>
        /// Entity to search is user avatars modification timestamps.
        /// </summary>
        UserAvatarsCache,

        /// <summary>
        /// Entity to search is an incoming friend request.
        /// </summary>
        IncomingFriendRequests,

        /// <summary>
        /// Entity to search is a sent out friend request.
        /// </summary>
        OutgoingFriendRequests
    }
}