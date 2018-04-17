// Created by Laxale 23.11.2016
//
//

using System;

using Freengy.Settings.ModuleSettings;


namespace Freengy.Settings.Interfaces 
{
    /// <summary>
    /// A repository interface to store and interact with registered settings units.
    /// </summary>
    public interface ISettingsRepository 
    {
        SettingsUnit GetUnit(string unitName);

        SettingsUnit GetUnit(Type settingsUnitType);
        
        /// <summary>
        /// Get settings unit (must be registered in <see cref="ISettingsRepository"/> implementer before getting)
        /// </summary>
        /// <typeparam name="TUnitType"><see cref="SettingsUnit"/> child</typeparam>
        /// <returns>Registered unit or null if not registered</returns>
        TUnitType GetUnit<TUnitType>() where TUnitType : SettingsUnit, new();

        /// <summary>
        /// Get settings unit or register it if not yet registered
        /// </summary>
        /// <typeparam name="TUnitType"></typeparam>
        /// <returns>Already or just yet registered unit</returns>
        TUnitType GetOrCreateUnit<TUnitType>() where TUnitType : SettingsUnit, new();

        /// <summary>
        /// Flush instance state to a database
        /// </summary>
        /// <typeparam name="TUnitType"></typeparam>
        /// <param name="unitInstance"></param>
        /// <returns>Self - for fluent purposes</returns>
        ISettingsRepository UpdateUnit<TUnitType>(TUnitType unitInstance) where TUnitType : SettingsUnit, new();
    }
}