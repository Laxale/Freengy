// Created by Laxale 24.04.2018
//
//

using System;
using Freengy.Common.Helpers;
using Freengy.Common.Models;
using Freengy.Networking.DefaultImpl;
using Nancy;

using Url = Freengy.Networking.Constants.Url;


namespace Freengy.Networking.Modules 
{
    /// <summary>
    /// Client chat module.
    /// </summary>
    public class ChatModule : NancyModule 
    {
        public ChatModule() 
        {
            Post[Url.Http.Chat.ChatSubRoute] = OnIncomingChatMessage;
        }


        private dynamic OnIncomingChatMessage(dynamic arg) 
        {
            var messageModel = new SerializeHelper().DeserializeObject<ChatMessageModel>(Request.Body);

            ServerListener.InternalInstance.InformOfANewMessage(messageModel);

            return HttpStatusCode.OK;
        }
    }
}