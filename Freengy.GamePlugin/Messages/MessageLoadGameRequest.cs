// Created by Laxale 27.10.2016
//
//


namespace Freengy.GamePlugin.Messages 
{
    using Freengy.Base.Messages;
    using Freengy.Base.Interfaces;
    using Freengy.GamePlugin.Interfaces;


    /// <summary>
    /// Someone requests to load a game
    /// TODO: maybe add request source to filter bad requests?
    /// </summary>
    public class MessageLoadGameRequest : MessageGameStateRequest 
    {
        public MessageLoadGameRequest(IGamePlugin gamePlugin, IUserAccount friendToInvite) 
        {
            this.Plugin = gamePlugin;
            this.FriendToInvite = friendToInvite;
        }


        public IGamePlugin Plugin { get; }

        public IUserAccount FriendToInvite { get; }
    }
}