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
using Freengy.Base.Helpers;
using Freengy.Base.Helpers.Commands;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.FriendList.Views;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;
using Freengy.Networking.DefaultImpl;
using Freengy.Networking.Messages;
using Freengy.Common.Interfaces;
using Freengy.Networking.Helpers;
using Freengy.Common.Helpers.Result;
using Freengy.Base.Messages.Notification;
using Freengy.Common.Enums;

using Catel.IoC;

using LocalizedRes = Freengy.Localization.StringResources;


namespace Freengy.FriendList.ViewModels 
{
    /// <summary>
    /// Viewmodel for a <see cref="FriendListView"/>.
    /// </summary>
    public class FriendListViewModel : WaitableViewModel 
    {
        private readonly ICurtainedExecutor curtainedExecutor;
        private readonly IFriendStateController friendStateController;
        private readonly ChatMessageSender messageSender = new ChatMessageSender();
        private readonly ObservableCollection<FriendRequest> friendRequests = new ObservableCollection<FriendRequest>();
        private readonly ObservableCollection<AccountStateViewModel> friendViewModels = new ObservableCollection<AccountStateViewModel>();
        private readonly Dictionary<AccountState, IChatSession> startedChatSessions = new Dictionary<AccountState, IChatSession>();

        private string mySessionToken;
        private UserAccount myAccount;


        public FriendListViewModel() 
        {
            curtainedExecutor = ServiceLocatorProperty.ResolveType<ICurtainedExecutor>();
            friendStateController = ServiceLocatorProperty.ResolveType<IFriendStateController>();

            FriendList = CollectionViewSource.GetDefaultView(friendViewModels);
            FriendRequests = CollectionViewSource.GetDefaultView(friendRequests);

            Mediator.Register<MessageFriendStateUpdate>(this, OnFriendStateUpdated);
            Mediator.Register<MessageFriendRequestState>(this, OnFriendRequestReply);
            Mediator.Register<MessageNewFriendRequest>(this, OnNewFriendRequest);
            Mediator.SendMessage(new MessageInitializeModelRequest(this, "Loading friends"));
        }

