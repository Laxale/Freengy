// Created by Laxale 31.10.2016
//
//


using Freengy.Base.Messages.Base;

namespace Freengy.GamePlugin.Messages
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Freengy.Base.Messages;
    using Freengy.GamePlugin.Interfaces;


    public class MessageGamesAdded : MessageBase 
    {
        public MessageGamesAdded(IEnumerable<IGamePlugin> newGames) 
        {
            if (newGames == null) throw new ArgumentNullException(nameof(newGames));

            var gamePlugins = newGames as IGamePlugin[] ?? newGames.ToArray();
            if (!gamePlugins.Any()) throw new ArgumentException("Dont spam empty messages");

            this.NewGames = gamePlugins;
        }
        

        public IEnumerable<IGamePlugin> NewGames { get; }
    }
}