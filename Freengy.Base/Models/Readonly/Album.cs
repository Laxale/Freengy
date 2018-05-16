// Created by Laxale 04.05.2018
//
//

using System;
using System.Collections.Generic;
using Freengy.Common.Interfaces;
using Freengy.Common.Models;

namespace Freengy.Base.Models.Readonly 
{
    /// <summary>
    /// Представляет собой readonly обёртку над моделью данных <see cref="AlbumModel"/>.
    /// </summary>
    public class Album : INamedObject, IObjectWithId 
    {
        private readonly AlbumModel model;


        public Album(AlbumModel model) 
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            Id = model.Id;
            Name = model.Name;
            CreationTime = model.CreationTime;
            OwnerAccount = new UserAccount(model.OwnerAccountModel);

            model.Images.ForEach(Images.Add);
        }


        /// <summary>
        /// Returns unique identifier of an implementer object.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Returns the name of an implementer object.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the creation timestamp of album.
        /// </summary>
        public DateTime CreationTime { get; }

        /// <summary>
        /// Returns albums's owner user account.
        /// </summary>
        public UserAccount OwnerAccount { get; }

        /// <summary>
        /// Gets the collection of album images.
        /// </summary>
        public List<ImageModel> Images { get; } = new List<ImageModel>();
    }
}