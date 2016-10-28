// Created by Laxale 27.10.2016
//
//


namespace Freengy.GamePlugin.DefaultImpl 
{
    using System;
    using System.Windows;

    using Freengy.Base.Messages;
    using Freengy.GamePlugin.Messages;
    using Freengy.GamePlugin.Interfaces;
    
    using Catel.IoC;
    using Catel.Messaging;


    public class GameDispatcher : IGameDispatcher 
    {
        private readonly IMessageMediator messageMediator;
        private readonly IServiceLocator serviceLocator = ServiceLocator.Default;


        #region Singleton

        private static GameDispatcher instance;
        
        private GameDispatcher() 
        {
            this.messageMediator = this.serviceLocator.ResolveType<IMessageMediator>();
            this.messageMediator.Register<MessageGameStateRequest>(this, this.MessageListener);
        }

        public static GameDispatcher Instance => GameDispatcher.instance ?? (GameDispatcher.instance = new GameDispatcher());

        #endregion Singleton


        /// <summary>
        /// TODO dont know possible conditions yet
        /// </summary>
        public bool CanUnloadCurrentGame => true;

        public IGamePlugin CurrentRunningGame { get; private set; }

        public bool UnloadCurrentGame() 
        {
            if (this.CurrentRunningGame == null) return true;

            if (MessageBox.Show("Leave current game session?", "Leaving game", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // some logic

                return true;
            }

            return false;
        }

        public bool LoadGame(IGamePlugin gamePlugin) 
        {
            if (gamePlugin == null) throw new ArgumentNullException(nameof(gamePlugin));

            if (!this.CanLoadGame(gamePlugin)) return false;

            if (!this.UnloadCurrentGame()) return false;

            MessageBase requestGameUiMessage = new MessageRequestGameUi(gamePlugin.ExportedViewType);

            this.messageMediator.SendMessage(requestGameUiMessage);

            this.CurrentRunningGame = gamePlugin;

            return true;
        }

        public bool CanLoadGame(IGamePlugin gamePlugin) 
        {
            if (gamePlugin == null) throw new ArgumentNullException(nameof(gamePlugin));

            if (gamePlugin.ExportedViewType == null) throw new ArgumentException("gamePlugin.ExportedViewType is null");

            bool canLoad = gamePlugin.ExportedViewType != this.CurrentRunningGame?.ExportedViewType;

            return canLoad;
        }


        [MessageRecipient]
        private void MessageListener(MessageGameStateRequest gameStateRequest) 
        {
            // TODO: implement chainer?
            var loadGameRequest = gameStateRequest as MessageLoadGameRequest;

            if (loadGameRequest != null)
            {
                if(loadGameRequest.Plugin == null) throw new ArgumentException("loadGameRequest.Plugin is null");

                this.LoadGame(loadGameRequest.Plugin);

                return;
            }

            var unloadGameRequest = gameStateRequest as MessageUnloadGameRequest;

            if (unloadGameRequest != null)
            {
                this.UnloadCurrentGame();
            }
        }
    }
}