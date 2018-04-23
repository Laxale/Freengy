// Created by Laxale 20.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Freengy.Base.Interfaces;
using Freengy.Common.Helpers.Result;

using Catel.IoC;
using Freengy.Base.ErrorReasons;
using Freengy.Common.Enums;
using Freengy.Common.Helpers.ErrorReason;
using Freengy.Common.Models;
using Freengy.Networking.Interfaces;


namespace Freengy.UI.Helpers 
{
    /// <summary>
    /// Logout process controller.
    /// </summary>
    internal class LogOutController 
    {
        private readonly IServiceLocator serviceLocator;


        public LogOutController(IServiceLocator serviceLocator) 
        {
            this.serviceLocator = serviceLocator;
        }


        /// <summary>
        /// Invoke logout process.
        /// </summary>
        /// <returns>Logout result.</returns>
        public Result LogOut() 
        {
            IEnumerable<IUserActivity> activities = serviceLocator.ResolveType<IUserActivityHub>().GetRunningActivities().ToList();

            if (activities.Any())
            {
                MessageBoxResult result = MessageBox.Show("wow", CommonResources.StringResources.ProjectName, MessageBoxButton.YesNo);
                if (result == MessageBoxResult.No)
                {
                    return Result.Fail(new UserCancelledReason());
                }

                foreach (IUserActivity activity in activities)
                {
                    activity.Cancel();
                }
            }
            
            // check if can close activities
            Result<AccountStateModel> logoutResult = serviceLocator.ResolveType<ILoginController>().LogOut();

            if (logoutResult.Failure)
            {
                MessageBox.Show(logoutResult.Error.Message);
                return Result.Fail(logoutResult.Error);
            }

            if (logoutResult.Value.OnlineStatus == AccountOnlineStatus.Offline)
            {
                return Result.Ok();
            }

            return Result.Fail(new UnexpectedErrorReason($"Failed to log out: { logoutResult }"));
        }
    }
}