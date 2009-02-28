using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for a class that is used to perform a non-reader query on a database.
    /// </summary>
    public interface IDbQueryNonReader : IDisposable
    {
        /// <summary>
        /// Gets the CommandText used by this IDbQueryNonReader.
        /// </summary>
        string CommandText { get; }
    }

    /// <summary>
    /// Interface for a class that is used to perform a non-reader query on a database.
    /// </summary>
    /// <typeparam name="T">Type of the object used for executing the query.</typeparam>
    public interface IDbQueryNonReader<T> : IDbQueryNonReader
    {
        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>Number of rows affected by the query.</returns>
        int Execute(T item);
    }
}
