// Created by Laxale 27.10.2016
//
//


namespace Freengy.UI.Module 
{
    using Freengy.UI.Helpers;
    using Freengy.Base.Interfaces;
    using Freengy.Base.DefaultImpl;

    using Catel.IoC;

    using Prism.Modularity;


    public class MainModule : IModule 
    {
        public void Initialize() 
        {
            ServiceLocator.Default.RegisterInstance<IGuiDispatcher>(UiDispatcher.Instance);
            ServiceLocator.Default.RegisterTypeIfNotYetRegistered<ITaskWrapper, TaskWrapper>(RegistrationType.Transient);
        }
    }
}