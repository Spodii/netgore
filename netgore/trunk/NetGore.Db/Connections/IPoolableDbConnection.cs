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

        /// <summary>
        /// Gets the <see cref="IQueryStatsTracker"/> to use to track the statistics for queries executed by
        /// this object. If null, statistics will not be tracked.
        /// </summary>
        IQueryStatsTracker QueryStats { get; }
    }
}