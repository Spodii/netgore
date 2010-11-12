using System;
using System.Linq;
using System.Text;

namespace NetGore.Db
{
    /// <summary>
    /// Contains the statistics for a single query.
    /// </summary>
    public class QueryStats : IQueryStats, IQueryStatsMutator
    {
        readonly object _query;

        DateTime _lastExecuted;
        int _timesExecuted = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryStats"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public QueryStats(object query)
        {
            _query = query;
        }

        #region IQueryStats Members

        /// <summary>
        /// Gets the query that these stats are for.
        /// </summary>
        public object Query
        {
            get { return _query; }
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> that this query was last executed.
        /// </summary>
        public DateTime TimeLastExecuted
        {
            get { return _lastExecuted; }
        }

        /// <summary>
        /// Gets the number of times that the query has been executed.
        /// </summary>
        public int TimesExecuted
        {
            get { return _timesExecuted; }
        }

        /// <summary>
        /// Appends the detailed statistics of the <see cref="IQueryStats"/> to a <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> to write the details to.</param>
        public void AppendStatsLine(StringBuilder sb)
        {
            sb.Append(Query);
            sb.Append(": ");
            sb.Append(TimesExecuted);
            sb.AppendLine();
        }

        #endregion

        #region IQueryStatsMutator Members

        /// <summary>
        /// Increments the execution count for the query.
        /// </summary>
        public void NotifyExecuted()
        {
            _timesExecuted++;
            _lastExecuted = DateTime.Now;
        }

        #endregion
    }
}