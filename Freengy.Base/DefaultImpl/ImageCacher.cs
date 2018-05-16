// Created by Laxale 16.05.2018
//
//

using System;
using System.IO;

using Freengy.Base.Interfaces;
using Freengy.Base.Models;
using Freengy.Common.Constants;
using Freengy.Common.ErrorReason;
using Freengy.Common.Helpers;
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

        private readonly string cacheFolderPath;


        private ImageCacher() 
        {
            string appDataPath = new FolderHelper().LocalAppDataPath;
            cacheFolderPath = Path.Combine(appDataPath, FreengyPaths.AppDataRootFolderName, "Avatars");

            if (!Directory.Exists(cacheFolderPath))
            {
                Directory.CreateDirectory(cacheFolderPath);
            }
        }


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
            try
            {
                var avatarName = $"User_Avatar_{avatarModel.ParentId}";
                var avatarPath = Path.Combine(cacheFolderPath, avatarName);

                if (File.Exists(avatarPath))
                {
                    File.WriteAllBytes(avatarPath, avatarModel.AvatarBlob);
                }
                else
                {
                    using (FileStream stream = File.Create(avatarPath))
                    {
                        stream.Write(avatarModel.AvatarBlob, 0, avatarModel.AvatarBlob.Length);
                    }
                }

                return Result.Ok(avatarPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<string>.Fail(new UnexpectedErrorReason(ex.Message));
            }
        }
    }
}