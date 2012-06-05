using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

namespace NetGore.Graphics
{
    /// <summary>
    /// Exception for when a <see cref="GrhData"/> is of an unsupported derived type.
    /// </summary>
    [Serializable]
    public class UnsupportedGrhDataTypeException : GrhDataException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedGrhDataTypeException"/> class.
        /// </summary>
        public UnsupportedGrhDataTypeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhDataException"/> class.
        /// </summary>
        /// <param name="rawMessage">The raw message.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public UnsupportedGrhDataTypeException(string rawMessage, Exception innerException) : base(rawMessage, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedGrhDataTypeException"/> class.
        /// </summary>
        /// <param name="rawMessage">The raw message.</param>
        public UnsupportedGrhDataTypeException(string rawMessage) : base(rawMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedGrhDataTypeException"/> class.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> related to the <see cref="Exception"/>.</param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "GrhData")]
        public UnsupportedGrhDataTypeException(GrhData grhData)
            : base(
                grhData,
                string.Format("GrhData `{0}` is of derived type `{1}`, which is not supported by this operation.", grhData,
                    grhData.GetType()))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedGrhDataTypeException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds
        /// the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that
        /// contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null.</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null
        /// or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        protected UnsupportedGrhDataTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}