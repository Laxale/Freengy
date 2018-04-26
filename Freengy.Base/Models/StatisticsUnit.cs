// Created by Laxale 26.04.2018
//
//

using System;


namespace Freengy.Base.Models 
{
    public class StatisticsUnit 
    {
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
    }
}