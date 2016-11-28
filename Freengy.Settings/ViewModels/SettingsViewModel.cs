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
        private bool canSave;


        public SettingsViewModel() 
        {
            base.messageMediator.Register<MessageSettingChanged>(this, this.MessageListener);

            this.SetupCommands();
        }


        public bool ShowWindowTitle => false;

        public Command CommandSave { get; private set; }
        public Command<Window> CommandClose{ get; private set; }


        protected override void SetupCommands() 
        {
            this.CommandSave  = new Command(this.SaveImpl, this.CanSave);
            this.CommandClose = new Command<Window>(window => window.Close());
        }


        private void SaveImpl() 
        {
            var saveRequestMessage = new MessageSaveRequest();

            base.messageMediator.SendMessage(saveRequestMessage);
        }
        private bool CanSave()
        {
            return this.canSave;
        }

        [MessageRecipient]
        private void MessageListener(MessageSettingChanged isDirtyMesage) 
        {
            this.canSave = true;
            this.CommandSave.RaiseCanExecuteChanged();
        }
    }
}