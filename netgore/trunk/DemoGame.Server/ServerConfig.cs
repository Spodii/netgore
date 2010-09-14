using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// The compile-time configuration for the server. This should mostly just contain consts used by the server that are related
    /// to performance. They are all grouped together here to make tweaking for performance easier.
    /// Actual game configuration and configuration used by more than just the server go into <see cref="GameData"/>.
    /// </summary>
    public static class ServerConfig
    {
        /// <summary>
        /// If remote connections are allowed. By default, this is set to false, which will only allow connections
        /// made locally (from the same computer). If set to true, you must make sure that the <see cref="GameData.ServerPort"/>
        /// allow incoming connections and can be listened on. If you have a firewall, you need to add an exception in it
        /// to allow this. If you are behind a router, forward these ports (for UDP) to the local IP
        /// address of the machine hosting the server process in the network.
        /// </summary>
        public const bool AllowRemoteConnections = false;

        /// <summary>
        /// The maximum accounts that can be created for a single IP address over a given period of time. The period
        /// of time is defined by the query itself (CountRecentlyCreatedAccounts).
        /// </summary>
        public const int MaxRecentlyCreatedAccounts = 4;

        /// <summary>
        /// The maximum number of connections allowed for a single IP address. Set to a value less than or equal to 0 to disable.
        /// </summary>
        public const int MaxConnectionsPerIP = 6;

        /// <summary>
        /// The maximum number of connections that can be made to the server.
        /// </summary>
        public const int MaxConnections = 100;

        /// <summary>
        /// The amount of time an item may remain on the map before it is removed automatically.
        /// </summary>
        public const int DefaultMapItemLife = 1000 * 60 * 3;

        /// <summary>
        /// The minimum amount of time in milliseconds that may elapse between checks for expired items.
        /// The lower this value, the closer the time the items are removed will be to the
        /// actual sepcified time, but the greater the performance cost. It is recommended to keep this
        /// value greater than at least 10 seconds to avoid unneccesary performance overhead.
        /// </summary>
        public const int MapItemExpirationUpdateRate = 1000 * 30;

        /// <summary>
        /// The maximum allowed distance allowed between two group members for them to be allowed to share rewards
        /// with the other group members.
        /// </summary>
        public const float MaxGroupShareDistance = 1000;

        /// <summary>
        /// How often, in milliseconds, to wait between check to respawn <see cref="IRespawnable"/> entities.
        /// Lower values will result in <see cref="IRespawnable"/>s respawning closer to their desired time, but
        /// will require more overhead.
        /// </summary>
        public const int RespawnablesUpdateRate = 800;

        /// <summary>
        /// How frequently, in milliseconds, that the server will save the world state. The lower this value, the less
        /// the server will "roll-back" when it crashes. World saves can be expensive since it is done all at once,
        /// so it is recommended to keep this value relatively high unless you are experiencing frequently crashes.
        /// </summary>
        public const int RoutineServerSaveRate = 1000 * 60 * 60 * 10; // 10 minutes

        /// <summary>
        /// Millisecond rate at which the server updates. The server update rate does not affect the rate
        /// at which physics is update, so modifying the update rate will not affect the game
        /// speed. Server update rate is used to determine how frequently the server checks
        /// for performing updates and how long it is able to "sleep". It is recommended
        /// a high update rate is used to allow for more precise updating.
        /// </summary>
        public const int ServerUpdateRate = 10; // 100 FPS

        /// <summary>
        /// How often, in milliseconds, to wait between calls to <see cref="User.SynchronizeExtraUserInformation"/>.
        /// Lower values will result in smaller delays for certain things (such as changes to stats and inventory)
        /// to update, but require more overhead.
        /// </summary>
        public const int SyncExtraUserInformationRate = 150;

        /// <summary>
        /// The map to use for a persistent <see cref="NPC"/> who does not have a valid <see cref="MapID"/> to use for their
        /// loading position. This should point to an isolated region not accessible by players. Ideally, no NPC will ever
        /// end up here.
        /// </summary>
        /// <seealso cref="InvalidPersistentNPCLoadPosition"/>
        public static readonly MapID InvalidPersistentNPCLoadMap = new MapID(1);

        /// <summary>
        /// The position to use for a persistent <see cref="NPC"/> who does not have a valid <see cref="MapID"/> to use for their
        /// loading position. This should point to an isolated region not accessible by players. Ideally, no NPC will ever
        /// end up here.
        /// </summary>
        /// <seealso cref="InvalidPersistentNPCLoadMap"/>
        public static readonly Vector2 InvalidPersistentNPCLoadPosition = new Vector2(10, 10);

        /// <summary>
        /// The map to use for a <see cref="User"/> who does not have a valid <see cref="MapID"/> or position to use for their
        /// position position.
        /// </summary>
        /// <seealso cref="InvalidUserLoadPosition"/>
        public static readonly MapID InvalidUserLoadMap = new MapID(1);

        /// <summary>
        /// The position to use for a <see cref="User"/> who does not have a valid <see cref="MapID"/> or position to use for their
        /// position position.
        /// </summary>
        /// <seealso cref="InvalidUserLoadMap"/>
        public static readonly Vector2 InvalidUserLoadPosition = new Vector2(765, 45);

        /// <summary>
        /// The <see cref="ItemTemplateID"/> that represents the template of the item used for attacking when
        /// no weapon is specified (see: <see cref="World.UnarmedWeapon"/>).
        /// </summary>
        public static readonly ItemTemplateID UnarmedItemTemplateID = new ItemTemplateID(0);

    }
}