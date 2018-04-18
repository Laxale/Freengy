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

using Catel.MVVM;


namespace Freengy.FriendList.ViewModels 
{
    internal class AddNewFriendViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<UserAccount> foundUsers = new ObservableCollection<UserAccount>();


        public AddNewFriendViewModel() 
        {
            FoundUsers = CollectionViewSource.GetDefaultView(foundUsers);
        }


        /// <summary>
        /// Command to search registered users on server.
        /// </summary>
        public Command SearchUsersCommand { get; private set; }


        public ICollectionView FoundUsers { get; }


        /// <summary>
        /// This is called in InitializeAsync - force coderast to not init commands manually
        /// </summary>
        protected override void SetupCommands() 
        {
            SearchUsersCommand = new Command(SearchUsersImpl);
        }


        private void SearchUsersImpl() 
        {
            throw new NotImplementedException();
        }
    }
}