// Created by Laxale 04.05.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;

using Freengy.Base.DbContext;
using Freengy.Base.Extensions;
using Freengy.Base.Interfaces;
using Freengy.Base.Models.Readonly;
using Freengy.Common.ErrorReason;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models;
using NLog;


namespace Freengy.Base.DefaultImpl 
{
    /// <summary>
    /// <see cref="IAlbumManager"/> implementer.
    /// </summary>
    internal class AlbumManager : IAlbumManager 
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Сохранить состояние альбома или добавить, если он новый.
        /// </summary>
        /// <param name="album">Альбом для сохранения.</param>
        /// <returns>Результат сохранения.</returns>
        public Result SaveAlbum(Album album) 
        {
            try
            {
                if(album == null) throw new ArgumentNullException(nameof(album));

                using (var context = new ComplexAlbumContext())
                {
                    var existingAlbum = context.Objects.FirstOrDefault(albumModel => albumModel.Id == album.Id);

                    if (existingAlbum == null)
                    {
                        var model = album.ToModel();
                        model.PrepareMappedProps();
                        context.Objects.Add(model);
                    }
                    else
                    {
                        existingAlbum.PrepareMappedProps();
                        for(int index = 0; index < existingAlbum.Images.Count; index++)
                        {
                            context.Entry(existingAlbum.Images[index]).State = EntityState.Deleted;
                        }
                        
                        existingAlbum.AcceptPropertiesFrom(album);
                    }

                    context.SaveChanges();

                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Failed to save album '{ album?.Name }'");

                return Result<IEnumerable<Album>>.Fail(new UnexpectedErrorReason(ex.Message));
            }
        }

        /// <summary>
        /// Асинхронно получить коллекцию моих сохранённых альбомов.
        /// </summary>
        /// <returns>Результат поиска моих альбомов.</returns>
        public async Task<Result<IEnumerable<Album>>> GetMyAlbums() 
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var context = new ComplexAlbumContext())
                    {
                        var albumsProxies = context.Objects.Include(album => album.OwnerAccountModel).ToList();
                        var realModels = 
                                albumsProxies
                                    .Select(proxy => proxy.CreateFromProxy(proxy))
                                    .Select(model => new Album((AlbumModel)model))
                                    .ToList();

                        return Result<IEnumerable<Album>>.Ok(realModels);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Failed to get my albums");
                    return Result<IEnumerable<Album>>.Fail(new UnexpectedErrorReason(ex.Message));
                }
            });
        }
    }
}