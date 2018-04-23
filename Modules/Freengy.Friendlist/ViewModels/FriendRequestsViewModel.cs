// Created by Laxale 21.04.2018
//
//

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

using Freengy.Base.ViewModels;
using Freengy.Base.Helpers.Commands;
using Freengy.Common.Enums;
using Freengy.Common.Models;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;
using Freengy.FriendList.Views;

using Catel.IoC;
using Freengy.Common.Database;


namespace Freengy.FriendList.ViewModels 
{
    /// <summary>
    /// Viewmodel for the <see cref="FriendRequestsView"/>.
    /// </summary>
    internal class FriendRequestsViewModel : WaitableViewModel
    {
        private readonly Guid myId;
        private readonly string sessionToken;
        private readonly ObservableCollection<UserAccountViewModel> requestAccounts = new ObservableCollection<UserAccountViewModel>();
        private readonly Dictionary<UserAccountViewModel, FriendRequest> requestPairs = new Dictionary<UserAccountViewModel, FriendRequest>();


        public FriendRequestsViewModel(IEnumerable<FriendRequest> requests) 
        {
            var listedRequests = requests?.ToList() ?? throw new ArgumentNullException(nameof(requests));

            IEnumerable<UserAccountViewModel> accounts =
                listedRequests
                    .Select(request => new UserAccount(request.RequesterAccount))
                    .Select(account => new UserAccountViewModel(account));

            requestAccounts.AddRange(accounts);

            for (int index = 0; index < requestAccounts.Count; index++)
            {
                requestPairs.Add(requestAccounts[index], listedRequests[index]);
            }

            RequestAccounts = CollectionViewSource.GetDefaultView(requestAccounts);

            CommandAcceptFriend = new MyCommand<UserAccountViewModel>(AcceptFriendImpl);

            var controller = ServiceLocatorProperty.ResolveType<ILoginController>();
            myId = controller.CurrentAccount.Id;
            sessionToken = controller.SessionToken;
        }


        /// <summary>
        /// Command to accept a friend.
        /// </summary>
        public MyCommand<UserAccountViewModel> CommandAcceptFriend { get; }


        /// <summary>
        /// Gets the collection of incoming friend requests.
        /// </summary>
        public ICollectionView RequestAccounts { get; }


        private void AcceptFriendImpl(UserAccountViewModel userAccount) 
        {
            using (var actor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                actor.SetAddress(Url.Http.ReplyFriendRequestUrl);
                var reply = new FriendRequestReply
                {
                    Id = myId,
                    UserToken = sessionToken,
                    Request = requestPairs[userAccount],
                    Reaction = FriendRequestReaction.Accept
                };

                FriendRequestReply result;
                try
                {
                    result = actor.PostAsync<FriendRequestReply, FriendRequestReply>(reply).Result;
                }
                catch (Exception ex)
                {
                    ReportMessage(ex.Message);
                    return;
                }
                
                ReportMessage(result.EstablishedDate != DateTime.MinValue ? "Friend accepted. Yey!" : "Failed to send reply");
            }
        }
    }
}