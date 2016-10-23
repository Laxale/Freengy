// Created by Laxale 19.10.2016
//
//


namespace Freengy.UI 
{
    using System;
    using System.Windows;
    
    using Freengy.UI.Helpers;

    using Catel.IoC;
    using Catel.MVVM;
    using Catel.Logging;

    using Prism.Regions;


    public partial class App : Application 
    {
        protected override void OnStartup(StartupEventArgs e) 
        {
            var splasher = new SplashScreen("Images/splash.jpg");
            splasher.Show(true);

            base.OnStartup(e);
#if DEBUG
            LogManager.AddDebugListener();
#endif
            //Catel.Windows.StyleHelper.CreateStyleForwardersForDefaultStyles();

//            var serviceLocator = ServiceLocator.Default;
//            var typeFactory = this.GetTypeFactory();
            
//            var bootstrapper = serviceLocator.ResolveType<ShellBootstrapper>();
            var bootstrapper = new ShellBootstrapper();
            // из-за бутера к главному окну не применяются автостили, лежащие или подключённые в App.xaml.
            // нарушен порядок загрузки, видимо. Потому что к другим чилдовым окнам главного окна стили уже будут готовы
            bootstrapper.Run();
            
            //            var controller = typeFactory.CreateInstance<AppController>();
            //            serviceLocator.RegisterInstance(controller);

            //            controller.Register();

            Application.Current.MainWindow.Closed += this.OnMainWindowClosed;
        }


        private void OnMainWindowClosed(object sender, EventArgs args) 
        {
            // log or whatever
            this.Shutdown();
        }
    }
}