// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.Module 
{
    using Freengy.Settings.DefaultImpl;
    
    using Catel.IoC;

    using Prism.Modularity;


    public sealed class SettingsFacadeModule : IModule 
    {
        public void Initialize() 
        {
            ServiceLocator.Default.RegisterInstance(SettingsFacade.Instance);
        }
    }
}