// Created by Laxale 17.04.2018
//
//

using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;


namespace Freengy.Database
{
    /// <summary>
    /// Интерсептор SQL команд, генерируемых Entity Framework.
    /// TODO удалить весь класс, если проблема двойного инсерта не будет больше возникать
    /// </summary>
    public class EfCommandInterceptor : IDbCommandInterceptor 
    {
        /// <summary>
        /// This method is called before a call to <see cref="M:System.Data.Common.DbCommand.ExecuteNonQuery" /> or
        /// one of its async counterparts is made.
        /// </summary>
        /// <param name="command">The command being executed.</param>
        /// <param name="interceptionContext">Contextual information associated with the call.</param>
        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            //TODO удалить весь класс, если проблема не будет больше возникать
            //            if (command.CommandText.Contains("INSERT INTO") &&
            //                command.CommandText.Contains(DbConst.TableNames.ReceiversCertificates) &&
            //                command.Parameters.Count < 2)
            //            {
            //                //EF долбит неправильный инсерт по неизвестной причине. Инсерт пытается записать NULL в Required поле. Не выполняем его
            //                interceptionContext.SuppressExecution();
            //                interceptionContext.Result = 1;
            //            }
            //
            //            if (command.CommandText.Contains("DELETE FROM"))
            //            {
            //                var t = 0;
            //            }
        }

        /// <summary>
        /// This method is called after a call to <see cref="M:System.Data.Common.DbCommand.ExecuteNonQuery" />  or
        /// one of its async counterparts is made. The result used by Entity Framework can be changed by setting
        /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext`1.Result" />.
        /// </summary>
        /// <remarks>
        /// For async operations this method is not called until after the async task has completed
        /// or failed.
        /// </remarks>
        /// <param name="command">The command being executed.</param>
        /// <param name="interceptionContext">Contextual information associated with the call.</param>
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {

        }

        /// <summary>
        /// This method is called before a call to <see cref="M:System.Data.Common.DbCommand.ExecuteReader(System.Data.CommandBehavior)" /> or
        /// one of its async counterparts is made.
        /// </summary>
        /// <param name="command">The command being executed.</param>
        /// <param name="interceptionContext">Contextual information associated with the call.</param>
        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {

        }

        /// <summary>
        /// This method is called after a call to <see cref="M:System.Data.Common.DbCommand.ExecuteReader(System.Data.CommandBehavior)" /> or
        /// one of its async counterparts is made. The result used by Entity Framework can be changed by setting
        /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext`1.Result" />.
        /// </summary>
        /// <remarks>
        /// For async operations this method is not called until after the async task has completed
        /// or failed.
        /// </remarks>
        /// <param name="command">The command being executed.</param>
        /// <param name="interceptionContext">Contextual information associated with the call.</param>
        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {

        }

        /// <summary>
        /// This method is called before a call to <see cref="M:System.Data.Common.DbCommand.ExecuteScalar" /> or
        /// one of its async counterparts is made.
        /// </summary>
        /// <param name="command">The command being executed.</param>
        /// <param name="interceptionContext">Contextual information associated with the call.</param>
        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {

        }

        /// <summary>
        /// This method is called after a call to <see cref="M:System.Data.Common.DbCommand.ExecuteScalar" /> or
        /// one of its async counterparts is made. The result used by Entity Framework can be changed by setting
        /// <see cref="P:System.Data.Entity.Infrastructure.Interception.DbCommandInterceptionContext`1.Result" />.
        /// </summary>
        /// <remarks>
        /// For async operations this method is not called until after the async task has completed
        /// or failed.
        /// </remarks>
        /// <param name="command">The command being executed.</param>
        /// <param name="interceptionContext">Contextual information associated with the call.</param>
        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {

        }
    }
}
