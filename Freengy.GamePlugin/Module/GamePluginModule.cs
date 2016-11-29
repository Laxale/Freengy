// Created by Laxale 23.10.2016
//
//


namespace Freengy.GamePlugin.Module 
{
    using System;
    using System.IO;

    using Freengy.Base.Interfaces;
    using Freengy.GamePlugin.Constants;
    using Freengy.GamePlugin.Interfaces;
    using Freengy.GamePlugin.DefaultImpl;
    
    using Catel.IoC;

    using Prism.Modularity;


    /// <summary>
    /// Exposes GamePlugin assembly <see cref="IModule"/> implementation
    /// </summary>
    public class GamePluginModule : IModule 
    {
        /// <summary>
        /// Registers GamePlugin internal types to service locator
        /// </summary>
        public void Initialize() 
        {
            ServiceLocator.Default.RegisterInstance<IGameDispatcher>(GameDispatcher.Instance);
            ServiceLocator.Default.RegisterInstance<IGameListProvider>(GameListProvider.Instance);
        }
    }
}