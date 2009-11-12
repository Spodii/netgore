using System.Data;

namespace NetGore.Db.MySql
{
    /// <summary>
    /// A query that finds the foreign keys for the given primary key.
    /// </summary>
    [DbControllerQuery]
    public class MySqlFindForeignKeysQuery : FindForeignKeysQuery
    {
        const string _queryStr =
            "SELECT `TABLE_NAME`, `COLUMN_NAME`" + " FROM information_schema.KEY_COLUMN_USAGE" + " WHERE `TABLE_SCHEMA` = {0} AND" +
            " `REFERENCED_TABLE_SCHEMA` = {0} AND" + " `REFERENCED_TABLE_NAME` = {1} AND" + " `REFERENCED_COLUMN_NAME` = {2};";

        /// <summary>
        /// Initializes a new instance of the <see cref="FindForeignKeysQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public MySqlFindForeignKeysQuery(DbConnectionPool connectionPool)
            : base(connectionPool, string.Format(_queryStr, DbParameterName, TableParameterName, ColumnParameterName))
        {
        }

        /// <summary>
        /// Reads a single row from the results of this query.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
        /// <returns>The values read from the <see cref="IDataReader"/>.</returns>
        protected override TableColumnPair ReadRow(IDataReader reader)
        {
            var retTable = reader.GetString("TABLE_NAME");
            var retColumn = reader.GetString("COLUMN_NAME");
            return new TableColumnPair(retTable, retColumn);
        }
    }
}