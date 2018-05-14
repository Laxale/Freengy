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
using System.Windows.Media;
using Freengy.Base.ViewModels;
using Freengy.Base.Windows;
using Freengy.Base.Messages;
using Freengy.Base.Interfaces;
using Freengy.Base.Chat.Interfaces;
using Freengy.Base.DefaultImpl;
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
using Freengy.CommonResources;
using LocalizedRes = Freengy.Localization.StringResources;


namespace Freengy.FriendList.ViewModels 
{
    /// <summary>
    /// Viewmodel for a <see cref="FriendListView"/>.
    /// </summary>
    public class FriendListViewModel : WaitableViewModel 
    {
        private readonly IChatHub chatHub;
        private readonly ICurtainedExecutor curtainedExecutor;
        private readonly IFriendStateController friendStateController;
        private readonly ChatMessageSender messageSender = new ChatMessageSender();
        private readonly ObservableCollection<FriendRequest> friendRequests = new ObservableCollection<FriendRequest>();
        private readonly Dictionary<AccountState, IChatSession> startedChatSessions = new Dictionary<AccountState, IChatSession>();
        private readonly ObservableCollection<AccountStateViewModel> friendViewModels = new ObservableCollection<AccountStateViewModel>();

        private string mySessionToken;
        private UserAccount myAccount;


        public FriendListViewModel(ITaskWrapper taskWrapper, IGuiDispatcher guiDispatcher, IMyServiceLocator serviceLocator) : 
            base(taskWrapper, guiDispatcher, serviceLocator)
        {
            chatHub = ServiceLocator.Resolve<IChatHub>();
            curtainedExecutor = ServiceLocator.Resolve<ICurtainedExecutor>();
            friendStateController = ServiceLocator.Resolve<IFriendStateController>();

            FriendList = CollectionViewSource.GetDefaultView(friendViewModels);
            FriendRequests = CollectionViewSource.GetDefaultView(friendRequests);

            this.Subscribe<MessageNewFriendRequest>(OnNewFriendRequest);
            this.Subscribe<MessageReceivedMessage>(OnNewMessaggeReceived);
            this.Subscribe<MessageFriendStateUpdate>(OnFriendStateUpdated);
            this.Subscribe<MessageFriendRequestState>(OnFriendRequestReply);
            
            this.Publish(new MessageInitializeModelRequest(this, "Loading friends"));
        }

        ~FriendListViewModel() 
        {
            this.Unsubscribe();
        }

        
        /// <summary>
        /// Command to search for new friends.
        /// </summary>
        public MyCommand CommandSearchFriends { get; private set; }

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
        /// Command to show chat session with the given friend.
        /// </summary>
        public MyCommand<AccountState> CommandShowChatSessionWith { get; private set; }


        /// <summary>
        /// Gets teh collection of a friends of current user.
        /// </summary>
        public ICollectionView FriendList { get; }

        /// <summary>
        /// Gets the collection of a friendrequests to current user.
        /// </summary>
        public ICollectionView FriendRequests { get; }

        /// <summary>
        /// Обновить вьюмодель.
        /// </summary>
        public override void Refresh() 
        {
            base.Refresh();

            friendViewModels.Clear();
            friendRequests.Clear();

            InitializeImpl();
        }


        protected override void SetupCommands() 
        {
            CommandSearchFriends = new MyCommand(SearchFriendsImpl);
            CommandStartChat = new MyCommand<AccountState>(StartChatImpl);
            CommandShowFriendRequests = new MyCommand(ShowFriendRequestsImpl);
            CommandShowChatSessionWith = new MyCommand<AccountState>(ShowChatSessionWithImpl);
            CommandRemoveFriend = new MyCommand<AccountStateViewModel>(RemoveFriendImpl, CanRemoveFriend);
        }

        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в Initialize().
        /// </summary>
        protected override async void InitializeImpl() 
        {
            base.InitializeImpl();

            var loginController = ServiceLocator.Resolve<ILoginController>();
            
            myAccount = loginController.MyAccountState.Account;
            mySessionToken = loginController.MySessionToken;

            IEnumerable<AccountState> realFriends = await friendStateController.GetFriendStatesAsync();
            IEnumerable<FriendRequest> requests = await SearchFriendRequests();
            
            foreach (AccountState friendState in realFriends)
            {
                var viewModel = ServiceLocator.Resolve<AccountStateViewModel>();
                viewModel.AccountState = friendState;
                GUIDispatcher.InvokeOnGuiThread(() => friendViewModels.Add(viewModel));
            }

            foreach (FriendRequest friendRequest in requests)
            {
                GUIDispatcher.InvokeOnGuiThread(() => friendRequests.Add(friendRequest));
            }
        }


