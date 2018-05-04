// Created by Laxale 04.05.2018
//
//

using System;
using System.IO;


namespace Freengy.Common.Helpers 
{
    /// <summary>
    /// Содержит вспомогательные методы / пути к стандартным папкам.
    /// </summary>
    public class FolderHelper 
    {
        protected readonly string temp = "Temp";


        /// <summary>
        /// Путь к папке \AppData\Local\.
        /// </summary>
        public string LocalAppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        /// <summary>
        /// Путь к папке \Program Files\. Добавляется (x86) для х64 машины.
        /// </summary>
        public string ProgramFilesPath =>
            Environment.Is64BitOperatingSystem ?
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) :
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

        /// <summary>
        /// Путь к папке \ProgramData\.
        /// </summary>
        public string ProgramDataPath => Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        /// <summary>
        /// Путь к папке \Temp\.
        /// </summary>
        public string TempDirectoryPath => Path.Combine(LocalAppDataPath, temp);


        /// <summary>
        /// Получить префикс в виде форматированного времени DateTime.Now.
        /// </summary>
        /// <returns>Префикс для использования в названиях папок.</returns>
        public string GetDateTimeNowPrefix()
        {
            var now = DateTime.Now;

            return now.ToString("F").Replace(" ", "_").Replace(":", ".") + $".{ now.Millisecond }";
        }

        /// <summary>
        /// Получить строку, представляющую уникальный уникальный путь ко временной папке.
        /// </summary>
        /// <param name="prefix">Некий префикс для внедрения в название папки.</param>
        /// <returns>Уникальный уникальный путь ко временной папке.</returns>
        public string GetUniqueTempDirPath(string prefix)
        {
            //string uniqueDirPath = Path.Combine(LocalAppDataPath, temp, $"{ prefix }_{ Guid.NewGuid().ToString() }");
            string uniqueDirPath = Path.Combine(LocalAppDataPath, temp, $"{ prefix }_{ GetDateTimeNowPrefix() }");

            return uniqueDirPath;
        }
    }
}
