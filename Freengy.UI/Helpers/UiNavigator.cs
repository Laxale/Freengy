// Created by Laxale 22.10.2016
//
//

using System;
using System.Windows;

using Freengy.UI.Constants;
using Freengy.Base.Messages;
using Freengy.Base.Interfaces;
using Freengy.Base.Exceptions;
using Freengy.Base.DefaultImpl;
using Freengy.Networking.Messages;
using Freengy.GamePlugin.Messages;
using Freengy.Base.Messages.Base;
using Freengy.Base.Messages.Login;
using Freengy.UI.Messages;

using Prism.Regions;

using Microsoft.Practices.Unity;


namespace Freengy.UI.Helpers 
{    
    /// <summary>
    /// Centralized handler of navigation request messages or any messages related to navigation
    /// </summary>
    internal class UiNavigator 
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer unityContainer;
        private readonly IResponsibilityChainer<MessageBase> requestHandleChain;
        private readonly IMyServiceLocator serviceLocator = MyServiceLocator.Instance;


        #region Singleton

        private static UiNavigator instance;

        private UiNavigator() 
        {
            requestHandleChain = new ResponsibilityChainer<MessageBase>();
            regionManager  = serviceLocator.Resolve<IRegionManager>();
            unityContainer = serviceLocator.Resolve<IUnityContainer>();

            this.Subscribe<MessageBase>(MessageListener);

            SetupChainer();
        }

        public static UiNavigator Instance => instance ?? (instance = new UiNavigator());

        #endregion Singleton


        private async void MessageListener(MessageBase message) 
        {
            bool handled = await requestHandleChain.HandleAsync(message);

            // must never call this. Default handler should always handle possibly unknown messages
            if (!handled) throw new NotHandledException();
        }

        private void SetupChainer() 
        {
            // possibility of having more navigation ways later
            requestHandleChain
                .AddHandler(HandleLogInFailedMessage)
                .AddHandler(HandleLogInAttemptMessage)
                .AddHandler(HandleLogInSuccessMessage)
                .AddHandler(HandleRequestNavigate)
                .AddHandler(HandleLogOutRequestMessage)
                // default handler is a good thing to catch unknown messages
                .AddHandler(DefaultHandler);
        }

        /// <summary>
        /// Put this to the end of a chain to avoid unhandled messages
        /// </summary>
        /// <param name="message">Possibly unhandled message</param>
        /// <returns>Always true</returns>
        private bool DefaultHandler(MessageBase message) 
        {
            // log situation or do smth else
            return true;
        }

        private bool HandleRequestNavigate(MessageBase message) 
        {
            if (!(message is MessageRequestGameUi requestGameUi)) return false;

            UiDispatcher.Invoke(() => regionManager.RegisterViewWithRegion(RegionNames.GameRegion, requestGameUi.GameUiType));

            string gameViewName = requestGameUi.GameUiType.FullName;

            unityContainer.RegisterType(typeof(object), requestGameUi.GameUiType, gameViewName);

            Navigate(RegionNames.GameRegion, gameViewName);

            return true;
        }

        private bool HandleLogInFailedMessage(MessageBase message) 
        {
            if (!(message is MessageLogInFailed)) return false;

            Navigate(RegionNames.MainWindowRegion, ViewNames.ShellViewName);

            return true;
        }

        private bool HandleLogInSuccessMessage(MessageBase message) 
        {
            if (!(message is MessageLogInSuccess)) return false;

            Navigate(RegionNames.MainWindowRegion, ViewNames.ShellViewName);

            UiDispatcher.Invoke(SetMainWindowSizeForShell);

            return true;
        }

        private bool HandleLogInAttemptMessage(MessageBase message) 
        {
            if (!(message is MessageLogInAttempt)) return false;

            return true;
        }

        private bool HandleLogOutRequestMessage(MessageBase message) 
        {
            if (!(message is MessageLogoutRequest)) return false;

            Navigate(RegionNames.MainWindowRegion, ViewNames.LoginViewName);
            UiDispatcher.Invoke(SetMainWindowSizeForLogIn);

            return true;
        }

        private void Navigate(string regionName, string viewName) 
        {
            if (string.IsNullOrWhiteSpace(viewName))   throw new ArgumentNullException(nameof(viewName));
            if (string.IsNullOrWhiteSpace(regionName)) throw new ArgumentNullException(nameof(regionName));
            
            UiDispatcher.Invoke(() => regionManager.RequestNavigate(regionName, viewName));
        }

        private void SetMainWindowSizeForShell() 
        {
            Application.Current.MainWindow.MinWidth  = 800;
            Application.Current.MainWindow.MinHeight = 600;
        }

        private void SetMainWindowSizeForLogIn() 
        {
            Application.Current.MainWindow.MinWidth = 630;
            Application.Current.MainWindow.Width = 630;

            Application.Current.MainWindow.MinHeight = 360;
            Application.Current.MainWindow.Height = 360;
        }
    }
}