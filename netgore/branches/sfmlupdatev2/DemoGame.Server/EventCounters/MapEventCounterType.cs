using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// The different events for the <see cref="EventCounterManager.Map"/>.
    /// </summary>
    public enum MapEventCounterType : byte
    {
        /// <summary>
        /// A user has was added to the map.
        /// </summary>
        UserAdded = 0,

        /// <summary>
        /// A NPC was added to the map.
        /// </summary>
        NPCAdded = 1,

        /// <summary>
        /// An item was added to the map.
        /// </summary>
        ItemAdded = 2,

        /// <summary>
        /// A spawned NPC (from an <see cref="NPCSpawner"/>) from the map was killed.
        /// </summary>
        SpawnedNPCKilled = 3,

        /// <summary>
        /// A non-spawned NPC was killed on the map.
        /// </summary>
        NPCKilled = 4,

        /// <summary>
        /// A user was killed on the map.
        /// </summary>
        UserKilled = 5,

        /// <summary>
        /// A NPC from the <see cref="NPCSpawner"/> spawned on the map.
        /// </summary>
        NPCSpawned = 6,

        /// <summary>
        /// Amount of experience <see cref="User"/>s have gained from killing while on this map.
        /// </summary>
        UserGainedKillExp = 7,

        /// <summary>
        /// Amount of cash <see cref="User"/>s have gained from killing while on this map.
        /// </summary>
        UserGainedKillCash = 8,
    }
}