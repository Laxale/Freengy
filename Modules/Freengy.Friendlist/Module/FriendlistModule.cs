// Created by Laxale 23.10.2016
//
//


namespace Freengy.Friendlist.Module 
{
    using System;

    using Freengy.Base.Interfaces;
    using Freengy.Friendlist.Views;


    public class FriendlistModule : IUiModule 
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
    }
}