// Created by Laxale 26.04.2018
//
//

using System;
using System.Collections.Generic;
using Freengy.Common.Interfaces;
using Freengy.Common.Models;

namespace Freengy.Common.Helpers.Statistics 
{
    /// <summary>
    /// <see cref="IStatisticsCollector"/> implementer.
    /// </summary>
    public sealed class StatisticsCollector : IStatisticsCollector 
    {
        private static readonly object Locker = new object();

        private static StatisticsCollector instance;

        private readonly List<StatisticsUnit> units = new List<StatisticsUnit>();

        private Action<IEnumerable<StatisticsUnit>> flushImpl;


        private StatisticsCollector() { }


        /// <summary>
        /// Единственный инстанс <see cref="StatisticsCollector"/>.
        /// </summary>
        public static IStatisticsCollector Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new StatisticsCollector());
                }
            }
        }


        /// <summary>
        /// Write down all currently collected statistics using a provided Flush action.
        /// </summary>
        public void FlushStatistics() 
        {
            flushImpl?.Invoke(units);
        }

        public void AddUnit(StatisticsUnit unit) 
        {
            units.Add(unit);
        }

        public void Configure(Action<IEnumerable<StatisticsUnit>> flushAction) 
        {
            flushImpl = flushAction ?? throw new ArgumentNullException(nameof(flushAction));
        }
    }
}   