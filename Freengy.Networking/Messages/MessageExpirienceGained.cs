// Created by Laxale 14.05.2018
//
//

using Freengy.Base.Messages.Base;
using Freengy.Common.Enums;


namespace Freengy.Networking.Messages 
{
    /// <summary>
    /// Сообщение о том, что получен опыт.
    /// </summary>
    public class MessageExpirienceGained : MessageBase 
    {
        /// <summary>
        /// Конструирует <see cref="MessageExpirienceGained"/> с заданной причиной и количеством полученного опыта.
        /// </summary>
        /// <param name="gainReason">Причина получения опыта.</param>
        /// <param name="amount">Количество полученного опыта.</param>
        public MessageExpirienceGained(GainExpReason gainReason, uint amount)
        {
            GainReason = gainReason;
            Amount = amount;
        }


        /// <summary>
        /// Возвращает количество полученного опыта.
        /// </summary>
        public uint Amount { get; }

        /// <summary>
        /// Возвращает причину получения опыта.
        /// </summary>
        public GainExpReason GainReason { get; }
    }
}