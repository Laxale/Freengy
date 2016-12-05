﻿// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.Module 
{
    using Freengy.Settings.Views;
    using Freengy.Settings.ViewModels;
    using Freengy.Settings.DefaultImpl;

    using Catel.IoC;
    using Catel.Services;

    using Prism.Modularity;


    public sealed class SettingsModule : IModule 
    {
        public void Initialize() 
        {
            // instantiate a singleton
            var wut = Freengy.Settings.Helpers.DataContextSetter.Instance;

            ServiceLocator.Default.RegisterInstance(SettingsFacade.Instance);

            var vizualizer = ServiceLocator.Default.ResolveType<IUIVisualizerService>();
            vizualizer.Register<SettingsViewModel, SettingsWindow>();
        }
    }
}