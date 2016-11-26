// Created by Laxale 24.11.2016
//
//


namespace Freengy.Settings.ModuleSettings 
{
    using System;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    using Freengy.Settings.Interfaces;
    

    /// <summary>
    /// Just a base class for settings units. Nothing special. Just OOP purposes
    /// </summary>
    public abstract class SettingsUnitBase : IObjectWithLongId 
    {
        protected SettingsUnitBase() 
        {
            
        }


        /// <summary>
        /// Represents primary key in a table
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// This exposes mappable property names and its attributes to ORM without reflection
        /// </summary>
        public abstract IDictionary<string, ICollection<Attribute>> ColumnsProperties { get; }
    }
}