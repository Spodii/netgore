using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// A query that finds the foreign keys for the given primary key.
    /// </summary>
    public abstract class FindForeignKeysQuery : DbQueryReader<FindForeignKeysQuery.QueryArgs>
    {
        /// <summary>
        /// The name of the parameter for the column name.
        /// </summary>
        protected const string ColumnParameterName = "column";

        /// <summary>
        /// The name of the parameter for the schema name.
        /// </summary>
        protected const string SchemaParameterName = "schema";

        /// <summary>
        /// The name of the parameter for the table name.
        /// </summary>
        protected const string TableParameterName = "table";

        /// <summary>
        /// Initializes a new instance of the <see cref="FindForeignKeysQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        /// <param name="commandText">String containing the command to use for the query.</param>
        protected FindForeignKeysQuery(DbConnectionPool connectionPool, string commandText) : base(connectionPool, commandText)
        {
        }

        /// <summary>
        /// Executes the query and gets the results.
        /// </summary>
        /// <param name="database">Database of the <paramref name="table"/>.</param>
        /// <param name="table">The table of the primary key.</param>
        /// <param name="column">The column of the primary key.</param>
        /// <returns>The results of the query.</returns>
        public IEnumerable<SchemaTableColumn> Execute(string database, string table, string column)
        {
            var ret = new List<SchemaTableColumn>();

            using (var r = ExecuteReader(new QueryArgs(database, table, column)))
            {
                while (r.Read())
                {
                    var value = ReadRow(r);
                    ret.Add(value);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters(SchemaParameterName, TableParameterName, ColumnParameterName);
        }

        /// <summary>
        /// Reads a single row from the results of this query.
        /// </summary>
        /// <param name="reader">The <see cref="IDataReader"/> to read from.</param>
        /// <returns>The values read from the <see cref="IDataReader"/>.</returns>
        protected abstract SchemaTableColumn ReadRow(IDataReader reader);

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p[SchemaParameterName] = item.Database;
            p[TableParameterName] = item.Table;
            p[ColumnParameterName] = item.Column;
        }

        /// <summary>
        /// Contains the arguments for executing the <see cref="FindForeignKeysQuery"/> query.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public struct QueryArgs
        {
            readonly string _column;
            readonly string _database;
            readonly string _table;

            /// <summary>
            /// Initializes a new instance of the <see cref="QueryArgs"/> struct.
            /// </summary>
            /// <param name="database">The database.</param>
            /// <param name="table">The table.</param>
            /// <param name="column">The column.</param>
            public QueryArgs(string database, string table, string column)
            {
                _database = database;
                _table = table;
                _column = column;
            }

            /// <summary>
            /// Gets the column.
            /// </summary>
            public string Column
            {
                get { return _column; }
            }

            /// <summary>
            /// Gets the database.
            /// </summary>
            public string Database
            {
                get { return _database; }
            }

            /// <summary>
            /// Gets the table.
            /// </summary>
            public string Table
            {
                get { return _table; }
            }
        }
    }
}