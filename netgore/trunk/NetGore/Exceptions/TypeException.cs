using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Exception for when a <see cref="Type"/> does not include require information or is not of the expected type.
    /// </summary>
    public class TypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        public TypeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeException"/> class.
        /// </summary>
        /// <param name="type">The type that caused the exception.</param>
        public TypeException(Type type) : this(string.Empty, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TypeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="type">The type that caused the exception.</param>
        public TypeException(string message, Type type) : base("TypeException from `" + type + "`. " + message ?? string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null
        /// reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public TypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}