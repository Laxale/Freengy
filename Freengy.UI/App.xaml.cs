// Created by Laxale 19.10.2016
//
//

using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Controls;
using Freengy.Base.Helpers;
using Freengy.UI.Helpers;
using Freengy.UI.Constants;

using Catel.IoC;
using Catel.Logging;
using Freengy.Common.Helpers;
using Freengy.Common.Helpers.Statistics;
using Freengy.Common.Models;
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

                splasher.Close(TimeSpan.FromMilliseconds(100));
            }

            if (MainWindow != null)
            {
                using (new StatisticsDeployer("Navigate main window"))
                {
                    ServiceLocator.Default.ResolveType<IRegionManager>().RequestNavigate(RegionNames.MainWindowRegion, ViewNames.LoginViewName);
                }

                using (new StatisticsDeployer("Show main window"))
                {
                    MainWindow.Show();
                }

                // show loading results
                //StatisticsCollector.Instance.FlushStatistics();

                MainWindow.Closed += OnMainWindowClosed;
            }
            else
            {
                MessageBox.Show("Wow. Main window is not found");
            }
        }


        private void FlushStatistics(IEnumerable<StatisticsUnit> units) 
        {
            var list = new ListBox
            {
                ItemsSource = units.ToList()
            };
            
            ScrollViewer.SetVerticalScrollBarVisibility(list, ScrollBarVisibility.Visible);

            var logWin = new Window
            {
                MaxHeight = 500,
                MaxWidth = 800,
                Title = "Statistics",
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = list
            };

            logWin.ShowDialog();
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