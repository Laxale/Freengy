// Created by Laxale 15.05.2018
//
//

using System;

using Freengy.Common.Interfaces;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Модель двоичного блоба - любых данных в виде байт.
    /// </summary>
    public class BinaryDataModel : IObjectWithId 
    {
        /// <summary>
        /// Returns unique identifier of an implementer object.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Возвращает или задаёт идентификатор родительского объекта.
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// Возвращает или задаёт собственно блоб.
        /// </summary>
        public byte[] Blob { get; set; }

        /// <summary>
        /// Возвращает или задаёт дату последней модификации данных.
        /// </summary>
        public DateTime LastModified { get; set; }
    }
}