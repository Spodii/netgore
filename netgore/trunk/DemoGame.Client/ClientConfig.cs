using System.Linq;

namespace DemoGame.Client
{
    /// <summary>
    /// The compile-time configuration for the client. This should mostly just contain consts used by the client that are related
    /// to performance. They are all grouped together here to make tweaking for performance easier.
    /// Actual game configuration and configuration used by more than just the client go into <see cref="GameData"/>.
    /// </summary>
    public static class ClientConfig
    {
        /// <summary>
        /// How frequently, in milliseconds, the game time is re-acquired from the server.
        /// Synchronizing only helps with dealing with systems that have an internal clock that runs too slow or too fast.
        /// Default is 10 minutes.
        /// </summary>
        public const int SyncGameTimeFrequency = 1000 * 60 * 10;
    }
}