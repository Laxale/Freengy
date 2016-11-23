// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.DefaultImpl 
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Data.SQLite;
    using System.Collections.Generic;

    using Freengy.Settings.Interfaces;

    
    internal class SettingsFacade : ISettingsFacade
    {
        private const string DbFileName = "Settings.sqlite";
        private readonly IDictionary<string, IBaseSettingsUnit> registeredUnits = new Dictionary<string, IBaseSettingsUnit>();


        #region Singleton

        private static ISettingsFacade instance;

        private SettingsFacade()
        {
            var dbFolderFullPath = Path.Combine(Environment.CurrentDirectory, "db");
            var dbFileFullPath = Path.Combine(dbFolderFullPath, DbFileName);

            if (!Directory.Exists(dbFolderFullPath))
            {
                Directory.CreateDirectory(dbFolderFullPath);
            }

            if (!File.Exists(dbFileFullPath))
            {
                SQLiteConnection.CreateFile(dbFileFullPath);
            }
        }

        public static ISettingsFacade Instance => SettingsFacade.instance ?? (SettingsFacade.instance = new SettingsFacade());

        #endregion Singleton


        public TUnitType GetUnit<TUnitType>() where TUnitType : class 
        {
            using (var unitContext = new SettingsUnitDbContext<TUnitType>())
            {
                var unit = unitContext.SettingsUnit.First();

                return unit;
            }
        }

        public IBaseSettingsUnit GetUnit(string unitName) 
        {
            throw new NotImplementedException();
        }

        public IBaseSettingsUnit GetUnit(Type settingsUnitType) 
        {
            throw new NotImplementedException();
        }

        public ISettingsFacade RegisterUnit(IBaseSettingsUnit settingsUnit) 
        {
            if (settingsUnit == null) throw new ArgumentNullException(nameof(settingsUnit));



            return this;
        }
    }
}