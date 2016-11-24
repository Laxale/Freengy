// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.Interfaces 
{
    using System;
    using System.Collections.Generic;

    using Freengy.Settings.ModuleSettings;


    /// <summary>
    /// Exposes all registered settings units
    /// </summary>
    public interface ISettingsFacade 
    {
        SettingsUnitBase GetUnit(string unitName);
        SettingsUnitBase GetUnit(Type settingsUnitType);
        TUnitType GetUnit<TUnitType>() where TUnitType : class;

        void RegisterEntityType(Type entityType);

        ICollection<Type> GetRegisteredEntityTypes();
    }
}