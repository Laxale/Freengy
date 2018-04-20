// Created by Laxale 27.10.2016
//
//

using Freengy.UI.Views;
using Freengy.UI.Helpers;
using Freengy.UI.Windows;
using Freengy.UI.ViewModels;
using Freengy.UI.DefaultImpl;
using Freengy.Base.Interfaces;
using Freengy.Base.DefaultImpl;

using Catel.IoC;
using Catel.Services;

using Prism.Modularity;


namespace Freengy.UI.Module 
{
    public class MainModule : IModule 
    {
        public void Initialize() 
        {
            ServiceLocator.Default.RegisterInstance<IGuiDispatcher>(UiDispatcher.Instance);

            ServiceLocator.Default.RegisterTypeIfNotYetRegistered<IAccountManager, AccountManager>();
            ServiceLocator.Default.RegisterTypeIfNotYetRegistered<ITaskWrapper, TaskWrapper>(RegistrationType.Transient);
        }
    }
}