        ~FriendListViewModel() 
        {
            Mediator.UnregisterRecipient(this);
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
        public MyCommand<AccountStateViewModel> CommandRemoveFriend { get; private set; }

        /// <summary>
        /// Command to start a chat with friend.
        /// </summary>
        public MyCommand<AccountState> CommandStartChat { get; private set; }


        /// <summary>
        /// Gets teh collection of a friends of current user.
        /// </summary>
        public ICollectionView FriendList { get; }

        /// <summary>
        /// Gets the collection of a friendrequests to current user.
        /// </summary>
        public ICollectionView FriendRequests { get; }


        /// <inheritdoc />
        public override void Refresh() 
        {
            base.Refresh();

            friendViewModels.Clear();
            friendRequests.Clear();

            InitializeImpl();
        }


        protected override void SetupCommands() 
        {
            CommandStartChat = new MyCommand<AccountState>(StartChatImpl);
            CommandSearchFriend = new MyCommand(RequestFriendImpl, CanAddFriend);
            CommandShowFriendRequests = new MyCommand(ShowFriendRequestsImpl);
            CommandRemoveFriend = new MyCommand<AccountStateViewModel>(RemoveFriendImpl, CanRemoveFriend);
        }
        
        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override async void InitializeImpl() 
        {
            base.InitializeImpl();

            var loginController = ServiceLocatorProperty.ResolveType<ILoginController>();
            
            myAccount = loginController.MyAccountState.Account;
            mySessionToken = loginController.MySessionToken;

            IEnumerable<AccountState> realFriends = await friendStateController.GetFriendStatesAsync();
            IEnumerable<FriendRequest> requests = await SearchFriendRequests();
            
            foreach (AccountState friendState in realFriends)
            {
                GUIDispatcher.InvokeOnGuiThread(() => friendViewModels.Add(new AccountStateViewModel(friendState)));
            }

            foreach (FriendRequest friendRequest in requests)
            {
                GUIDispatcher.InvokeOnGuiThread(() => friendRequests.Add(friendRequest));
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
            sessionFactory.SetNetworkInterface(messageSender.SendMessageToFriend);

            IChatSession chatSession = sessionFactory.CreateInstance(targetAccountState.Account.Name, targetAccountState.Account.Name);
            chatHub.AddSession(chatSession);
            startedChatSessions.Add(targetAccountState, chatSession);
            chatSession.AddToChat(targetAccountState);

            return chatSession;
        }

        private async Task<IEnumerable<FriendRequest>> SearchFriendRequests() 
        {
            using (var httpActor = ServiceLocatorProperty.ResolveType<IHttpActor>())
            {
                httpActor.SetRequestAddress(Url.Http.SearchFriendRequestsUrl).SetClientSessionToken(mySessionToken);
                SearchRequest searchRequest = SearchRequest.CreateAlienFriendRequestSearch(myAccount);

                Result<List<FriendRequest>> result = await httpActor.PostAsync<SearchRequest, List<FriendRequest>>(searchRequest);

                if (result.Failure)
                {
                    ReportMessage(result.Error.Message);
                }

                return result.Value;
            }
        }

        private async void RequestFriendImpl() 
        {
            curtainedExecutor.ExecuteWithCurtain
            (
                KnownCurtainedIds.MainWindowId,
                () => new AddNewFriendWindow().ShowDialog()
            );

            //await UpdateFriends();
        }

        private async Task UpdateFriends() 
        {
            IEnumerable<AccountState> realFriends = await friendStateController.GetFriendStatesAsync();

            foreach (AccountState friendState in realFriends)
            {
                if (friendViewModels.All(viewModel => viewModel.AccountState.Account.Id != friendState.Account.Id))
                {
                    GUIDispatcher.InvokeOnGuiThread(() => friendViewModels.Add(new AccountStateViewModel(friendState)));
                }
            }
        }

        private bool CanAddFriend() 
        {
            // just a stub
            return true;
        }

        private void RemoveFriendImpl(AccountStateViewModel viewModel) 
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            friendViewModels.Remove(viewModel);
        }

        private bool CanRemoveFriend(AccountStateViewModel friendAccount) 
        {
            // check if friend is not null and exists
            return friendAccount != null;
        }

        private async void ShowFriendRequestsImpl() 
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

            curtainedExecutor.ExecuteWithCurtain
            (
                KnownCurtainedIds.MainWindowId,
                () => window.ShowDialog()
            );

            foreach (UserAccountViewModel accountViewModel in viewModel.GetAcceptedAccounts())
            {
                var requestToRemove = friendRequests.First(request => request.RequesterAccount.Id == accountViewModel.Account.Id);

                friendRequests.Remove(requestToRemove);
            }

            await UpdateFriends();
        }

        private void OnFriendStateUpdated(MessageFriendStateUpdate message) 
        {
            var targetStateViewModel = friendViewModels.FirstOrDefault(viewModel => viewModel.AccountState.Account.Id == message.FriendState.Account.Id);

            if(targetStateViewModel == null) throw new InvalidOperationException($"Friend account { message.FriendState.Account.Name } not found");

            // dont update anything. Account object already updated. Just raise propertychanged
            //targetState.UpdateFromModel(null);
            targetStateViewModel.RaiseAccountPropertyCahnged();
        }

        private async void OnFriendRequestReply(MessageFriendRequestState message) 
        {
            string text = $"User { message.RepliedAccount.Name } replied for your friendrequest: { message.RequestReaction }";

            MessageBox.Show(text, LocalizedRes.ProjectName, MessageBoxButton.OK);

            if (message.RequestReaction == FriendRequestReaction.Accept)
            {
                await UpdateFriends();
            }
        }

        private void OnNewFriendRequest(MessageNewFriendRequest requestMessage) 
        {
            if (friendRequests.Contains(requestMessage.NewFriendRequest))
            {
                throw new InvalidOperationException($"Friendrequest from { requestMessage.NewFriendRequest.RequesterAccount.Name } already cached");
            }

            GUIDispatcher.BeginInvokeOnGuiThread(() => friendRequests.Add(requestMessage.NewFriendRequest));
        }

        #endregion privates
    }
}