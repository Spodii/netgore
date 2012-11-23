using System;
using System.Data.Common;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for an object that runs database queries in non-blocking mode, when possible, while retaining
    /// order of execution for queries.
    /// </summary>
    public interface IDbQueryRunner : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="DbConnection"/> used by this <see cref="IDbQueryRunner"/>.
        /// </summary>
        DbConnection Connection { get; }

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Executes a query that returns a <see cref="DbDataReader"/> to read the results.
        /// This runs in blocking mode.
        /// </summary>
        /// <param name="cmd">The <see cref="DbCommand"/> to execute.</param>
        /// <returns>The <see cref="DbDataReader"/> to use to read the results.</returns>
        DbDataReader BeginExecuteReader(DbCommand cmd);

        /// <summary>
        /// Ends the execution of <see cref="IDbQueryRunner.BeginExecuteReader"/> call. This must
        /// be called once done reading to prevent deadlocks.
        /// </summary>
        /// <exception cref="System.Threading.SynchronizationLockException">This thread did not own the lock or the lock
        /// was not acquired.</exception>
        void EndExecuteReader();

        /// <summary>
        /// Executes a query that has no return value.
        /// This runs in non-blocking mode.
        /// </summary>
        /// <param name="cmd">The <see cref="DbCommand"/> to execute.</param>
        /// <param name="queryBase">The <see cref="DbQueryBase"/> that is executing this command.</param>
        void ExecuteNonReader(DbCommand cmd, DbQueryBase queryBase);

        /// <summary>
        /// Executes a query that returns the number of rows affected.
        /// This runs in blocking mode.
        /// </summary>
        /// <param name="cmd">The <see cref="DbCommand"/> to execute.</param>
        /// <param name="queryBase">The <see cref="DbQueryBase"/> that is executing this command.</param>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonReaderWithResult(DbCommand cmd, DbQueryBase queryBase);

        /// <summary>
        /// Executes a query that returns the number of rows affected.
        /// This runs in blocking mode.
        /// </summary>
        /// <param name="cmd">The <see cref="DbCommand"/> to execute.</param>
        /// <param name="queryBase">The <see cref="DbQueryBase"/> that is executing this command.</param>
        /// <param name="lastInsertedId">Contains the ID for the row that was inserted into the database. Only valid when the
        /// query contains an auto-increment column and the operation being performed is an insert.</param>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonReaderWithResult(DbCommand cmd, DbQueryBase queryBase, out long lastInsertedId);

        /// <summary>
        /// Flushes the query queue, and blocks until all queries in the queue at the time this method was called have been
        /// executed.
        /// </summary>
        /// <returns>The number of queries that were flushed from the internal queue.</returns>
        int Flush();
    }
}