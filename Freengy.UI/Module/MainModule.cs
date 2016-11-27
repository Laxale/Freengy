// Created by Laxale 27.10.2016
//
//


namespace Freengy.UI.Module 
{
    using Freengy.UI.Views;
    using Freengy.UI.Helpers;
    using Freengy.UI.Windows;
    using Freengy.UI.ViewModels;
    using Freengy.Base.Interfaces;
    using Freengy.Base.DefaultImpl;

    using Catel.IoC;
    using Catel.Services;

    using Prism.Modularity;


    public class MainModule : IModule 
    {
        public void Initialize() 
        {
            var uiVisualizer = ServiceLocator.Default.ResolveType<IUIVisualizerService>();

            uiVisualizer.Register<RegistrationViewModel, RegistrationWindow>();
            uiVisualizer.Register<RecoverPasswordViewModel, RecoverPasswordWindow>();

            ServiceLocator.Default.RegisterInstance<IGuiDispatcher>(UiDispatcher.Instance);
            ServiceLocator.Default.RegisterTypeIfNotYetRegistered<ITaskWrapper, TaskWrapper>(RegistrationType.Transient);
        }
    }
}