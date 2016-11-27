// Created by Laxale 19.10.2016
//
//


namespace Freengy.UI.ViewModels 
{
    using System;

    using Catel.IoC;
    using Catel.MVVM;
    
    using Freengy.Base.ViewModels;
    using Freengy.Settings.ViewModels;


    public class ShellViewModel : WaitableViewModel 
    {
        protected override void SetupCommands() 
        {
            this.CommandShowSettings = new Command(this.CommandShowSettingsImpl);
        }


        public Command CommandShowSettings { get; private set; }


        private async void CommandShowSettingsImpl() 
        {
            var settingsViewModel = base.serviceLocator.ResolveType<SettingsViewModel>();

            bool? result = await base.uiVisualizer.ShowDialogAsync(settingsViewModel);
        }
    }
}