// Created by Laxale 22.10.2016
//
//


using System.Windows;


namespace Freengy.UI.Helpers 
{
    using System;

    using Freengy.UI.Constants;
    using Freengy.Base.Messages;
    using Freengy.Base.Interfaces;
    using Freengy.Base.Exceptions;
    using Freengy.Base.DefaultImpl;
    using Freengy.Networking.Messages;
    using Freengy.GamePlugin.Messages;

    using Prism.Regions;

    using Catel.IoC;
    using Catel.Services;
    using Catel.Messaging;

    using Microsoft.Practices.Unity;

    
    /// <summary>
    /// Centralized handler of navigation request messages or any messages related to navigation
    /// </summary>
    internal class UiNavigator 
    {
        #region vars

        private readonly IPleaseWaitService waiter;
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer unityContainer;
        private readonly IResponsibilityChainer<MessageBase> requestHandleChain;
        private readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        private readonly IMessageMediator messageMediator = MessageMediator.Default;
        
        #endregion vars


        #region Singleton

        private static UiNavigator instance;

        private UiNavigator() 
        {
            this.requestHandleChain = new ResponsibilityChainer<MessageBase>();
            this.waiter = this.serviceLocator.ResolveType<IPleaseWaitService>();
            this.regionManager  = this.serviceLocator.ResolveType<IRegionManager>();
            this.unityContainer = this.serviceLocator.ResolveType<IUnityContainer>();

            this.messageMediator.Register<MessageBase>(this, this.MessageListener);

            this.SetupChainer();
        }

        public static UiNavigator Instance => UiNavigator.instance ?? (UiNavigator.instance = new UiNavigator());

        #endregion Singleton


        [MessageRecipient]
        private async void MessageListener(MessageBase message) 
        {
            bool handled = await this.requestHandleChain.HandleAsync(message);

            // must never call this. Default handler should always handle possibly unknown messages
            if (!handled) throw new NotHandledException();
        }

        private void SetupChainer() 
        {
            // possibility of having more navigation ways later
            this.requestHandleChain.AddHandler(this.HandleLogInFailedMessage);
            this.requestHandleChain.AddHandler(this.HandleLogInAttemptMessage);
            this.requestHandleChain.AddHandler(this.HandleLogInSuccessMessage);
            this.requestHandleChain.AddHandler(this.HandleRequestNavigate);
            // default handler is a good thing to catch unknown messages
            this.requestHandleChain.AddHandler(this.DefaultHandler);
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
            var requestGameUi = message as MessageRequestGameUi;

            if (requestGameUi == null) return false;

            UiDispatcher.Invoke(() => this.regionManager.RegisterViewWithRegion(RegionNames.GameRegion, requestGameUi.GameUiType));

            string gameViewName = requestGameUi.GameUiType.FullName;

            this.unityContainer.RegisterType(typeof(object), requestGameUi.GameUiType, gameViewName);

            this.Navigate(RegionNames.GameRegion, gameViewName);

            return true;
        }

        private bool HandleLogInFailedMessage(MessageBase message) 
        {
            var loggedInMessage = message as MessageLogInFailed;

            if (loggedInMessage == null) return false;

            this.waiter.Hide();

            this.Navigate(RegionNames.MainWindowRegion, ViewNames.ShellViewName);

            return true;
        }

        private bool HandleLogInSuccessMessage(MessageBase message) 
        {
            var loggedInMessage = message as MessageLogInSuccess;

            if (loggedInMessage == null) return false;

            this.waiter.Hide();

            this.Navigate(RegionNames.MainWindowRegion, ViewNames.ShellViewName);

            UiDispatcher.Invoke(this.SetMainWindowSizeForShell);

            return true;
        }

        private bool HandleLogInAttemptMessage(MessageBase message) 
        {
            var loggedInMessage = message as MessageLogInAttempt;

            if (loggedInMessage == null) return false;

            this.waiter.Show("Connecting...");

            return true;
        }

        private void Navigate(string regionName, string viewName) 
        {
            if (string.IsNullOrWhiteSpace(viewName))   throw new ArgumentNullException(nameof(viewName));
            if (string.IsNullOrWhiteSpace(regionName)) throw new ArgumentNullException(nameof(regionName));
            
            UiDispatcher.Invoke(() => this.regionManager.RequestNavigate(regionName, viewName));
        }

        private void SetMainWindowSizeForShell() 
        {
            Application.Current.MainWindow.MinWidth  = 800;
            Application.Current.MainWindow.MinHeight = 600;
        }
    }
}