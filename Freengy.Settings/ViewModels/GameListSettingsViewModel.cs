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
            Task.Factory.StartNew(InitializeAsync);

            messageMediator.Register<MessageSaveRequest>(this, MessageListener);
        }
        

        public Command CommandSelectGamesFolder { get; private set; }


        #region override

        public override string ToString() 
        {
            return LocalRes.GameListSettingsTitle;
        }

        protected override void SetupCommands() 
        {
            CommandSelectGamesFolder = new Command(SelectGamesFolderImpl);
        }

        protected override async Task InitializeAsync() 
        {
            await Task.Factory
                .StartNew(async () =>
                {
                    SetBusy("Initializing game list");

                    await base.InitializeAsync();
                        
                    gameListUnit = SettingsRepository.GetOrCreateUnit<GameListSettingsUnit>();

                    await FillPropertiesFromDatabase();

                    // call this in ctor if viewmodel is not created by catel
                    //SetupCommands();
                })
                .ContinueWith(InitializationContinuator);
        }

        protected override void InitializationContinuator(Task parentTask) 
        {
            ClearBusyState();

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

                if (LoadedFromDatabase) gameListUnit.GamesFolderPath = value;
            }
        }

        public static readonly PropertyData GamesFolderPathProperty =
            RegisterProperty<GameListSettingsViewModel, string>(viewModel => viewModel.GamesFolderPath, () => string.Empty);
        
        #endregion properties


        protected override async Task FillPropertiesFromDatabase() 
        {
            void FillAction()
            {
                GamesFolderPath = gameListUnit.GamesFolderPath;

                LoadedFromDatabase = true;
            }

            await taskWrapper.Wrap(FillAction, task => IsDirty = false);
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
                GamesFolderPath = dialog.SelectedPath;
            }
        }

        [MessageRecipient]
        private void MessageListener(MessageSaveRequest requestMessage) 
        {
            if (!IsDirty) return;

            SettingsRepository.UpdateUnit(gameListUnit);

            IsDirty = false;
        }
    }
}