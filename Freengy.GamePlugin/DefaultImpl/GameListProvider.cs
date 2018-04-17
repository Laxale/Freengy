// Created by Laxale 24.10.2016
//
//

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

using Freengy.Settings.Interfaces;
using Freengy.Settings.ModuleSettings;

using Freengy.GamePlugin.Helpers;
using Freengy.GamePlugin.Messages;
using Freengy.GamePlugin.Constants;
using Freengy.GamePlugin.Interfaces;

using Prism.Modularity;

using Catel.IoC;
using Catel.Messaging;


namespace Freengy.GamePlugin.DefaultImpl 
{
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
                bool contains = loadedMainViewTypes.Values.Contains(viewType);

                return contains;
            }
        }

        public IEnumerable<string> GetLoadedAssemblyPaths() 
        {
            lock (Locker)
            {
                return loadedPlugins.Keys;
            }
        }

        public IEnumerable<IGamePlugin> GetCurrentLoadedPlugins() 
        {
            lock (Locker)
            {
                return loadedPlugins.Values;
            }
        }

        public void AddPathAndViewType(string assemblyPath, string viewType) 
        {
            Common.ThrowIfArgumentsHasNull(assemblyPath, viewType);

            lock (Locker)
            {
                if (loadedMainViewTypes.ContainsKey(assemblyPath))
                {
                    throw new InvalidOperationException($"Already stored path '{assemblyPath}'");
                }

                loadedMainViewTypes.Add(assemblyPath, viewType);
            }
        }

        public void AddPathAndPlugin(string assemblyPath, IGamePlugin gamePlugin) 
        {
            Common.ThrowIfArgumentsHasNull(assemblyPath, gamePlugin);

            lock (Locker)
            {
                if (loadedPlugins.ContainsKey(assemblyPath))
                {
                    throw new InvalidOperationException(
                        $"Already stored plugin '{gamePlugin.Name}' on path '{assemblyPath}'");
                }

                loadedPlugins.Add(assemblyPath, gamePlugin);
            }
        }
    }

    internal class GameListProvider : IGameListProvider 
    {
        #region vars

        private readonly ITypeFactory typeFactory;
        private readonly ISettingsRepository settingsRepository;
        private readonly IAppDirectoryInspector directoryInspector;
        private readonly PluginsCache pluginsCache = new PluginsCache();
        private readonly GameDirectoryFilterStrategyBase gameFolderStrategy;
        private readonly IMessageMediator messageMediator = MessageMediator.Default;

        #endregion vars


        #region Singleton

        private static GameListProvider instance;

        private GameListProvider() 
        {
            typeFactory = this.GetTypeFactory();
            gameFolderStrategy = new GameFolderConfigFilterStrategy();
            settingsRepository = ServiceLocator.Default.ResolveType<ISettingsRepository>();
            directoryInspector = ServiceLocator.Default.ResolveType<IAppDirectoryInspector>();
            messageMediator.Register<MessageWorkingDirectoryChanged>(this, MessageListener);

            TryCreateDefaultGamesFolder();
        }

        public static GameListProvider Instance
            => instance ?? (instance = new GameListProvider());

        #endregion Singleton


        public IEnumerable<IGamePlugin> GetInstalledGames() 
        {
            IEnumerable<string> newDllInGameFolderPaths = GetNewDllPathsInGameFolder();

            IEnumerable<string> dllsWithNotLoadedViewType = FilterLoadedViewTyes(newDllInGameFolderPaths);

            LoadGameAssemblies(dllsWithNotLoadedViewType);

            return pluginsCache.GetCurrentLoadedPlugins();
        }

        public async Task<IEnumerable<IGamePlugin>> GetInstalledGamesAsync() 
        {
            return await Task.Factory.StartNew(GetInstalledGames);
        }

        
        private void TryCreateDefaultGamesFolder() 
        {
            var gameListSettings = settingsRepository.GetOrCreateUnit<GameListSettingsUnit>();

            string folderPath = gameListSettings.GamesFolderPath;

            if (!Directory.Exists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception)
                {
                    // log this
                }
            }
        }

        private IEnumerable<string> GetNewDllPathsInGameFolder() 
        {
            IEnumerable<string> loadedAssemblyPaths = pluginsCache.GetLoadedAssemblyPaths();

            var gameListSettings = settingsRepository.GetUnit<GameListSettingsUnit>();

            var allGameDllPaths = new List<string>();

            IEnumerable<string> gameDirectoryPaths = 
                new DirectoryInfo(gameListSettings.GamesFolderPath)
                .EnumerateDirectories()
                .Where
                (
                    innerDirectoryInfo =>
                    {
                        string gameDllPath;
                        bool isGameFolder = gameFolderStrategy.IsGameFolder(innerDirectoryInfo.FullName, out gameDllPath);

                        if (isGameFolder) allGameDllPaths.Add(gameDllPath);

                        return isGameFolder;
                    }
                )
                .Select(gameDirectoryInfo => gameDirectoryInfo.FullName)
                .ToArray();

            var newDllInGameFolderPaths = allGameDllPaths.Except(loadedAssemblyPaths);

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

                if (pluginsCache.ContainsViewType(mainViewType)) continue;

                filteredPaths.Add(newDllPath);

                pluginsCache.AddPathAndViewType(newDllPath, mainViewType);
            }

            return filteredPaths;
        }

        private void LoadGameAssemblies(IEnumerable<string> newGameDllPaths) 
        {
            foreach (string newDllPath in newGameDllPaths)
            {
                Assembly loadedAssembly = LoadAssembly(newDllPath);

                Type gamePluginImplementer = loadedAssembly.FindImplementingType<IGamePlugin>();

                TryInitializePluginModule(loadedAssembly);

                var gamePlugin = typeFactory.CreateInstanceWithParameters(gamePluginImplementer) as IGamePlugin;

                pluginsCache.AddPathAndPlugin(newDllPath, gamePlugin);
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

                var gameModule = typeFactory.CreateInstanceWithParameters(gameImoduleImplementer) as IModule;

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

            var gameListSettings = settingsRepository.GetUnit<GameListSettingsUnit>();

            if (!changedMessage.ChangedArgs.FullPath.Contains(gameListSettings.GamesFolderPath)) return;

            var currentGamesList = pluginsCache.GetCurrentLoadedPlugins().ToArray();

            var updatedGamesList = GetInstalledGames();

            var newGames = updatedGamesList.Except(currentGamesList).ToArray();

            if (newGames.Any())
            {
                var gamesAddedMessage = new MessageGamesAdded(newGames);
                messageMediator.SendMessage(gamesAddedMessage);
            }
        }
    }
}