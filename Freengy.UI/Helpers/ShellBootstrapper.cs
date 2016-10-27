// Created by Laxale 20.10.2016
//
//


namespace Freengy.UI.Helpers 
{
    using System;
    using System.Windows;
    
    using Freengy.UI.Views;
    using Freengy.UI.Module;
    using Freengy.UI.Constants;

    using Freengy.Base.Module;
    using Freengy.GameList.Module;
    using Freengy.Networking.Module;
    using Freengy.GamePlugin.Module;
    using Freengy.Friendlist.Module;

    using Catel.IoC;
    using Catel.Messaging;

    using Prism;
    using Prism.Unity;
    using Prism.Regions;
    using Prism.Modularity;

    using Microsoft.Practices.Unity;


    public class ShellBootstrapper : UnityBootstrapper 
    {
        private IRegionManager regionManager;


        protected override void InitializeShell() 
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();

            UiDispatcher.Invoke(() => { }); // initialize him with static ctor

            this.regionManager = base.Container.TryResolve<IRegionManager>();

            this.Register();

            ServiceLocator.Default.RegisterInstance(base.Container);
            ServiceLocator.Default.RegisterInstance(this.regionManager);

            TypeRegistrator.Instance.Register();

            this.regionManager.RequestNavigate(RegionNames.MainWindowRegion, ViewNames.LoginViewName);
        }
        
        protected override DependencyObject CreateShell() 
        {
            var mainWindow = ServiceLocator.Default.ResolveType<MainWindow>();

            return mainWindow;
        }

        protected override IModuleCatalog CreateModuleCatalog() 
        {
            var catalog = new ModuleCatalog();

            catalog.AddModule(typeof(BaseModule))
                   .AddModule(typeof(MainModule))
                   .AddModule(typeof(NetworkingModule))
                   .AddModule(typeof(GamePluginModule));

            return catalog;
        }

        protected override void ConfigureContainer() 
        {
            base.ConfigureContainer();

            string friendListViewName = FriendListModule.Instance.ExportedViewType.FullName;

            base.Container
                .RegisterType<object, LoginView>(ViewNames.LoginViewName)
                .RegisterType<object, ShellView>(ViewNames.ShellViewName)
                .RegisterType(typeof(object), FriendListModule.Instance.ExportedViewType, friendListViewName);
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
            this
                .regionManager
                .RegisterViewWithRegion(RegionNames.GameListRegion, GameListModule.Instance.ExportedViewType)
                .RegisterViewWithRegion(RegionNames.FriendListRegion, FriendListModule.Instance.ExportedViewType);
        }
    }
}