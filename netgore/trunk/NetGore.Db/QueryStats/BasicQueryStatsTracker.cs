using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace NetGore.Db
{
    /// <summary>
    /// A basic implementation of the <see cref="IQueryStatsTracker"/>.
    /// </summary>
    public class BasicQueryStatsTracker : IQueryStatsTracker
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Dictionary<object, QueryStats> _stats = new Dictionary<object, QueryStats>();
        readonly object _statsSync = new object();

        TickCount _nextLogTime = TickCount.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicQueryStatsTracker"/> class.
        /// </summary>
        public BasicQueryStatsTracker()
        {
            LogFilePath = null;
            LogFileFrequency = 1000 * 60 * 3;
        }

        /// <summary>
        /// Gets or sets how long, in milliseconds, to wait between each write to the query statistics log.
        /// Only used when <see cref="BasicQueryStatsTracker.LogFilePath"/> is set.
        /// Default value is 3 minutes.
        /// </summary>
        [DefaultValue(180000)]
        public int LogFileFrequency { get; set; }

        /// <summary>
        /// Gets or sets the path to the file to use to log the query statistics. If null, the statistics will not
        /// be written to file.
        /// </summary>
        [DefaultValue(null)]
        public string LogFilePath { get; set; }

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

        /// <summary>
        /// Attempts to write the log file.
        /// </summary>
        void WriteLog()
        {
            // Check if enough time has elapsed
            var now = TickCount.Now;
            if (now < _nextLogTime)
                return;

            // Update the next log time
            _nextLogTime = now + (TickCount)LogFileFrequency;

            // Check for a valid file path
            var filePath = LogFilePath;
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                // Get the stats in the order to write them
                IEnumerable<QueryStats> writeStats;
                lock (_statsSync)
                {
                    writeStats = _stats.Values.OrderByDescending(x => x.TimesExecuted).ToImmutable();
                }

                // Create the text
                var sb = new StringBuilder();

                foreach (var stat in writeStats)
                {
                    stat.AppendStatsLine(sb);
                }

                // Write the log file
                File.WriteAllText(filePath, sb.ToString());
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to write query stats log to file `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, filePath, ex);
                Debug.Fail(string.Format(errmsg, filePath, ex));
            }
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

            WriteLog();
        }

        #endregion
    }
}