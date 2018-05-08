// Created by Laxale 27.10.2016
//
//

using System;
using System.Windows;
using Freengy.Base.DefaultImpl;
using Freengy.Base.Interfaces;
using Freengy.Base.Messages;
using Freengy.GamePlugin.Messages;
using Freengy.GamePlugin.Interfaces;
using Freengy.Base.Messages.Base;


namespace Freengy.GamePlugin.DefaultImpl 
{  
    public class GameDispatcher : IGameDispatcher 
    {
        private readonly IMyServiceLocator serviceLocator = MyServiceLocator.Instance;


        #region Singleton

        private static GameDispatcher instance;
        
        private GameDispatcher() 
        {
            this.Subscribe<MessageGameStateRequest>(MessageListener);
        }

        public static GameDispatcher Instance => instance ?? (instance = new GameDispatcher());

        #endregion Singleton


        /// <summary>
        /// TODO dont know possible conditions yet
        /// </summary>
        public bool CanUnloadCurrentGame => true;

        public IGamePlugin CurrentRunningGame { get; private set; }

        public bool UnloadCurrentGame() 
        {
            if (CurrentRunningGame == null) return true;

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

            if (!CanLoadGame(gamePlugin)) return false;

            if (!UnloadCurrentGame()) return false;

            MessageBase requestGameUiMessage = new MessageRequestGameUi(gamePlugin.ExportedViewType);

            this.Publish(requestGameUiMessage);

            CurrentRunningGame = gamePlugin;

            return true;
        }

        public bool CanLoadGame(IGamePlugin gamePlugin) 
        {
            if (gamePlugin == null) throw new ArgumentNullException(nameof(gamePlugin));

            if (gamePlugin.ExportedViewType == null) throw new ArgumentException("gamePlugin.ExportedViewType is null");

            bool canLoad = gamePlugin.ExportedViewType != CurrentRunningGame?.ExportedViewType;

            return canLoad;
        }


        private void MessageListener(MessageGameStateRequest gameStateRequest) 
        {
            // TODO: implement chainer?
            var loadGameRequest = gameStateRequest as MessageLoadGameRequest;

            if (loadGameRequest != null)
            {
                if(loadGameRequest.Plugin == null) throw new ArgumentException("loadGameRequest.Plugin is null");

                LoadGame(loadGameRequest.Plugin);

                return;
            }

            var unloadGameRequest = gameStateRequest as MessageUnloadGameRequest;

            if (unloadGameRequest != null)
            {
                UnloadCurrentGame();
            }
        }
    }
}