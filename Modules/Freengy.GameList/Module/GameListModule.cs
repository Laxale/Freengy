// Created by Laxale 23.10.2016
//
//


namespace Freengy.GameList.Module 
{
    using System;

    using Freengy.GameList.Views;
    using Freengy.Base.Interfaces;
    using Freengy.GameList.Settings;
    using Freengy.Settings.Interfaces;
    using Freengy.GameList.Diagnostics;
    using Freengy.Diagnostics.Interfaces;
    
    using Prism.Modularity;

    using Catel.IoC;


    public class GameListModule : IUiModule, IModule 
    {
        public Type ExportedViewType { get; } = typeof (GameListView);

        public void Initialize() 
        {
            var controller = ServiceLocator.Default.ResolveType<IDiagnosticsController>();
            controller.RegisterCategory(new GameListDiagnosticsCategory());
            
            var settingsFacade = ServiceLocator.Default.ResolveType<ISettingsFacade>();
            //settingsFacade.RegisterUnit(GameListSettingsUnit.Instance);
        }
    }
}