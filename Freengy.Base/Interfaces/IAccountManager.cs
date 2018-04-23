// Created by Laxale 18.04.2018
//
//

using Freengy.Common.Models;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models.Readonly;


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Interface for managing client accounts.
    /// </summary>
    public interface IAccountManager 
    {
        /// <summary>
        /// Get account that was last logged in.
        /// </summary>
        /// <returns>Result of searching last user account.</returns>
        Result<UserAccount> GetLastLoggedIn();

        /// <summary>
        /// Save given account as last logged in.
        /// </summary>
        /// <param name="account">Account to save as last logged in.</param>
        /// <returns>Save result.</returns>
        Result SaveLastLoggedIn(UserAccount account);
    }
}