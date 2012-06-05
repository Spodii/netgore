using System;
using System.Linq;
using System.Runtime.Serialization;

namespace NetGore.Audio
{
    /// <summary>
    /// An <see cref="Exception"/> related to sound.
    /// </summary>
    [Serializable]
    public class SoundException : AudioException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SoundException"/> class.
        /// </summary>
        public SoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public SoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the
        /// serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains
        /// contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or
        /// <see cref="P:System.Exception.HResult"/> is zero (0).</exception>
        protected SoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}