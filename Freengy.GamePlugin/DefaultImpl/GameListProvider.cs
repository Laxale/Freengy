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
    using System.Collections.Concurrent;

    using Freengy.Base.Interfaces;
    using Freengy.Base.Extensions;
    using Freengy.GamePlugin.Constants;
    using Freengy.GamePlugin.Interfaces;
    
    using Catel.IoC;


    internal class PluginDictionary : Dictionary<string, KeyValuePair<Assembly, IGamePlugin>> 
    {
        public IEnumerable<IGamePlugin> LoadedGamePlugins => base.Values.Select(assemblyKey => assemblyKey.Value);
    }

    public class GameListProvider : IGameListProvider
    {
        private static readonly object Locker = new object();
        private readonly ITypeFactory typeFactory;
        private readonly IAppDirectoryInspector directoryInspector;
        /// <summary>
        /// Contains loaded assembly paths and assemblies as is
        /// </summary>
        private readonly PluginDictionary loadedAssemblies = new PluginDictionary();


        #region Singleton

        private static GameListProvider instance;
        
        private GameListProvider() 
        {
            this.typeFactory = this.GetTypeFactory();
            this.directoryInspector = ServiceLocator.Default.ResolveType<IAppDirectoryInspector>();
        }

        public static GameListProvider Instance => GameListProvider.instance ?? (GameListProvider.instance = new GameListProvider());

        #endregion Singleton


        public IEnumerable<IGamePlugin> GetInstalledGames() 
        {
            var newDllInGameFolderPaths = this.GetNewDllPathsInGameFolder();
            
            foreach (string newDllPath in newDllInGameFolderPaths)
            {
                this.LoadIfGameAssembly(newDllPath);
            }

            return this.loadedAssemblies.LoadedGamePlugins;
        }

        public async Task<IEnumerable<IGamePlugin>> GetInstalledGamesAsync() 
        {
            return await Task.Factory.StartNew(this.GetInstalledGames);
        }


        private string[] GetNewDllPathsInGameFolder() 
        {
            var newDllInGameFolderPaths =
                this
                .directoryInspector
                .GetDllsInSubFolder(StringConstants.GamesFolderName)
                .Except(this.loadedAssemblies.Keys)
                .ToArray();

            return newDllInGameFolderPaths;
        }

        private void LoadIfGameAssembly(string newDllPath) 
        {
            Assembly loadedDll = this.LoadAssembly(newDllPath);

            Type gamePluginImplementer = loadedDll.FindImplementingType<IGamePlugin>();

            if (gamePluginImplementer == null) return;

            IGamePlugin gamePlugin = this.typeFactory.CreateInstanceWithParameters(gamePluginImplementer) as IGamePlugin;

            var pluginPair = new KeyValuePair<Assembly, IGamePlugin>(loadedDll, gamePlugin);

            lock (GameListProvider.Locker)
            {
                if (this.loadedAssemblies.ContainsKey(newDllPath))
                {
                    throw new InvalidOperationException($"Already loaded '{ newDllPath }'! invalid situation");
                }

                this.loadedAssemblies.Add(newDllPath, pluginPair);
            }
        }

        private Assembly LoadAssembly(string assemblyPath) 
        {
            if (!File.Exists(assemblyPath)) throw new ArgumentNullException(nameof(assemblyPath));

            //var sdkAdapterAssembly = Assembly.Load(asmPath);
            var loadedAssembly = Assembly.LoadFrom(assemblyPath);

            return loadedAssembly;
        }
    }
}