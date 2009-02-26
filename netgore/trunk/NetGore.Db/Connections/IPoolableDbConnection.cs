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
    public interface IPoolableDbConnection : IDisposable
    {
        /// <summary>
        /// Gets the IDbConnection for this IPoolableDbConnection. Never dispose of this IDbConnection directly.
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Gets the IDbCommand for the IDbConnection in this IPoolableDbConnection. It is strongly suggested you use
        /// this command instead of creating a new one to prevent creating extra garbage. This command is guarenteed
        /// to be set up for the given Connection. Never dispose of this IDbCommand directly.
        /// </summary>
        IDbCommand Command { get; }
    }
}
