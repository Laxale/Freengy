﻿// Created by Laxale 20.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Freengy.Base.Interfaces;
using Freengy.Common.Helpers.Result;

using Freengy.Base.ErrorReasons;
using Freengy.Common.Enums;
using Freengy.Common.ErrorReason;
using Freengy.Common.Models;
using Freengy.Networking.Interfaces;

using LocalizedRes = Freengy.Localization.StringResources;


namespace Freengy.UI.Helpers 
{
    /// <summary>
    /// Logout process controller.
    /// </summary>
    internal class LogOutController 
    {
        private readonly IMyServiceLocator serviceLocator;


        public LogOutController(IMyServiceLocator serviceLocator) 
        {
            this.serviceLocator = serviceLocator;
        }


        /// <summary>
        /// Invoke logout process.
        /// </summary>
        /// <returns>Logout result.</returns>
        public Result LogOut() 
        {
            IEnumerable<IUserActivity> activities = serviceLocator.Resolve<IUserActivityHub>().GetRunningActivities().ToList();

            if (activities.Any())
            {
                var nonSilentActivities = activities.Where(act => !act.CanCancelInSilent).ToList();
                if (nonSilentActivities.Any())
                {
                    string activitiesDescription = string.Join(Environment.NewLine, nonSilentActivities.Select(activity => activity.CancelDescription));
                    string ask = $"{LocalizedRes.CancelActivities}?{ Environment.NewLine }{ Environment.NewLine }{ activitiesDescription }";
                    MessageBoxResult result = MessageBox.Show(ask, LocalizedRes.ProjectName, MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return Result.Fail(new UserCancelledReason());
                    }
                }

                foreach (IUserActivity activity in activities)
                {
                    activity.Cancel();
                }
            }
            
            // check if can close activities
            Result<AccountStateModel> logoutResult = serviceLocator.Resolve<ILoginController>().LogOut();

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