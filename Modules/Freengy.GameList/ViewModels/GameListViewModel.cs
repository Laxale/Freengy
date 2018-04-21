// Created by Laxale 24.10.2016
//
//


using Freengy.Base.Helpers;

namespace Freengy.GameList.ViewModels 
{
    using System;
    using System.Windows.Data;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Base.ViewModels;
    using Base.Interfaces;
    using GamePlugin.Messages;
    using GamePlugin.Interfaces;
    using GamePlugin.DefaultImpl;
    using Freengy.Diagnostics.Interfaces;
    
    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Messaging;

    using Freengy.Base.Helpers.Commands;


    public class GameListViewModel : WaitableViewModel 
    {
        private readonly IGameListProvider gameListProvider;
        private readonly IDiagnosticsController diagnosticsController;
        private readonly ObservableCollection<IGamePlugin> gameList = new ObservableCollection<IGamePlugin>();


        public GameListViewModel() 
        {
            GameList = CollectionViewSource.GetDefaultView(gameList);

            gameListProvider = ServiceLocatorProperty.ResolveType<IGameListProvider>();
            Mediator.Register<MessageGamesAdded>(this, MessageListener);
            diagnosticsController = ServiceLocatorProperty.ResolveType<IDiagnosticsController>();
        }


        public ICollectionView GameList { get; private set; }


        #region commands

        public MyCommand CommandResolveProblems { get; private set; }

        public MyCommand<IGamePlugin> CommandRequestLoadGame { get; private set; }

        #endregion commands


        protected override void SetupCommands() 
        {
            CommandResolveProblems = new MyCommand(CommandResolveProblemsImpl, CanResolveProblems);
            CommandRequestLoadGame = new MyCommand<IGamePlugin>(CommandRequestLoadGameImpl, CanRequestLoadGame);
        }

        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            FillGameList().RunSynchronously();
        }

        private void CommandRequestLoadGameImpl(IGamePlugin gamePluginToLoad) 
        {
            MessageGameStateRequest loadRequest = new MessageLoadGameRequest(gamePluginToLoad, null);

            Mediator.SendMessage(loadRequest);
        }
        private bool CanRequestLoadGame(IGamePlugin gamePluginToLoad) 
        {
            return true;
            bool canRequestLoad = (gamePluginToLoad != null) && gameList.Contains((IGamePlugin)gamePluginToLoad);

            return canRequestLoad;
        }

        private async void CommandResolveProblemsImpl() 
        {
            await diagnosticsController.ShowDialogAsync();
        }

        private bool CanResolveProblems() 
        {
            return GameList?.IsEmpty ?? true;
        }

        private async Task FillGameList() 
        {
            await TaskerWrapper.Wrap(PutInstalledGamesToList, OnFillGameListException);
        }

        private void PutInstalledGamesToList() 
        {
            IEnumerable<IGamePlugin> installedGameList = gameListProvider.GetInstalledGames();

            foreach (var installedGame in installedGameList)
            {
                gameList.Add(installedGame);
            }
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
            var uiDispatcher = ServiceLocatorProperty.ResolveType<IGuiDispatcher>();

            uiDispatcher
                .InvokeOnGuiThread
                (
                    () =>
                    {
                        foreach (IGamePlugin newGamePlugin in newGamesMessage.NewGames)
                        {
                            gameList.Add(newGamePlugin);
                        }
                    }
                );
        }
    }
}