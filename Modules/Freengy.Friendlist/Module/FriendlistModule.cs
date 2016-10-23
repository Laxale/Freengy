// Created by Laxale 23.10.2016
//
//


namespace Freengy.Friendlist.Module 
{
    using System;

    using Freengy.Base.Interfaces;
    using Freengy.Friendlist.Views;

    using Prism.Modularity;

    using Catel.IoC;


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
        public Type ExportedViewType => this.exportedViewType ?? (this.exportedViewType = typeof (FriendlistView));


//        public void Initialize() 
//        {
//            ServiceLocator.Default.RegisterType<i>
//        }
    }
}