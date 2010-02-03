using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    /// <summary>
    /// The settings for the server. This should mostly just contain consts used by the server that are related
    /// to performance. They are all grouped together here to make tweaking for performance easier.
    /// Actual game settings and settings used by more than just the server go into <see cref="GameData"/>.
    /// </summary>
    public static class ServerSettings
    {
        /// <summary>
        /// How often, in milliseconds, to wait between calls to <see cref="User.SynchronizeExtraUserInformation"/>.
        /// Lower values will result in smaller delays for certain things (such as changes to stats and inventory)
        /// to update, but require more overhead.
        /// </summary>
        public const int SyncExtraUserInformationRate = 150;

        /// <summary>
        /// How often, in milliseconds, to wait between calls to <see cref="World.UpdateRespawnables"/>.
        /// Lower values will result in <see cref="IRespawnable"/>s respawning closer to their desired time, but
        /// will require more overhead.
        /// </summary>
        public const int RespawnablesUpdateRate = 800;
    }
}
