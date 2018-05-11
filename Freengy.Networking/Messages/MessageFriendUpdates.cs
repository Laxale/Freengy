// Created by Laxale 1.05.2018
//
//

using System.Collections.Generic;

using Freengy.Base.Models.Update;


namespace Freengy.Networking.Messages 
{
    /// <summary>
    /// Сообщение о получении изменений в аккаунте друга.
    /// </summary>
    public class MessageFriendUpdates 
    {
        /// <summary>
        /// Конструирует <see cref="MessageFriendUpdates"/> с заданной колекцией изменений.
        /// </summary>
        /// <param name="updates">Коллекция произошедших в аккаунте изменений.</param>
        public MessageFriendUpdates(IEnumerable<FriendUpdate> updates) 
        {
            Updates = updates;
        }


        /// <summary>
        /// Коллекция произошедших в аккаунте изменений.
        /// </summary>
        public IEnumerable<FriendUpdate> Updates { get; }
    }
}