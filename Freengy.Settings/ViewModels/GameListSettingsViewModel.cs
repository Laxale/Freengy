// Created by Laxale 27.11.2016
//
//

using System;
using System.Windows;
using System.Windows.Forms;
using System.Threading.Tasks;

using Freengy.Base.Extensions;
using Freengy.Settings.Messages;
using Freengy.Settings.ModuleSettings;

using Catel.Data;
using Catel.MVVM;
using Catel.Messaging;

using LocalRes = Freengy.Settings.Properties.Resources;


namespace Freengy.Settings.ViewModels 
{   
    internal sealed class GameListSettingsViewModel : UnitViewModelBase 
    {
        private GameListSettingsUnit gameListUnit;


        public GameListSettingsViewModel() 
        {
            Task.Factory.StartNew(this.InitializeAsync);

            base.messageMediator.Register<MessageSaveRequest>(this, this.MessageListener);
        }
        

        public Command CommandSelectGamesFolder { get; private set; }


        #region override

        public override string ToString() 
        {
            return LocalRes.GameListSettingsTitle;
        }

        protected override void SetupCommands() 
        {
            this.CommandSelectGamesFolder = new Command(this.SelectGamesFolderImpl);
        }

        protected override async Task InitializeAsync() 
        {
            await
                Task.Factory.StartNew
                (
                    async () =>
                    {
                        base.IsWaiting = true;

                        await base.InitializeAsync();
                        
                        this.gameListUnit = base.SettingsRepository.GetOrCreateUnit<GameListSettingsUnit>();

                        await this.FillPropertiesFromDatabase();

                        // call this in ctor if viewmodel is not created by catel
                        this.SetupCommands();
                    }
                )
                .ContinueWith(this.InitializationContinuator);
        }

        protected override void InitializationContinuator(Task parentTask) 
        {
            base.IsWaiting = false;

            if (parentTask.Exception != null)
            {
                System.Windows.MessageBox.Show(parentTask.Exception.GetReallyRootException().Message, "GameList");
            }
        }

        #endregion override


        #region properties

        public string GamesFolderPath 
        {
            get { return (string)GetValue(GamesFolderPathProperty); }
            private set
            {
                SetValue(GamesFolderPathProperty, value);

                if (this.LoadedFromDatabase) this.gameListUnit.GamesFolderPath = value;
            }
        }

        public static readonly PropertyData GamesFolderPathProperty =
            ModelBase.RegisterProperty<GameListSettingsViewModel, string>(viewModel => viewModel.GamesFolderPath, () => string.Empty);
        
        #endregion properties


        protected override async Task FillPropertiesFromDatabase() 
        {
            Action fillAction =
                () =>
                {
                    this.GamesFolderPath = this.gameListUnit.GamesFolderPath;

                    base.LoadedFromDatabase = true;
                };

            await base.taskWrapper.Wrap(fillAction, task => base.IsDirty = false);
        }


        private void SelectGamesFolderImpl() 
        {
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                SelectedPath = Environment.CurrentDirectory
            };
            
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.GamesFolderPath = dialog.SelectedPath;
            }
        }

        [MessageRecipient]
        private void MessageListener(MessageSaveRequest requestMessage) 
        {
            if (!this.IsDirty) return;

            this.SettingsRepository.UpdateUnit(this.gameListUnit);

            base.IsDirty = false;
        }
    }
}