// Created by Laxale 18.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

using Freengy.Base.Helpers.Commands;
using Freengy.Base.Messages;
using Freengy.Base.ViewModels;
using Freengy.Common.Models;
using Freengy.Common.Enums;
using Freengy.Common.Helpers;
using Freengy.FriendList.Views;
using Freengy.Networking.Interfaces;
using Freengy.Networking.Constants;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Interfaces;
using Freengy.Common.Models.Readonly;

using NLog;

using Freengy.Base.DefaultImpl;
using Freengy.Base.Interfaces;


namespace Freengy.FriendList.ViewModels 
{
    /// <summary>
    /// Viewmodel for <see cref="AddNewFriendWindow"/>.
    /// </summary>
    internal class SearchFriendsViewModel : WaitableViewModel, IDisposable 
    {
        private readonly UserAccount myAccount;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly List<UserAccount> myCurrentFriends = new List<UserAccount>();
        private readonly DelayedEventInvoker delayedInvoker = new DelayedEventInvoker(400);
        private readonly ObservableCollection<FriendRequest> requestResults = new ObservableCollection<FriendRequest>();
        private readonly ObservableCollection<UserAccountViewModel> foundUsers = new ObservableCollection<UserAccountViewModel>();

        private string searchFilter;


        public SearchFriendsViewModel(ITaskWrapper taskWrapper, IGuiDispatcher guiDispatcher, IMyServiceLocator serviceLocator) :
            base(taskWrapper, guiDispatcher, serviceLocator) 
        {
            myAccount = ServiceLocator.Resolve<ILoginController>().MyAccountState.Account;

            delayedInvoker.DelayedEvent += SearchUsersImpl;
            FoundUsers = CollectionViewSource.GetDefaultView(foundUsers);
            SentRequestResults = CollectionViewSource.GetDefaultView(requestResults);

            this.Publish(new MessageInitializeModelRequest(this, "Loading user search"));
        }

        ~SearchFriendsViewModel() 
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

        public void SetMyCurrentFriends(IEnumerable<UserAccount> friendAccounts) 
        {
            myCurrentFriends.Clear();
            myCurrentFriends.AddRange(friendAccounts);
        }

        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            IEnumerable<AccountState> myFriends = ServiceLocator.Resolve<IFriendStateController>().GetFriendStatesAsync().Result;

            SetMyCurrentFriends(myFriends.Select(friendState => friendState.Account));
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

            var searcher = ServiceLocator.Resolve<IEntitySearcher>();

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
                        bool isFriend = myCurrentFriends.Any(friendAccount => friendAccount.Id == foundUser.Id);
                        foundUsers.Add(new UserAccountViewModel(foundUser) { IsMyFriend = isFriend });
                    }
                });
            }
        }

        private void RequestFriend(UserAccount targetAccount) 
        {
            try
            {
                using (var httpActor = ServiceLocator.Resolve<IHttpActor>())
                {
                    httpActor.SetRequestAddress(Url.Http.AddFriendUrl);

                    FriendRequest request = FriendRequest.Create(myAccount, targetAccount);
                    Result<FriendRequest> result = httpActor.PostAsync<FriendRequest, FriendRequest>(request).Result;

                    if (result.Failure)
                    {
                        ReportMessage(result.Error.Message);
                        return;
                    }

                    requestResults.Add(result.Value);
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