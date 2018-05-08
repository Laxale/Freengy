// Created by Laxale 24.04.2018
//
//

using Freengy.Base.Chat.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Base.Extensions;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.Common.Interfaces;
using Freengy.Networking.Constants;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// A chat message network sender.
    /// </summary>
    public class ChatMessageSender 
    {
        public void SendMessageToFriend(IChatMessageDecorator messageDecorator, AccountState account) 
        {
            using (var actor = MyServiceLocator.Instance.Resolve<IHttpActor>())
            {
                string chatAddress = $"{ account.UserAddress.TrimEnd('/') }{ Url.Http.Chat.ChatSubRoute }";
                actor.SetRequestAddress(chatAddress);

                ChatMessageModel messageModel = messageDecorator.ToModel();

                var result = actor.PostAsync<ChatMessageModel, ChatMessageModel>(messageModel).Result;
            }
        }
    }
}