using System.Linq;
using NetGore;
using NetGore.World;

namespace DemoGame.Server
{
    /// <summary>
    /// Interface for a DynamicEntity that can respawn on the map after they die.
    /// </summary>
    public interface IRespawnable
    {
        /// <summary>
        /// Gets the DynamicEntity to respawn (typically just "this").
        /// </summary>
        DynamicEntity DynamicEntity { get; }

        /// <summary>
        /// Checks if the IRespawnable is ready to be respawned. If the object is already spawned, this should
        /// still return true.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>True if the IRespawnable is ready to respawn, else false.</returns>
        bool ReadyToRespawn(TickCount currentTime);

        /// <summary>
        /// Handles respawning the DynamicEntity. This must also take care of setting the Map.
        /// </summary>
        void Respawn();
    }
}