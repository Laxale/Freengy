// Created by Laxale 23.10.2016
//
//


namespace Freengy.Networking.Module 
{
    using System;

    using Freengy.Base.Interfaces;
    using Freengy.Base.DefaultImpl;
    using Freengy.Networking.Interfaces;
    using Freengy.Networking.DefaultImpl;

    using Catel.IoC;

    using Prism.Modularity;


    /// <summary>
    /// Exposes networking <see cref="IModule"/> implementation
    /// </summary>
    public class NetworkingModule : IModule 
    {
        /// <summary>
        /// Registers networking internal types to service locator.
        /// TODO: may be need implement <see cref="IRegistrator"/> ?
        /// </summary>
        public void Initialize() 
        {
            ServiceLocator.Default.RegisterTypeIfNotYetRegistered<ITaskWrapper, TaskWrapper>(RegistrationType.Transient);
            // login controller uses task wrapper, so must register them this order
            ServiceLocator.Default.RegisterInstance<ILoginController>(LoginController.Instance);
        }
    }
}