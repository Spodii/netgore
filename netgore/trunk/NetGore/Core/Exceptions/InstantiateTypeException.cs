using System;
using System.Linq;
using System.Runtime.Serialization;

namespace NetGore
{
    /// <summary>
    /// Exception for when a <see cref="Type"/> failed to be instantiated.
    /// </summary>
    [Serializable]
    public sealed class InstantiateTypeException : TypeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        public InstantiateTypeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeException"/> class.
        /// </summary>
        /// <param name="type">The type that caused the exception.</param>
        public InstantiateTypeException(Type type) : base(type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InstantiateTypeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="type">The type that caused the exception.</param>
        public InstantiateTypeException(string message, Type type) : base(message, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null
        /// reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public InstantiateTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstantiateTypeException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that
        /// contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null
        /// or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        InstantiateTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}