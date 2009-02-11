using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace MySql.Data.MySqlClient
{
    /// <summary>
    /// The exception that is thrown when MySQL returns an error. This class cannot be inherited.
    /// </summary>
    /// <include file='docs/MySqlException.xml' path='MyDocs/MyMembers[@name="Class"]/*'/>
    [Serializable]
    public sealed class MySqlException : DbException
    {
        readonly int errorCode;
        readonly bool isFatal;

        /// <summary>
        /// True if this exception was fatal and cause the closing of the connection, false otherwise.
        /// </summary>
        internal bool IsFatal
        {
            get { return isFatal; }
        }

        /// <summary>
        /// Gets a number that identifies the type of error.
        /// </summary>
        public int Number
        {
            get { return errorCode; }
        }

        internal MySqlException()
        {
        }

        internal MySqlException(string msg) : base(msg)
        {
        }

        internal MySqlException(string msg, Exception ex) : base(msg, ex)
        {
        }

        internal MySqlException(string msg, bool isFatal, Exception inner) : base(msg, inner)
        {
            this.isFatal = isFatal;
        }

        internal MySqlException(string msg, int errno, Exception inner) : this(msg, inner)
        {
            errorCode = errno;
            Data.Add("Server Error Code", errno);
        }

        internal MySqlException(string msg, int errno) : this(msg, errno, null)
        {
        }

        MySqlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}