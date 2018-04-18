// Created by Laxale 18.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;

using Freengy.Base.Interfaces;
using Freengy.Common.Helpers.ErrorReason;
using Freengy.Common.Models;
using Freengy.Common.Helpers.Result;
using Freengy.Database.Context;

using NLog;


namespace Freengy.UI.DefaultImpl 
{
    internal static class DbSetExtensions 
    {
        /// <summary>
        /// Find user account with latest log-in time.
        /// </summary>
        /// <param name="accounts">Set of user accounts.</param>
        /// <returns>Latest logged-in account or null.</returns>
        public static UserAccount FindMax(this IEnumerable<UserAccount> accounts) 
        {
            UserAccount maxAccount = null;
            var maximumLoginTime = DateTime.MinValue;

            foreach (UserAccount account in accounts)
            {
                if (account.LastLogInTime > maximumLoginTime)
                {
                    maxAccount = account;
                    maximumLoginTime = account.LastLogInTime;
                }
            }

            return maxAccount;
        }
    }


    /// <summary>
    /// <see cref="IAccountManager"/> implementer.
    /// </summary>
    public class AccountManager : IAccountManager 
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Get account that was last logged in.
        /// </summary>
        /// <returns>Result of searching last user account.</returns>
        public Result<UserAccount> GetLastLoggedIn() 
        {
            try
            {
                using (var context = new SimpleDbContext<UserAccount>())
                {
                    UserAccount lastLogged = context.Objects.FindMax();

                    if (lastLogged == null)
                    {
                        return Result<UserAccount>.Fail(new UnexpectedErrorReason("No saved accounts found"));
                    }

                    lastLogged.SyncUniqueIdToId();

                    return Result<UserAccount>.Ok(lastLogged);
                }
            }
            catch (Exception ex)
            {
                string message = "Failed to get last logged in account";
                logger.Error(ex, message);

                return Result<UserAccount>.Fail(new UnexpectedErrorReason(message));
            }
        }

        /// <summary>
        /// Save given account as last logged in.
        /// </summary>
        /// <param name="account">Account to save as last logged in.</param>
        /// <returns>Save result.</returns>
        public Result SaveLastLoggedIn(UserAccount account) 
        {
            try
            {
                using (var context = new SimpleDbContext<UserAccount>())
                {
                    UserAccount savedAcc = context.Objects.FirstOrDefault(acc => acc.Id == account.Id);

                    if (savedAcc != null)
                    {
                        savedAcc.LastLogInTime = account.LastLogInTime;
                    }
                    else
                    {
                        context.Objects.Add(account);
                    }

                    context.SaveChanges();

                    return Result.Ok();
                }
            }
            catch (Exception ex)
            {
                string message = "Failed to save last logged in account";
                logger.Error(ex, message);

                return Result.Fail(new UnexpectedErrorReason(message));
            }
        }
    }
}