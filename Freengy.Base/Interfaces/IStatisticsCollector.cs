// Created by Laxale 26.04.2018
//
//

using System;

using Freengy.Base.Models;


namespace Freengy.Base.Interfaces 
{
    using System.Collections.Generic;


    /// <summary>
    /// Interface for a statistics collector.
    /// </summary>
    public interface IStatisticsCollector 
    {
        /// <summary>
        /// Write down
        /// </summary>
        void FlushStatistics();

        /// <summary>
        /// Add in-memory statistics unit.
        /// </summary>
        /// <param name="unit">Unit of work.</param>
        void AddUnit(StatisticsUnit unit);

        /// <summary>
        /// Configure a flush (writing to storage) method implementation.
        /// </summary>
        /// <param name="flushAction">Method that implements a statistics flush logic.</param>
        void Configure(Action<IEnumerable<StatisticsUnit>> flushAction);
    }
}