using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Client;
using DemoGame.Server.Queries;
using NetGore.Db;
using NetGore.IO;

namespace DemoGame.Editor
{
    public static class MapHelper
    {
        /// <summary>
        /// Saves a map completely and fully.
        /// </summary>
        /// <param name="map">The map to save.</param>
        public static void SaveMap(Map map)
        {
            // Add the MapGrh-bound walls
            var extraWalls = GlobalState.Instance.MapGrhWalls.CreateWallList(map.MapGrhs);
            foreach (var wall in extraWalls)
            {
                map.AddEntity(wall);
            }

            // Save the map
            map.Save(ContentPaths.Dev, MapEditorDynamicEntityFactory.Instance);

            // Update the database
            GlobalState.Instance.DbController.GetQuery<InsertMapQuery>().Execute(map);

            // Pull the MapGrh-bound walls back out
            foreach (var wall in extraWalls)
            {
                map.RemoveEntity(wall);
            }
        }
    }
}
