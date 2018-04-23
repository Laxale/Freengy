// Created by Laxale 23.10.2016
//
//

using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Freengy.Base.ViewModels;
using Freengy.Base.Windows;
using Freengy.Base.Messages;
using Freengy.Base.Interfaces;
using Freengy.Base.Chat.Interfaces;
using Freengy.Base.Helpers.Commands;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.FriendList.Views;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;

using Catel.IoC;


namespace Freengy.FriendList.ViewModels 
{
    using Freengy.Base.Extensions;


    /// <summary>
    /// Viewmodel for a <see cref="FriendListView"/>.
    /// </summary>
    public class FriendListViewModel : WaitableViewModel 
    {
        private readonly ObservableCollection<AccountState> friends = new ObservableCollection<AccountState>();
        private readonly ObservableCollection<FriendRequest> friendRequests = new ObservableCollection<FriendRequest>();

        private string mySessionToken;
        private UserAccount myAccount;
        private Dictionary<AccountState, IChatSession> startedChatSessions = new Dictionary<AccountState, IChatSession>();


        public FriendListViewModel() 
        {
            FriendList = (ListCollectionView)CollectionViewSource.GetDefaultView(friends);
            FriendRequests = CollectionViewSource.GetDefaultView(friendRequests);

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
        public MyCommand<AccountState> CommandRemoveFriend { get; private set; }

        /// <summary>
        /// Command to start a chat with friend.
        /// </summary>
        public MyCommand<AccountState> CommandStartChat { get; private set; }


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
            CommandStartChat = new MyCommand<AccountState>(StartChatImpl);
            CommandSearchFriend = new MyCommand(AddFriendImpl, CanAddFriend);
            CommandShowFriendRequests = new MyCommand(ShowFriendRequestsImpl);
            CommandRemoveFriend = new MyCommand<AccountState>(RemoveFriendImpl, CanRemoveFriend);
        }


        /// <inheritdoc />
        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override async void InitializeImpl() 
        {
            base.InitializeImpl();

            var dispatcher = ServiceLocatorProperty.ResolveType<IGuiDispatcher>();
            var loginController = ServiceLocatorProperty.ResolveType<ILoginController>();
            myAccount = loginController.CurrentAccount;
            mySessionToken = loginController.SessionToken;

            IEnumerable<AccountState> realFriends = await SearchRealFriends();
            IEnumerable<FriendRequest> requests = await SearchFriendRequests();
            
            foreach (AccountState friend in realFriends)
            {
                dispatcher.InvokeOnGuiThread(() => friends.Add(friend));
            }

            foreach (FriendRequest friendRequest in requests)
            {
                dispatcher.InvokeOnGuiThread(() => friendRequests.Add(friendRequest));
            }
        }


        #region privates

        private void StartChatImpl(AccountState targetAccountState) 
        {
            var chatHub = ServiceLocatorProperty.ResolveType<IChatHub>();
            var messageFactory = ServiceLocatorProperty.ResolveType<IChatMessageFactory>();
            
            messageFactory.Author = myAccount;
            
            IChatSession session;
            if (startedChatSessions.ContainsKey(targetAccountState))
            {
                session = 
                    chatHub.GetSession(startedChatSessions[targetAccountState].Id) 
                    ?? 
                    AddNewSession(chatHub, targetAccountState);
            }
            else
            {
                session = AddNewSession(chatHub, targetAccountState);
            }

            messageFactory.Author = myAccount;

            var testMessage = messageFactory.CreateMessage("Lets chat!");

            IChatMessageDecorator processedMessage;
            session.SendMessage(testMessage, out processedMessage);
        }

        private IChatSession AddNewSession(IChatHub chatHub, AccountState targetAccountState) 
        {
            var sessionFactory = ServiceLocatorProperty.ResolveType<IChatSessionFactory>();
            sessionFactory.SetNetworkInterface(SendMessageTo);

            IChatSession chatSession = sessionFactory.CreateInstance(targetAccountState.Account.Name, targetAccountState.Account.Name);
            chatHub.AddSession(chatSession);
            chatSession.AddToChat(targetAccountState);

            return chatSession;
        }

        private async Task<IEnumerable<AccountState>> SearchRealFriends() 
        {
            using (var httpActor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                httpActor.SetRequestAddress(Url.Http.SearchUsersUrl);
                SearchRequest searchRequest = SearchRequest.CreateFriendSearch(myAccount, string.Empty, mySessionToken);

                var result = await httpActor.PostAsync<SearchRequest, List<AccountStateModel>>(searchRequest);

                return result.Select(stateModel => new AccountState(stateModel));
            }
        }

        private async Task<IEnumerable<FriendRequest>> SearchFriendRequests() 
        {
            using (var httpActor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                httpActor.SetRequestAddress(Url.Http.SearchFriendRequestsUrl);
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

        private void RemoveFriendImpl(AccountState friendAccount) 
        {
            if (friendAccount == null) throw new ArgumentNullException(nameof(friendAccount));

            friends.Remove(friendAccount);
        }

        private bool CanRemoveFriend(AccountState friendAccount) 
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

        private void SendMessageTo(IChatMessageDecorator messageDecorator, AccountState account) 
        {
            using (var actor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                string chatAddress = $"{ account.UserAddress }{ Url.Http.Chat.ChatSubRoute }";
                actor.SetRequestAddress(chatAddress);

                ChatMessageModel messageModel = messageDecorator.ToModel();

                var result = actor.PostAsync<ChatMessageModel, ChatMessageModel>(messageModel).Result;
            }
        }

        #endregion privates
    }
}