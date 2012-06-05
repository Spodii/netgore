using System;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for an object that tracks the statistics for queries.
    /// </summary>
    public interface IQueryStatsTracker
    {
        /// <summary>
        /// Gets the <see cref="IQueryStats"/> for a query.
        /// </summary>
        /// <param name="query">The query to get the statistics for.</param>
        /// <returns>The statistics for the <paramref name="query"/>, or null if no statistics are available for the
        /// <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="query"/> is null.</exception>
        IQueryStats GetQueryStats(object query);

        /// <summary>
        /// Notifies this object that a query has been executed.
        /// </summary>
        /// <param name="query">The query object that was executed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="query"/> is null.</exception>
        void QueryExecuted(object query);
    }
}