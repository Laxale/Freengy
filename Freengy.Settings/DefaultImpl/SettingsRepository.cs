// Created by Laxale 17.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Linq;
using Freengy.Base.Helpers;
using Freengy.Database;
using Freengy.Database.Context;
using Freengy.Settings.Interfaces;
using Freengy.Settings.ModuleSettings;

using NLog;


namespace Freengy.Settings.DefaultImpl 
{
    /// <summary>
    /// <see cref="ISettingsRepository"/> implementer.
    /// </summary>
    public class SettingsRepository : ISettingsRepository 
    {
        private static readonly object Locker = new object();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static SettingsRepository instance;


        private SettingsRepository() 
        {

        }


        /// <summary>
        /// Единственный инстанс <see cref="SettingsRepository"/>.
        /// </summary>
        public static SettingsRepository Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new SettingsRepository());
                }
            }
        }


        public SettingsUnit GetUnit(string unitName) 
        {
            throw new NotImplementedException();
        }

        public SettingsUnit GetUnit(Type settingsUnitType) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get settings unit (must be registered in <see cref="ISettingsRepository"/> implementer before getting)
        /// </summary>
        /// <typeparam name="TUnitType"><see cref="SettingsUnit"/> child</typeparam>
        /// <returns>Registered unit or null if not registered</returns>
        public TUnitType GetUnit<TUnitType>() where TUnitType : SettingsUnit, new() 
        {
            try
            {
                using (var context = new SimpleDbContext<TUnitType>())
                {
                    var dbObjects = context.Objects;

                    return dbObjects.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Не удалось получить { typeof(TUnitType).Name } из базы");
                return null;
            }
        }

        /// <summary>
        /// Get settings unit or register it if not yet registered
        /// </summary>
        /// <typeparam name="TUnitType"></typeparam>
        /// <returns>Already or just yet registered unit</returns>
        public TUnitType GetOrCreateUnit<TUnitType>() where TUnitType : SettingsUnit, new() 
        {
            try
            {
                var storage = new DbObjectStorage();
                Result<TUnitType> result = storage.GetSingleSimple<TUnitType>();

                if (result.Value == null)
                {
                    var newObject = new TUnitType();
                    storage.SaveSingleSimple(newObject);

                    return newObject;
                }

                return result.Value;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Не удалось получить { typeof(TUnitType).Name } из базы");
                return null;
            }
        }

        /// <summary>
        /// Flush instance state to a database
        /// </summary>
        /// <typeparam name="TUnitType"></typeparam>
        /// <param name="unitInstance"></param>
        /// <returns>Self - for fluent purposes</returns>
        public ISettingsRepository UpdateUnit<TUnitType>(TUnitType unitInstance) where TUnitType : SettingsUnit, new() 
        {
            throw new NotImplementedException();
        }
    }
}