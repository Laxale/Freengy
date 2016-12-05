﻿// Created by Laxale 27.11.2016
//
//


namespace Freengy.Settings.ViewModels 
{
    using System.ComponentModel;
    using System.Threading.Tasks;

    using Freengy.Base.ViewModels;
    using Freengy.Settings.Messages;
    using Freengy.Settings.Interfaces;
    using Freengy.Settings.ModuleSettings;

    using Catel.Messaging;


    internal abstract class UnitViewModelBase : WaitableViewModel
    {
        protected readonly ISettingsFacade SettingsFacade = DefaultImpl.SettingsFacade.Instance;


        protected UnitViewModelBase() 
        {
            this.PropertyChanged += this.PropertyChangedListener;
        }


        protected bool LoadedFromDatabase { get; set; }

        protected abstract Task FillPropertiesFromDatabase();


        private void PropertyChangedListener(object sender, PropertyChangedEventArgs args) 
        {
            if (!this.CanBroadcastDirtyState(args)) return;

            var isDirtyMessage = new MessageSettingChanged(this.ToString(), base.IsDirty);
            // inform Settings window that some of settings changed
            base.messageMediator.SendMessage(isDirtyMessage);
        }

        private bool CanBroadcastDirtyState(PropertyChangedEventArgs args) 
        {
            bool mustBroadcastDirty =
                // ignore inital settings properties in FillPropertiesFromDatabase()
                this.LoadedFromDatabase && 
                args.PropertyName == nameof(base.IsDirty);

            return mustBroadcastDirty;
        }
    }
}