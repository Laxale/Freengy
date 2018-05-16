// Created by Laxale 07.05.2018
//
//

using Freengy.Base.Messages.Base;
using Freengy.Base.Models.Readonly;
using Freengy.Common.Enums;
using Freengy.Common.Models;


namespace Freengy.Base.Messages.Notification 
{
    /// <summary>
    /// Сообщение об изменении статуса запроса в друзья.
    /// </summary>
    public class MessageFriendRequestState : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageFriendRequestState"/> по данной модели ответа на запрос.
        /// </summary>
        /// <param name="reply">Модель ответа на запрос в друзья.</param>
        public MessageFriendRequestState(FriendRequestReply reply) 
        {
            RequestReaction = reply.Reaction;
            RepliedAccount = new UserAccount(reply.Request.TargetAccount);
        }


        /// <summary>
        /// Реакция целевого аккаунта на запрос его в друзья.
        /// </summary>
        public UserAccount RepliedAccount { get; }

        /// <summary>
        /// Целевой аккаунт, ответивший на запрос в друзья.
        /// </summary>
        public FriendRequestReaction RequestReaction { get; }
    }
}