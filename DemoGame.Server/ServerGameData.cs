using System;
using System.Linq;
using DemoGame.Server.Properties;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Server
{
    /// <summary>
    /// Same as <see cref="GameData"/>, but for information only available on the server.
    /// </summary>
    public static class ServerGameData
    {
        /// <summary>
        /// Gets the default amount of money a Character will pay for buying the given <paramref name="item"/> from
        /// a shop.
        /// </summary>
        /// <param name="item">The item to purchase.</param>
        /// <returns>the default amount of money a Character will pay for buying the given <paramref name="item"/>
        /// from a shop.</returns>
        public static int GetItemBuyValue(ItemEntity item)
        {
            return item.Value;
        }

        /// <summary>
        /// Gets the default amount of money a Character will get for selling the given <paramref name="item"/> to
        /// a shop.
        /// </summary>
        /// <param name="item">The item to sell.</param>
        /// <returns>the default amount of money a Character will get for selling the given <paramref name="item"/>
        /// to a shop.</returns>
        public static int GetItemSellValue(ItemEntity item)
        {
            return Math.Max(item.Value / 2, 1);
        }

        /// <summary>
        /// Gets the map and position to spawn a <see cref="User"/> after they have been killed.
        /// </summary>
        /// <param name="user">The <see cref="User"/> to respawn.</param>
        /// <param name="mapID">The ID of the map to spawn the <paramref name="user"/> on.</param>
        /// <param name="position">The position to spawn the <paramref name="user"/> at.</param>
        public static void GetUserRespawnPosition(User user, out MapID mapID, out Vector2 position)
        {
            // Grab the position/map from the user's properties
            if (user.RespawnMapID.HasValue)
            {
                var map = user.World.GetMap(user.RespawnMapID.Value);
                var mapPos = user.RespawnPosition;
                if (map != null && map.Size.X > mapPos.X && map.Size.Y > mapPos.Y && mapPos.X >= 0 && mapPos.Y >= 0)
                {
                    mapID = user.RespawnMapID.Value;
                    position = user.RespawnPosition;
                    return;
                }
            }

            // Failed to grab valid value from user's properties. Use fallback values.
            mapID = ServerSettings.Default.InvalidUserLoadMap;
            position = ServerSettings.Default.InvalidUserLoadPosition;
        }
    }
}