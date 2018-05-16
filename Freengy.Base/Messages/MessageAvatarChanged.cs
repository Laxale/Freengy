// Created by Laxale 16.05.2018
//
//

using Freengy.Base.Messages.Base;


namespace Freengy.Base.Messages 
{
    /// <summary>
    /// Сообщение о том, что мой аватар изменился.
    /// </summary>
    public class MessageAvatarChanged : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageAvatarChanged"/> с заданным путём к файлу аватара.
        /// </summary>
        /// <param name="newAvatarPath">Путь к новому файлу аватара.</param>
        public MessageAvatarChanged(string newAvatarPath) 
        {
            NewAvatarPath = newAvatarPath;
        }


        /// <summary>
        /// Возвращает путь к новому файлу аватара.
        /// </summary>
        public string NewAvatarPath { get; }
    }
}