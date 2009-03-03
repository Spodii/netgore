using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Network.Bits
{
    /// <summary>
    /// BitStream seek origin
    /// </summary>
    public enum BitStreamSeekOrigin
    {
        /// <summary>
        /// Specifies the beginning of the stream
        /// </summary>
        Beginning,
        /// <summary>
        /// Specifies the current position of the stream
        /// </summary>
        Current
    }
}