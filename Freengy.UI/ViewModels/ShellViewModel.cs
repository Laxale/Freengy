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
        public ShellViewModel() : base(true) 
        {

        }


        protected override void SetupCommands() 
        {
            this.CommandShowSettings = new Command(this.CommandShowSettingsImpl);
        }


        public Command CommandShowSettings { get; private set; }


        private void CommandShowSettingsImpl() 
        {
            var t = 0;
        }
    }
}