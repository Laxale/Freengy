// Created by Laxale 15.05.2018
//
//

using System;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Модель метки времени последней модификации объекта.
    /// </summary>
    public class ObjectModificationTime 
    {
        /// <summary>
        /// Идентификатор объекта.
        /// </summary>
        public Guid ObjectId { get; set; }

        /// <summary>
        /// Время последней модификации объекта.
        /// </summary>
        public DateTime ModificationTime { get; set; }
    }
}