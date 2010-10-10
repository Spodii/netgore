using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.QueryBuilder
{
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

        public IInsertQuery Insert(string tableName)
        {
            return new MySqlInsertQuery(tableName);
        }

        public IUpdateQuery Update(string tableName)
        {
            return new MySqlUpdateQuery(tableName);
        }

        public ISelectQuery Select(string tableName, string alias = null)
        {
            return new MySqlSelectQuery(tableName, alias);
        }

        public IDeleteQuery Delete(string tableName)
        {
            return new MySqlDeleteQuery(tableName);
        }
    }
}
