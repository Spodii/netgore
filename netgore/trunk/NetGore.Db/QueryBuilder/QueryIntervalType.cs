using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// The different interval types available in a query.
    /// </summary>
    public enum QueryIntervalType
    {
        /// <summary>
        /// Microseconds.
        /// </summary>
        Microsecond,

        /// <summary>
        /// Seconds.
        /// </summary>
        Second,

        /// <summary>
        /// Minutes.
        /// </summary>
        Minute,

        /// <summary>
        /// Hours.
        /// </summary>
        Hour,

        /// <summary>
        /// Days.
        /// </summary>
        Day,

        /// <summary>
        /// Weeks.
        /// </summary>
        Week,

        /// <summary>
        /// Months.
        /// </summary>
        Month,

        /// <summary>
        /// Quarters.
        /// </summary>
        Quarter,

        /// <summary>
        /// Years.
        /// </summary>
        Year
    }
}