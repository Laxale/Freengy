// Created by Laxale 27.11.2016
//
//


namespace Freengy.Settings.ViewModels 
{
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

    using LocalRes = Resources;
    
    
    internal sealed class GameListSettingsViewModel : UnitViewModelBase
    {
        private bool loadedFromDatabase;
        private GameListSettingsUnit gameListUnit;


        public GameListSettingsViewModel() 
        {
             this.InitializeAsync();
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

        protected override Task InitializeAsync() 
        {
            return 
                Task.Factory.StartNew
                    (
                        () =>
                        {
#pragma warning disable 4014
                            base.InitializeAsync();
#pragma warning restore 4014
                        
                            base.messageMediator.Register<MessageSaveRequest>(this, this.MessageListener);

                            this.gameListUnit = base.settingsFacade.GetUnit<GameListSettingsUnit>();
                            this.FillPropertiesFromDatabase(gameListUnit);

                            // call this in ctor if viewmodel is not created by catel
                            this.SetupCommands();
                        }
                    );
        }

        protected override void InitializationContinuator(Task parentTask) 
        {
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

                if (this.loadedFromDatabase) this.gameListUnit.GamesFolderPath = value;
            }
        }

        public static readonly PropertyData GamesFolderPathProperty =
            ModelBase.RegisterProperty<GameListSettingsViewModel, string>(viewModel => viewModel.GamesFolderPath, () => string.Empty);


        #endregion properties


        private void SelectGamesFolderImpl() 
        {
            var dialog = new FolderBrowserDialog
            {
                SelectedPath = Environment.CurrentDirectory
            };

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.GamesFolderPath = dialog.SelectedPath;
            }
        }

        private void FillPropertiesFromDatabase(GameListSettingsUnit gameListUnit) 
        {
            this.GamesFolderPath = gameListUnit.GamesFolderPath;

            this.loadedFromDatabase = true;
        }

        [MessageRecipient]
        private void MessageListener(MessageSaveRequest requestMessage) 
        {
            if (!this.IsDirty) return;

            base.settingsFacade.UpdateUnit(this.gameListUnit);
        }
    }
}