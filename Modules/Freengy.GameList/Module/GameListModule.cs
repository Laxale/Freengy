// Created by Laxale 23.10.2016
//
//


namespace Freengy.GameList.Module 
{
    using System;

    using Freengy.GameList.Views;
    using Freengy.Base.Interfaces;
    

    public class GameListModule : IUiModule 
//        , IModule 
// remove this if no any types to register
    {
        #region Singleton

        private static GameListModule instance;

        private GameListModule() 
        {

        }

        public static GameListModule Instance => GameListModule.instance ?? (GameListModule.instance = new GameListModule());

        #endregion Singleton


        private Type exportedViewType;
        public Type ExportedViewType => this.exportedViewType ?? (this.exportedViewType = typeof (GameListView));


//        public void Initialize() 
//        {
//            ServiceLocator.Default.RegisterType<i>
//        }
    }
}