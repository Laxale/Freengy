// Created by Laxale 27.11.2016
//
//

using System;
using System.Windows;
using System.Windows.Forms;
using System.Threading.Tasks;
using Freengy.Base.DefaultImpl;
using Freengy.Base.Extensions;
using Freengy.Base.Helpers.Commands;
using Freengy.Settings.Messages;
using Freengy.Settings.ModuleSettings;

using LocalRes = Freengy.Settings.Properties.Resources;


namespace Freengy.Settings.ViewModels 
{
    internal sealed class GameListSettingsViewModel : UnitViewModelBase 
    {
        private GameListSettingsUnit gameListUnit;


        public GameListSettingsViewModel() 
        {
            this.Subscribe<MessageSaveRequest>(OnSaveRequest);
        }
        

        public MyCommand CommandSelectGamesFolder { get; private set; }


        #region override

        public override string ToString() 
        {
            return LocalRes.GameListSettingsTitle;
        }

        protected override void SetupCommands() 
        {
            CommandSelectGamesFolder = new MyCommand(SelectGamesFolderImpl);
        }

        /// <inheritdoc />
        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override void InitializeImpl()
        {
            base.InitializeImpl();

            SetBusyState("Initializing game list");

            gameListUnit = SettingsRepository.GetOrCreateUnit<GameListSettingsUnit>();

            FillPropertiesFromDatabase().RunSynchronously();

            ClearBusyState();
        }

        #endregion override



        private string gamesFolderPath;

        public string GamesFolderPath 
        {
            get => gamesFolderPath;

            set
            {
                if (gamesFolderPath == value) return;

                gamesFolderPath = value;

                OnPropertyChanged();

                if (LoadedFromDatabase) gameListUnit.GamesFolderPath = value;
            }
        }
        
    
        protected override Task FillPropertiesFromDatabase() 
        {
            void FillAction()
            {
                GamesFolderPath = gameListUnit.GamesFolderPath;

                LoadedFromDatabase = true;
            }

            return Task.Run(() => FillAction());
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

        private void OnSaveRequest(MessageSaveRequest requestMessage) 
        {
            SettingsRepository.UpdateUnit(gameListUnit);
        }
    }
}