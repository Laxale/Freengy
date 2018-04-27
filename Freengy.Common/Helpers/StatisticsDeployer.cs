// Created by Laxale 26.04.2018
//
//

using System;
using Freengy.Common.Helpers.Statistics;
using Freengy.Common.Models;

namespace Freengy.Common.Helpers 
{
    /// <summary>
    /// Allows to send code execution timestamps to <see cref="StatisticsCollector"/>.
    /// </summary>
    public class StatisticsDeployer : IDisposable 
    {
        private readonly StatisticsUnit unit;


        /// <summary>
        /// Constructs new <see cref="StatisticsDeployer"/> with a given unit of work name.
        /// </summary>
        /// <param name="unitName">Some name for a unit of work or loading module.</param>
        public StatisticsDeployer(string unitName) 
        {
            unit = new StatisticsUnit(unitName, DateTime.Now);
            StatisticsCollector.Instance.AddUnit(unit);
        }


        /// <inheritdoc />
        public void Dispose() 
        {
            unit.Finished = DateTime.Now;
        }
    }
}