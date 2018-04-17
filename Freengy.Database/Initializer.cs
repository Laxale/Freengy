// Created by Laxale 17.04.2018
//
//

using System;
using System.Collections.Generic;
using System.IO;

using NLog;


namespace Freengy.Database 
{
    /// <summary>
    /// Static initializer for database interactions.
    /// </summary>
    public static class Initializer 
    {
        private static string dbFileName;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Set the path to a storage folder for database.
        /// </summary>
        /// <param name="dbFileName">Full path to a storage folder for database.</param>
        public static void SetDbFileName(string dbFileName) 
        {
            if (string.IsNullOrWhiteSpace(dbFileName)) throw new ArgumentNullException(nameof(dbFileName));

            Initializer.dbFileName = dbFileName;
        }

        public static void SetStorageDirectoryPath(string storageFolderPath) 
        {
            EnsureDirectoryExistsUnsafely(storageFolderPath);

            if (!Directory.Exists(storageFolderPath))
            {
                throw new InvalidOperationException($"Database folder '{ storageFolderPath }' doesnt exist");
            }

            StorageDirectoryPath = storageFolderPath;
        }

        public static string GetFolderPathInAppData(string folderSubName) 
        {
            if (string.IsNullOrWhiteSpace(folderSubName)) throw new ArgumentNullException(nameof(folderSubName));

            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), folderSubName);

            return folderPath;
        }


        /// <summary>
        /// Gets the path to a databse file.
        /// </summary>
        public static string DbFilePath => Path.Combine(StorageDirectoryPath, dbFileName);

        /// <summary>
        /// Gets the path to a database storage folder.
        /// </summary>
        public static string StorageDirectoryPath { get; private set; }


        private static void EnsureDirectoryExistsUnsafely(string directory) 
        {
            if (!Directory.Exists(directory))
            {
                logger.Debug($"Folder {directory} doesnt exist. Going to create");
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Не удалось создать папку {directory}");
                    throw new Exception($"Не удалось создать папку {directory}", ex);
                }
            }
        }
    }
}