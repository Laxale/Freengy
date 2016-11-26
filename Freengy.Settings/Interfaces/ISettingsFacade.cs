// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.Interfaces 
{
    using System;
    using System.Collections.Generic;

    using Freengy.Settings.ModuleSettings;


    /// <summary>
    /// Exposes facade to interact with registered settings units
    /// </summary>
    public interface ISettingsFacade 
    {
        SettingsUnitBase GetUnit(string unitName);
        SettingsUnitBase GetUnit(Type settingsUnitType);
        TUnitType GetUnit<TUnitType>() where TUnitType : SettingsUnitBase;

        //ISettingsFacade RegisterUnit(SettingsUnitBase settningsUnit);
        
        /// <summary>
        /// Register settings unit by type
        /// </summary>
        /// <typeparam name="TEntityType"><see cref="SettingsUnitBase"/> child type</typeparam>
        /// <returns>Self. Just for lulz (well, fluent chaining)</returns>
        ISettingsFacade RegisterUnitType<TEntityType>() where TEntityType : SettingsUnitBase, new();

        //ICollection<SettingsUnitBase> GetRegisteredEntityTypes();
    }
}