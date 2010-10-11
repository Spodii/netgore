using System;
using System.Data.Common;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for an object that is used to pool a DbConnection.
    /// </summary>
    public interface IPoolableDbConnection : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="DbConnection"/> for this <see cref="IPoolableDbConnection"/>.
        /// Never dispose of this <see cref="DbConnection"/> directly.
        /// </summary>
        DbConnection Connection { get; }
    }
}