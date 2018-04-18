// Created by Laxale 19.10.2016
//
//


namespace Freengy.UI 
{
    using System;
    using System.Windows;

    using Helpers;

    using Catel.Logging;

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
            var bootstrapper = new ShellBootstrapper();
            // из-за бутера к главному окну не применяются автостили, лежащие или подключённые в App.xaml.
            // нарушен порядок загрузки, видимо. Потому что к другим чилдовым окнам главного окна стили уже будут готовы
            bootstrapper.Run();

            if (MainWindow != null)
            {
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
    }
}