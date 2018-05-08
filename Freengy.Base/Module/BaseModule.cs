// Created by Laxale 25.10.2016
//
//

using Freengy.Base.Helpers;
using Freengy.Base.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Base.Chat.Interfaces;
using Freengy.Base.Chat.DefaultImpl;
using Freengy.Common.Helpers;

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

                MyServiceLocator.Instance.RegisterInstance(ChatHub.Instance);
                MyServiceLocator.Instance.RegisterInstance(UserActivityHub.Instance);
                MyServiceLocator.Instance.RegisterInstance(CurtainedExecutor.Instance);
                MyServiceLocator.Instance.RegisterInstance(ChatSessionFactory.Instance);

                MyServiceLocator.Instance.RegisterIfNotRegistered<IAlbumManager, AlbumManager>();
                MyServiceLocator.Instance.RegisterIfNotRegistered<IAccountManager, DbAccountManager>();
                MyServiceLocator.Instance.RegisterIfNotRegistered<IAppDirectoryInspector, AppDirectoryInspector>();
                MyServiceLocator.Instance.RegisterIfNotRegistered<ITaskWrapper, TaskWrapper>();
                MyServiceLocator.Instance.RegisterIfNotRegistered<IChatMessage, ChatMessage>();
                MyServiceLocator.Instance.RegisterIfNotRegistered<IChatMessageFactory, ChatMessageFactory>();
            }
        }
    }
}