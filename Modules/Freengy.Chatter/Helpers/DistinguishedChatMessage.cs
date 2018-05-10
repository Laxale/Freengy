// Created by Laxale 10.06.2018
//
//

using Freengy.Base.Chat.Interfaces;


namespace Freengy.Chatter.Helpers 
{
    /// <summary>
    /// Вспомогательная обёртка для удобства отображения сообщений.
    /// </summary>
    internal class DistinguishedChatMessage 
    {
        public DistinguishedChatMessage(IChatMessageDecorator messageDecorator, bool isMy) 
        {
            IsMy = isMy;
            MessageDecorator = messageDecorator;
        }


        /// <summary>
        /// Возвращает значение - моё ли это сообщение.
        /// </summary>
        public bool IsMy { get; }

        public IChatMessageDecorator MessageDecorator { get; }
    }
}