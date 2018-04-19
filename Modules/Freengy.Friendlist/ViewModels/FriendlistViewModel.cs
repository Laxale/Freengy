// Created by Laxale 23.10.2016
//
//

using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Freengy.Base.ViewModels;
using Freengy.Base.Interfaces;
using Freengy.Common.Models;
using Freengy.FriendList.Views;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;

using Catel.IoC;
using Catel.MVVM;
using Catel.Services;


namespace Freengy.FriendList.ViewModels 
{
    public class FriendListViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<UserAccount> friendList = new ObservableCollection<UserAccount>();

        private UserAccount myAccount;

        
        protected override void SetupCommands() 
        {
            CommandSearchFriend = new Command(AddFriendImpl, CanAddFriend);
            CommandRemoveFriend = new Command<UserAccount>(RemoveFriendImpl, CanRemoveFriend);
        }

        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            FillDebugFriendList();

            FriendList = CollectionViewSource.GetDefaultView(friendList);

            myAccount = serviceLocator.ResolveType<ILoginController>().CurrentAccount;

            IEnumerable<UserAccount> friends = SearchRealFriends();

            foreach (UserAccount friend in friends)
            {
                friendList.Add(friend);
            }
        }


        /// <summary>
        /// Command to search for new friend.
        /// </summary>
        public Command CommandSearchFriend { get; private set; }

        /// <summary>
        /// Command to remove a friend.
        /// </summary>
        public Command<UserAccount> CommandRemoveFriend { get; private set; }

        /// <summary>
        /// Friends of current user.
        /// </summary>
        public ICollectionView FriendList { get; private set; }


        #region privates

        private void FillDebugFriendList() 
        {
            var friendOne = serviceLocator.ResolveType<UserAccount>();
            var friendTwo = serviceLocator.ResolveType<UserAccount>();
            
            friendList.Add(friendOne);
            friendList.Add(friendTwo);
        }

        private IEnumerable<UserAccount> SearchRealFriends() 
        {
            using (var httpActor = serviceLocator.ResolveType<IHttpActor>())
            {
                httpActor.SetAddress(Url.Http.SearchUsersUrl);
                SearchRequest searchRequest = SearchRequest.CreateFriendSearch(myAccount, string.Empty);

                List<UserAccount> result = httpActor.PostAsync<SearchRequest, List<UserAccount>>(searchRequest).Result;

                return result;
            }
        }

        private async void AddFriendImpl() 
        {
            var viewModel = new AddNewFriendViewModel(myAccount);
            var service = serviceLocator.ResolveType<IUIVisualizerService>();

            await service.ShowDialogAsync(viewModel);
        }

        private bool CanAddFriend() 
        {
            // just a stub
            return true;
        }

        private void RemoveFriendImpl(UserAccount friendAccount)
        {
            if (friendAccount == null) throw new ArgumentNullException(nameof(friendAccount));

            friendList.Remove(friendAccount);
        }
        private bool CanRemoveFriend(UserAccount friendAccount) 
        {
            // check if friend is not null and exists
            return friendAccount != null;
        }

        #endregion privates
    }
}