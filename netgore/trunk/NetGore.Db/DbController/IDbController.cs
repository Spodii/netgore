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
        /// Removes all of the primary keys from a table where there is no foreign keys for the respective primary key.
        /// For safety reasons, if a column has no foreign keys, the query will be aborted.
        /// </summary>
        /// <param name="schema">The schema or database name of the table.</param>
        /// <param name="table">The table to check.</param>
        /// <param name="column">The primary key column.</param>
        /// <returns>The number of rows removed, or -1 if there were no foreign keys for the given column in the first place.</returns>
        int RemoveUnreferencedPrimaryKeys(string schema, string table, string column);

        /// <summary>
        /// Gets the schema, table, and column tuples for columns containing a reference to the specified primary key.
        /// </summary>
        /// <param name="database">Database or schema object that the <paramref name="table"/> belongs to.</param>
        /// <param name="table">The table of the primary key.</param>
        /// <param name="column">The column of the primary key.</param>
        /// <returns>An IEnumerable of the name of the tables and columns that reference a the given primary key.</returns>
        IEnumerable<SchemaTableColumn> GetPrimaryKeyReferences(string database, string table, string column);

        /// <summary>
        /// Gets the name of the database that this <see cref="IDbController"/> instance is connected to.
        /// </summary>
        string Database { get; }

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