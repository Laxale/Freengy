// Created by Laxale 16.05.2018
//
//

using System;
using System.IO;
using System.Windows.Media.Imaging;
using Freengy.Common.ErrorReason;
using Freengy.Common.Helpers.Result;


namespace Freengy.Base.Helpers 
{
    /// <summary>
    /// Загрузчик изображений. Позволяет не занимать дескриптор загруженного файла изображения.
    /// </summary>
    public class ImageLoader 
    {
        /// <summary>
        /// Загрузить изображение по указанному пути.
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public Result<BitmapImage> LoadBitmapImage(string imagePath) 
        {
            try
            {
                using (var stream = new FileStream(imagePath, FileMode.Open))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze(); // just in case you want to load the image in another thread

                    return Result<BitmapImage>.Ok(bitmapImage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<BitmapImage>.Fail(new UnexpectedErrorReason(ex.Message));
            }
        }
    }
}