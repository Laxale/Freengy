// Created by Laxale 23.10.2016
//
//


namespace Freengy.GameList.Module 
{
    using System;

    using Freengy.GameList.Views;
    using Freengy.Base.Interfaces;
    

    public class FriendlistModule : IUiModule 
//        , IModule 
// remove this if no any types to register
    {
        #region Singleton

        private static FriendlistModule instance;

        private FriendlistModule() 
        {

        }

        public static FriendlistModule Instance => FriendlistModule.instance ?? (FriendlistModule.instance = new FriendlistModule());

        #endregion Singleton


        private Type exportedViewType;
        public Type ExportedViewType => this.exportedViewType ?? (this.exportedViewType = typeof (GameListView));


//        public void Initialize() 
//        {
//            ServiceLocator.Default.RegisterType<i>
//        }
    }
}