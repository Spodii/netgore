using System;
using System.Linq;

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
        /// <param name="key">The key that caused the <see cref="DuplicateKeyException"/>.</param>
        /// <param name="innerException">The inner exception.</param>
        public DuplicateKeyException(string key, Exception innerException) : base(GetMessage(key), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException"/> class.
        /// </summary>
        /// <param name="key">The key that caused the <see cref="DuplicateKeyException"/>.</param>
        public DuplicateKeyException(string key) : base(GetMessage(key))
        {
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