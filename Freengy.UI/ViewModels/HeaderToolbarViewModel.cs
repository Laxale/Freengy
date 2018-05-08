// Created by Laxale 05.05.2018
//
//

using System;
using Freengy.Base.DefaultImpl;
using Freengy.UI.Views;
using Freengy.Base.Messages;
using Freengy.Base.Messages.Notification;
using Freengy.Base.ViewModels;
using Freengy.CommonResources.Styles;


namespace Freengy.UI.ViewModels 
{
    /// <summary>
    /// Вьюмодель <see cref="HeaderToolbarView"/>.
    /// </summary>
    internal class HeaderToolbarViewModel : WaitableViewModel 
    {
        private bool isServerOnline;


        public HeaderToolbarViewModel() 
        {
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


        private void OnServerStatusInform(MessageServerOnlineStatus statusInformer) 
        {
            IsServerOnline = statusInformer.IsOnline;
        }
    }
}