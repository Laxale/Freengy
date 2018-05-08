// Created by Laxale 24.04.2018
//
//

using System;

using Freengy.Base.Messages;
using Freengy.Base.Messages.Base;
using Freengy.Common.Models.Readonly;


namespace Freengy.Networking.Messages 
{
    /// <summary>
    /// A message about friend state change.
    /// </summary>
    public class MessageFriendStateUpdate : MessageBase 
    {
        /// <summary>
        /// Creates a new <see cref="MessageFriendStateUpdate"/> with a given friend account state.
        /// </summary>
        /// <param name="friendState">Changed friend state.</param>
        public MessageFriendStateUpdate(AccountState friendState) 
        {
            FriendState = friendState ?? throw new ArgumentNullException(nameof(friendState));
        }


        /// <summary>
        /// Changed friend state.
        /// </summary>
        public AccountState FriendState { get; }
    }
}