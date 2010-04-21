using System;
using System.Data.Common;
using System.Linq;

namespace NetGore.Db
{
    /// <summary>
    /// A generic database exception.
    /// </summary>
    [Serializable]
    public class DatabaseException : DbException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseException"/> class.
        /// </summary>
        public DatabaseException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DatabaseException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}