// Created by Laxale 29.11.2016
//
//


namespace Freengy.Settings.Helpers 
{
    using System;
    using System.Windows;
    using System.Collections.Generic;

    using Freengy.Settings.Views;
    using Freengy.Settings.Messages;
    using Freengy.Settings.ViewModels;
    
    using Catel.IoC;
    using Catel.Messaging;


    /// <summary>
    /// This is intended to provide datacontext for known local views without setting
    /// it explicitly in xaml or in back-code. Explicit setting causes design-time
    /// exceptions, as viewmodels can not be constructed without all used types being
    /// registered in Catel infrastructure
    /// </summary>
    internal class DataContextSetter 
    {
        private readonly ITypeFactory typeFactory;
        private static readonly object Locker = new object();
        private readonly IDictionary<string, Type> contextPairs = new Dictionary<string, Type>();


        #region Singleton

        private static DataContextSetter instance;

        private DataContextSetter() 
        {
            this.FillContextPairs();
            this.typeFactory = this.GetTypeFactory();

            ServiceLocator.Default.ResolveType<IMessageMediator>().Register<MessageRequestContext>(this, this.MessageListener);
        }

        public static DataContextSetter Instance 
        {
            get
            {
                lock (Locker)
                {
                    return DataContextSetter.instance ?? (DataContextSetter.instance = new DataContextSetter());
                }
            }
        }
            

        #endregion Singleton


        public void SetDatatContext(FrameworkElement requester) 
        {
            string requesterTypeName = requester.GetType().FullName;
            Type requesterContextType = this.contextPairs[requesterTypeName];
            object dataContext = this.typeFactory.CreateInstance(requesterContextType);

            requester.DataContext = dataContext;
        }


        private void FillContextPairs() 
        {
            this.contextPairs.Add(typeof(GameListSettingsView).FullName, typeof(GameListSettingsViewModel));
            this.contextPairs.Add(typeof(FriendListSettingsView).FullName, typeof(FriendListSettingsViewModel));
        }

        [MessageRecipient]
        private void MessageListener(MessageRequestContext requestMessage)
        {
            this.SetDatatContext(requestMessage.RequesterInstance);
        }
    }
}