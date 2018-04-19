// Created by Laxale 18.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

using Freengy.Base.ViewModels;
using Freengy.Common.Models;
using Freengy.Common.Enums;
using Freengy.Common.Helpers;
using Freengy.FriendList.Views;
using Freengy.Networking.Interfaces;

using Catel.Data;
using Catel.IoC;
using Catel.MVVM;


namespace Freengy.FriendList.ViewModels 
{
    /// <summary>
    /// Viewmodel for <see cref="AddNewFriendWindow"/>.
    /// </summary>
    internal class AddNewFriendViewModel : WaitableViewModel, IDisposable 
    {
        private readonly DelayedEventInvoker delayedInvoker = new DelayedEventInvoker(300);
        private readonly ObservableCollection<UserAccount> foundUsers = new ObservableCollection<UserAccount>();

        
        public AddNewFriendViewModel() 
        {
            delayedInvoker.DelayedEvent += SearchUsersImpl;
            FoundUsers = CollectionViewSource.GetDefaultView(foundUsers);
        }

        ~AddNewFriendViewModel() 
        {
            Dispose(false);
        }


        /// <summary>
        /// Command to search registered users on server.
        /// </summary>
        public Command SearchUsersCommand { get; private set; }


        public ICollectionView FoundUsers { get; }

        /// <summary>
        /// Filter to search user accounts by.
        /// </summary>
        public string SearchFilter 
        {
            get => (string)GetValue(SearchFilterProperty);

            set
            {
                if ((string) GetValue(SearchFilterProperty) == value) return;

                SetValue(SearchFilterProperty, value);

                delayedInvoker.RequestDelayedEvent();
            }
        }

        public static readonly PropertyData SearchFilterProperty =
            RegisterProperty<AddNewFriendViewModel, string>(model => model.SearchFilter, () => string.Empty);


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// This is called in InitializeAsync - force coderast to not init commands manually
        /// </summary>
        protected override void SetupCommands() 
        {
            SearchUsersCommand = new Command(SearchUsersImpl);
        }


        private async void SearchUsersImpl() 
        {
            if (string.IsNullOrWhiteSpace(SearchFilter)) return;

            var searcher = serviceLocator.ResolveType<IEntitySearcher>();

            var searchResult = await searcher.SearchUsersAsync(SearchFilter);

            if (searchResult.Failure)
            {
                ReportMessage(searchResult.Error.Message);
            }
            else
            {
                guiDispatcher.InvokeOnGuiThread(() =>
                {
                    foundUsers.Clear();

                    foreach (UserAccount foundUser in searchResult.Value)
                    {
                        foundUsers.Add(foundUser);
                    }
                });
            }
        }

        private void ReleaseUnmanagedResources() 
        {
            // TODO release unmanaged resources here
            delayedInvoker.DelayedEvent -= SearchUsersImpl;
        }

        private void Dispose(bool disposing) 
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
                delayedInvoker?.Dispose();
            }
        }
    }
}