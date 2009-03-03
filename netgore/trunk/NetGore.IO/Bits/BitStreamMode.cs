using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.IO.Bits
{
    /// <summary>
    /// I/O method to use on the bit stream
    /// </summary>
    public enum BitStreamMode
    {
        /// <summary>
        /// Reads values from the buffer
        /// </summary>
        Read,
        /// <summary>
        /// Writes values to the buffer
        /// </summary>
        Write
    }
}