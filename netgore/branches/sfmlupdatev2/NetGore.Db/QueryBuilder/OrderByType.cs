using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// The possible ways to order the rows in a query.
    /// </summary>
    public enum OrderByType : byte
    {
        /// <summary>
        /// Order by ascending.
        /// </summary>
        Ascending,

        /// <summary>
        /// Order by descending.
        /// </summary>
        Descending
    }
}