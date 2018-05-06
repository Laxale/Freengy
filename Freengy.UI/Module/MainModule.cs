// Created by Laxale 27.10.2016
//
//

using Freengy.UI.Helpers;
using Freengy.Common.Helpers;
using Freengy.Base.Interfaces;
using Freengy.Base.DefaultImpl;

using Catel.IoC;

using Prism.Modularity;


namespace Freengy.UI.Module 
{
    /// <summary>
    /// Prism-модуль библиотеки UI.
    /// </summary>
    public class MainModule : IModule 
    {
        public void Initialize() 
        {
            using (new StatisticsDeployer(nameof(MainModule)))
            {
                ServiceLocator.Default.RegisterInstance<IGuiDispatcher>(UiDispatcher.Instance);

                ServiceLocator.Default.RegisterTypeIfNotYetRegistered<ITaskWrapper, TaskWrapper>(RegistrationType.Transient);
            }
        }
    }
}