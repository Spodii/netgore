using System.Linq;

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
        /// If remote connections are allowed. By default, this is set to false, which will only allow connections
        /// made locally (from the same computer). If set to true, you must make sure that the needed ports defined by
        /// <see cref="GameData.ServerPort"/> and <see cref="GameData.ServerPingPort"/> allow incoming connections
        /// and can be listened on. If you have a firewall, you need to add an exception in it to allow this. If you are
        /// behind a router, forward these ports (for both TCP and UDP) to the local IP address of the machine hosting
        /// the server process in the network.
        /// </summary>
        public const bool AllowRemoteConnections = true;

        /// <summary>
        /// The maximum allowed distance allowed between two group members for them to be allowed to share rewards
        /// with the other group members.
        /// </summary>
        public const float MaxGroupShareDistance = 1000;

        /// <summary>
        /// How often, in milliseconds, to wait between calls to <see cref="World.UpdateRespawnables"/>.
        /// Lower values will result in <see cref="IRespawnable"/>s respawning closer to their desired time, but
        /// will require more overhead.
        /// </summary>
        public const int RespawnablesUpdateRate = 800;

        /// <summary>
        /// How often, in milliseconds, to wait between calls to <see cref="User.SynchronizeExtraUserInformation"/>.
        /// Lower values will result in smaller delays for certain things (such as changes to stats and inventory)
        /// to update, but require more overhead.
        /// </summary>
        public const int SyncExtraUserInformationRate = 150;

        /// <summary>
        /// The <see cref="ItemTemplateID"/> that represents the template of the item used for attacking when
        /// no weapon is specified (see: <see cref="World.UnarmedWeapon"/>).
        /// </summary>
        public static readonly ItemTemplateID UnarmedItemTemplateID = new ItemTemplateID(0);
    }
}