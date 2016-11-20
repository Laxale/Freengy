// Created by Laxale 24.10.2016
//
//


namespace Freengy.GamePlugin.DefaultImpl 
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Freengy.Base.Helpers;
    using Freengy.Base.Messages;
    using Freengy.Base.Interfaces;
    using Freengy.Base.Extensions;
    using Freengy.GamePlugin.Helpers;
    using Freengy.GamePlugin.Messages;
    using Freengy.GamePlugin.Constants;
    using Freengy.GamePlugin.Interfaces;

    using Prism.Modularity;

    using Catel.IoC;
    using Catel.Messaging;


    /// <summary>
    /// Thread-safe plugins cache. No need to use lock() outside
    /// </summary>
    internal sealed class PluginsCache 
    {
        private static readonly object Locker = new object();

        /// <summary>
        /// Contains loaded assembly paths and their <see cref="IGamePlugin"/> implementation
        /// </summary>
        private readonly IDictionary<string, IGamePlugin> loadedPlugins = new Dictionary<string, IGamePlugin>();
        /// <summary>
        /// Contains loaded assembly paths and their main view types
        /// </summary>
        private readonly IDictionary<string, string> loadedMainViewTypes = new Dictionary<string, string>();


        public bool ContainsViewType(string viewType) 
        {
            if (string.IsNullOrWhiteSpace(viewType)) throw new ArgumentNullException(nameof(viewType));

            lock (Locker)
            {
                bool contains = this.loadedMainViewTypes.Values.Contains(viewType);

                return contains;
            }
        }

        public IEnumerable<string> GetLoadedAssemblyPaths() 
        {
            lock (Locker)
            {
                return this.loadedPlugins.Keys;
            }
        }

        public IEnumerable<IGamePlugin> GetCurrentLoadedPlugins() 
        {
            lock (PluginsCache.Locker)
            {
                return this.loadedPlugins.Values;
            }
        }

        public void AddPathAndViewType(string assemblyPath, string viewType) 
        {
            Common.ThrowIfArgumentsHasNull(assemblyPath, viewType);

            lock (Locker)
            {
                if (this.loadedMainViewTypes.ContainsKey(assemblyPath))
                {
                    throw new InvalidOperationException($"Already stored path '{ assemblyPath }'");
                }

                this.loadedMainViewTypes.Add(assemblyPath, viewType);
            }
        }

        public void AddPathAndPlugin(string assemblyPath, IGamePlugin gamePlugin) 
        {
            Common.ThrowIfArgumentsHasNull(assemblyPath, gamePlugin);

            lock (Locker)
            {
                if (this.loadedPlugins.ContainsKey(assemblyPath))
                {
                    throw new InvalidOperationException($"Already stored plugin '{ gamePlugin.Name }' on path '{ assemblyPath }'");
                }

                this.loadedPlugins.Add(assemblyPath, gamePlugin);
            }
        }
    }

    public class GameListProvider : IGameListProvider 
    {
        #region vars

        private readonly ITypeFactory typeFactory;
        private readonly IAppDirectoryInspector directoryInspector;
        private readonly PluginsCache pluginsCache = new PluginsCache();
        private readonly IMessageMediator messageMediator = MessageMediator.Default;

        #endregion vars


        #region Singleton

        private static GameListProvider instance;
        
        private GameListProvider() 
        {
            this.typeFactory = this.GetTypeFactory();
            this.directoryInspector = ServiceLocator.Default.ResolveType<IAppDirectoryInspector>();
            this.messageMediator.Register<MessageWorkingDirectoryChanged>(this, this.MessageListener);
        }

        public static GameListProvider Instance => GameListProvider.instance ?? (GameListProvider.instance = new GameListProvider());

        #endregion Singleton


        public IEnumerable<IGamePlugin> GetInstalledGames() 
        {
            IEnumerable<string> newDllInGameFolderPaths = this.GetNewDllPathsInGameFolder();

            IEnumerable<string> dllsWithNotLoadedViewType = this.FilterLoadedViewTyes(newDllInGameFolderPaths);

            this.LoadGameAssemblies(dllsWithNotLoadedViewType);

            return this.pluginsCache.GetCurrentLoadedPlugins();
        }

        public async Task<IEnumerable<IGamePlugin>> GetInstalledGamesAsync() 
        {
            return await Task.Factory.StartNew(this.GetInstalledGames);
        }


        private IEnumerable<string> GetNewDllPathsInGameFolder() 
        {
            IEnumerable<string> loadedAssemblyPaths = this.pluginsCache.GetLoadedAssemblyPaths();

            var newDllInGameFolderPaths =
                this
                .directoryInspector
                .GetDllsInSubFolder(StringConstants.GamesFolderName)
                .Except(loadedAssemblyPaths);

            return newDllInGameFolderPaths;
        }

        private IEnumerable<string> FilterLoadedViewTyes(IEnumerable<string> newDllInGameFolderPaths)
        {
            var filteredPaths = new List<string>();

            foreach (var newDllPath in newDllInGameFolderPaths)
            {
                var сonfigResearcher = new PluginConfigResearcher(newDllPath);

                string mainViewType;
                if(!сonfigResearcher.GetMainPluginViewName(out mainViewType)) continue;

                if (this.pluginsCache.ContainsViewType(mainViewType)) continue;

                filteredPaths.Add(newDllPath);

                this.pluginsCache.AddPathAndViewType(newDllPath, mainViewType);
            }

            return filteredPaths;
        }

        private void LoadGameAssemblies(IEnumerable<string> newGameDllPaths) 
        {
            foreach (string newDllPath in newGameDllPaths)
            {
                Assembly loadedAssembly = this.LoadAssembly(newDllPath);

                Type gamePluginImplementer = loadedAssembly.FindImplementingType<IGamePlugin>();

                this.TryInitializePluginModule(loadedAssembly);

                var gamePlugin = this.typeFactory.CreateInstanceWithParameters(gamePluginImplementer) as IGamePlugin;

                this.pluginsCache.AddPathAndPlugin(newDllPath, gamePlugin);
            }
        }

        private Assembly LoadAssembly(string assemblyPath) 
        {
            if (!File.Exists(assemblyPath)) throw new ArgumentNullException(nameof(assemblyPath));

            //var sdkAdapterAssembly = Assembly.Load(asmPath);
            var loadedAssembly = Assembly.LoadFrom(assemblyPath);

            return loadedAssembly;
        }

        private void TryInitializePluginModule(Assembly pluginAssembly) 
        {
            try
            {
                Type gameImoduleImplementer = pluginAssembly.FindImplementingType<IModule>();

                var gameModule = this.typeFactory.CreateInstanceWithParameters(gameImoduleImplementer) as IModule;

                gameModule?.Initialize();
            }
            catch (Exception)
            {
                // log this?
                throw;
            }
        }

        [MessageRecipient]
        private void MessageListener(MessageWorkingDirectoryChanged changedMessage) 
        {
            // listen only to creation events, not interested in deleting anyth
            if (changedMessage.ChangedArgs.ChangeType != WatcherChangeTypes.Created) return;
            // it could be file or folder created - cant say exactly until checking
            if (!File.Exists(changedMessage.ChangedArgs.FullPath)) return;
            
            if (!changedMessage.ChangedArgs.FullPath.Contains(StringConstants.GamesFolderPath)) return;

            var currentGamesList = this.pluginsCache.GetCurrentLoadedPlugins().ToArray();

            var updatedGamesList = this.GetInstalledGames();

            var newGames = updatedGamesList.Except(currentGamesList).ToArray();

            if (newGames.Any())
            {
                var gamesAddedMessage = new MessageGamesAdded(newGames);
                this.messageMediator.SendMessage(gamesAddedMessage);
            }
        }
    }
}