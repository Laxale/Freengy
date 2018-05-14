// Created by Laxale 24.04.2018
//
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Freengy.Base.DefaultImpl;
using Freengy.Base.Interfaces;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;
using Freengy.Base.Messages;
using Freengy.Base.Models.Update;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Interfaces;
using Freengy.Networking.Helpers;
using Freengy.Networking.Messages;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// <see cref="IFriendStateController"/> implementer.
    /// </summary>
    internal class FriendStateController : IFriendStateController, IUserActivity 
    {
        private static readonly object Locker = new object();

        private static FriendStateController instance;

        private readonly IMyServiceLocator serviceLocator = MyServiceLocator.Instance;
        private readonly List<AccountState> friendStates = new List<AccountState>();

        private bool inited;
        private bool isActivityStarted;
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


        /// <summary>
        /// Возвращает значение - можно ли остановить данную активити без ведома юзера.
        /// </summary>
        public bool CanCancelInSilent { get; } = true;


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

        /// <summary>
        /// Get current friend accounts states.
        /// </summary>
        /// <returns>Collection of a friends accounts states.</returns>
        public async Task<IEnumerable<AccountState>> GetFriendStatesAsync() 
        {
            List<AccountStateModel> stateModels;
            using (var httpActor = serviceLocator.Resolve<IHttpActor>())
            {
                httpActor.SetRequestAddress(Url.Http.SearchUsersUrl).SetClientSessionToken(sessionTokenGetter());
                SearchRequest searchRequest = SearchRequest.CreateFriendSearch(myAccountGetter(), string.Empty);

                Result<List<AccountStateModel>> result = await httpActor.PostAsync<SearchRequest, List<AccountStateModel>>(searchRequest);
                stateModels = result.Failure ? new List<AccountStateModel>() : result.Value;
            }

            foreach (AccountStateModel fromServer in stateModels)
            {
                AccountState savedState = friendStates.FirstOrDefault(state => state.Account.Id == fromServer.AccountModel.Id);

                if (savedState == null)
                {
                    friendStates.Add(new AccountState(fromServer));
                }
                else
                {
                    savedState.UpdateFromModel(fromServer);
                }
            }

            if (!isActivityStarted)
            {
                this.Publish(new MessageActivityChanged(this, true));
            }

            isActivityStarted = true;
            
            return friendStates;
        }


        /// <summary>
        /// Cancel activity.
        /// </summary>
        /// <returns>Result of a cancel attempt.</returns>
        public Result Cancel() 
        {
            friendStates.Clear();

            return Result.Ok();
        }


        internal void InitInternal() 
        {
            if (inited) return;

            loginController = serviceLocator.Resolve<ILoginController>();

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
                var savedState = friendStates.FirstOrDefault(state => state.Account.Id == stateModel.AccountModel.Id);
                IEnumerable<FriendUpdate> updates = null;

                if (savedState == null)
                {
                    friendStates.Add(new AccountState(stateModel));
                }
                else
                {
                    updates = GetUpdates(savedState, stateModel);

                    savedState.UpdateFromModel(stateModel);
                }

                this.Publish(new MessageFriendStateUpdate(savedState));

                if (updates != null)
                {
                    this.Publish(new MessageFriendUpdates(updates));
                }
            }
        }


        private IEnumerable<FriendUpdate> GetUpdates(AccountState beforeUpdate, AccountStateModel fromServerModel) 
        {
            var updates = new List<FriendUpdate>();

            if (beforeUpdate.Account.Level != fromServerModel.AccountModel.Level)
            {
                updates.Add(new LevelUpdate(beforeUpdate.Account, false));
            }

            if (beforeUpdate.UserAddress != fromServerModel.Address)
            {
                updates.Add(new AddressUpdate(beforeUpdate.Account, fromServerModel.Address));
            }

            if (beforeUpdate.Account.Name != fromServerModel.AccountModel.Name) 
            {
                updates.Add(new NameUpdate(beforeUpdate.Account, beforeUpdate.Account.Name));
            }

            if (beforeUpdate.AccountStatus != fromServerModel.OnlineStatus)
            {
                updates.Add(new OnlineStatusUpdate(beforeUpdate.Account, fromServerModel.OnlineStatus));
            }

            if (beforeUpdate.Account.Privilege != fromServerModel.AccountModel.Privilege)
            {
                updates.Add(new PrivilegeUpdate(beforeUpdate.Account, fromServerModel.AccountModel.Privilege));
            }

            return updates;
        }
    }
}