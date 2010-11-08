using System;
using System.Linq;
using System.Runtime.Serialization;

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
        /// Initializes a new instance of the <see cref="InvalidQueryException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.
        /// If the <paramref name="innerException"/> parameter is not a null reference (Nothing in Visual Basic),
        /// the current exception is raised in a catch block that handles the inner exception.</param>
        public InvalidQueryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidQueryException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected InvalidQueryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Creates an <see cref="InvalidQueryException"/> for when a column list is empty when it shouldn't be.
        /// </summary>
        /// <returns>
        /// An <see cref="InvalidQueryException"/> for when a column list is empty when it shouldn't be.
        /// </returns>
        public static InvalidQueryException CreateEmptyColumnList()
        {
            return new InvalidQueryException("The column list cannot be empty.");
        }
    }
}