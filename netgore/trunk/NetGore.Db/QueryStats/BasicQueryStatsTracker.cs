using System;
using System.Collections.Generic;

namespace NetGore.Db
{
    /// <summary>
    /// A basic implementation of the <see cref="IQueryStatsTracker"/>.
    /// </summary>
    public class BasicQueryStatsTracker : IQueryStatsTracker
    {
        readonly Dictionary<object, QueryStats> _stats = new Dictionary<object, QueryStats>();
        readonly object _statsSync = new object();

        /// <summary>
        /// Gets the <see cref="IQueryStats"/> for a query.
        /// </summary>
        /// <param name="query">The query to get the statistics for.</param>
        /// <param name="create">If true, the stats will be created for the <paramref name="query"/> if they do
        /// not already exist.</param>
        /// <returns>
        /// The statistics for the <paramref name="query"/>, or null if no statistics are available for the
        /// <paramref name="query"/> and <paramref name="create"/> is false.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="query"/> is null.</exception>
        QueryStats GetQueryStats(object query, bool create)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            QueryStats ret;

            lock (_statsSync)
            {
                // Try to grab the stats from the dictionary
                if (!_stats.TryGetValue(query, out ret))
                {
                    if (create)
                    {
                        // Create and add the stats
                        ret = new QueryStats(query);
                        _stats.Add(query, ret);
                    }
                    else
                    {
                        // Do not create
                        ret = null;
                    }
                }
            }

            // Return the QueryStats
            return ret;
        }

        #region IQueryStatsTracker Members

        /// <summary>
        /// Gets the <see cref="IQueryStats"/> for a query.
        /// </summary>
        /// <param name="query">The query to get the statistics for.</param>
        /// <returns>The statistics for the <paramref name="query"/>, or null if no statistics are available for the
        /// <paramref name="query"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="query"/> is null.</exception>
        public IQueryStats GetQueryStats(object query)
        {
            return GetQueryStats(query, false);
        }

        /// <summary>
        /// Notifies this object that a query has been executed.
        /// </summary>
        /// <param name="query">The query object that was executed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="query"/> is null.</exception>
        public void QueryExecuted(object query)
        {
            var stats = GetQueryStats(query, true);
            stats.NotifyExecuted();
        }

        #endregion
    }
}