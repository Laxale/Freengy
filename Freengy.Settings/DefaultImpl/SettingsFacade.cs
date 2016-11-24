// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.DefaultImpl
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Data.Entity;
    using System.Data.SQLite;
    using System.Collections.Generic;
    
    using Freengy.Settings.DefaultImpl;
    using Freengy.Settings.Configuration;

    using Freengy.Settings.Constants;
    using Freengy.Settings.Interfaces;
    using Freengy.Settings.ModuleSettings;


    internal class SettingsFacade : ISettingsFacade
    {
        private readonly IDictionary<string, Type> registeredEntityTypes = new Dictionary<string, Type>();

        private readonly IDictionary<string, SettingsUnitBase> registeredUnits =
            new Dictionary<string, SettingsUnitBase>();


        #region Singleton

        private static ISettingsFacade instance;

        private SettingsFacade()
        {
            //this.CreateEmptyDatabase();
        }

        public static ISettingsFacade Instance
            => SettingsFacade.instance ?? (SettingsFacade.instance = new SettingsFacade());

        #endregion Singleton


        public TUnitType GetUnit<TUnitType>() where TUnitType : class
        {
            using (var unitContext = new SettingsContext())
            {
                var unit = unitContext.Set<TUnitType>().FirstOrDefault();

                return unit;
            }
        }

        public void RegisterEntityType(Type entityType) 
        {
            if (entityType == null) throw new ArgumentNullException(nameof(entityType));

            try
            {
                this.registeredEntityTypes.Add(entityType.FullName, entityType);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public ICollection<Type> GetRegisteredEntityTypes() 
        {
            return this.registeredEntityTypes.Values;
        }

        public SettingsUnitBase GetUnit(string unitName) 
        {
            throw new NotImplementedException();
        }

        public SettingsUnitBase GetUnit(Type settingsUnitType) 
        {
            throw new NotImplementedException();
        }

        public ISettingsFacade RegisterUnit<TUnitType>(TUnitType settingsUnit) where TUnitType : class
        {
            if (settingsUnit == null) throw new ArgumentNullException(nameof(settingsUnit));

            using (var unitContext = new SettingsContext()) 
            {
                var unit = unitContext.Set<TUnitType>();
                unit.Add(settingsUnit);
                unitContext.SaveChanges();
            }

            return this;
        }


        private void CreateEmptyDatabase() 
        {
            var dbFolderFullPath = Path.Combine(Environment.CurrentDirectory, Container.DatabaseFolderName);
            var dbFileFullPath = Path.Combine(dbFolderFullPath, Container.SettingsDbFileName);

            if (!Directory.Exists(dbFolderFullPath))
            {
                Directory.CreateDirectory(dbFolderFullPath);
            }

            if (!File.Exists(dbFileFullPath))
            {
                SQLiteConnection.CreateFile(dbFileFullPath);
            }
        }
    }
}