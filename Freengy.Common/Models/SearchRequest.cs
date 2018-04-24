// Created by Laxale 18.04.2018
//
//

using System;

using Freengy.Common.Enums;
using Freengy.Common.Models.Readonly;


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
        /// <param name="sessionToken">User session token.</param>
        /// <returns>Ready for use <see cref="SearchRequest"/> instance.</returns>
        public static SearchRequest CreateFriendSearch(UserAccount account, string nameFilter, string sessionToken) 
        {
            if (string.IsNullOrWhiteSpace(sessionToken)) throw new ArgumentNullException(nameof(sessionToken));

            return new SearchRequest
            {
                Entity = SearchEntity.Friends,
                SenderId = account.Id,
                SearchFilter = nameFilter
            };
        }

        /// <summary>
        /// Create a request to search for user's outgoing friend requests.
        /// </summary>
        /// <param name="account">User account.</param>
        /// <returns>Ready to use search request instance.</returns>
        public static SearchRequest CreateUserFriendRequestSearch(UserAccount account) 
        {
            return CreateFriendRequestSearch(account, false);
        }

        /// <summary>
        /// Create a request to search for user's incoming friend requests.
        /// </summary>
        /// <param name="account">User account.</param>
        /// <returns>Ready to use search request instance.</returns>
        public static SearchRequest CreateAlienFriendRequestSearch(UserAccount account) 
        {
            return CreateFriendRequestSearch(account, true);
        }


        private static SearchRequest CreateFriendRequestSearch(UserAccount account, bool incoming) 
        {
            return new SearchRequest
            {
                SenderId = account.Id,
                Entity = incoming ? SearchEntity.IncomingFriendRequests : SearchEntity.OutgoingFriendRequests
            };
        }
    }
}