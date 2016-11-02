// Created by Laxale 24.10.2016
//
//


namespace Freengy.GameList.ViewModels 
{
    using System;
    using System.Windows.Data;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Freengy.Base.ViewModels;
    using Freengy.GamePlugin.Messages;
    using Freengy.GamePlugin.Interfaces;
    using Freengy.GamePlugin.DefaultImpl;
    
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Messaging;


    public class GameListViewModel : WaitableViewModel 
    {
        private readonly IGameListProvider gameListProvider;
        private readonly ObservableCollection<IGamePlugin> gameList = new ObservableCollection<IGamePlugin>();


        public GameListViewModel() 
        {
            this.gameListProvider = base.serviceLocator.ResolveType<IGameListProvider>();
            base.messageMediator.Register<MessageGamesAdded>(this, this.MessageListener);
        }


        public ICollectionView GameList { get; private set; }


        #region commands

        public Command CommandResolveProblems { get; private set; }

        public Command<IGamePlugin> CommandRequestLoadGame { get; private set; }

        #endregion commands



        #region Override

        protected override void SetupCommands() 
        {
            this.CommandResolveProblems = new Command(this.CommandResolveProblemsImpl, this.CanResolveProblems);
            this.CommandRequestLoadGame = new Command<IGamePlugin>(this.CommandRequestLoadGameImpl, this.CanRequestLoadGame);
        }

        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            await this.FillGameList();
        }

        #endregion Override


        private void CommandRequestLoadGameImpl(IGamePlugin gamePluginToLoad)
        {
            MessageGameStateRequest loadRequest = new MessageLoadGameRequest(gamePluginToLoad, null);

            base.messageMediator.SendMessage(loadRequest);
        }
        private bool CanRequestLoadGame(IGamePlugin gamePluginToLoad)
        {
            return true;
            bool canRequestLoad = (gamePluginToLoad != null) && this.gameList.Contains(gamePluginToLoad);

            return canRequestLoad;
        }

        private void CommandResolveProblemsImpl() 
        {
            
        }
        private bool CanResolveProblems() 
        {
            return this.GameList?.IsEmpty ?? true;
        }

        private async Task FillGameList() 
        {
            Action fillAction =
                () =>
                {
                    this.PutInstalledGamesToList();

                    this.InitGameListOnUiThread();
                };

            await base.taskWrapper.Wrap(fillAction, this.OnFillGameListException);
        }

        private void PutInstalledGamesToList() 
        {
            IEnumerable<IGamePlugin> installedGameList = this.gameListProvider.GetInstalledGames();

            foreach (var installedGame in installedGameList)
            {
                this.gameList.Add(installedGame);
            }
        }

        private void InitGameListOnUiThread() 
        {
            base.guiDispatcher
                .InvokeOnGuiThread
                (
                    () =>
                    {
                        this.GameList = CollectionViewSource.GetDefaultView(this.gameList);

                        base.RaisePropertyChanged(() => this.GameList);
                    }
                );
        }

        private void OnFillGameListException(Task parentTask) 
        {
            if (parentTask.Exception != null)
            {
                System.Windows.MessageBox.Show(parentTask.Exception.Message, "Failed to load games");
            }
        }

        [MessageRecipient]
        private void MessageListener(MessageGamesAdded newGamesMessage) 
        {
            foreach (var newGamePlugin in newGamesMessage.NewGames)
            {
                this.gameList.Add(newGamePlugin);
            }
        }
    }
}