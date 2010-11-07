using System;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// Exception for when a database connection failed.
    /// </summary>
    [Serializable]
    public sealed class DatabaseConnectionException : DatabaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseException"/> class.
        /// </summary>
        public DatabaseConnectionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DatabaseConnectionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DatabaseConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}