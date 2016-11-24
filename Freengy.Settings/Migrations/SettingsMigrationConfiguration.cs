// Created by Laxale 24.11.2016
//
//


namespace Freengy.Settings.Migrations 
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;

    using Freengy.Settings.Interfaces;
    using Freengy.Settings.DefaultImpl;

    using Catel.IoC;
    

    internal sealed class SettingsMigrationConfiguration : DbMigrationsConfiguration<SettingsContext> 
    {
        private static readonly ITypeFactory typeFactory = ServiceLocator.Default.ResolveType<ITypeFactory>();


        public SettingsMigrationConfiguration() 
        {
            base.AutomaticMigrationsEnabled = true;
            base.ContextKey = typeof(SettingsContext).FullName;
        }

        protected override void Seed(SettingsContext context) 
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //                context.People.AddOrUpdate(
            //                  p => p.FullName,
            //                  new Person { FullName = "Andrew Peters" },
            //                  new Person { FullName = "Brice Lambson" },
            //                  new Person { FullName = "Rowan Miller" }
            //                );
        }
    }
}