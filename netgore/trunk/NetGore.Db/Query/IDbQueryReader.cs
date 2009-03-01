using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace NetGore.Db.Query
{
    /// <summary>
    /// Interface for a class that is used to perform a queries on a database that return a reader.
    /// </summary>
    /// <typeparam name="T">Type of the object used for executing the query.</typeparam>
    public interface IDbQueryReader<T> : IDbQueryHandler
    {
        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>IDataReader used to read the results of the query.</returns>
        IDataReader Execute(T item);
    }
}
