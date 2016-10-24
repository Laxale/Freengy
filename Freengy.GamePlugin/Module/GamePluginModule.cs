// Created by Laxale 23.10.2016
//
//


namespace Freengy.GamePlugin.Module 
{
    using Freengy.Base.Interfaces;
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
        /// Registers networking internal types to service locator.
        /// TODO: may be need implement <see cref="IRegistrator"/> ?
        /// </summary>
        public void Initialize() 
        {
            ServiceLocator.Default.RegisterInstance<IGameListProvider>(GameListProvider.Instance);
        }
    }
}