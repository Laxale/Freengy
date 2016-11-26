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

    using Freengy.Settings.Configuration;

    using Freengy.Settings.Constants;
    using Freengy.Settings.Interfaces;
    using Freengy.Settings.ModuleSettings;

    using NHibernate.Mapping.ByCode;

    
    internal class SettingsFacade : ISettingsFacade 
    {
        #region vars

        private static readonly object Locker = new object();

        private readonly IDictionary<string, Type> registeredEntityTypes = new Dictionary<string, Type>();

        private readonly IDictionary<string, IConformistHoldersProvider> registeredMappngs =
            new Dictionary<string, IConformistHoldersProvider>();

        private readonly IDictionary<string, SettingsUnitBase> registeredUnits =
            new Dictionary<string, SettingsUnitBase>();

        #endregion vars


        #region Singleton

        private static SettingsFacade instance;
        
        private SettingsFacade() 
        {
            //this.CreateEmptyDatabase();

        }

        internal SettingsFacade(string connectionString) 
        {
            
        }

        public static ISettingsFacade Instance 
        {
            get
            {
                lock (Locker)
                {
                    return SettingsFacade.instance ?? (SettingsFacade.instance = new SettingsFacade());
                }
            }
        }

        internal static SettingsFacade FullInstance 
        {
            get
            {
                lock (Locker)
                {
                    return SettingsFacade.instance ?? (SettingsFacade.instance = new SettingsFacade());
                }
            }
        }
        
        #endregion Singleton


        public SettingsUnitBase GetUnit(string unitName)
        {
            throw new NotImplementedException();
        }

        public SettingsUnitBase GetUnit(Type settingsUnitType)
        {
            throw new NotImplementedException();
        }

        public TUnitType GetUnit<TUnitType>() where TUnitType : SettingsUnitBase
        {
            throw new NotImplementedException();
        }

        public ISettingsFacade RegisterUnitType<TEntityType>() where TEntityType : SettingsUnitBase, new()
        {
            var mapping = new GenericMapping<TEntityType>();

            try
            {
                lock (Locker)
                {
                    this.registeredMappngs.Add(mapping.GetType().FullName, mapping);
                }
            }
            catch (Exception)
            {
                // ignore if already registered
                throw;
            }

            

            throw new NotImplementedException();
        }


        internal ICollection<IConformistHoldersProvider> GetRegisteredMappings() 
        {
            lock (Locker)
            {
                return this.registeredMappngs.Values;
            }
        }


        private void CreateEmptyDatabase() 
        {
            var dbFolderFullPath = Path.Combine(Environment.CurrentDirectory, SettingsConstants.DatabaseFolderName);
            var dbFileFullPath = Path.Combine(dbFolderFullPath, SettingsConstants.SettingsDbFileName);

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