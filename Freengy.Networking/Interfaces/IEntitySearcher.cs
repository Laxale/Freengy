// Created by Laxale 18.04.2018
//
//

using System.Collections.Generic;
using System.Threading.Tasks;
using Freengy.Base.Models.Readonly;
using Freengy.Common.Models;
using Freengy.Common.Helpers.Result;


namespace Freengy.Networking.Interfaces 
{
    /// <summary>
    /// Interface of a searchable entities searcher.
    /// </summary>
    public interface IEntitySearcher 
    {
        /// <summary>
        /// Invoke entities search by given search parameters.
        /// </summary>
        /// <param name="searchRequest">Search parameters model.</param>
        /// <returns>Search result.</returns>
        Task<Result<TResponce>> SearchEntitiesAsync<TResponce>(SearchRequest searchRequest) where TResponce : class, new ();

        /// <summary>
        /// Invoke users search by given search parameters.
        /// </summary>
        /// <param name="nameFilter">Search name filter.</param>
        /// <returns>Search result.</returns>
        Task<Result<IEnumerable<UserAccount>>> SearchUsersAsync(string nameFilter);
    }
}