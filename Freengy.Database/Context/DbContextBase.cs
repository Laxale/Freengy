// Created by Laxale 17.04.2018
//
//

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.Validation;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Freengy.Common.Database;
using Freengy.Common.Helpers;
using Freengy.Database.Object;

using NLog;

using SQLite.CodeFirst;


namespace Freengy.Database.Context 
{
    /// <summary>
    /// Базовый класс для EF-контекстов.
    /// </summary>
    /// <typeparam name="T">Тип хранимы контекстом объектов - наследник <see cref="DbObject"/>.</typeparam>
    [DbConfigurationType(typeof(SQLiteConfiguration))]
    public abstract class DbContextBase<T> : DbContext where T : DbObject, new() 
    {
        private static readonly string dbFilePath;
        
        protected static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly int commandTimeoutInSeconds = 5;


        static DbContextBase() 
        {
            dbFilePath = Initializer.DbFilePath;

            //DbInterception.Add(new EfCommandInterceptor());
        }

        /// <summary>
        /// Конструирует контекст базы данных со строкой подключения по умолчанию.
        /// </summary>
        protected DbContextBase() : base(CreateConnection(), true)
        {
            // убирает появление ошибки "требуется поле ххх" - EF не всегда загружает все свойства проксей
            //Configuration.ValidateOnSaveEnabled = false;
            //Configuration.LazyLoadingEnabled = false;

            //Configuration.ProxyCreationEnabled = false;
        }


        /// <summary>
        /// Сет объектов типа <typeparam name="T"></typeparam> в базе.
        /// </summary>
        public DbSet<T> Objects { get; set; }


        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">An error occurred sending updates to the database.</exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        /// A database command did not affect the expected number of rows. This usually indicates an optimistic
        /// concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        /// The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        /// on the same context instance.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The context or connection have been disposed.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// Some error occurred attempting to process entities in the context either before or after sending commands
        /// to the database.
        /// </exception>
        public override int SaveChanges() 
        {
            SetEntityValidation(false);

            EnableForeignKeysPragma();

            var saveResult = base.SaveChanges();

            SetEntityValidation(true);

            return saveResult;
        }


        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items) 
        {
            //игнорим ошибку "Требуется поле ..." - дурная прокси EF не загружает нужное свойство
            var result = base.ValidateEntity(entityEntry, items);
            if (result.IsValid || (entityEntry.State != EntityState.Added && entityEntry.State != EntityState.Modified))
            {
                return result;
            }

            return new
                DbEntityValidationResult
                (
                    entityEntry,
                    result
                        .ValidationErrors
                        .Where(e => !IsReferenceAndNotLoaded(entityEntry, e.PropertyName))
                );
        }

        protected void CreateTable(DbModelBuilder modelBuilder) 
        {
            try
            {
                CreateTableIfNotExists(modelBuilder);
            }
            catch (Exception ex)
            {
                //logger.Error(ex, $"Не удалось создать таблицу для типа '{ typeof(T).Name} '");
                //throw;
            }
            finally
            {
                Database.Connection.Close();
            }
        }


        private static SQLiteConnection CreateConnection() 
        {
            var builder = new SQLiteConnectionStringBuilder
            {
                BinaryGUID = false,
                DataSource = dbFilePath
            };

            return new SQLiteConnection(builder.ConnectionString);
        }


        private void SetEntityValidation(bool isEnabled)
        {
            Configuration.ValidateOnSaveEnabled = isEnabled;
        }

        public void EnableForeignKeysPragma()
        {
            TryOpenConnection();

            using (var pragmaCmd = Database.Connection.CreateCommand())
            {
                pragmaCmd.CommandText = "PRAGMA foreign_keys = ON";
                var result = pragmaCmd.ExecuteNonQuery();
            }

            //Database.Connection.Close();
        }

        private void TryOpenConnection()
        {
            try
            {
                Database.Connection.Open();
            }
            catch
            {
                //Console.WriteLine(e);
                //throw;
            }
        }

        private bool IsReferenceAndNotLoaded(DbEntityEntry entry, string memberName)
        {
            var member = entry.Member(memberName);
            //var reference = member as DbReferenceEntry;

            return member is DbPropertyEntry property && !property.IsModified;
        }

        private void CreateTableIfNotExists(DbModelBuilder modelBuilder) 
        {
            using (var creationCommand = Database.Connection.CreateCommand())
            {
                var generator = new SqliteSqlGenerator();

                DbModel model = modelBuilder.Build(Database.Connection);
                string sql = generator.Generate(model.StoreModel).Insert(12, " IF NOT EXISTS ");

                creationCommand.CommandText = sql;
                creationCommand.CommandTimeout = commandTimeoutInSeconds;

                TryOpenConnection();

                creationCommand.ExecuteNonQuery();
            }
        }
    }
}