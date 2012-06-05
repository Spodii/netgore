using System.Data;
using System.Linq;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql
{
    /// <summary>
    /// A query that finds the foreign keys for the given primary key.
    /// </summary>
    [DbControllerQuery]
    public class MySqlFindForeignKeysQuery : FindForeignKeysQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FindForeignKeysQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public MySqlFindForeignKeysQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // CALL find_foreign_keys(@schema,@table,@column)

            var q = qb.CallProcedure("find_foreign_keys").AddParam(SchemaParameterName, TableParameterName, ColumnParameterName);
            return q.ToString();
        }

        /// <summary>
        /// Reads a single row from the results of this query.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
        /// <returns>The values read from the <see cref="IDataReader"/>.</returns>
        protected override SchemaTableColumn ReadRow(IDataReader reader)
        {
            var retSchema = reader.GetString("TABLE_SCHEMA");
            var retTable = reader.GetString("TABLE_NAME");
            var retColumn = reader.GetString("COLUMN_NAME");
            return new SchemaTableColumn(retSchema, retTable, retColumn);
        }
    }
}