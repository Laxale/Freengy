// Created by Laxale 18.04.2018
//
//

using System;
using System.Threading.Tasks;

using Freengy.Base.Interfaces;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models;


namespace Freengy.Base.DefaultImpl 
{
    /// <summary>
    /// <see cref="IAccountManager"/> implementer.
    /// </summary>
    public class AccountManager : IAccountManager 
    {
        /// <summary>
        /// Get account that was last logged in.
        /// </summary>
        /// <returns>Result of searching last user account.</returns>
        public Result<UserAccount> GetLastLoggedIn() 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save given account as last logged in.
        /// </summary>
        /// <param name="account">Account to save as last logged in.</param>
        /// <returns>Save result.</returns>
        public Result SaveLastLoggedIn(UserAccount account) 
        {
            throw new NotImplementedException();
        }
    }
}