// Created by Laxale 26.04.2018
//
//

using System;
using System.Collections.Generic;
using Freengy.Common.Models;

namespace Freengy.Common.Interfaces 
{
    /// <summary>
    /// Interface for a statistics collector.
    /// </summary>
    public interface IStatisticsCollector 
    {
        /// <summary>
        /// Write down all currently collected statistics using a provided Flush action.
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