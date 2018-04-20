// Created by Laxale 19.04.2018
//
//

using Freengy.Common.Models;


namespace Freengy.Common.Enums 
{
    /// <summary>
    /// Contains possible values of a <see cref="FriendRequest"/> state.
    /// </summary>
    public enum FriendRequestState 
    {
        /// <summary>
        /// Undefined.
        /// </summary>
        None,

        /// <summary>
        /// Request is stored. User answer is not yet incame.
        /// </summary>
        AwaitingUserAnswer,

        /// <summary>
        /// User accepted friendship request. Yey!
        /// </summary>
        Accepted,

        /// <summary>
        /// User declined friend request.
        /// </summary>
        Declined,

        /// <summary>
        /// Target account doesnt exist.
        /// </summary>
        DoesntExist,

        /// <summary>
        /// Some error occured.
        /// </summary>
        Error
    }
}