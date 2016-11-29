// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.DefaultImpl 
{
    using System;
    using System.Collections.Generic;

    using Freengy.Settings.Configuration;

    using Freengy.Settings.Interfaces;
    using Freengy.Settings.ModuleSettings;

    using NHibernate.Mapping.ByCode;
    

    internal class SettingsFacade : ISettingsFacade 
    {
        #region vars

        private static readonly object Locker = new object();

        private readonly IDictionary<string, IConformistHoldersProvider> registeredMappngs =
            new Dictionary<string, IConformistHoldersProvider>();
        
        #endregion vars


        #region Singleton

        private static SettingsFacade instance;

        private SettingsFacade()
        {
            this.FillMappings();
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
            if (string.IsNullOrWhiteSpace(unitName)) throw new ArgumentNullException(nameof(unitName));

            SettingsUnitBase unit;

            bool unitCreated = this.IsUnitCreated(unitName, out unit);

            if (!unitCreated)
            {
                // create
            }

            throw new NotImplementedException();
        }

        public SettingsUnitBase GetUnit(Type settingsUnitType) 
        {
            throw new NotImplementedException();
        }

        public TUnitType GetUnit<TUnitType>() where TUnitType : SettingsUnitBase, new() 
        {
            TUnitType unit;

            bool unitCreated = this.IsUnitCreated(out unit);
            
            return unit;
        }

        public TUnitType GetOrCreateUnit<TUnitType>() where TUnitType : SettingsUnitBase, new() 
        {
            TUnitType registeredUnit = this.GetUnit<TUnitType>();

            if (registeredUnit == null)
            {
                this.AddUnit(out registeredUnit);
            }

            return registeredUnit;
        }

        public ISettingsFacade UpdateUnit<TUnitType>(TUnitType unitInstance) where TUnitType : SettingsUnitBase, new() 
        {
            using (var session = Initializer.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(unitInstance);
                    transaction.Commit();
                }

                return this;
            }
        }


        internal ICollection<IConformistHoldersProvider> GetRegisteredMappings() 
        {
            lock (Locker)
            {
                return this.registeredMappngs.Values;
            }
        }


        private void FillMappings() 
        {
            var gameListMapping = new GenericMapping<GameListSettingsUnit>();
            var friendListMapping = new GenericMapping<FriendListSettingsUnit>();

            registeredMappngs.Add(typeof(GameListSettingsUnit).FullName, gameListMapping);
            registeredMappngs.Add(typeof(FriendListSettingsUnit).FullName, friendListMapping);
        }

        private void AddUnit<TUnitType>(out TUnitType addedUnit) where TUnitType : SettingsUnitBase, new() 
        {
            using (var session = Initializer.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    addedUnit = new TUnitType();
                    session.SaveOrUpdate(addedUnit);
                    transaction.Commit();
                }
            }
        }

        private bool IsUnitCreated<TUnitType>(out TUnitType unit) where TUnitType : SettingsUnitBase, new() 
        {
            unit = null;

            using (var session = Initializer.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        unit = session.Get<TUnitType>(1);
                    }
                    catch (Exception)
                    {
                        // log this
                        // may be unit is just not registered in hibernate configuration by mapping
                    }
                    
                    return unit!= null;
                }
            }
        }

        private bool IsUnitCreated(string unitName, out SettingsUnitBase unit) 
        {
            using (var session = Initializer.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    object unitEntry = session.Get(unitName, 1);

                    unit = unitEntry as SettingsUnitBase;

                    return unitEntry != null;
                }
            }
        }
    }
}