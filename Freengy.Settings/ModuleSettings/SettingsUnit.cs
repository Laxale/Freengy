// Created by Laxale 24.11.2016
//
//

using Freengy.Base.Interfaces;
using Freengy.Database;
using Freengy.Database.Object;


namespace Freengy.Settings.ModuleSettings 
{
    /// <summary>
    /// Just a base class for settings units. Nothing special. Just OOP purposes
    /// </summary>
    public abstract class SettingsUnit : SimpleDbObject, INamedObject  
    {
        protected SettingsUnit() 
        {

        }


        /// <summary>
        /// Returns the name of a settings unit.
        /// </summary>
        public abstract string Name { get; }
    }
}