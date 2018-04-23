// Created by Laxale 27.10.2016
//
//

using System.Collections.Generic;

using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.GamePlugin.Interfaces;


namespace Freengy.GamePlugin.Messages 
{
    
    /// <summary>
    /// Someone requests to load a game
    /// TODO: maybe add request source to filter bad requests?
    /// </summary>
    public class MessageLoadGameRequest : MessageGameStateRequest 
    {
        public MessageLoadGameRequest(IGamePlugin gamePlugin, IEnumerable<UserAccount> friendsToInvite) 
        {
            this.Plugin = gamePlugin;
            this.FriendsToInvite = friendsToInvite;
        }


        public IGamePlugin Plugin { get; }

        public IEnumerable<UserAccount> FriendsToInvite { get; }
    }
}