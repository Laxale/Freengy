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

using Freengy.Common.Constants;
using Freengy.Common.Database;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Interfaces;
using Freengy.Common.Models.Readonly;


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
            myId = controller.MyAccountState.Account.Id;
            sessionToken = controller.MySessionToken;
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
                actor.SetRequestAddress(Url.Http.ReplyFriendRequestUrl).AddHeader(FreengyHeaders.ClientSessionTokenHeaderName, sessionToken);

                FriendRequestReply reply = CreateReplyTo(userAccount);

                FriendRequestReply result;
                try
                {
                    Result<FriendRequestReply> postResult = actor.PostAsync<FriendRequestReply, FriendRequestReply>(reply).Result;

                    if (postResult.Failure)
                    {
                        throw new InvalidOperationException(postResult.Error.Message);
                    }

                    result = postResult.Value;
                }
                catch (Exception ex)
                {
                    ReportMessage(ex.Message);
                    return;
                }

                if (result.Reaction == FriendRequestReaction.Accept)
                {
                    requestAccounts.Remove(userAccount);
                    ReportMessage($"{ result.Request.TargetAccount.Name } is now your friend. Yey!");

                }
                else
                {
                    ReportMessage($"Failed to send reply to { userAccount.Account.Name }: { result.Reaction }");
                }
            }
        }

        private FriendRequestReply CreateReplyTo(UserAccountViewModel userAccount) 
        {
            var reply = new FriendRequestReply
            {
                Id = myId,
                Request = requestPairs[userAccount],
                Reaction = FriendRequestReaction.Accept
            };

            return reply;
        }
    }
}