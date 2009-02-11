using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// Delegate for a socket receive event for a message.
    /// </summary>
    /// <param name="conn">Connection on which the event occured.</param>
    /// <param name="msg">Data received.</param>
    public delegate void SocketReceiveEventHandler(TCPSocket conn, byte[] msg);
}