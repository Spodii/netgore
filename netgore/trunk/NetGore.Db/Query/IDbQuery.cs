using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for a class that is used to perform queries on a database.
    /// </summary>
    public interface IDbQuery : IDisposable
    {
        /// <summary>
        /// Gets the CommandText used by this IDbQueryHandler. All commands executed by this <see cref="IDbQuery"/>
        /// will use this same CommandText.
        /// </summary>
        string CommandText { get; }

        /// <summary>
        /// Gets the <see cref="IDbConnectionPool"/> used to manage the database connections.
        /// </summary>
        IDbConnectionPool ConnectionPool { get; }

        /// <summary>
        /// Gets the parameters used in this <see cref="IDbQuery"/>.
        /// </summary>
        IEnumerable<DbParameter> Parameters { get; }
    }
}