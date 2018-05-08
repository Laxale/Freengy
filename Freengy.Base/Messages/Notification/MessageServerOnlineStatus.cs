// Created by Laxale 05.05.2018
//
//


using Freengy.Base.Messages.Base;

namespace Freengy.Base.Messages.Notification 
{
    /// <summary>
    /// Сообщение об онлайн-статусе сервера.
    /// </summary>
    public class MessageServerOnlineStatus : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageServerOnlineStatus"/> с заданным флагом статуса сервера.
        /// </summary>
        /// <param name="isOnline">Онлайн ли сейчас сервер.</param>
        public MessageServerOnlineStatus(bool isOnline) 
        {
            IsOnline = isOnline;
        }


        /// <summary>
        /// Возвращает значение - онлайн ли сейчас сервер.
        /// </summary>
        public bool IsOnline { get; }
    }
}