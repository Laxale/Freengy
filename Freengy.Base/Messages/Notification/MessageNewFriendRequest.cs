// Created by Laxale 07.05.2018
//
//

using Freengy.Base.Messages.Base;
using Freengy.Common.Enums;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Messages.Notification 
{
    /// <summary>
    /// Сообщение о новом запросе в друзья.
    /// </summary>
    public class MessageNewFriendRequest : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageNewFriendRequest"/> по данной модели запроса в друзья.
        /// </summary>
        /// <param name="newFriendRequest">Модель запроса в друзья.</param>
        public MessageNewFriendRequest(FriendRequest newFriendRequest) 
        {
            NewFriendRequest = newFriendRequest;
        }

        
        /// <summary>
        /// Модель запроса в друзья.
        /// </summary>
        public FriendRequest NewFriendRequest { get; }
    }
}