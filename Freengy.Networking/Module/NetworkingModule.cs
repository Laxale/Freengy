﻿// Created by Laxale 23.10.2016
//
//

using System;

using Freengy.Base.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Networking.Interfaces;
using Freengy.Networking.DefaultImpl;

using Freengy.Common.Helpers;
using Freengy.Common.Interfaces;

using Catel.IoC;

using Prism.Modularity;


namespace Freengy.Networking.Module 
{
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
            ServiceLocator.Default.RegisterTypeIfNotYetRegistered<IHttpActor, HttpActor>(RegistrationType.Transient);
            ServiceLocator.Default.RegisterTypeIfNotYetRegistered<ITaskWrapper, TaskWrapper>(RegistrationType.Transient);
            ServiceLocator.Default.RegisterTypeIfNotYetRegistered<IEntitySearcher, EntitySearcher>(RegistrationType.Transient);
            // login controller uses task wrapper, so must register them this order
            ServiceLocator.Default.RegisterInstance(FriendStateController.ExposedInstance);
            ServiceLocator.Default.RegisterInstance(ServerListener.ExposedInstance);
            ServiceLocator.Default.RegisterInstance(LoginController.Instance);

            FriendStateController.InternalInstance.InitInternal();
        }
    }
}