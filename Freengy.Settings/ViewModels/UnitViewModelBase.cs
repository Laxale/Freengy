﻿// Created by Laxale 27.11.2016
//
//

using System.ComponentModel;
using System.Threading.Tasks;

using Freengy.Base.DefaultImpl;
using Freengy.Base.ViewModels;
using Freengy.Settings.Messages;
using Freengy.Settings.Interfaces;
using Freengy.Settings.ModuleSettings;


namespace Freengy.Settings.ViewModels 
{
    internal abstract class UnitViewModelBase : WaitableViewModel
    {
        protected readonly ISettingsRepository SettingsRepository = DefaultImpl.SettingsRepository.Instance;


        protected UnitViewModelBase() 
        {
            this.PropertyChanged += this.PropertyChangedListener;
        }


        protected bool LoadedFromDatabase { get; set; }

        protected abstract Task FillPropertiesFromDatabase();


        private void PropertyChangedListener(object sender, PropertyChangedEventArgs args) 
        {
            if (!this.CanBroadcastDirtyState(args)) return;

            var isDirtyMessage = new MessageSettingChanged(this.ToString(), true);
            // inform Settings window that some of settings changed
            this.Publish(isDirtyMessage);
        }

        private bool CanBroadcastDirtyState(PropertyChangedEventArgs args) 
        {
            bool mustBroadcastDirty =
                // ignore inital settings properties in FillPropertiesFromDatabase()
                this.LoadedFromDatabase && 
                args.PropertyName == "IsDirty";

            return mustBroadcastDirty;
        }
    }
}