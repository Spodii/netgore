using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for a database controller, which provides a medium for getting an instance of query classes
    /// that implement the <see cref="DbControllerQueryAttribute"/> attribute.
    /// </summary>
    public interface IDbController : IDisposable
    {
        /// <summary>
        /// Gets the <see cref="DbConnectionPool"/> used by this <see cref="IDbController"/>.
        /// </summary>
        DbConnectionPool ConnectionPool { get; }

        /// <summary>
        /// Gets the name of the table and column that reference a the given primary key.
        /// </summary>
        /// <param name="database">Database of the <paramref name="table"/>.</param>
        /// <param name="table">The table of the primary key.</param>
        /// <param name="column">The column of the primary key.</param>
        /// <returns>An IEnumerable of the name of the tables and columns that reference a the given primary key.</returns>
        IEnumerable<TableColumnPair> GetPrimaryKeyReferences(string database, string table, string column);

        /// <summary>
        /// Gets a query that was marked with the attribute DbControllerQueryAttribute.
        /// </summary>
        /// <typeparam name="T">The Type of query.</typeparam>
        /// <returns>The query instance of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">Type <typeparamref name="T"/> was not found in the query cache.</exception>
        T GetQuery<T>();

        /// <summary>
        /// Finds all of the column names in the given <paramref name="table"/>.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>All of the column names in the given <paramref name="table"/>.</returns>
        IEnumerable<string> GetTableColumns(string table);
    }
}