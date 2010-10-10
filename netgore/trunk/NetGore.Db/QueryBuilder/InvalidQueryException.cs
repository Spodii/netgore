using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Db.QueryBuilder
{
    public class InvalidQueryException : InvalidOperationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidQueryException"/> class.
        /// </summary>
        /// <param name="message">The message describing the exception.</param>
        public InvalidQueryException(string message)
            : base(message)
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
