// Created by Laxale 07.12.2016
//
//


namespace Freengy.GamePlugin.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Configuration;

    using Freengy.GamePlugin.Constants;


    /// <summary>
    /// Decides if passed directory is a game directory based on presense of special file in it
    /// </summary>
    internal class GameFolderConfigFilterStrategy : GameDirectoryFilterStrategyBase
    {
        public override bool IsGameFolder(string folderPath, out string gameDllPath)
        {
            gameDllPath = null;

            if (string.IsNullOrWhiteSpace(folderPath)) throw new ArgumentNullException(nameof(folderPath));

            var folderInfo = new DirectoryInfo(folderPath);

            var dllFiles = folderInfo.EnumerateFiles("*.dll").Select(fileInfo => fileInfo.FullName);

            foreach (var dllFile in dllFiles)
            {
                try
                {
                    var config = ConfigurationManager.OpenExeConfiguration(dllFile);

                    читает пустоту из нормального конфига
                    if (config.AppSettings.Settings.AllKeys.Contains(StringConstants.MainGameViewSettingsKey))
                    {
                        gameDllPath = dllFile;
                        return true;
                    }
                }
                catch (Exception)
                {
                    // ignore
                    throw;
                }    
            }
            
            return false;
        }
    }
}