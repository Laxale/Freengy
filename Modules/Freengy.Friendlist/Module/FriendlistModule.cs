// Created by Laxale 23.10.2016
//
//


namespace Freengy.Friendlist.Module 
{
    using System;

    using Freengy.Base.Interfaces;
    using Freengy.FriendList.Views;

    using Prism.Modularity;

    using Catel.IoC;


    public class FriendListModule : IUiModule
//        , IModule 
// remove this if no any types to register
    {
        #region Singleton

        private static FriendListModule instance;

        private FriendListModule() 
        {

        }

        public static FriendListModule Instance => FriendListModule.instance ?? (FriendListModule.instance = new FriendListModule());

        #endregion Singleton


        public Type ExportedViewType { get; } = typeof (FriendListView);


//        public void Initialize() 
//        {
//            ServiceLocator.Default.RegisterType<i>
//        }
    }
}