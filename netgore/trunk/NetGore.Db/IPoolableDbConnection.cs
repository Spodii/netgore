using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for an object that is used to pool a DbConnection.
    /// </summary>
    /// <typeparam name="T">Type of DbConnection to be pooled.</typeparam>
    public interface IPoolableDbConnection<T> : IDisposable, IPoolable<PooledDbConnection<T>> where T : DbConnection
    {
        /// <summary>
        /// Sets the DbConnection to be used by this IPoolableDbConnection. This should only be called by the
        /// DbConnectionPool, and can only be called once.
        /// </summary>
        /// <param name="connection">Connection to be used by the IPoolableDbConnection.</param>
        void SetConnection(T connection);

        /// <summary>
        /// Gets the IDbConnection for this IPoolableDbConnection.
        /// </summary>
        IDbConnection Connection { get; }
    }
}
