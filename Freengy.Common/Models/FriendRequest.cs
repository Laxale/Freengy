// Created by Laxale 19.04.2018
//
//

using System;
using System.ComponentModel.DataAnnotations.Schema;

using Freengy.Common.Enums;
using Freengy.Common.Database;
using Freengy.Common.Extensions;


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
        [NotMapped]
        public UserAccountModel TargetAccount { get; set; }

        /// <summary>
        /// User account that wants to be friends.
        /// </summary>
        [NotMapped]
        public UserAccountModel RequesterAccount { get; set; }

        /// <summary>
        /// State of this request. Is set by server.
        /// </summary>
        public FriendRequestState RequestState { get; set; }

        /// <summary>
        /// Request creation timestamp.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Request decision timestamp.
        /// </summary>
        public DateTime DecisionDate { get; set; }
    }
}