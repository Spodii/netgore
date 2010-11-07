using System;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// An <see cref="Exception"/> for when a query is improperly formatted. Specifically intended for the query builder classes.
    /// </summary>
    [Serializable]
    public class InvalidQueryException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidQueryException"/> class.
        /// </summary>
        /// <param name="message">The message describing the exception.</param>
        public InvalidQueryException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidQueryException"/> class.
        /// </summary>
        public InvalidQueryException()
        {
        }

        /// <summary>
        /// Creates an <see cref="InvalidQueryException"/> for when a column list is empty when it shouldn't be.
        /// </summary>
        /// <returns>An <see cref="InvalidQueryException"/> for when a column list is empty when it shouldn't be.</returns>
        public static InvalidQueryException CreateEmptyColumnList()
        {
            return new InvalidQueryException("The column list cannot be empty.");
        }
    }
}