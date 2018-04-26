// Created by Laxale 26.04.2018
//
//

using System;
using System.Collections.Generic;

using Freengy.Base.Models;
using Freengy.Base.Interfaces;


namespace Freengy.Base.Helpers 
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
        public static StatisticsCollector Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new StatisticsCollector());
                }
            }
        }


        /// <inheritdoc />
        public void FlushStatistics() 
        {
            flushImpl(units);
        }

        /// <inheritdoc />
        public void AddUnit(StatisticsUnit unit) 
        {
            units.Add(unit);
        }

        /// <inheritdoc />
        public void Configure(Action<IEnumerable<StatisticsUnit>> flushAction) 
        {
            flushImpl = flushAction ?? throw new ArgumentNullException(nameof(flushAction));
        }
    }
}   