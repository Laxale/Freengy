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
using Freengy.FriendList.Views;

using Catel.MVVM;


namespace Freengy.FriendList.ViewModels 
{
    using Catel.Data;
    using Catel.IoC;

    using Freengy.Common.Enums;
    using Freengy.Networking.Interfaces;


    /// <summary>
    /// Viewmodel for <see cref="AddNewFriendWindow"/>.
    /// </summary>
    internal class AddNewFriendViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<UserAccount> foundUsers = new ObservableCollection<UserAccount>();


        public AddNewFriendViewModel() 
        {
            FoundUsers = CollectionViewSource.GetDefaultView(foundUsers);

            foundUsers.Add(new UserAccount() { Name = "Fuk" });
            foundUsers.Add(new UserAccount() { Name = "Wow", Level = 50 });
        }


        /// <summary>
        /// Command to search registered users on server.
        /// </summary>
        public Command SearchUsersCommand { get; private set; }


        public ICollectionView FoundUsers { get; }

        /// <summary>
        /// Filter to search user accounts by.
        /// </summary>
        public string SearchFilter 
        {
            get => (string)GetValue(SearchFilterProperty);

            set => SetValue(SearchFilterProperty, value);
        }

        public static readonly PropertyData SearchFilterProperty =
            RegisterProperty<AddNewFriendViewModel, string>(model => model.SearchFilter, () => string.Empty);


        /// <summary>
        /// This is called in InitializeAsync - force coderast to not init commands manually
        /// </summary>
        protected override void SetupCommands() 
        {
            SearchUsersCommand = new Command(SearchUsersImpl);
        }


        private void SearchUsersImpl() 
        {
            var searcher = serviceLocator.ResolveType<IEntitySearcher>();

            var parameters = new SearchRequest
            {
                Entity = SearchEntity.Users,
                SearchFilter = this.SearchFilter
            };

            var searchResult = searcher.SearchEntities(parameters);
        }
    }
}