// Created by Laxale 24.04.2018
//
//

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Interface of a friendlist state controller.
    /// </summary>
    public interface IFriendStateController
    {
        /// <summary>
        /// Get the state of a friend account.
        /// </summary>
        /// <param name="friendId">Friend account identifier.</param>
        /// <returns><see cref="AccountState"/> of a friend or null.</returns>
        AccountState GetFriendState(Guid friendId);

        /// <summary>
        /// Get current friend accounts states.
        /// </summary>
        /// <returns>Collection of a friends accounts states.</returns>
        Task<IEnumerable<AccountState>> GetFriendStatesAsync();
    }
}