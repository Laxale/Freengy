// Created by Laxale 25.10.2016
//
//

using Freengy.Base.Helpers;
using Freengy.Base.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Base.Chat.Interfaces;
using Freengy.Base.Chat.DefaultImpl;
using Freengy.Common.Helpers;

using Catel.IoC;

using Prism.Modularity;


namespace Freengy.Base.Module 
{
    public class BaseModule : IModule 
    {
        public void Initialize() 
        {
            using (new StatisticsDeployer(nameof(BaseModule)))
            {
                //just create singleton
                var initializer = ViewModelInitializer.Instance;

                ServiceLocator.Default.RegisterInstance(ChatHub.Instance);
                ServiceLocator.Default.RegisterInstance(UserActivityHub.Instance);
                ServiceLocator.Default.RegisterInstance(CurtainedExecutor.Instance);
                ServiceLocator.Default.RegisterInstance(ChatSessionFactory.Instance);

                ServiceLocator.Default.RegisterTypeIfNotYetRegistered<IAlbumManager, AlbumManager>();
                ServiceLocator.Default.RegisterTypeIfNotYetRegistered<IAppDirectoryInspector, AppDirectoryInspector>();
                ServiceLocator.Default.RegisterTypeIfNotYetRegistered<ITaskWrapper, TaskWrapper>(RegistrationType.Transient);
                ServiceLocator.Default.RegisterTypeIfNotYetRegistered<IChatMessage, ChatMessage>(RegistrationType.Transient);
                ServiceLocator.Default.RegisterTypeIfNotYetRegistered<IChatMessageFactory, ChatMessageFactory>(RegistrationType.Transient);
            }
        }
    }
}