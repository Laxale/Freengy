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
using Freengy.Common.Helpers.ErrorReason;
using Freengy.Networking.Constants;
using Freengy.Networking.Interfaces;

using NLog;

using Catel.IoC;


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
                    using (var httpActor = ServiceLocator.Default.ResolveType<IHttpActor>())
                    {
                        httpActor.SetAddress(Url.Http.AddFriendUrl);

                        var responce = httpActor.PostAsync<SearchRequest, TResponce>(searchRequest).Result;

                        return Result<TResponce>.Ok(responce);
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
                    using (var httpActor = ServiceLocator.Default.ResolveType<IHttpActor>())
                    {
                        var searchRequest = new SearchRequest
                        {
                            Entity = SearchEntity.Users,
                            SearchFilter = nameFilter
                        };

                        httpActor.SetAddress(Url.Http.AddFriendUrl);

                        var responce = httpActor.PostAsync<SearchRequest, List<UserAccount>>(searchRequest).Result;

                        return Result<IEnumerable<UserAccount>>.Ok(responce.AsEnumerable());
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
