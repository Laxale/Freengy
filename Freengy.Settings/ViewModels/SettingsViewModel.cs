﻿// Created by Laxale 27.11.2016
//
//

using System;
using System.Windows;
using System.Collections.Generic;
using Freengy.Base.DefaultImpl;
using Freengy.Base.ViewModels;
using Freengy.Base.Helpers.Commands;
using Freengy.Settings.Messages;


namespace Freengy.Settings.ViewModels 
{
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
            this.changedSettings.HasChangedSettingsLeft += SetCanSaveState;
            this.changedSettings.NoChangedSettingsLeft  += SetCannotSaveState;

            this.Subscribe<MessageSettingChanged>(MessageListener);

            this.SetupCommands();
        }


        public bool ShowWindowTitle => false;

        public MyCommand CommandSave { get; private set; }

        public MyCommand<Window> CommandClose{ get; private set; }


        protected override void SetupCommands() 
        {
            this.CommandSave  = new MyCommand(SaveImpl, this.CanSave);
            this.CommandClose = new MyCommand<Window>(window => window.Close());
        }


        private void SaveImpl() 
        {
            var saveRequestMessage = new MessageSaveRequest();

            this.Publish(saveRequestMessage);
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

        private void MessageListener(MessageSettingChanged isDirtyMesage) 
        {
            this.changedSettings.SetChangedState(isDirtyMesage.SettingsUnitName, isDirtyMesage.IsChanged);
        }
    }
}