// Created by Laxale 18.04.2018
//
//

using System;

using Freengy.Common.Enums;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Search request model.
    /// </summary>
    public class SearchRequest 
    {
        /// <summary>
        /// Identifier of a request sender.
        /// </summary>
        public Guid SenderId { get; set; }

        /// <summary>
        /// Search entity type.
        /// </summary>
        public SearchEntity Entity { get; set; }

        /// <summary>
        /// Name filter to search entities by.
        /// </summary>
        public string SearchFilter { get; set; }


        /// <summary>
        /// Create a friend-search request of a given user.
        /// </summary>
        /// <param name="account">Account to search it's friends.</param>
        /// <param name="nameFilter">Friends name filter.</param>
        /// <returns>Ready for use <see cref="SearchRequest"/> instance.</returns>
        public static SearchRequest CreateFriendSearch(UserAccount account, string nameFilter) 
        {
            return new SearchRequest
            {
                Entity = SearchEntity.Friends,
                SenderId = account.UniqueId,
                SearchFilter = nameFilter
            };
        }
    }
}