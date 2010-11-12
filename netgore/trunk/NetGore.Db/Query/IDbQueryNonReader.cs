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
    }
}