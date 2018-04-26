// Created by Laxale 19.10.2016
//
//

using System;
using System.Windows;
using System.Windows.Threading;

using Freengy.UI.Helpers;
using Freengy.UI.Constants;

using Catel.IoC;
using Catel.Logging;

using Prism.Regions;


namespace Freengy.UI 
{
    using System.Collections.Generic;
    using System.Linq;

    using Freengy.Base.Helpers;
    using Freengy.Base.Models;


    public partial class App : Application 
    {
        protected override void OnStartup(StartupEventArgs e) 
        {
            var splasher = new SplashScreen("Images/splash.jpg");
            splasher.Show(false);

            base.OnStartup(e);

            DispatcherUnhandledException += OnUnhandledException;

#if DEBUG
            LogManager.AddDebugListener();
#endif
            UiDispatcher.Invoke(() => { }); // initialize him with static ctor

            StatisticsCollector.Instance.Configure(FlushStatistics);

            using (new StatisticsDeployer("Booter.Run"))
            {
                var bootstrapper = new ShellBootstrapper();
                // из-за бутера к главному окну не применяются автостили, лежащие или подключённые в App.xaml.
                // нарушен порядок загрузки, видимо. Потому что к другим чилдовым окнам главного окна стили уже будут готовы
                bootstrapper.Run();
            }

            StatisticsCollector.Instance.FlushStatistics();
            
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


        private void FlushStatistics(IEnumerable<StatisticsUnit> units) 
        {
            string log =
                string.Join
                (
                    Environment.NewLine,
                    units.Select(unit => $"{unit.UnitName} started '{unit.Started}'; finished '{unit.Finished}'")
                );

            MessageBox.Show(log);
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