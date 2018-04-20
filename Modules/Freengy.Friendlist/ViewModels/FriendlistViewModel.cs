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
using Freengy.Base.Helpers;
using Freengy.Base.Messages;
using Freengy.Common.Models;
using Freengy.FriendList.Views;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;

using Catel.IoC;
using Catel.MVVM;
using Catel.Services;


namespace Freengy.FriendList.ViewModels 
{
    /// <summary>
    /// Viewmodel for a <see cref="FriendListView"/>.
    /// </summary>
    public class FriendListViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<UserAccount> friendList = new ObservableCollection<UserAccount>();

        private UserAccount myAccount;


        public FriendListViewModel() 
        {
            Mediator.SendMessage(new MessageInitializeModelRequest(this, "Loading friends"));
        }

        
        protected override void SetupCommands() 
        {
            CommandSearchFriend = new MyCommand(AddFriendImpl, CanAddFriend);
            CommandRemoveFriend = new MyCommand(RemoveFriendImpl, CanRemoveFriend);
        }

        /// <inheritdoc />
        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            FillDebugFriendList();

            FriendList = CollectionViewSource.GetDefaultView(friendList);

            myAccount = ServiceLocatorProperty.ResolveType<ILoginController>().CurrentAccount;

            IEnumerable<UserAccount> friends = SearchRealFriends();

            foreach (UserAccount friend in friends)
            {
                friendList.Add(friend);
            }
        }


        /// <summary>
        /// Command to search for new friend.
        /// </summary>
        public MyCommand CommandSearchFriend { get; private set; }

        /// <summary>
        /// Command to remove a friend.
        /// </summary>
        public MyCommand CommandRemoveFriend { get; private set; }

        /// <summary>
        /// Friends of current user.
        /// </summary>
        public ICollectionView FriendList { get; private set; }


        #region privates

        private void FillDebugFriendList() 
        {
            var friendOne = ServiceLocatorProperty.ResolveType<UserAccount>();
            var friendTwo = ServiceLocatorProperty.ResolveType<UserAccount>();
            
            friendList.Add(friendOne);
            friendList.Add(friendTwo);
        }

        private IEnumerable<UserAccount> SearchRealFriends() 
        {
            using (var httpActor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                httpActor.SetAddress(Url.Http.SearchUsersUrl);
                SearchRequest searchRequest = SearchRequest.CreateFriendSearch(myAccount, string.Empty);

                List<UserAccount> result = httpActor.PostAsync<SearchRequest, List<UserAccount>>(searchRequest).Result;

                return result;
            }
        }

        private void AddFriendImpl(object notUsed) 
        {
            new AddNewFriendWindow().ShowDialog();
        }

        private bool CanAddFriend(object notUsed) 
        {
            // just a stub
            return true;
        }

        private void RemoveFriendImpl(object friendAccount) 
        {
            if (friendAccount == null) throw new ArgumentNullException(nameof(friendAccount));

            friendList.Remove((UserAccount)friendAccount);
        }
        private bool CanRemoveFriend(object friendAccount) 
        {
            // check if friend is not null and exists
            return (UserAccount)friendAccount != null;
        }

        #endregion privates
    }
}