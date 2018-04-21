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
using Freengy.Networking.Constants;

using NLog;

using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Freengy.Base.Helpers;


namespace Freengy.FriendList.ViewModels 
{
    using Freengy.Base.Helpers.Commands;
    using Freengy.Base.Messages;


    /// <summary>
    /// Viewmodel for <see cref="AddNewFriendWindow"/>.
    /// </summary>
    internal class AddNewFriendViewModel : WaitableViewModel, IDisposable 
    {
        private readonly UserAccount myAccount;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly DelayedEventInvoker delayedInvoker = new DelayedEventInvoker(400);
        private readonly ObservableCollection<UserAccount> foundUsers = new ObservableCollection<UserAccount>();
        private readonly ObservableCollection<FriendRequest> requestResults = new ObservableCollection<FriendRequest>();

        private string searchFilter;


        public AddNewFriendViewModel() 
        {
            myAccount = ServiceLocatorProperty.ResolveType<ILoginController>().CurrentAccount;

            delayedInvoker.DelayedEvent += SearchUsersImpl;
            FoundUsers = CollectionViewSource.GetDefaultView(foundUsers);
            SentRequestResults = CollectionViewSource.GetDefaultView(requestResults);

            Mediator.SendMessage(new MessageInitializeModelRequest(this, "Loading user search"));
        }

        ~AddNewFriendViewModel() 
        {
            Dispose(false);
        }


        /// <summary>
        /// Command to search registered users on server.
        /// </summary>
        public MyCommand SearchUsersCommand { get; private set; }

        /// <summary>
        /// Command to send a friendship request.
        /// </summary>
        public MyCommand<UserAccount> RequestFriendCommand { get; private set; }


        /// <summary>
        /// A collection of currently found users.
        /// </summary>
        public ICollectionView FoundUsers { get; }

        /// <summary>
        /// A collection of request results.
        /// </summary>
        public ICollectionView SentRequestResults { get; }

        /// <summary>
        /// Filter to search user accounts by.
        /// </summary>
        public string SearchFilter 
        {
            get => searchFilter;

            set
            {
                if (searchFilter == value) return;

                searchFilter = value;

                OnPropertyChanged();

                delayedInvoker.RequestDelayedEvent();
            }
        }

        
        /// <inheritdoc />
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
            SearchUsersCommand = new MyCommand(SearchUsersImpl);
            RequestFriendCommand = new MyCommand<UserAccount>(RequestFriend);
        }


        private async void SearchUsersImpl() 
        {
            if (string.IsNullOrWhiteSpace(SearchFilter)) return;

            var searcher = ServiceLocatorProperty.ResolveType<IEntitySearcher>();

            var searchResult = await searcher.SearchUsersAsync(SearchFilter);

            if (searchResult.Failure)
            {
                ReportMessage(searchResult.Error.Message);
            }
            else
            {
                GUIDispatcher.InvokeOnGuiThread(() =>
                {
                    foundUsers.Clear();

                    foreach (UserAccount foundUser in searchResult.Value)
                    {
                        foundUsers.Add(foundUser);
                    }
                });
            }
        }

        private void RequestFriend(UserAccount targetAccount) 
        {
            try
            {
                using (var httpActor = ServiceLocatorProperty.ResolveType<IHttpActor>())
                {
                    httpActor.SetAddress(Url.Http.AddFriendUrl);

                    FriendRequest request = FriendRequest.Create(myAccount, targetAccount);
                    FriendRequest result = httpActor.PostAsync<FriendRequest, FriendRequest>(request).Result;

                    requestResults.Add(result);
                }
            }
            catch (Exception ex)
            {
                string message = "Failed to send friend request";
                logger.Error(ex, message);
                ReportMessage(message);
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