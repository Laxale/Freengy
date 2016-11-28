// Created by Laxale 27.11.2016
//
//


namespace Freengy.Settings.ViewModels 
{
    using System;
    using System.Windows.Forms;
    using System.ComponentModel;

    using Freengy.Base.ViewModels;
    using Freengy.Settings.Messages;

    using Catel.Data;
    using Catel.MVVM;
    using Catel.Messaging;

    using LocalRes = Resources;


    internal sealed class GameListSettingsViewModel : UnitViewModelBase 
    {
        public GameListSettingsViewModel() 
        {
            base.messageMediator.Register<MessageSaveRequest>(this, this.MessageListener);

            // call this in ctor if viewmodel is not created by catel
            this.SetupCommands();
        }
        
        public Command CommandSelectGamesFolder { get; private set; }


        protected override void SetupCommands() 
        {
            this.CommandSelectGamesFolder = new Command(this.SelectGamesFolderImpl);
        }

        public override string ToString() 
        {
            return LocalRes.GameListSettingsTitle;
        }

        public string GamesFolderPath 
        {
            get { return (string)GetValue(GamesFolderPathProperty); }
            private set { SetValue(GamesFolderPathProperty, value); }
        }

        public static readonly PropertyData GamesFolderPathProperty =
            ModelBase.RegisterProperty<GameListSettingsViewModel, string>(viewModel => viewModel.GamesFolderPath, () => string.Empty);

        
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

        [MessageRecipient]
        private void MessageListener(MessageSaveRequest requestMessage) 
        {
            var t = 0;
            // save changes to db
        }
    }
}