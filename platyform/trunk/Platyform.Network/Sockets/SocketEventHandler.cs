using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Platyform.Extensions;

namespace Platyform.Network
{
    /// <summary>
    /// Delegate for a generic socket event.
    /// </summary>
    /// <param name="conn">Connection on which the event occured.</param>
    public delegate void SocketEventHandler(TCPSocket conn);
}