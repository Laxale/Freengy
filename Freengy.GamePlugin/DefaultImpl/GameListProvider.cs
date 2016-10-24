// Created by Laxale 24.10.2016
//
//


namespace Freengy.GamePlugin.DefaultImpl
{
    using System;
    using System.Collections.Generic;

    using Freengy.GamePlugin.Interfaces;


    public class GameListProvider : IGameListProvider 
    {
        #region Singleton

        private static GameListProvider instance;

        private GameListProvider() 
        {

        }

        public static GameListProvider Instance => GameListProvider.instance ?? (GameListProvider.instance = new GameListProvider());

        #endregion Singleton


        public IEnumerable<IGamePlugin> GetInstalledGames() 
        {
            throw new NotImplementedException();
        }
    }
}