// Created by Laxale 19.10.2016
//
//

using System;
using System.Windows;
using System.Windows.Threading;
using Catel.IoC;
using Freengy.UI.Helpers;

using Catel.Logging;
using Freengy.UI.Constants;
using Prism.Regions;


namespace Freengy.UI 
{
    public partial class App : Application 
    {
        protected override void OnStartup(StartupEventArgs e) 
        {
            var splasher = new SplashScreen("Images/splash.jpg");
            splasher.Show(false);

            base.OnStartup(e);

            this.DispatcherUnhandledException += OnUnhandledException;

#if DEBUG
            LogManager.AddDebugListener();
#endif
            var bootstrapper = new ShellBootstrapper();
            // из-за бутера к главному окну не применяются автостили, лежащие или подключённые в App.xaml.
            // нарушен порядок загрузки, видимо. Потому что к другим чилдовым окнам главного окна стили уже будут готовы
            bootstrapper.Run();

            splasher.Close(TimeSpan.FromMilliseconds(100));
            if (MainWindow != null)
            {
                ServiceLocator.Default.ResolveType<IRegionManager>().RequestNavigate(RegionNames.MainWindowRegion, ViewNames.LoginViewName);
                MainWindow.Show();
                MainWindow.Closed += OnMainWindowClosed;
            }
            else
            {
                MessageBox.Show("Wow. Main window is not found");
            }
        }


        private void OnMainWindowClosed(object sender, EventArgs args) 
        {
            // log or whatever
            Shutdown();
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args) 
        {
            MessageBox.Show(args.Exception.Message);

            Shutdown();
        }
    }
}