using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="IQueryBuilder"/> for MySql.
    /// </summary>
    public class MySqlQueryBuilder : IQueryBuilder
    {
        static readonly MySqlQueryBuilder _instance;

        /// <summary>
        /// Gets the <see cref="MySqlQueryBuilder"/> instance.
        /// </summary>
        public static MySqlQueryBuilder Instance { get { return _instance; } }

        /// <summary>
        /// Initializes the <see cref="MySqlQueryBuilder"/> class.
        /// </summary>
        static MySqlQueryBuilder()
        {
            _instance = new MySqlQueryBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlQueryBuilder"/> class.
        /// </summary>
        MySqlQueryBuilder()
        {
        }

        /// <summary>
        /// Creates an <see cref="IInsertQuery"/>.
        /// </summary>
        /// <param name="tableName">The name of the table to insert on.</param>
        /// <returns>The <see cref="IInsertQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableName"/> is an invalid table name.</exception>
        public IInsertQuery Insert(string tableName)
        {
            return new MySqlInsertQuery(tableName);
        }

        /// <summary>
        /// Creates an <see cref="IUpdateQuery"/>.
        /// </summary>
        /// <param name="tableName">The name of the table to update.</param>
        /// <returns>The <see cref="IUpdateQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableName"/> is an invalid table name.</exception>
        public IUpdateQuery Update(string tableName)
        {
            return new MySqlUpdateQuery(tableName);
        }

        /// <summary>
        /// Creates an <see cref="ISelectQuery"/>.
        /// </summary>
        /// <param name="tableName">The name of the table to select from.</param>
        /// <param name="alias">The alias to give the <paramref name="tableName"/>. Set as null to not use an alias.</param>
        /// <returns>The <see cref="ISelectQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableName"/> is an invalid table name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="alias"/> is an invalid table alias.</exception>
        public ISelectQuery Select(string tableName, string alias = null)
        {
            return new MySqlSelectQuery(tableName, alias);
        }

        /// <summary>
        /// Creates an <see cref="IDeleteQuery"/>.
        /// </summary>
        /// <param name="tableName">The name of the table to delete from.</param>
        /// <returns>The <see cref="IDeleteQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="tableName"/> is an invalid table name.</exception>
        public IDeleteQuery Delete(string tableName)
        {
            return new MySqlDeleteQuery(tableName);
        }
    }
}
