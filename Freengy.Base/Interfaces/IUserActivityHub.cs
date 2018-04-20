// Created by Laxale 20.10.2016
//
//

using System.Collections.Generic;


namespace Freengy.Base.Interfaces 
{
    /// <summary>
    /// Interface of an all user activities collector.
    /// </summary>
    public interface IUserActivityHub 
    {
        /// <summary>
        /// Get running user activities.
        /// </summary>
        /// <returns>Collection of running activities.</returns>
        IEnumerable<IUserActivity> GetRunningActivities();
    }
}