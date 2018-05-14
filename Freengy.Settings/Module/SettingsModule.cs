// Created by Laxale 23.11.2016
//
//

using Freengy.Database;
using Freengy.Base.Helpers;
using Freengy.Common.Constants;
using Freengy.Settings.Constants;
using Freengy.Settings.Interfaces;
using Freengy.Settings.DefaultImpl;
using Freengy.Base.DefaultImpl;
using Freengy.Common.Helpers;

using Prism.Modularity;


namespace Freengy.Settings.Module 
{
    public sealed class SettingsModule : IModule 
    {
        public void Initialize() 
        {
            using (new StatisticsDeployer(nameof(SettingsModule)))
            {
                string appDataFolderPath = Initializer.GetFolderPathInAppData(FreengyPaths.AppDataRootFolderName);
                Initializer.SetStorageDirectoryPath(appDataFolderPath);
                Initializer.SetDbFileName(SettingsConstants.SettingsDbFileName);

                MyServiceLocator.Instance.RegisterInstance<ISettingsRepository>(SettingsRepository.Instance);
            }
        }
    }
}