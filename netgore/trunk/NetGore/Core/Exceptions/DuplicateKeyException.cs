using System;
using System.Linq;
using System.Runtime.Serialization;

namespace NetGore
{
    /// <summary>
    /// An <see cref="Exception"/> for when a key already exists in a collection.
    /// </summary>
    [Serializable]
    public class DuplicateKeyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException"/> class.
        /// </summary>
        public DuplicateKeyException() : base(GetMessage(null))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException"/> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public DuplicateKeyException(Exception innerException) : base(GetMessage(null), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DuplicateKeyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DuplicateKeyException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that
        /// contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null
        /// or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected DuplicateKeyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Creates a <see cref="DuplicateKeyException"/> with the message internally generated.
        /// </summary>
        /// <param name="key">The value of the key.</param>
        /// <returns>The <see cref="DuplicateKeyException"/> instance.</returns>
        public static DuplicateKeyException CreateForKey(string key)
        {
            return new DuplicateKeyException(GetMessage(key));
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The message.</returns>
        static string GetMessage(string key)
        {
            if (string.IsNullOrEmpty(key))
                return "The desired key already exists in the collection.";

            return "The key `{0}` already exists in the collection.";
        }
    }
}