// Created by Laxale 24.11.2016
//
//


namespace Freengy.Settings.ModuleSettings 
{
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// Just a base class for settings units. Nothing special. Just OOP purposes
    /// </summary>
    public abstract class SettingsUnitBase  
    {
        protected SettingsUnitBase() 
        {
            
        }


        /// <summary>
        /// Represents primary key in a table
        /// </summary>
        protected internal virtual long Id { get; set; }

        public virtual string Name { get; set; }

        /// <summary>
        /// This exposes mappable property names and its attributes to ORM without reflection
        /// </summary>
        protected internal abstract IDictionary<string, ICollection<Attribute>> ColumnsProperties { get; }
    }
}