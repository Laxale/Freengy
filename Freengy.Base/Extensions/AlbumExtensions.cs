// Created by Laxale 04.05.2018
//
//

using Freengy.Base.Models.Readonly;
using Freengy.Common.Extensions;
using Freengy.Common.Models;


namespace Freengy.Base.Extensions 
{
    using System;


    /// <summary>
    /// Содержит расширения для моделей альбомов.
    /// </summary>
    public static class AlbumExtensions 
    {
        public static AlbumModel ToModel(this Album album) 
        {
            var model = new AlbumModel
            {
                Id = album.Id,
                Name = album.Name,
                OwnerAccountModel = album.OwnerAccount.ToModel(),
                CreationTime = album.CreationTime,
                OwnerAccountId = album.OwnerAccount.Id
            };

            album.Images.ForEach(model.Images.Add);

            return model;
        }

        public static void AcceptPropertiesFrom(this AlbumModel model, Album album) 
        {
            model.Name = album.Name;

            model.Images.Clear();
            model.Images.AddRange(album.Images);

            foreach (ImageModel image in model.Images)
            {
                image.PrepareMappedProps();
                image.ParentId = album.Id;
            }
        }

        public static bool IsImagePath(string url) 
        {
            if(string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));

            bool isImage =
                url.EndsWith(".jpg") ||
                url.EndsWith(".png") ||
                url.EndsWith(".bmp") ||
                url.EndsWith(".gif");

            return isImage;
        }
    }
}