        #region privates

        private void StartChatImpl(AccountState targetAccountState) 
        {
            var messageFactory = ServiceLocator.Resolve<IChatMessageFactory>();
            
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

            session.SendMessage(testMessage, out _);
            this.Publish(new MessageShowChatSession(session.Id));
        }

        private IChatSession AddNewSession(IChatHub chatHub, AccountState targetAccountState) 
        {
            var sessionFactory = ServiceLocator.Resolve<IChatSessionFactory>();
            sessionFactory.SetNetworkInterface(messageSender.SendMessageToFriend);

            IChatSession chatSession = sessionFactory.CreateInstance(targetAccountState.Account.Name, targetAccountState.Account.Name);
            chatHub.AddSession(chatSession);
            startedChatSessions.Add(targetAccountState, chatSession);
            chatSession.AddToChat(targetAccountState);

            return chatSession;
        }

        private async Task<IEnumerable<FriendRequest>> SearchFriendRequests() 
        {
            using (var httpActor = ServiceLocator.Resolve<IHttpActor>())
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

        private void SearchFriendsImpl() 
        {
            var view = new SearchFriendsView();
            SolidColorBrush graBrush = new CommonResourceExposer().GetBrush(CommonResourceExposer.LightGrayBrushKey);

            var win = new EmptyCustomToolWindow
            {
                Title = LocalizedRes.SearchFriends,
                Background = graBrush,
                MainContent = view,
                Height = 400,
                MaxHeight = 600,
                Width = 500,
                MaxWidth = 700
            };

            curtainedExecutor.ExecuteWithCurtain
            (
                KnownCurtainedIds.MainWindowId,
                () => win.ShowDialog()
            );
        }

        private async Task UpdateFriends() 
        {
            IEnumerable<AccountState> realFriends = await friendStateController.GetFriendStatesAsync();

            foreach (AccountState friendState in realFriends)
            {
                if (friendViewModels.All(viewModel => viewModel.AccountState.Account.Id != friendState.Account.Id))
                {
                    var viewModel = ServiceLocator.Resolve<AccountStateViewModel>();
                    viewModel.AccountState = friendState;
                    GUIDispatcher.InvokeOnGuiThread(() => friendViewModels.Add(viewModel));
                }
            }
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
            var viewModel = ServiceLocator.Resolve<FriendRequestsViewModel>();
            viewModel.SetRequests(friendRequests);
            var window = new EmptyCustomToolWindow
            {
                Title = LocalizedRes.NewFriendRequestNotice,
                Owner = Application.Current?.MainWindow,
                MainContent = new FriendRequestsView(),
                DataContext = viewModel,
                MaxHeight = 400,
                Width = 500,
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

        private void ShowChatSessionWithImpl(AccountState accountState) 
        {
            Guid targetSessionId = startedChatSessions[accountState].Id;

            this.Publish(new MessageShowChatSession(targetSessionId));
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

        private void OnNewMessaggeReceived(MessageReceivedMessage message) 
        {
            var senderModel = friendViewModels.FirstOrDefault(model => model.AccountState.Account.Id == message.SenderId);

            if (senderModel != null)
            {
                senderModel.HasNewMessages = true;

                if (!startedChatSessions.ContainsKey(senderModel.AccountState))
                {
                    IChatSession friendSession = chatHub.GetSession(message.SessionId);
                    startedChatSessions.Add(senderModel.AccountState, friendSession);
                }
            }
        }

        #endregion privates
    }
}