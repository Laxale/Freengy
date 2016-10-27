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
    public class MessageLoadGameRequest : MessageBase 
    {
        private readonly IGamePlugin gamePlugin;
        private readonly IUserAccount friendToInvite;

        
        public MessageLoadGameRequest(IGamePlugin gamePlugin, IUserAccount friendToInvite) 
        {
            this.gamePlugin = gamePlugin;
            this.friendToInvite = friendToInvite;
        }
    }
}