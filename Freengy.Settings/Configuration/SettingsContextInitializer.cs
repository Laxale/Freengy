// Created by Laxale 24.11.2016
//
//


namespace Freengy.Settings.Configuration 
{
    using System;
    using System.Data.Entity;

    using Freengy.Settings.Extensions;
    using Freengy.Settings.Interfaces;
    using Freengy.Settings.DefaultImpl;
    
    using Catel.IoC;

    using SQLite.CodeFirst;


    internal class SettingsContextInitializer : SqliteCreateDatabaseIfNotExists<SettingsContext> 
    {
        public SettingsContextInitializer(DbModelBuilder modelBuilder) : base(modelBuilder) 
        {

        }

        public SettingsContextInitializer(DbModelBuilder modelBuilder, bool nullByteFileMeansNotExisting) : 
            base(modelBuilder, nullByteFileMeansNotExisting)
        {

        }


        protected override void Seed(SettingsContext context) 
        {
            base.Seed(context);

            context.PopulateWithRegisteredTypes();
        }

        public override void InitializeDatabase(SettingsContext context) 
        {
            base.InitializeDatabase(context);

            context.PopulateWithRegisteredTypes();
        }
    }
}