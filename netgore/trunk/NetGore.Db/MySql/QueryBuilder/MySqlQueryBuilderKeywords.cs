using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="IQueryBuilderKeywords"/> implementation for MySql.
    /// </summary>
    public class MySqlQueryBuilderKeywords : IQueryBuilderKeywords
    {
        static readonly MySqlQueryBuilderKeywords _instance;

        /// <summary>
        /// Gets the <see cref="MySqlQueryBuilderKeywords"/> instance.
        /// </summary>
        public static MySqlQueryBuilderKeywords Instance { get { return _instance; } }

        /// <summary>
        /// Initializes the <see cref="MySqlQueryBuilderKeywords"/> class.
        /// </summary>
        static MySqlQueryBuilderKeywords()
        {
            _instance = new MySqlQueryBuilderKeywords();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlQueryBuilderKeywords"/> class.
        /// </summary>
        MySqlQueryBuilderKeywords()
        {
        }

        /// <summary>
        /// Gets the keyword for counting the number of rows.
        /// </summary>
        /// <example>
        /// COUNT(*) in MySql.
        /// </example>
        public string Count
        {
            get { return "COUNT(*)"; }
        }
    }
}