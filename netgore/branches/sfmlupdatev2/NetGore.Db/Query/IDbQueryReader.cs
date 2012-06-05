using System.Data;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for a class that is used to perform queries on a database and return a reader.
    /// </summary>
    public interface IDbQueryReader : IDbQuery
    {
        /// <summary>
        /// Executes the query on the database without parameters.
        /// </summary>
        /// <returns>IDataReader used to read the results of the query.</returns>
        /// <exception cref="DuplicateKeyException">An insert is being performed using a key that already exists.</exception>
        IDataReader ExecuteReader();
    }

    /// <summary>
    /// Interface for a class that is used to perform a queries on a database that return a reader.
    /// </summary>
    /// <typeparam name="T">Type of the object used for executing the query.</typeparam>
    public interface IDbQueryReader<in T> : IDbQuery
    {
        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        /// <returns>IDataReader used to read the results of the query.</returns>
        /// <exception cref="DuplicateKeyException">An insert is being performed using a key that already exists.</exception>
        IDataReader ExecuteReader(T item);
    }
}