// Created by Laxale 16.05.2018
//
//

using Freengy.Base.Models;
using Freengy.Common.Helpers.Result;


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Интерфейс для реализации кэширования картинок.
    /// </summary>
    public interface IImageCacher 
    {
        /// <summary>
        /// Создать картинку из блоба и вернуть путь к ней.
        /// </summary>
        /// <param name="imageBlob">Бинарный блоб изображения.</param>
        /// <returns>Результат кэширования.</returns>
        Result<string> CacheImage(byte[] imageBlob);

        /// <summary>
        /// Кэшировать картинку-аватар.
        /// </summary>
        /// <param name="avatarModel">Модель аватара.</param>
        /// <returns>Результат кэширования.</returns>
        Result<string> CacheAvatar(UserAvatarModel avatarModel);
    }
}