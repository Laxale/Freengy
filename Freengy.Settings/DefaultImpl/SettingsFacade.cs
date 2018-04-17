using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freengy.Settings.Interfaces;
using Freengy.Settings.ModuleSettings;

namespace Freengy.Settings.DefaultImpl
{
    public class SettingsFacade : ISettingsFacade 
    {
        private static readonly object Locker = new object();

        private static SettingsFacade instance;


        private SettingsFacade() 
        {

        }


        /// <summary>
        /// Единственный инстанс <see cref="SettingsFacade"/>.
        /// </summary>
        public static SettingsFacade Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new SettingsFacade());
                }
            }
        }


        public SettingsUnitBase GetUnit(string unitName) 
        {
            throw new NotImplementedException();
        }

        public SettingsUnitBase GetUnit(Type settingsUnitType) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get settings unit (must be registered in <see cref="ISettingsFacade"/> implementer before getting)
        /// </summary>
        /// <typeparam name="TUnitType"><see cref="SettingsUnitBase"/> child</typeparam>
        /// <returns>Registered unit or null if not registered</returns>
        public TUnitType GetUnit<TUnitType>() where TUnitType : SettingsUnitBase, new() 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get settings unit or register it if not yet registered
        /// </summary>
        /// <typeparam name="TUnitType"></typeparam>
        /// <returns>Already or just yet registered unit</returns>
        public TUnitType GetOrCreateUnit<TUnitType>() where TUnitType : SettingsUnitBase, new() 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Flush instance state to a database
        /// </summary>
        /// <typeparam name="TUnitType"></typeparam>
        /// <param name="unitInstance"></param>
        /// <returns>Self - for fluent purposes</returns>
        public ISettingsFacade UpdateUnit<TUnitType>(TUnitType unitInstance) where TUnitType : SettingsUnitBase, new() 
        {
            throw new NotImplementedException();
        }
    }
}