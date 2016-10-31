// Created by Laxale 23.10.2016
//
//


namespace Freengy.FriendList.ViewModels 
{
    using System;
    using System.Windows.Data;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;

    using Freengy.Base.ViewModels;
    using Freengy.Base.Interfaces;

    using Catel.IoC;
    using Catel.MVVM;


    public class FriendListViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<IUserAccount> friendList = new ObservableCollection<IUserAccount>();

        
        protected override void SetupCommands() 
        {
            this.CommandAddFriend = new Command(this.AddFriendImpl, this.CanAddFriend);
            this.CommandRemoveFriend = new Command<IUserAccount>(this.RemoveFriendImpl, this.CanRemoveFriend);
        }

        protected override async Task InitializeAsync() 
        {
            await base.InitializeAsync();

            this.FillFriendList();

            this.FriendList = CollectionViewSource.GetDefaultView(this.friendList);
        }


        public ICollectionView FriendList { get; private set; }


        #region Commands

        public Command CommandAddFriend { get; private set; }

        public Command<IUserAccount> CommandRemoveFriend { get; private set; }

        #endregion Commands


        #region privates

        private void FillFriendList() 
        {
            var friendOne = base.serviceLocator.ResolveType<IUserAccount>();
            var friendTwo = base.serviceLocator.ResolveType<IUserAccount>();
            
            this.friendList.Add(friendOne);
            this.friendList.Add(friendTwo);
        }

        private void AddFriendImpl() 
        {
            // just for testing
            var friendOne = base.serviceLocator.ResolveType<IUserAccount>();

            this.friendList.Add(friendOne);
        }
        private bool CanAddFriend() 
        {
            // just a stub
            return true;
        }

        private void RemoveFriendImpl(IUserAccount friendAccount)
        {
            if (friendAccount == null) throw new ArgumentNullException(nameof(friendAccount));

            this.friendList.Remove(friendAccount);
        }
        private bool CanRemoveFriend(IUserAccount friendAccount) 
        {
            // check if friend is not null and exists
            return friendAccount != null;
        }

        #endregion privates
    }
}