using System;
using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// A generic <see cref="Exception"/> involving a <see cref="GrhData"/>.
    /// </summary>
    public class GrhDataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> related to the <see cref="Exception"/>.</param>
        public GrhDataException(GrhData grhData) : base(GetStr(grhData, null))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error message.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> related to the <see cref="Exception"/>.</param>
        /// <param name="message">The message that describes the error.</param>
        public GrhDataException(GrhData grhData, string message) : base(GetStr(grhData, message))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Exception"/> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> related to the <see cref="Exception"/>.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public GrhDataException(GrhData grhData, string message, Exception innerException)
            : base(GetStr(grhData, message), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhDataException"/> class.
        /// </summary>
        /// <param name="rawMessage">The raw message.</param>
        protected GrhDataException(string rawMessage) : base(rawMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrhDataException"/> class.
        /// </summary>
        /// <param name="rawMessage">The raw message.</param>
        /// <param name="innerException">The exception that is the cause of the current exception,
        /// or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        protected GrhDataException(string rawMessage, Exception innerException) : base(rawMessage, innerException)
        {
        }

        static string GetStr(GrhData grhData, string msg)
        {
            var s = string.Format("Exception caused by GrhData `{0}`.", grhData != null ? grhData.ToString() : "[NULL]");
            if (!string.IsNullOrEmpty(s))
                s += " " + msg;
            return s;
        }
    }
}