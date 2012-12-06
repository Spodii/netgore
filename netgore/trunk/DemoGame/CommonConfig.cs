using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Compile-time settings shared by the server and client. This should mostly just contain consts used by both the 
    /// server and the client that are related to performance. They are all grouped together here to make tweaking for
    /// performance easier. Actual game settings go into <see cref="GameData"/>.
    /// </summary>
    public static class CommonConfig
    {
        /// <summary>
        /// The number of seconds of non-response before disconnecting because of time out.
        /// </summary>
        public const float ConnectionTimeout = 8;

        /// <summary>
        /// The string used to identify this application over the network. The actual string isn't too important, but it is recommended
        /// you keep it relatively short. Only applications with the same identifier string will be able to connect to one
        /// another.
        /// </summary>
        public const string NetworkAppIdentifier = "NetGore";

        /// <summary>
        /// The number of seconds between performing pings (which are used to determine the latency of the connection).
        /// </summary>
        public const float PingInterval = 6;

        /// <summary>
        /// The port that the server listens on, and that the client uses when connecting to the server.
        /// </summary>
        public const int ServerPort = 44447;

        /// <summary>
        /// Holds the string to pass to <see cref="ContentPaths.TryCopyContent"/>.
        /// </summary>
        public const string TryCopyContentArgs = "--clean=\"[Engine,Font,Fx,Grh,Languages,Maps,Music,Skeletons,Sounds]\"";
    }
}