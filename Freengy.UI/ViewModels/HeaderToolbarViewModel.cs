// Created by Laxale 05.05.2018
//
//

using System;

using Freengy.Base.DefaultImpl;
using Freengy.UI.Views;
using Freengy.Base.Messages;
using Freengy.Base.Messages.Base;
using Freengy.Base.Messages.Login;
using Freengy.Base.Messages.Notification;
using Freengy.Base.ViewModels;
using Freengy.Common.Helpers;
using Freengy.CommonResources.Styles;
using Freengy.Localization;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;
using Freengy.Networking.Messages;
using Freengy.UI.Messages;


namespace Freengy.UI.ViewModels 
{
    /// <summary>
    /// Вьюмодель <see cref="HeaderToolbarView"/>.
    /// </summary>
    internal class HeaderToolbarViewModel : WaitableViewModel 
    {
        private readonly DelayedEventInvoker delayedInvoker;

        private bool isServerOnline;
        private bool isLoggedIn;
        private DateTime loggedInTime;
        private string serverAddress;
        private string onlinePeriodNotice;


        public HeaderToolbarViewModel() 
        {
            ServerAddress = Url.Http.RootServerUrl;
            delayedInvoker = new DelayedEventInvoker(1000);
            delayedInvoker.DelayedEvent += OnTimerTick;

            OnlinePeriodNotice = StringResources.NotLoggedIn;

            this.Subscribe<MessageBase>(OnLoggedIn);
            this.Subscribe<MessageBase>(OnLoggedOut);
            this.Subscribe<MessageServerOnlineStatus>(OnServerStatusInform);
        }


        /// <summary>
        /// Gets the value - is server online.
        /// </summary>
        public bool IsServerOnline 
        {
            get => isServerOnline;

            private set
            {
                if (isServerOnline == value) return;

                isServerOnline = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает значение - выполнен ли логин.
        /// </summary>
        public bool IsLoggedIn 
        {
            get => isLoggedIn;

            private set
            {
                if (isLoggedIn == value) return;

                isLoggedIn = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets current server address.
        /// </summary>
        public string ServerAddress 
        {
            get => serverAddress;

            private set
            {
                if (serverAddress == value) return;

                serverAddress = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает сообщение о времени нахождения в онлайне.
        /// </summary>
        public string OnlinePeriodNotice 
        {
            get => onlinePeriodNotice;

            private set
            {
                if (onlinePeriodNotice == value) return;

                onlinePeriodNotice = value;

                OnPropertyChanged();
            }
        }


        private void OnTimerTick() 
        {
            var span = DateTime.Now - loggedInTime;

            var hours = span.Hours;
            var minus = span.Minutes;
            var secos = span.Seconds;

            var hourString = hours < 10 ? $"0{ hours }" : hours.ToString();
            var minuString = minus < 10 ? $"0{ minus }" : minus.ToString();
            var secoString = secos < 10 ? $"0{ secos }" : secos.ToString();

            string status = $"{ StringResources.OnlinePeriod } { hourString }:{ minuString }:{ secoString }";

            OnlinePeriodNotice = status;

            delayedInvoker.RequestDelayedEvent();
        }

        private void OnLoggedIn(MessageBase message) 
        {
            if (!(message is MessageLogInSuccess))
            {
                return;
            }

            IsLoggedIn = true;
            loggedInTime = DateTime.Now;
            
            delayedInvoker.RemoveDelayedEventRequest();
            delayedInvoker.RequestDelayedEvent();
        }

        private void OnLoggedOut(MessageBase message) 
        {
            if (!(message is MessageLogoutRequest))
            {
                return;
            }

            IsLoggedIn = false;
            delayedInvoker.RemoveDelayedEventRequest();
            OnlinePeriodNotice = StringResources.NotLoggedIn;
        }

        private void OnServerStatusInform(MessageServerOnlineStatus statusInformer) 
        {
            IsServerOnline = statusInformer.IsOnline;
        }
    }
}