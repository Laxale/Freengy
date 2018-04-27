// Created by Laxale 18.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;

using Freengy.Base.Interfaces;
using Freengy.Common.Database;
using Freengy.Common.Extensions;
using Freengy.Common.Helpers.ErrorReason;
using Freengy.Common.Models;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models.Readonly;
using Freengy.Database.Context;

using NLog;


namespace Freengy.UI.DefaultImpl 
{
    internal static class DbSetExtensions 
    {
        /// <summary>
        /// Find user account model with latest log-in time.
        /// </summary>
        /// <param name="accounts">Set of user accounts.</param>
        /// <returns>Latest logged-in account or null.</returns>
        public static UserAccountModel FindLatestLogIn(this IEnumerable<UserAccountModel> accounts) 
        {
            UserAccountModel maxAccount = null;
            var maximumLoginTime = DateTime.MinValue;

            foreach (var account in accounts)
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
    public class DbAccountManager : IAccountManager 
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();


        /// <inheritdoc />
        /// <summary>
        /// Get account that was last logged in.
        /// </summary>
        /// <returns>Result of searching last user account.</returns>
        public Result<UserAccount> GetLastLoggedIn() 
        {
            try
            {
                using (var context = new SimpleDbContext<UserAccountModel>())
                {
                    var lastLogged = context.Objects.FindLatestLogIn();

                    if (lastLogged == null)
                    {
                        return Result<UserAccount>.Fail(new UnexpectedErrorReason("No saved accounts found"));
                    }

                    return Result<UserAccount>.Ok(new UserAccount(lastLogged));
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
                using (var context = new SimpleDbContext<UserAccountModel>())
                {
                    var savedAcc = context.Objects.FirstOrDefault(acc => acc.Id == ((DbObject) account).Id);

                    if (savedAcc != null)
                    {
                        savedAcc.LastLogInTime = account.LastLogInTime;
                    }
                    else
                    {
                        UserAccountModel model = account.ToModel();
                        context.Objects.Add(model);
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