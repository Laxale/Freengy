// Created by Laxale 19.10.2016
//
//


namespace Freengy.UI.ViewModels 
{
    using System;

    using Catel.MVVM;

    using Freengy.Base.ViewModels;


    public class ShellViewModel : WaitableViewModel 
    {
        protected override void SetupCommands() 
        {
            this.CommandShowSettings = new Command(this.CommandShowSettingsImpl);
        }


        public Command CommandShowSettings { get; private set; }


        private void CommandShowSettingsImpl() 
        {
            // TODO: implement this pls )
            var t = 0;
        }
    }
}