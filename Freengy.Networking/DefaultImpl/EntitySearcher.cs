// Created by Laxale 18.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Freengy.Common.Enums;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;
using Freengy.Base.DefaultImpl;
using Freengy.Common.ErrorReason;
using Freengy.Common.Interfaces;
using Freengy.Common.Models.Readonly;

using NLog;


namespace Freengy.Networking.DefaultImpl 
{
    /// <summary>
    /// <see cref="IEntitySearcher"/> implementer.
    /// </summary>
    public class EntitySearcher : IEntitySearcher 
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Invoke entities search by given search parameters.
        /// </summary>
        /// <param name="searchRequest">Search parameters model.</param>
        /// <returns>Search result.</returns>
        public Task<Result<TResponce>> SearchEntitiesAsync<TResponce>(SearchRequest searchRequest) where TResponce : class, new() 
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var httpActor = MyServiceLocator.Instance.Resolve<IHttpActor>())
                    {
                        httpActor.SetRequestAddress(Url.Http.AddFriendUrl);

                        Result<TResponce> result = httpActor.PostAsync<SearchRequest, TResponce>(searchRequest).Result;

                        if (result.Failure)
                        {
                            throw new InvalidOperationException(result.Error.Message);
                        }

                        return Result<TResponce>.Ok(result.Value);
                    }
                }
                catch (Exception ex)
                {
                    string message = "Failed to search users";
                    logger.Error(ex, message);

                    return Result<TResponce>.Fail(new UnexpectedErrorReason(message));
                }
            });
        }

        /// <summary>
        /// Invoke users search by given search parameters.
        /// </summary>
        /// <param name="nameFilter">Search name filter.</param>
        /// <returns>Search result.</returns>
        public Task<Result<IEnumerable<UserAccount>>> SearchUsersAsync(string nameFilter) 
        {
            return Task.Run(() =>
            {
                try
                {
                    UserAccount currentAccount = MyServiceLocator.Instance.Resolve<ILoginController>().MyAccountState.Account;

                    var searchRequest = new SearchRequest
                    {
                        Entity = SearchEntity.Users,
                        SearchFilter = nameFilter,
                        SenderId = currentAccount.Id
                    };

                    using (var httpActor = MyServiceLocator.Instance.Resolve<IHttpActor>())
                    {
                        httpActor.SetRequestAddress(Url.Http.Search.SearchUsersUrl);

                        Result<List<UserAccountModel>> result = httpActor.PostAsync<SearchRequest, List<UserAccountModel>>(searchRequest).Result;

                        if (result.Failure)
                        {
                            throw new InvalidOperationException(result.Error.Message);
                        }

                        return Result<IEnumerable<UserAccount>>.Ok(result.Value.Select(model => new UserAccount(model)));
                    }
                }
                catch (Exception ex)
                {
                    string message = "Failed to search users";
                    logger.Error(ex, message);

                    return Result<IEnumerable<UserAccount>>.Fail(new UnexpectedErrorReason(message));
                }
            });
        }
    }
}
