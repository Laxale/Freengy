// Created by Laxale 27.11.2016
//
//


using System.Threading.Tasks;


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

        protected override Task FillPropertiesFromDatabase()
        {
            return Task.FromResult(0);
        }
    }
}