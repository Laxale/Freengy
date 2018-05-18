// Created by Laxale 24.10.2016
//
//

using System;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Freengy.Base.DefaultImpl;
using Freengy.Base.ViewModels;
using Freengy.Base.Interfaces;
using Freengy.GamePlugin.Messages;
using Freengy.GamePlugin.Interfaces;
using Freengy.GamePlugin.DefaultImpl;
using Freengy.Diagnostics.Interfaces;
using Freengy.Base.Helpers.Commands;
using Freengy.Base.Messages;
using Freengy.Common.Helpers.Result;


namespace Freengy.GameList.ViewModels 
{
    public class GameListViewModel : WaitableViewModel, IUserActivity 
    {
        private readonly IGameListProvider gameListProvider;
        private readonly IDiagnosticsController diagnosticsController;
        private readonly ObservableCollection<IGamePlugin> gameList = new ObservableCollection<IGamePlugin>();


        public GameListViewModel(ITaskWrapper taskWrapper, IGuiDispatcher guiDispatcher, IMyServiceLocator serviceLocator) : 
            base(taskWrapper, guiDispatcher, serviceLocator)
        {
            GameList = CollectionViewSource.GetDefaultView(gameList);

            gameListProvider = ServiceLocator.Resolve<IGameListProvider>();
            diagnosticsController = ServiceLocator.Resolve<IDiagnosticsController>();

            this.Publish(new MessageRefreshRequired(this));
        }


        public MyCommand CommandResolveProblems { get; private set; }
        
        public MyCommand<IGamePlugin> CommandRequestLoadGame { get; private set; }


        public ICollectionView GameList { get; }

        /// <summary>
        /// Возвращает значение - можно ли остановить данную активити без ведома юзера.
        /// </summary>
        public bool CanCancelInSilent { get; } = true;

        /// <summary>
        /// Возвращает описание активности в контексте её остановки.
        /// </summary>
        public string CancelDescription { get; } = string.Empty;


        /// <summary>
        /// Cancel activity.
        /// </summary>
        /// <returns>Result of a cancel attempt.</returns>
        public Result Cancel() 
        {
            this.Unsubscribe();

            return Result.Ok();
        }

        /// <summary>
        /// Обновить вьюмодель.
        /// </summary>
        public override void Refresh() 
        {
            base.Refresh();

            this.Subscribe<MessageGamesAdded>(OnNewGameMessage);

            InitializeImpl();
        }


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

            this.Publish(loadRequest);
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

        private void OnNewGameMessage(MessageGamesAdded newGamesMessage) 
        {
            var uiDispatcher = ServiceLocator.Resolve<IGuiDispatcher>();

            uiDispatcher.InvokeOnGuiThread(() =>
            {
                foreach (IGamePlugin newGamePlugin in newGamesMessage.NewGames)
                {
                    gameList.Add(newGamePlugin);
                }
            });
        }
    }
}