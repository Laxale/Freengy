// Created by Laxale 27.11.2016
//
//


using System;
using System.Threading.Tasks;

using Freengy.Base.ViewModels;

using LocalRes = Freengy.Settings.Properties.Resources;


namespace Freengy.Settings.ViewModels 
{
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