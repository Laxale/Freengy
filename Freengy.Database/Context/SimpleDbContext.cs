// Created by Laxale 17.04.2018
//
//

using System.Data.Entity;

using Freengy.Common.Database;


namespace Freengy.Database.Context 
{
    /// <summary>
    /// EF-контекст базы данных PKI Client для работы с простыми настройками - не требующими маппинга и не содержащими вложенных типов.
    /// </summary>
    /// <typeparam name="T">Тип объекта простых настроек - наследник <see cref="DbObject"/>.</typeparam>
    public class SimpleDbContext<T> : DbContextBase<T> where T : DbObject, new() 
    {
        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        /// <param name="modelBuilder"> The builder that defines the model for the context being created. </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            CreateTable(modelBuilder);
        }
    }
}