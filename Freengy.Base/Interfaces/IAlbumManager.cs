// Created by Laxale 04.05.2018
//
//


using System.Collections.Generic;
using System.Threading.Tasks;
using Freengy.Base.Models.Readonly;
using Freengy.Common.Helpers.Result;


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Интерфейс управляющего альбомами.
    /// </summary>
    public interface IAlbumManager 
    {
        /// <summary>
        /// Сохранить состояние альбома или добавить, если он новый.
        /// </summary>
        /// <param name="album">Альбом для сохранения.</param>
        /// <returns>Результат сохранения.</returns>
        Result SaveAlbum(Album album);

        /// <summary>
        /// Асинхронно получить коллекцию моих сохранённых альбомов.
        /// </summary>
        /// <returns>Результат поиска моих альбомов.</returns>
        Task<Result<IEnumerable<Album>>> GetMyAlbums();
    }
}