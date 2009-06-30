using System.Data;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for a class that is used to perform queries on a database and return a reader.
    /// </summary>
    public interface IDbQueryReader : IDbQueryHandler
    {
        /// <summary>
        /// Executes the query on the database without parameters.
        /// </summary>
        /// <returns>IDataReader used to read the results of the query.</returns>
        IDataReader ExecuteReader();
    }

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
        IDataReader ExecuteReader(T item);
    }
}