// Created by Laxale 27.04.2018
//
//

using System;
using System.Configuration;
using System.Reflection;

using Freengy.Base.Interfaces;
using Freengy.Common.Helpers.Result;
using Freengy.Common.Models;
using Freengy.Common.Models.Readonly;


namespace Freengy.UI.DefaultImpl 
{
    using Freengy.Common.Helpers.ErrorReason;

    using NLog;


    /// <summary>
    /// <see cref="IAccountManager"/> implementer. Uses app config file as storage.
    /// </summary>
    internal class ConfigFileAccountManager : IAccountManager 
    {
        private const string parameterName = "LastLoggedUserName";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Configuration uiConfiguration = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);


        /// <summary>
        /// Get account that was last logged in.
        /// </summary>
        /// <returns>Result of searching last user account.</returns>
        public Result<UserAccount> GetLastLoggedIn() 
        {
            try
            {
                string lastLoggedName = uiConfiguration.AppSettings.Settings[parameterName].Value;

                return Result<UserAccount>.Ok(new UserAccount(new UserAccountModel{ Name = lastLoggedName }));
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Failed to get user name from config");

                return Result<UserAccount>.Fail(new UnexpectedErrorReason(ex.Message));
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
                uiConfiguration.AppSettings.Settings.Remove(parameterName);
                uiConfiguration.AppSettings.Settings.Add(parameterName, account.Name);

                uiConfiguration.Save();

                return Result.Ok();
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Failed to save user name to config");

                return Result.Fail(new UnexpectedErrorReason(ex.Message));
            }
        }
    }
}