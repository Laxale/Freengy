// Created by Laxale 04.05.2018
//
//

using System;
using System.Collections.Generic;

using Freengy.Common.Database;
using Freengy.Common.Interfaces;


namespace Freengy.Common.Models.Readonly 
{
    public class Album : IObjectWithId, INamedObject  
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

        public List<ImageModel> Images { get; } = new List<ImageModel>();
    }
}