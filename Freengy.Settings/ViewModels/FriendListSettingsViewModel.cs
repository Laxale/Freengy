// Created by Laxale 27.11.2016
//
//


namespace Freengy.Settings.ViewModels 
{
    using System;

    using Freengy.Base.ViewModels;

    using LocalRes = Freengy.Settings.Resources;


    internal sealed class FriendListSettingsViewModel : UnitViewModelBase 
    {


        protected override void SetupCommands() 
        {
            
        }

        public override string ToString() 
        {
            return LocalRes.FriendListSettingsTitle;
        }
    }
}