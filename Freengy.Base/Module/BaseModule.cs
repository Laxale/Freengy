// Created by Laxale 25.10.2016
//
//


namespace Freengy.Base.Module 
{
    using Freengy.Base.Interfaces;
    using Freengy.Base.DefaultImpl;
    using Freengy.Base.Chat.Interfaces;
    using Freengy.Base.Chat.DefaultImpl;
    using Freengy.SharedWebTypes.Objects;
    using Freengy.SharedWebTypes.Interfaces;
    
    using Catel.IoC;

    using Prism.Modularity;


    public class BaseModule : IModule 
    {
        public void Initialize() 
        {
            ServiceLocator.Default.RegisterInstance(ChatSessionFactory.Instance);
            ServiceLocator.Default.RegisterType<IChatMessageFactory, ChatMessageFactory>(RegistrationType.Transient);

            ServiceLocator.Default.RegisterType<IUserAccount, UserAccount>(RegistrationType.Transient);
            ServiceLocator.Default.RegisterTypeIfNotYetRegistered<IAppDirectoryInspector, AppDirectoryInspector>();
            ServiceLocator.Default.RegisterTypeIfNotYetRegistered<ITaskWrapper, TaskWrapper>(RegistrationType.Transient);
        }
    }
}