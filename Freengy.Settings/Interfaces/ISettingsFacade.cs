// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.Interfaces 
{
    using System;
    
    using Freengy.Settings.ModuleSettings;


    /// <summary>
    /// Exposes facade to interact with registered settings units
    /// </summary>
    public interface ISettingsFacade 
    {
        SettingsUnitBase GetUnit(string unitName);
        SettingsUnitBase GetUnit(Type settingsUnitType);
        /// <summary>
        /// Get settings unit (must be registered in <see cref="ISettingsFacade"/> implementer before getting)
        /// </summary>
        /// <typeparam name="TUnitType"><see cref="SettingsUnitBase"/> child</typeparam>
        /// <returns>Registered unit or null if not registered</returns>
        TUnitType GetUnit<TUnitType>() where TUnitType : SettingsUnitBase, new();

        /// <summary>
        /// Get settings unit or register it if not yet registered
        /// </summary>
        /// <typeparam name="TUnitType"></typeparam>
        /// <returns>Already or just yet registered unit</returns>
        TUnitType GetOrCreateUnit<TUnitType>() where TUnitType : SettingsUnitBase, new();

        /// <summary>
        /// Flush instance state to a database
        /// </summary>
        /// <typeparam name="TUnitType"></typeparam>
        /// <param name="unitInstance"></param>
        /// <returns>Self - for fluent purposes</returns>
        ISettingsFacade UpdateUnit<TUnitType>(TUnitType unitInstance) where TUnitType : SettingsUnitBase, new();
    }
}