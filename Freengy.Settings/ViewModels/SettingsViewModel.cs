// Created by Laxale 27.11.2016
//
//


namespace Freengy.Settings.ViewModels 
{
    using System;
    using System.Windows;

    using Freengy.Base.ViewModels;
    using Freengy.Settings.Messages;

    using Catel.MVVM;
    using Catel.Messaging;


    public sealed class SettingsViewModel : WaitableViewModel 
    {

        public Command CommandSave { get; private set; }
        public Command<Window> CommandClose{ get; private set; }


        public bool ShowWindowTitle => false;

        protected override void SetupCommands() 
        {
            this.CommandSave  = new Command(this.SaveImpl);
            this.CommandClose = new Command<Window>(window => window.Close());
        }


        private void SaveImpl() 
        {
            var saveRequestMessage = new MessageSaveRequest();

            base.messageMediator.SendMessage(saveRequestMessage);
        }
    }
}