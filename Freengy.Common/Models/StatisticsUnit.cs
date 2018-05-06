// Created by Laxale 26.04.2018
//
//

using System;
using System.Linq;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Unit of a statistics.
    /// </summary>
    public class StatisticsUnit 
    {
        /// <summary>
        /// Конструирует новый <see cref="StatisticsUnit"/> с заданным названием и временем старта работы.
        /// </summary>
        /// <param name="unitName"></param>
        /// <param name="started"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public StatisticsUnit(string unitName, DateTime started) 
        {
            if (string.IsNullOrWhiteSpace(unitName)) throw new ArgumentNullException(nameof(unitName));

            UnitName = unitName;
            Started = started;
        }


        /// <summary>
        /// Statistics unit name.
        /// </summary>
        public string UnitName { get; }

        /// <summary>
        /// Statistics unit started timestamp.
        /// </summary>
        public DateTime Started { get; }

        /// <summary>
        /// Statistics unit finished timestamp.
        /// </summary>
        public DateTime Finished { get; set; }


        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() 
        {
            string started = Started.ToString("O").Split('T').Last();
            string finished = Finished.ToString("O").Split('T').Last();

            return $"Work {UnitName} started '{ started }' finished '{ finished }'";
        }
    }
}