﻿// Created by Laxale 20.10.2016
//
//

using System;
using System.Windows;

using Freengy.UI.Views;
using Freengy.UI.Module;
using Freengy.UI.Windows;
using Freengy.UI.Constants;

using Freengy.Base.Module;
using Freengy.Chatter.Module;
using Freengy.GameList.Module;
using Freengy.Settings.Module;
using Freengy.Networking.Module;
using Freengy.GamePlugin.Module;
using Freengy.FriendList.Module;
using Freengy.Diagnostics.Module;

using Catel.IoC;
using Catel.Messaging;

using Prism;
using Prism.Unity;
using Prism.Regions;
using Prism.Modularity;

using Microsoft.Practices.Unity;
using Prism.Mvvm;


namespace Freengy.UI.Helpers 
{
    public class ShellBootstrapper : UnityBootstrapper 
    {
        private readonly ViewMappingCache viewMappingCache = new ViewMappingCache();

        private IRegionManager regionManager;


        protected override void InitializeShell() 
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)Shell;
            //Application.Current.MainWindow.Show();
            
            UiDispatcher.Invoke(() => { }); // initialize him with static ctor

            regionManager = Container.TryResolve<IRegionManager>();

            Register();

            ServiceLocator.Default.RegisterInstance(Container);
            ServiceLocator.Default.RegisterInstance(regionManager);

            TypeRegistrator.Instance.Register();
        }
        
        protected override DependencyObject CreateShell() 
        {
            var mainWindow = ServiceLocator.Default.ResolveType<MainWindow>();

            return mainWindow;
        }

        /// <summary>
        /// Configures the <see cref="T:Prism.Mvvm.ViewModelLocator" /> used by Prism.
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewMappingCache.GetViewModelType);

            ViewModelLocationProvider.SetDefaultViewModelFactory(type => Container.Resolve(type));
        }

        protected override IModuleCatalog CreateModuleCatalog() 
        {
            var catalog = new ModuleCatalog();

            catalog.AddModule(typeof(BaseModule))
                   .AddModule(typeof(SettingsModule))
                   .AddModule(typeof(MainModule))
                   .AddModule(typeof(DiagnosticsModule))
                   .AddModule(typeof(GameListModule))
                   .AddModule(typeof(GamePluginModule))
                   .AddModule(typeof(FriendListModule))
                   .AddModule(typeof(NetworkingModule));

            return catalog;
        }

        protected override void ConfigureContainer() 
        {
            base.ConfigureContainer();

            Container
                .RegisterType<object, LoginView>(ViewNames.LoginViewName)
                .RegisterType<object, ShellView>(ViewNames.ShellViewName);
        }

        //        protected override RegionAdapterMappings ConfigureRegionAdapterMappings() 
        //        {
        //            var mappings = base.ConfigureRegionAdapterMappings();
        //
        ////            mappings.RegisterMapping(typeof(ListBox), base.Container.Resolve<ListBoxAdapter>());
        ////            mappings.RegisterMapping(typeof(Ribbon), base.Container.Resolve<RibbonRegionAdapter>());
        ////            mappings.RegisterMapping(typeof(RibbonContentPresenter), base.Container.Resolve<RibbonContentPresenterAdapter>());
        ////            mappings.RegisterMapping(typeof(RibbonQuickAccessToolBar), base.Container.Resolve<RibbonQuickAccessToolBarAdapter>());
        //
        //            return mappings;
        //        }


        private void Register() 
        {
            var gameListModule = new GameListModule();
            var friendListModule = new FriendListModule();

            regionManager
                .RegisterViewWithRegion(RegionNames.GameListRegion, gameListModule.ExportedViewType)
                .RegisterViewWithRegion(RegionNames.ChatRegion, ChatterModule.Instance.ExportedViewType)
                .RegisterViewWithRegion(RegionNames.FriendListRegion, friendListModule.ExportedViewType);
        }
    }
}