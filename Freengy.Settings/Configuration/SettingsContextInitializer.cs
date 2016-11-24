// Created by Laxale 24.11.2016
//
//


namespace Freengy.Settings.Configuration 
{
    using System;
    using System.Data.Entity;
    
    using Freengy.Settings.Interfaces;
    using Freengy.Settings.DefaultImpl;
    
    using Catel.IoC;

    using SQLite.CodeFirst;


    internal class SettingsContextInitializer : SqliteCreateDatabaseIfNotExists<SettingsContext> 
    {
        private static readonly ITypeFactory typeFactory = ServiceLocator.Default.ResolveType<ITypeFactory>();


        public override void InitializeDatabase(SettingsContext context)
        {
            base.InitializeDatabase(context);

            var facade = ServiceLocator.Default.ResolveType<ISettingsFacade>();
            var entityTypesToRegister = facade.GetRegisteredEntityTypes();

            foreach (Type type in entityTypesToRegister)
            {
                var newRow = typeFactory.CreateInstance(type);

                //add and remove row to create empty table
                context.Set(type).Add(newRow);
                context.Set(type).Remove(newRow);
            }

            context.GameListUnit.Add(ModuleSettings.GameListSettingsUnit.Instance);
            context.SaveChanges();
        }

        protected override void Seed(SettingsContext context)
        {
            base.Seed(context);

            var facade = ServiceLocator.Default.ResolveType<ISettingsFacade>();
            var entityTypesToRegister = facade.GetRegisteredEntityTypes();

            foreach (Type type in entityTypesToRegister)
            {
                var newRow = typeFactory.CreateInstance(type);

                //add and remove row to create empty table
                context.Set(type).Add(newRow);
                context.Set(type).Remove(newRow);
            }
        }

        public SettingsContextInitializer(DbModelBuilder modelBuilder) : base(modelBuilder) 
        {

        }

        public SettingsContextInitializer(DbModelBuilder modelBuilder, bool nullByteFileMeansNotExisting) : 
            base(modelBuilder, nullByteFileMeansNotExisting)
        {

        }
    }
}