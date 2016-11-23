// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.Interfaces 
{
    using System;


    /// <summary>
    /// Exposes all registered settings units
    /// </summary>
    public interface ISettingsFacade 
    {
        IBaseSettingsUnit GetUnit(string unitName);
        IBaseSettingsUnit GetUnit(Type settingsUnitType);
        TUnitType GetUnit<TUnitType>() where TUnitType : class;

        /// <summary>
        /// Return value can be used to chain calls: facade.RegisterUnit().RegisterUnit()
        /// </summary>
        /// <param name="settingsUnit">Settings unit implementation to register</param>
        /// <returns>Meant to be facade itself</returns>
        ISettingsFacade RegisterUnit(IBaseSettingsUnit settingsUnit);
    }
}