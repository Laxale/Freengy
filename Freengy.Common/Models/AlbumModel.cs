// Created by Laxale 04.05.2018
//
//

using System;
using System.Collections.Generic;

using Freengy.Common.Database;
using Freengy.Common.Interfaces;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Модель хранимого альбома изображений.
    /// </summary>
    public class AlbumModel : DbObject, INamedObject 
    {
        /// <summary>
        /// Gets or sets the name of album.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the creation timestamp of album.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets album's owner account.
        /// </summary>
        public UserAccountModel OwnerAccountModel { get; set; }

        /// <summary>
        /// Изображения, принадлежащие данному альбому.
        /// </summary>
        public List<ImageModel> Images { get; set; } = new List<ImageModel>();
    }
}