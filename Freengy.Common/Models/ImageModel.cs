// Created by Laxale 04.05.2018
//
//

using Freengy.Common.Database;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Модель хранимого изображения.
    /// </summary>
    public class ImageModel : DbObject 
    {
        /// <summary>
        /// Локальный путь к изображению (если оно сохранено локально).
        /// </summary>
        public string LocalUrl { get; set; }

        /// <summary>
        /// Путь к данному изображению на неком сервере.
        /// </summary>
        public string RemoteUrl { get; set; }
    }
}