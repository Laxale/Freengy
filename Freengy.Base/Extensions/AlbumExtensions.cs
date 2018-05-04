// Created by Laxale 04.05.2018
//
//

using Freengy.Common.Extensions;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Extensions 
{
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
    }
}