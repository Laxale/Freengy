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
            CommandRemoveFriend = new MyCommand<UserAccount>(RemoveFriendImpl, CanRemoveFriend);
            CommandShowFriendRequests = new MyCommand(ShowFriendRequestsImpl);
        }

        /// <inheritdoc />
        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override async void InitializeImpl() 
        {
            base.InitializeImpl();

            Dispatcher current = Dispatcher.CurrentDispatcher;
            FriendList = (ListCollectionView)CollectionViewSource.GetDefaultView(friends);
            FriendRequests = CollectionViewSource.GetDefaultView(friendRequests);
            FillDebugFriendList();

            var dispatcher = ServiceLocatorProperty.ResolveType<IGuiDispatcher>();
            dispatcher.BeginInvokeOnGuiThread(() =>
                                         {
                                             
                                         });

            

            var loginController = ServiceLocatorProperty.ResolveType<ILoginController>();
            myAccount = loginController.CurrentAccount;
            mySessionToken = loginController.SessionToken;

            //friends.Add(new UserAccount(new UserAccountModel() { Name = "tost fuk" }));

            IEnumerable<UserAccount> realFriends = await SearchRealFriends();
            IEnumerable<FriendRequest> requests = await SearchFriendRequests();
            //dispatcher.InvokeOnGuiThread(() => friends.Add(new UserAccount(new UserAccountModel() { Name = "tost 123" })));
            //friends.Add(new UserAccount(new UserAccountModel(){ Name = "tost"}));
            //FriendList.Dispatcher.Invoke(() => friends.Add(new UserAccount(new UserAccountModel(){ Name = "tost"})));
            await current.BeginInvoke((Action)delegate
                                              {
                                                  try
                                                  {
                                                      friends.Add(new UserAccount(new UserAccountModel() { Name = "tost" }));
                                                  }
                                                  catch (Exception e)
                                                  {
                                                      MessageBox.Show(e.Message);
                                                  }
                                              });
            
            foreach (UserAccount friend in realFriends)
            {
                try
                {
                    friends.Add(friend);
                    dispatcher.InvokeOnGuiThread(() => friends.Add(friend));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //ignore fail exception
                }
            }

            FriendList.Refresh();

            foreach (FriendRequest friendRequest in requests)
            {
                try
                {
                    friendRequests.Add(friendRequest);
                }
                catch (Exception ex)
                {
                    //ignore
                }
            }
        }


        #region privates

        private void FillDebugFriendList() 
        {
            friends.Add(new UserAccount(new UserAccountModel() { Name = "Friend 1", Level = 10}));
            friends.Add(new UserAccount(new UserAccountModel() { Name = "Friend 2", Level = 20}));
        }

        private async Task<IEnumerable<UserAccount>> SearchRealFriends() 
        {
            using (var httpActor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                httpActor.SetAddress(Url.Http.SearchUsersUrl);
                SearchRequest searchRequest = SearchRequest.CreateFriendSearch(myAccount, string.Empty, mySessionToken);

                var result = await httpActor.PostAsync<SearchRequest, List<UserAccountModel>>(searchRequest);
                
                return result.Select(model => new UserAccount(model));
            }
        }

        private async Task<IEnumerable<FriendRequest>> SearchFriendRequests() 
        {
            using (var httpActor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                httpActor.SetAddress(Url.Http.SearchFriendRequestsUrl);
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