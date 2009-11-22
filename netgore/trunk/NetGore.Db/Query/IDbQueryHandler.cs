using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for a class that is used to perform queries on a database.
    /// </summary>
    public interface IDbQueryHandler : IDisposable
    {
        /// <summary>
        /// Gets the CommandText used by this IDbQueryHandler. All commands executed by this IDbQueryHandler
        /// will use this CommandText.
        /// </summary>
        string CommandText { get; }

        /// <summary>
        /// Gets the DbConnectionPool used to manage the database connections.
        /// </summary>
        DbConnectionPool ConnectionPool { get; }

        /// <summary>
        /// Gets an IEnumerable of all of the parameters used in this IDbQueryHandler.
        /// </summary>
        IEnumerable<DbParameter> Parameters { get; }
    }
}