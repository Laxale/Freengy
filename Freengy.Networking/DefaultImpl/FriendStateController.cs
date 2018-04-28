﻿// Created by Laxale 24.04.2018
//
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Freengy.Base.Interfaces;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;

using Catel.IoC;
using Catel.Messaging;
using Freengy.Common.Interfaces;
using Freengy.Networking.Helpers;
using Freengy.Networking.Messages;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// <see cref="IFriendStateController"/> implementer.
    /// </summary>
    internal class FriendStateController : IFriendStateController 
    {
        private static readonly object Locker = new object();

        private static FriendStateController instance;

        private readonly IMessageMediator mediator = MessageMediator.Default;
        private readonly IServiceLocator serviceLocator = ServiceLocator.Default;
        private readonly List<AccountState> friendStates = new List<AccountState>();

        private bool inited;
        private Func<string> sessionTokenGetter;
        private Func<UserAccount> myAccountGetter;
        private ILoginController loginController;


        private FriendStateController() { }

        
        /// <summary>
        /// Единственный инстанс <see cref="FriendStateController"/>.
        /// </summary>
        public static IFriendStateController ExposedInstance => InternalInstance;


        internal static FriendStateController InternalInstance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new FriendStateController());
                }
            }
        }


        /// <inheritdoc />
        /// <summary>
        /// Get the state of a friend account.
        /// </summary>
        /// <param name="friendId">Friend account identifier.</param>
        /// <returns><see cref="AccountState" /> of a friend or null.</returns>
        public AccountState GetFriendState(Guid friendId) 
        {
            if(friendId == Guid.Empty) throw new InvalidOperationException("Friend account id cannot be empty");

            AccountState friendState = friendStates.FirstOrDefault(state => state.Account.Id == friendId);

            return friendState;
        }

        /// <inheritdoc />
        /// <summary>
        /// Get current friend accounts states.
        /// </summary>
        /// <returns>Collection of a friends accounts states.</returns>
        public async Task<IEnumerable<AccountState>> GetFriendStatesAsync() 
        {
            List<AccountStateModel> stateModels;
            using (var httpActor = serviceLocator.ResolveType<IHttpActor>())
            {
                httpActor.SetRequestAddress(Url.Http.SearchUsersUrl).SetClientSessionToken(sessionTokenGetter());
                SearchRequest searchRequest = SearchRequest.CreateFriendSearch(myAccountGetter(), string.Empty);

                stateModels = await httpActor.PostAsync<SearchRequest, List<AccountStateModel>>(searchRequest);
            }

            foreach (AccountStateModel fromServer in stateModels)
            {
                AccountState savedState = friendStates.FirstOrDefault(state => state.Account.Id == fromServer.Account.Id);

                if (savedState == null)
                {
                    friendStates.Add(new AccountState(fromServer));
                }
                else
                {
                    savedState.UpdateFromModel(fromServer);
                }
            }

            return friendStates;
        }


        internal void InitInternal() 
        {
            if (inited) return;

            loginController = serviceLocator.ResolveType<ILoginController>();

            myAccountGetter = () => loginController.MyAccountState.Account;
            sessionTokenGetter = () => loginController.MySessionToken;

            inited = true;
        }

        /// <summary>
        /// Update cached friend account state by server message.
        /// </summary>
        /// <param name="stateModel">Account model to update from.</param>
        internal void UpdateFriendState(AccountStateModel stateModel) 
        {
            lock (Locker)
            {
                var savedState = friendStates.FirstOrDefault(state => state.Account.Id == stateModel.Account.Id);

                if (savedState == null)
                {
                    friendStates.Add(new AccountState(stateModel));
                }
                else
                {
                    savedState.UpdateFromModel(stateModel);
                }

                mediator.SendMessage(new MessageFriendStateUpdate(savedState));
            }
        }
    }
}