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
using Freengy.Base.Windows;
using Freengy.Base.Messages;
using Freengy.Base.Helpers.Commands;
using Freengy.Common.Models;
using Freengy.FriendList.Views;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;

using Catel.IoC;
using Freengy.Common.Models.Readonly;
using Prism.Regions;


namespace Freengy.FriendList.ViewModels 
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    using Freengy.Base.Interfaces;


    /// <summary>
    /// Viewmodel for a <see cref="FriendListView"/>.
    /// </summary>
    public class FriendListViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<AccountState> friends = new ObservableCollection<AccountState>();
        private readonly ObservableCollection<FriendRequest> friendRequests = new ObservableCollection<FriendRequest>();

        private string mySessionToken;
        private UserAccount myAccount;


        public FriendListViewModel() 
        {
            FriendList = (ListCollectionView)CollectionViewSource.GetDefaultView(friends);
            FriendRequests = CollectionViewSource.GetDefaultView(friendRequests);

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
        public MyCommand<AccountState> CommandRemoveFriend { get; private set; }


        /// <summary>
        /// Gets teh collection of a friends of current user.
        /// </summary>
        //public ICollectionView FriendList { get; private set; }
        public ListCollectionView FriendList { get; private set; }

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
            CommandRemoveFriend = new MyCommand<AccountState>(RemoveFriendImpl, CanRemoveFriend);
            CommandShowFriendRequests = new MyCommand(ShowFriendRequestsImpl);
        }

        /// <inheritdoc />
        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override async void InitializeImpl() 
        {
            base.InitializeImpl();

            var dispatcher = ServiceLocatorProperty.ResolveType<IGuiDispatcher>();
            var loginController = ServiceLocatorProperty.ResolveType<ILoginController>();
            myAccount = loginController.CurrentAccount;
            mySessionToken = loginController.SessionToken;

            IEnumerable<AccountState> realFriends = await SearchRealFriends();
            IEnumerable<FriendRequest> requests = await SearchFriendRequests();
            
            foreach (AccountState friend in realFriends)
            {
                dispatcher.InvokeOnGuiThread(() => friends.Add(friend));
            }

            foreach (FriendRequest friendRequest in requests)
            {
                dispatcher.InvokeOnGuiThread(() => friendRequests.Add(friendRequest));
            }
        }


        #region privates

        private async Task<IEnumerable<AccountState>> SearchRealFriends() 
        {
            using (var httpActor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                httpActor.SetRequestAddress(Url.Http.SearchUsersUrl);
                SearchRequest searchRequest = SearchRequest.CreateFriendSearch(myAccount, string.Empty, mySessionToken);

                var result = await httpActor.PostAsync<SearchRequest, List<AccountStateModel>>(searchRequest);

                return result.Select(stateModel => new AccountState(stateModel));
            }
        }

        private async Task<IEnumerable<FriendRequest>> SearchFriendRequests() 
        {
            using (var httpActor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                httpActor.SetRequestAddress(Url.Http.SearchFriendRequestsUrl);
                SearchRequest searchRequest = SearchRequest.CreateAlienFriendRequestSearch(myAccount, mySessionToken);

                var result = await httpActor.PostAsync<SearchRequest, List<FriendRequest>>(searchRequest);

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

        private void RemoveFriendImpl(AccountState friendAccount) 
        {
            if (friendAccount == null) throw new ArgumentNullException(nameof(friendAccount));

            friends.Remove(friendAccount);
        }

        private bool CanRemoveFriend(AccountState friendAccount) 
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