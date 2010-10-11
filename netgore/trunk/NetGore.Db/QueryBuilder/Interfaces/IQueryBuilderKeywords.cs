namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for various keywords for an <see cref="IQueryBuilder"/>.
    /// </summary>
    public interface IQueryBuilderKeywords
    {
        /// <summary>
        /// Gets the keyword for counting the number of rows.
        /// </summary>
        /// <example>
        /// COUNT(*) in MySql.
        /// </example>
        string Count { get; }
    }
}