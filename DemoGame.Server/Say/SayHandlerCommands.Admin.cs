using System;
using System.Linq;
using DemoGame.Server.AI;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// <see cref="SayHandlerCommands"/> for <see cref="UserPermissions.Admin"/>.
    /// </summary>
    public partial class SayHandlerCommands
    {
        /// <summary>
        /// Creates a new map instance and places the user on that map.
        /// </summary>
        /// <param name="mapID">The ID of the map to create the instance of.</param>
        [SayHandlerCommand("CreateMapInstance", UserPermissions.Admin)]
        public void CreateMapInstance(MapID mapID)
        {
            // Check for a valid map
            if (!MapBase.MapIDExists(mapID))
            {
                UserChat("Invalid map ID: " + mapID);
                return;
            }

            // Try to create the map
            MapInstance instance;
            try
            {
                instance = new MapInstance(mapID, World);
            }
            catch (Exception ex)
            {
                UserChat("Failed to create instance: " + ex);
                return;
            }

            // Add the user to the map
            User.Teleport(instance, new Vector2(50, 50));
        }

        /// <summary>
        /// Leaves an instanced map if the map the user is on is for an instanced map. The user is warped
        /// to their respawn position.
        /// </summary>
        [SayHandlerCommand("LeaveMapInstance", UserPermissions.Admin)]
        public void LeaveMapInstance()
        {
            // Check for a valid map
            if (User.Map == null || !User.Map.IsInstanced)
            {
                UserChat("You must be on an instanced map to do that.");
                return;
            }

            // Get the map to respawn on
            var mapID = User.RespawnMapID;
            Map map = null;

            if (mapID.HasValue)
                map = World.GetMap(mapID.Value);

            if (map == null)
            {
                UserChat("Could not teleport you to your respawn location - your respawn map is null for some reason...");
                return;
            }

            // Teleport to respawn map/position
            User.Teleport(map, User.RespawnPosition);
        }

        /// <summary>
        /// Toggles the AI.
        /// </summary>
        [SayHandlerCommand("ToggleAI", UserPermissions.Admin)]
        public void ToggleAI()
        {
            AISettings.AIDisabled = !AISettings.AIDisabled;

            UserChat("AI has been {0}.", AISettings.AIDisabled ? "disabled" : "enabled");
        }
    }
}