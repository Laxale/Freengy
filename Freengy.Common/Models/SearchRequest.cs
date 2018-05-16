// Created by Laxale 18.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;

using Freengy.Common.Enums;


namespace Freengy.Common.Models 
{
    /// <summary>
    /// Search request model.
    /// </summary>
    public class SearchRequest 
    {
        /// <summary>
        /// Identifier of a request sender.
        /// </summary>
        public Guid SenderId { get; set; }

        /// <summary>
        /// Search entity type.
        /// </summary>
        public SearchEntity Entity { get; set; }

        /// <summary>
        /// Name filter to search entities by.
        /// </summary>
        public string SearchFilter { get; set; }
    }
}