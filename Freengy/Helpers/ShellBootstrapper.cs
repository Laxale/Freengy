// Created 20.10.2016
//
//


namespace Freengy.UI.Helpers 
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Freengy.UI.Views;
    using Freengy.UI.Constants;

    using Catel.IoC;
    using Catel.Messaging;

    using Prism;
    using Prism.Unity;
    using Prism.Regions;
    using Prism.Modularity;


    public class ShellBootstrapper : UnityBootstrapper 
    {
        protected override void InitializeShell() 
        {
            base.InitializeShell();
            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();

            UiDispatcher.Invoke(() => { }); // initialize him with static ctor

            var regionManager = base.Container.TryResolve<IRegionManager>();

            regionManager.RequestNavigate(RegionNames.MainWindowRegion, ViewNames.LoginViewName);
        }
        
        protected override DependencyObject CreateShell() 
        {
            var mainWindow = ServiceLocator.Default.ResolveType<MainWindow>();

            return mainWindow;
        }
        
//        protected override IModuleCatalog CreateModuleCatalog() 
//        {
//            var catalog = new ModuleCatalog();
//
////            catalog
////                .AddModule(typeof(TopRibbonModule))
////                .AddModule(typeof(PoliciesViewModule))
////                .AddModule(typeof(ComputersViewModule));
//
//            return catalog;
//        }

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
    }
}
