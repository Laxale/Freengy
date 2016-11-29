// Created by Laxale 27.11.2016
//
//


namespace Freengy.Settings.ViewModels
{
    using System;
    using System.Windows;
    using System.Collections.Generic;

    using Freengy.Base.ViewModels;
    using Freengy.Settings.Messages;

    using Catel.MVVM;
    using Catel.Messaging;


    internal class ChangedSettings 
    {
        private static readonly object Locker = new object();

        private readonly IDictionary<string, bool> changedSettings = new Dictionary<string, bool>();

        public event Action NoChangedSettingsLeft;
        public event Action HasChangedSettingsLeft;


        public void SetChangedState(string viewmodelName, bool ischanged) 
        {
            if (string.IsNullOrWhiteSpace(viewmodelName)) throw new ArgumentNullException(nameof(viewmodelName));

            lock (Locker)
            {
                try
                {
                    if (ischanged)
                    {
                        this.changedSettings.Add(viewmodelName, true);
                    }
                    else
                    {
                        this.changedSettings.Remove(viewmodelName);
                    }

                    if (this.changedSettings.Count == 0)
                    {
                        this.NoChangedSettingsLeft?.Invoke();
                    }
                    else
                    {
                        this.HasChangedSettingsLeft?.Invoke();
                    }
                }
                catch (Exception)
                {
                    //throw;
                }
            }
        }
    }


    public sealed class SettingsViewModel : WaitableViewModel
    {
        private bool canSave;
        private readonly ChangedSettings changedSettings = new ChangedSettings();


        public SettingsViewModel() 
        {
            this.changedSettings.HasChangedSettingsLeft += this.SetCanSaveState;
            this.changedSettings.NoChangedSettingsLeft  += this.SetCannotSaveState;

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
        private void SetCanSaveState() 
        {
            this.canSave = true;
            this.CommandSave.RaiseCanExecuteChanged();
        }
        private void SetCannotSaveState() 
        {
            this.canSave = false;
            this.CommandSave.RaiseCanExecuteChanged();
        }

        [MessageRecipient]
        private void MessageListener(MessageSettingChanged isDirtyMesage) 
        {
            this.changedSettings.SetChangedState(isDirtyMesage.SettingsUnitName, isDirtyMesage.IsChanged);
        }
    }
}