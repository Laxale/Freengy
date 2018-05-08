// Created by Laxale 23.10.2016
//
//

using System;

using Freengy.Base.Helpers;
using Freengy.Base.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Common.Helpers;
using Freengy.Common.Interfaces;
using Freengy.Networking.Interfaces;
using Freengy.Networking.DefaultImpl;

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
        public async void Initialize() 
        {
            using (new StatisticsDeployer(nameof(NetworkingModule)))
            {
                MyServiceLocator.Instance.RegisterIfNotRegistered<IHttpActor, HttpActor>();
                MyServiceLocator.Instance.RegisterIfNotRegistered<ITaskWrapper, TaskWrapper>();
                MyServiceLocator.Instance.RegisterIfNotRegistered<IEntitySearcher, EntitySearcher>();
                // login controller uses task wrapper, so must register them this order
                MyServiceLocator.Instance.RegisterInstance(FriendStateController.ExposedInstance);
                MyServiceLocator.Instance.RegisterInstance(ServerListener.ExposedInstance);
                MyServiceLocator.Instance.RegisterInstance(LoginController.Instance);

                await ServerListener.InternalInstance.InitInternalAsync();

                FriendStateController.InternalInstance.InitInternal();
            }
        }
    }
}