// Created by Laxale 22.10.2016
//
//

using Freengy.Base.Messages.Base;

namespace Freengy.GamePlugin.Messages
{
    using System;

    using Freengy.Base.Messages;


    /// <summary>
    /// Indicates that game is ready accepted to load its UI
    /// </summary>
    public class MessageRequestGameUi : MessageBase 
    {
        public MessageRequestGameUi(Type gameUiType) 
        {
            this.GameUiType = gameUiType;
        }


        public Type GameUiType { get; }
    }
}