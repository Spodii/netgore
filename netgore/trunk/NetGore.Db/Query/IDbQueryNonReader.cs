using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for a class that is used to perform a non-reader queries on a database.
    /// </summary>
    public interface IDbQueryNonReader : IDbQuery
    {
        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        void Execute();

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <returns>Number of rows affected by the query.</returns>
        int ExecuteWithResult();

        /// <summary>
        /// Executes the query on the database.
        /// </summary>
        /// <param name="lastInsertedId">Contains the ID for the row that was inserted into the database. Only valid when the
        /// query contains an auto-increment column and the operation being performed is an insert.</param>
        /// <returns>Number of rows affected by the query.</returns>
        int ExecuteWithResult(out long lastInsertedId);
    }

    /// <summary>
    /// Interface for a class that is used to perform a non-reader queries on a database.
    /// </summary>
    /// <typeparam name="T">Type of the object used for executing the query.</typeparam>
    public interface IDbQueryNonReader<in T> : IDbQuery
    {
        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        void Execute(T item);

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>Number of rows affected by the query.</returns>
        int ExecuteWithResult(T item);

        /// <summary>
        /// Executes the query on the database using the specified <paramref name="item"/>.
        /// </summary>
        /// <param name="lastInsertedId">Contains the ID for the row that was inserted into the database. Only valid when the
        /// query contains an auto-increment column and the operation being performed is an insert.</param>
        /// <param name="item">Item containing the value or values used for executing the query.</param>
        /// <returns>Number of rows affected by the query.</returns>
        int ExecuteWithResult(T item, out long lastInsertedId);
    }
}