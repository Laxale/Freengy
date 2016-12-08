// Created by Laxale 07.12.2016
//
//


namespace Freengy.GamePlugin.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

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
                if (this.TryCheckConfigFile(dllFile, out gameDllPath)) return true;
            }
            
            return false;
        }


        private bool TryCheckConfigFile(string dllFilePath, out string gameDllPath) 
        {
            gameDllPath = null;

            try
            {
                string configFilePath = $"{ dllFilePath }.config";

                if (File.Exists(configFilePath))
                {
                    XDocument config = XDocument.Parse(File.ReadAllText(configFilePath));

                    Func<XElement, bool> predicate =
                        element => 
                            element
                            .Attributes()
                            .Any
                            (
                                attribute => attribute.Value.ToString() == StringConstants.MainGameViewSettingsKey
                            );

                    bool hasMainViewTypeSetting = this.CheckNodeHasCertainChild(config.Root, predicate); 

                    if (hasMainViewTypeSetting)
                    {
                        gameDllPath = dllFilePath;
                        return true;
                    }
                }

            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        private bool CheckNodeHasCertainChild(XElement node, Func<XElement, bool> predicate) 
        {
            var nodeChildren = node?.Elements();

            return 
                nodeChildren != null && 
                (predicate(node) || nodeChildren.Any(nodeChild => this.CheckNodeHasCertainChild(nodeChild, predicate)));
        }
    }
}