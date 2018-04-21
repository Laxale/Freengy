// Created by Laxale 23.10.2016
//
//

using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;

using Freengy.Base.ViewModels;
using Freengy.Base.Helpers;
using Freengy.Base.Messages;
using Freengy.Base.Helpers.Commands;
using Freengy.CommonResources.Windows;
using Freengy.Common.Models;
using Freengy.FriendList.Views;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;

using Catel.IoC;

using Prism.Regions;


namespace Freengy.FriendList.ViewModels 
{
    using Prism;
    /// <summary>
    /// Viewmodel for a <see cref="FriendListView"/>.
    /// </summary>
    public class FriendListViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<UserAccount> friends = new ObservableCollection<UserAccount>();
        private readonly ObservableCollection<FriendRequest> friendRequests = new ObservableCollection<FriendRequest>();

        private string mySessionToken;
        private UserAccount myAccount;


        public FriendListViewModel() 
        {
            Mediator.SendMessage(new MessageInitializeModelRequest(this, "Loading friends"));
        }

        
        /// <summary>
        /// Command to search for new friend.
        /// </summary>
        public MyCommand CommandSearchFriend { get; private set; }

        /// <summary>
        /// Command to show incoming friend requests.
        /// </summary>
        public MyCommand CommandShowFriendRequests { get; private set; }

        /// <summary>
        /// Command to remove a friend.
        /// </summary>
        public MyCommand<UserAccount> CommandRemoveFriend { get; private set; }


        /// <summary>
        /// Gets teh collection of a friends of current user.
        /// </summary>
        public ICollectionView FriendList { get; private set; }

        /// <summary>
        /// Gets the collection of a friendrequests to current user.
        /// </summary>
        public ICollectionView FriendRequests { get; private set; }


        /// <inheritdoc />
        public override void Refresh() 
        {
            base.Refresh();

            friends.Clear();
            friendRequests.Clear();

            InitializeImpl();
        }


        protected override void SetupCommands() 
        {
            CommandSearchFriend = new MyCommand(AddFriendImpl, CanAddFriend);
            CommandRemoveFriend = new MyCommand<UserAccount>(RemoveFriendImpl, CanRemoveFriend);
            CommandShowFriendRequests = new MyCommand(ShowFriendRequestsImpl);
        }

        /// <inheritdoc />
        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            FillDebugFriendList();

            FriendList = CollectionViewSource.GetDefaultView(friends);
            FriendRequests = CollectionViewSource.GetDefaultView(friendRequests);

            var loginController = ServiceLocatorProperty.ResolveType<ILoginController>();
            myAccount = loginController.CurrentAccount;
            mySessionToken = loginController.SessionToken;

            IEnumerable<UserAccount> realFriends = SearchRealFriends();
            IEnumerable<FriendRequest> requests = SearchFriendRequests();

            foreach (UserAccount friend in realFriends)
            {
                friends.Add(friend);
            }

            foreach (FriendRequest friendRequest in requests)
            {
                friendRequests.Add(friendRequest);
            }
        }


        #region privates

        private void FillDebugFriendList() 
        {
            var friendOne = ServiceLocatorProperty.ResolveType<UserAccount>();
            var friendTwo = ServiceLocatorProperty.ResolveType<UserAccount>();
            
            friends.Add(friendOne);
            friends.Add(friendTwo);
        }

        private IEnumerable<UserAccount> SearchRealFriends() 
        {
            using (var httpActor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                httpActor.SetAddress(Url.Http.SearchUsersUrl);
                SearchRequest searchRequest = SearchRequest.CreateFriendSearch(myAccount, string.Empty, mySessionToken);

                List<UserAccount> result = httpActor.PostAsync<SearchRequest, List<UserAccount>>(searchRequest).Result;

                return result;
            }
        }

        private IEnumerable<FriendRequest> SearchFriendRequests() 
        {
            using (var httpActor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                httpActor.SetAddress(Url.Http.SearchFriendRequestsUrl);
                SearchRequest searchRequest = SearchRequest.CreateAlienFriendRequestSearch(myAccount, mySessionToken);

                var result = httpActor.PostAsync<SearchRequest, List<FriendRequest>>(searchRequest).Result;

                return result;
            }
        }

        private void AddFriendImpl() 
        {
            new AddNewFriendWindow().ShowDialog();
        }

        private bool CanAddFriend() 
        {
            // just a stub
            return true;
        }

        private void RemoveFriendImpl(UserAccount friendAccount) 
        {
            if (friendAccount == null) throw new ArgumentNullException(nameof(friendAccount));

            friends.Remove(friendAccount);
        }

        private bool CanRemoveFriend(UserAccount friendAccount) 
        {
            // check if friend is not null and exists
            return friendAccount != null;
        }

        private void ShowFriendRequestsImpl() 
        {
            var viewModel = new FriendRequestsViewModel(friendRequests);
            var window = new EmptyCustomToolWindow
            {
                Title = "Friend requests",
                Owner = Application.Current?.MainWindow,
                MainContent = new FriendRequestsView(),
                DataContext = viewModel,
                MaxHeight = 400,
                MaxWidth = 600
            };

            window.ShowDialog();
        }

        #endregion privates
    }
}