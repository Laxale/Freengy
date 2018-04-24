// Created by Laxale 24.04.2018
//
//

using Freengy.Base.Chat.Interfaces;
using Freengy.Base.Extensions;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;

using Catel.IoC;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// A chat message network sender.
    /// </summary>
    public class ChatMessageSender 
    {
        public void SendMessageToFriend(IChatMessageDecorator messageDecorator, AccountState account) 
        {
            using (var actor = ServiceLocator.Default.ResolveType<IHttpActor>())
            {
                string chatAddress = $"{ account.UserAddress }{ Url.Http.Chat.ChatSubRoute }";
                actor.SetRequestAddress(chatAddress);

                ChatMessageModel messageModel = messageDecorator.ToModel();

                var result = actor.PostAsync<ChatMessageModel, ChatMessageModel>(messageModel).Result;
            }
        }
    }
}