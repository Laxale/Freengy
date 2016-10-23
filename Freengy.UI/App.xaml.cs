// Created by Laxale 19.10.2016
//
//


namespace Freengy.UI 
{
    using System;
    using System.Windows;
    
    using Freengy.UI.Helpers;

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
            
            Application.Current.MainWindow.Closed += this.OnMainWindowClosed;
        }


        private void OnMainWindowClosed(object sender, EventArgs args) 
        {
            // log or whatever
            this.Shutdown();
        }
    }
}