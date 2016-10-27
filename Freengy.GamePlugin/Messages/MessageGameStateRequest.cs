// Created by Laxale 27.10.2016
//
//


namespace Freengy.GamePlugin.Messages 
{
    using Freengy.Base.Messages;


    /// <summary>
    /// Base class - request for a game state change (load/unload)
    /// </summary>
    public abstract class MessageGameStateRequest : MessageBase 
    {
        protected MessageGameStateRequest() 
        {
            
        }
    }
}