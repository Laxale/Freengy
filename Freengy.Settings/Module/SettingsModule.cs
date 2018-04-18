// Created by Laxale 23.11.2016
//
//

using Freengy.Database;
using Freengy.Common.Constants;
using Freengy.Settings.Views;
using Freengy.Settings.Constants;
using Freengy.Settings.ViewModels;
using Freengy.Settings.Interfaces;
using Freengy.Settings.DefaultImpl;

using Catel.IoC;
using Catel.Services;

using Prism.Modularity;


namespace Freengy.Settings.Module 
{
    public sealed class SettingsModule : IModule 
    {
        public void Initialize() 
        {
            // instantiate a singleton
            var wut = Freengy.Settings.Helpers.DataContextSetter.Instance;

            string appDataFolderPath = Initializer.GetFolderPathInAppData(FreengyPaths.AppDataRootFolderName);
            Initializer.SetStorageDirectoryPath(appDataFolderPath);
            Initializer.SetDbFileName(SettingsConstants.SettingsDbFileName);

            ServiceLocator.Default.RegisterInstance<ISettingsRepository>(SettingsRepository.Instance);

            var vizualizer = ServiceLocator.Default.ResolveType<IUIVisualizerService>();
            vizualizer.Register<SettingsViewModel, SettingsWindow>();
        }
    }
}