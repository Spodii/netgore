using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for an object that is used to pool a DbConnection.
    /// </summary>
    public interface IPoolableDbConnection : IDisposable
    {
        /// <summary>
        /// Gets the IDbConnection for this IPoolableDbConnection. Never dispose of this IDbConnection directly.
        /// </summary>
        DbConnection Connection { get; }
    }
}