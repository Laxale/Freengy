// Created by Laxale 16.05.2018
//
//

using System;

using Freengy.Base.Interfaces;
using Freengy.Base.Models;
using Freengy.Common.Helpers.Result;


namespace Freengy.Base.DefaultImpl 
{
    /// <summary>
    /// Дефолтная реализация <see cref="IImageCacher"/>.
    /// </summary>
    internal class ImageCacher : IImageCacher 
    {
        private static readonly object Locker = new object();

        private static ImageCacher instance;


        private ImageCacher() { }


        /// <summary>
        /// Единственный инстанс <see cref="ImageCacher"/>.
        /// </summary>
        public static IImageCacher Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new ImageCacher());
                }
            }
        }


        /// <summary>
        /// Создать картинку из блоба и вернуть путь к ней.
        /// </summary>
        /// <param name="imageBlob">Бинарный блоб изображения.</param>
        /// <returns>Результат кэширования.</returns>
        public Result<string> CacheImage(byte[] imageBlob) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Кэшировать картинку-аватар.
        /// </summary>
        /// <param name="avatarModel">Модель аватара.</param>
        /// <returns>Результат кэширования.</returns>
        public Result<string> CacheAvatar(UserAvatarModel avatarModel) 
        {
            throw new NotImplementedException();
        }
    }
}