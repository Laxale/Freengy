// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.DefaultImpl 
{
    using System;
    using System.Linq;
    using System.Data.SQLite;
    using System.Data.Entity;

    using Freengy.Settings.Constants;
    using Freengy.Settings.Interfaces;
    using Freengy.Settings.Configuration;
    using Freengy.Settings.ModuleSettings;

    using Catel.IoC;
    

    /// <summary>
    /// Database representaion. Contains table representations
    /// </summary>
    public class SettingsContext : DbContext 
    {
        public SettingsContext() : 
            base(new SqliteConnectionFactory().CreateConnection(SettingsConstants.ConnectionString), true)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            var sqliteConnectionInitializer = new SettingsContextInitializer(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);

            var facade = ServiceLocator.Default.ResolveType<ISettingsFacade>();
            var entityTypesToRegister = facade.GetRegisteredEntityTypes();

            foreach (Type type in entityTypesToRegister)
            {
                modelBuilder.RegisterEntityType(type);
            }
        }
    }
}