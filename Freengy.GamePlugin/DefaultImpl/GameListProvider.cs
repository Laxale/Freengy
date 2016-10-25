// Created by Laxale 24.10.2016
//
//


using Freengy.GamePlugin.Constants;


namespace Freengy.GamePlugin.DefaultImpl
{
    using System;
    using System.Collections.Generic;

    using Freengy.Base.Interfaces;
    using Freengy.GamePlugin.Interfaces;
    
    using Catel.IoC;


    public class GameListProvider : IGameListProvider
    {
        private readonly IAppDirectoryInspector directoryInspector;


        #region Singleton

        private static GameListProvider instance;

        private GameListProvider() 
        {
            this.directoryInspector = ServiceLocator.Default.ResolveType<IAppDirectoryInspector>();
        }

        public static GameListProvider Instance => GameListProvider.instance ?? (GameListProvider.instance = new GameListProvider());

        #endregion Singleton


        public IEnumerable<IGamePlugin> GetInstalledGames() 
        {
            var dllsInGameFolder = this.directoryInspector.GetDllsInSubFolder(StringConstants.GamesFolderName);

            // TODO: find dlls implementing plugin interface

            return dllsInGameFolder;
        }
    }
}