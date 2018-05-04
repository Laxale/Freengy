// Created by Laxale 04.05.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Freengy.Base.DbContext;
using Freengy.Base.Extensions;
using Freengy.Base.Interfaces;
using Freengy.Common.Helpers.ErrorReason;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;

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
                        context.Objects.Add(model);
                    }
                    else
                    {
                        // TODO update props
                    }

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
                        var albums = context.Objects.ToList().Select(model => new Album(model));

                        return Result<IEnumerable<Album>>.Ok(albums);
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