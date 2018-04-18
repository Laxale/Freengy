// Created by Laxale 18.04.2018
//
//

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
        Result SearchEntities(SearchRequest searchRequest);
    }
}