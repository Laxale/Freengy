// Created by Laxale 20.10.2016
//
//

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Freengy.Base.Messages;
using Freengy.Base.Interfaces;
using Freengy.Base.Messages.Base;
using Freengy.Base.Messages.Login;


namespace Freengy.Base.DefaultImpl 
{
    /// <summary>
    /// <see cref="IUserActivityHub"/> implementer.
    /// </summary>
    internal class UserActivityHub : IUserActivityHub 
    {
        private static readonly object Locker = new object();

        private static UserActivityHub instance;

        private readonly List<IRefreshable> refreshables = new List<IRefreshable>();
        private readonly List<IUserActivity> activities = new List<IUserActivity>();

        private bool wasLoggedIn;


        private UserActivityHub() 
        {
            this.Subscribe<MessageBase>(OnLoginOccured);
            this.Subscribe<MessageRefreshRequired>(OnRefreshableCreated);
            this.Subscribe<MessageActivityChanged>(OnActivityChanged);
        }


        /// <summary>
        /// Единственный инстанс <see cref="UserActivityHub"/>.
        /// </summary>
        public static IUserActivityHub Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new UserActivityHub());
                }
            }
        }


        /// <summary>
        /// Get running user activities.
        /// </summary>
        /// <returns>Collection of running activities.</returns>
        public IEnumerable<IUserActivity> GetRunningActivities() 
        {
            return activities;
        }


        private void OnLoginOccured(MessageBase messageBase) 
        {
            if (wasLoggedIn && messageBase is MessageLogInSuccess)
            {
                Task.Run(() =>
                {
                    foreach (IRefreshable refreshable in refreshables)
                    {
                        refreshable.Refresh();
                    }
                });

                return;
            }

            if (messageBase is MessageLogInSuccess)
            {
                wasLoggedIn = true;
            }
        }

        private void OnRefreshableCreated(MessageRefreshRequired message) 
        {
            lock (Locker)
            {
                if (refreshables.Contains(message.Refreshable)) return;

                refreshables.Add(message.Refreshable);
            }
        }

        private void OnActivityChanged(MessageActivityChanged message) 
        {
            IUserActivity changedActivity = message.Activity;

            if (activities.Contains(changedActivity))
            {
                if (!message.IsStarted)
                {
                    activities.Remove(changedActivity);
                }
                else
                {
                    throw new InvalidOperationException($"Double activity '{ changedActivity.GetType().Name }' start!");
                }
            }
            else
            {
                if (message.IsStarted)
                {
                    activities.Add(changedActivity);
                }
                else
                {
                    throw new InvalidOperationException($"Activity '{ changedActivity.GetType().Name }' is finished, but was not registered in hub!");
                }
            }
        }
    }
}