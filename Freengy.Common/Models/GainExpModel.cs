// Created by Laxale 14.05.2018
//
//

using System;

using Freengy.Common.Enums;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Модель начисления аккаунту экспы.
    /// </summary>
    public class GainExpModel 
    {
        /// <summary>
        /// Возвращает или задаёт количество начисленого опыта.
        /// </summary>
        public uint Amount { get; set; }

        /// <summary>
        /// Возвращает или задаёт метку времени начисления опыта.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Возвращает или задаёт причину начисления опыта.
        /// </summary>
        public GainExpReason GainReason { get; set; }
    }
}