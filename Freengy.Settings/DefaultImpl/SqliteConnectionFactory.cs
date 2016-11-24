// Created by Laxale 24.11.2016
//
//


namespace Freengy.Settings.DefaultImpl
{
    using System.Data.Common;
    using System.Data.SQLite;
    using System.Data.Entity.Infrastructure;


    public class SqliteConnectionFactory : IDbConnectionFactory 
    {
        public DbConnection CreateConnection(string connectionString) 
        {
            return new SQLiteConnection(connectionString);
        }
    }
}