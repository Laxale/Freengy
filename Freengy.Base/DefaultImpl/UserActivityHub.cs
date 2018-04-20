// Created by Laxale 20.10.2016
//
//

using System;
using System.Collections.Generic;

using Freengy.Base.Messages;
using Freengy.Base.Interfaces;

using Catel.Messaging;


namespace Freengy.Base.DefaultImpl 
{
    /// <summary>
    /// <see cref="IUserActivityHub"/> implementer.
    /// </summary>
    internal class UserActivityHub : IUserActivityHub 
    {
        private static readonly object Locker = new object();

        private static UserActivityHub instance;

        private readonly List<IUserActivity> activities = new List<IUserActivity>();


        private UserActivityHub() 
        {
            MessageMediator.Default.Register<MessageActivityChanged>(this, OnActivityChanged);
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


        /// <inheritdoc />
        /// <summary>
        /// Get running user activities.
        /// </summary>
        /// <returns>Collection of running activities.</returns>
        public IEnumerable<IUserActivity> GetRunningActivities()
        {
            return activities;
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