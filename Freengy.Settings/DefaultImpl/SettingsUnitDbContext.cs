// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.DefaultImpl 
{
    using System;
    using System.Linq;
    using System.Data.Entity;

    using Freengy.Settings.Configuration;


    [DbConfigurationType(typeof(SqliteConfiguration))]
    internal class SettingsUnitDbContext<TUnitType> : DbContext where TUnitType : class
    {
        public DbSet<TUnitType> SettingsUnit { get; set; }
    }
}