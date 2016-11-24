// Created by Laxale 23.11.2016
//
//


namespace Freengy.Settings.Configuration 
{
    using System.Data.SQLite;
    using System.Data.Entity;
    using System.Data.SQLite.EF6;
    using System.Data.Entity.Core.Common;


    public class SqliteConfiguration : DbConfiguration 
    {
        public SqliteConfiguration()
        {
            base.SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            base.SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);

            base.SetProviderServices("System.Data.SQLite", (DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(DbProviderServices)));
        }
    }
}