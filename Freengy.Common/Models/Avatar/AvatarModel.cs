// Created by Laxale 15.05.2018
//
//

using System;
using System.Collections.Generic;

using Freengy.Common.Database;
using Freengy.Common.Interfaces;


namespace Freengy.Common.Models.Avatar 
{
    /// <summary>
    /// Модель аватара пользователя или альбома или группы или чего угодно.
    /// </summary>
    public class AvatarModel : IObjectWithId 
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
        /// Возвращает или задаёт бинарный блоб изображения.
        /// </summary>
        public byte[] ImageBlob { get; set; }

        /// <summary>
        /// Возвращает или задаёт дату последней модификации аватара.
        /// </summary>
        public DateTime LastModified { get; set; }
    }
}