using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Enum containing all of the different in-game messages sent from the Server to the Client.
    /// </summary>
    public enum GameMessage : byte
    {
        /// <summary>
        /// Invalid Say chat command
        /// </summary>
        InvalidCommand,
        TellWithoutName
    }
}