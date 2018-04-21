// Created by Laxale 21.04.2018
//
//

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using Freengy.Common.Models;
using Freengy.Base.ViewModels;
using Freengy.FriendList.Views;


namespace Freengy.FriendList.ViewModels 
{
    /// <summary>
    /// Viewmodel for the <see cref="FriendRequestsView"/>.
    /// </summary>
    internal class FriendRequestsViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<UserAccountViewModel> requests = new ObservableCollection<UserAccountViewModel>();


        public FriendRequestsViewModel(IEnumerable<FriendRequest> requests) 
        {
            var accounts = 
                requests?
                    .Select(request => new UserAccount(request.RequesterAccount))
                    .Select(account => new UserAccountViewModel(account))
                    ?? throw new ArgumentNullException(nameof(requests));

            this.requests.AddRange(accounts);

            Requests = CollectionViewSource.GetDefaultView(this.requests);
        }


        /// <summary>
        /// Gets the collection of incoming friend requests.
        /// </summary>
        public ICollectionView Requests { get; }
    }
}