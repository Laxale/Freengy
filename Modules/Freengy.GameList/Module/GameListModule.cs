// Created by Laxale 23.10.2016
//
//


namespace Freengy.GameList.Module 
{
    using System;

    using Freengy.GameList.Views;
    using Freengy.Base.Interfaces;
    using Freengy.GameList.Diagnostics;
    using Freengy.Diagnostics.Interfaces;
    
    using Prism.Modularity;

    using Catel.IoC;


    public class GameListModule : IUiModule, IModule 
    {
        #region Singleton

        private static GameListModule instance;

        private GameListModule() 
        {

        }

        public static GameListModule Instance => GameListModule.instance ?? (GameListModule.instance = new GameListModule());

        #endregion Singleton


        public Type ExportedViewType { get; } = typeof (GameListView);

        public void Initialize() 
        {
            ServiceLocator.Default.ResolveType<IDiagnosticsController>().RegisterCategory(new GameListDiagnosticsCategory());
        }
    }
}