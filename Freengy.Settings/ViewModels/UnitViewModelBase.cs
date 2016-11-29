// Created by Laxale 27.11.2016
//
//


namespace Freengy.Settings.ViewModels 
{
    using System.ComponentModel;

    using Freengy.Base.ViewModels;
    using Freengy.Settings.Messages;
    using Freengy.Settings.Interfaces;
    using Freengy.Settings.DefaultImpl;


    internal abstract class UnitViewModelBase : WaitableViewModel
    {
        protected readonly ISettingsFacade settingsFacade = SettingsFacade.Instance;


        protected UnitViewModelBase() 
        {
            this.PropertyChanged += this.PropertyChangedListener;
        }

        private void PropertyChangedListener(object sender, PropertyChangedEventArgs args) 
        {
            if (args.PropertyName != nameof(base.IsDirty)) return;

            var isDirtyMessage = new MessageSettingChanged();
            // inform Settings window that some of settings changed
            base.messageMediator.SendMessage(isDirtyMessage);
        }
    }
}