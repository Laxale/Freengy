// Created by Laxale 23.10.2016
//
//

using System;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Freengy.Base.ViewModels;
using Freengy.Base.Interfaces;
using Freengy.Common.Models;

using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using Freengy.FriendList.Views;


namespace Freengy.FriendList.ViewModels 
{
    public class FriendListViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<UserAccount> friendList = new ObservableCollection<UserAccount>();

        
        protected override void SetupCommands() 
        {
            CommandAddFriend = new Command(AddFriendImpl, CanAddFriend);
            CommandRemoveFriend = new Command<UserAccount>(RemoveFriendImpl, CanRemoveFriend);
        }

        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            FillFriendList();

            FriendList = CollectionViewSource.GetDefaultView(friendList);
        }


        /// <summary>
        /// Command to add new friend.
        /// </summary>
        public Command CommandAddFriend { get; private set; }

        /// <summary>
        /// Command to remove a friend.
        /// </summary>
        public Command<UserAccount> CommandRemoveFriend { get; private set; }


        public ICollectionView FriendList { get; private set; }


        #region privates

        private void FillFriendList() 
        {
            var friendOne = serviceLocator.ResolveType<UserAccount>();
            var friendTwo = serviceLocator.ResolveType<UserAccount>();
            
            friendList.Add(friendOne);
            friendList.Add(friendTwo);
        }

        private async void AddFriendImpl() 
        {
            // just for testing
            var viewModel = new AddNewFriendViewModel();
            var service = serviceLocator.ResolveType<IUIVisualizerService>();
            var result = await service.ShowDialogAsync(viewModel);

            friendList.Add(null);
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