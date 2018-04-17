// Created by Laxale 17.04.2018
//
//

using System.Data.Entity;
using System.Data.SQLite;
using System.Data.SQLite.EF6;


namespace Freengy.Database 
{
    /// <summary>
    /// Конфигурация EF для работы с SQLite.
    /// </summary>
    public class SQLiteConfiguration : DbConfiguration 
    {
        /// <summary>
        /// Конструктор <see cref="SQLiteConfiguration"/>.
        /// </summary>
        public SQLiteConfiguration()
        {
            SetProviderFactory("System.Data.SQLite", SQLiteFactory.Instance);
            SetProviderFactory("System.Data.SQLite.EF6", SQLiteProviderFactory.Instance);

            SetProviderServices("System.Data.SQLite",
                (System.Data.Entity.Core.Common.DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(System.Data.Entity.Core.Common.DbProviderServices)));

            SetProviderServices("System.Data.SQLite.EF6",
                (System.Data.Entity.Core.Common.DbProviderServices)SQLiteProviderFactory.Instance.GetService(typeof(System.Data.Entity.Core.Common.DbProviderServices)));
        }
    }
}