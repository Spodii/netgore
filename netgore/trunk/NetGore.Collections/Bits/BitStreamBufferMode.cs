using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Decides what happens when the end of the buffer is reached
    /// </summary>
    public enum BitStreamBufferMode
    {
        /// <summary>
        /// The buffer will not attempt to expand and an error will
        /// be raised if trying to pass the end of the buffer and ensure the
        /// internal buffer reference object never changes
        /// </summary>
        Static,
        /// <summary>
        /// The buffer will silently expand when the end has been surpassed but
        /// does not guarentee the internal buffer reference object will not change
        /// </summary>
        Dynamic
    }
}