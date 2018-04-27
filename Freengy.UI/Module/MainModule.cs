// Created by Laxale 27.10.2016
//
//

using Freengy.UI.Helpers;
using Freengy.UI.DefaultImpl;
using Freengy.Common.Helpers;
using Freengy.Base.Interfaces;
using Freengy.Base.DefaultImpl;

using Catel.IoC;

using Prism.Modularity;


namespace Freengy.UI.Module 
{
    public class MainModule : IModule 
    {
        public void Initialize() 
        {
            using (new StatisticsDeployer(nameof(MainModule)))
            {
                ServiceLocator.Default.RegisterInstance<IGuiDispatcher>(UiDispatcher.Instance);

                //ServiceLocator.Default.RegisterType<IAccountManager, DbAccountManager>(RegistrationType.Transient);
                ServiceLocator.Default.RegisterTypeIfNotYetRegistered<IAccountManager, ConfigFileAccountManager>();
                ServiceLocator.Default.RegisterTypeIfNotYetRegistered<ITaskWrapper, TaskWrapper>(RegistrationType.Transient);
            }
        }
    }
}