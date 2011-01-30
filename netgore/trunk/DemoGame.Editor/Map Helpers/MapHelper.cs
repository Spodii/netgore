using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace DemoGame.Editor
{
    public static class MapHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Creates a new map.
        /// </summary>
        /// <param name="showConfirmation">If true, a confirmation will be shown to make sure the user wants to
        /// perform this operation.</param>
        /// <returns>The <see cref="MapID"/> of the newly created map, or null if no map was created.</returns>
        public static MapID? CreateNewMap(bool showConfirmation = true)
        {
            try
            {
                // Show the confirmation message
                if (showConfirmation)
                {
                    const string confirmMsg = "Are you sure you wish to create a new map?";
                    if (MessageBox.Show(confirmMsg, "Create new map?", MessageBoxButtons.YesNo) == DialogResult.No)
                        return null;
                }

                // Get the map ID to use
                var id = MapBase.GetNextFreeIndex(ContentPaths.Dev);

                // Create the map and save it
                using (
                    var map = new EditorMap(id, new Camera2D(new Vector2(800, 600)), GetTimeDummy.Instance) { Name = "New map" })
                {
                    map.SetDimensions(new Vector2(960, 960));
                    SaveMap(map, false);
                }

                return id;
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to create a new map. Exception: {0}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, ex);
                Debug.Fail(string.Format(errmsg, ex));
            }

            return null;
        }

        /// <summary>
        /// Deletes a map.
        /// </summary>
        /// <param name="map">The map to delete.</param>
        /// <param name="showConfirmation">If true, a confirmation will be shown to make sure the user wants to
        /// perform this operation.</param>
        public static void DeleteMap(EditorMap map, bool showConfirmation = true)
        {
            try
            {
                if (map == null)
                    return;

                // Show the confirmation message
                if (showConfirmation)
                {
                    const string confirmMsg = "Are you sure you wish to delete map `{0}`? This cannot be undone!";
                    if (MessageBox.Show(string.Format(confirmMsg, map), "Delete map?", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }

                // Delete the map file
                var path = MapBase.GetMapFilePath(ContentPaths.Dev, map.ID);
                if (File.Exists(path))
                    File.Delete(path);

                // Update the database
                GlobalState.Instance.DbController.GetQuery<DeleteMapQuery>().Execute(map.ID);

                // Delete successful
                if (showConfirmation)
                {
                    const string deletedMsg = "Successfully deleted map `{0}`!";
                    MessageBox.Show(string.Format(deletedMsg, map), "Map deleted", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to delete map `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, map, ex);
                Debug.Fail(string.Format(errmsg, map, ex));
            }
        }

        /// <summary>
        /// Gets the <see cref="IMapTable"/>s for all of the maps.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> to use. If null, will attempt to find
        /// the <see cref="IDbController"/> instance automatically.</param>
        /// <returns>
        /// The <see cref="IMapTable"/>s for all of the maps.
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="dbController"/> is null and no valid <see cref="IDbController"/>
        /// instance could be found automatically.</exception>
        public static IEnumerable<IMapTable> FindAllMaps(IDbController dbController = null)
        {
            if (dbController == null)
                dbController = DbControllerBase.GetInstance();

            if (dbController == null)
                throw new ArgumentException("Param was null and could not find a valid IDbController instance.", "dbController");

            // Get the IDs
            var ids = dbController.GetQuery<SelectMapIDsQuery>().Execute();

            // Get all of the maps one at a time using the IDs
            var ret = new List<IMapTable>();
            var templateQuery = dbController.GetQuery<SelectMapQuery>();
            foreach (var id in ids)
            {
                var template = templateQuery.Execute(id);
                ret.Add(template);
            }

            // Return the results sorted
            return ret.OrderBy(x => x.ID).ToImmutable();
        }

        /// <summary>
        /// Gets the NPC spawns on a map.
        /// </summary>
        /// <param name="mapID">The ID of the map to get the spawns for.</param>
        /// <returns>The NPC spawns for the given <paramref name="mapID"/>.</returns>
        public static IEnumerable<IMapSpawnTable> GetSpawns(MapID mapID)
        {
            var dbController = DbControllerBase.GetInstance();
            var q = dbController.GetQuery<SelectMapSpawnsOnMapQuery>();
            var spawns = q.Execute(mapID);
            return spawns;
        }

        /// <summary>
        /// Saves a map.
        /// </summary>
        /// <param name="map">The map to save.</param>
        /// <param name="showConfirmation">If true, a confirmation will be shown to make sure the user wants to
        /// perform this operation.</param>
        public static void SaveMap(EditorMap map, bool showConfirmation = true)
        {
            try
            {
                if (map == null)
                    return;

                // Show confirmation
                if (showConfirmation)
                {
                    const string confirmMsg = "Are you sure you wish to save the changes to map `{0}`?";
                    if (MessageBox.Show(string.Format(confirmMsg, map), "Save map?", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;

                    // Allow specifying a new map ID
                    bool firstEnterIDAttempt = true;
                    while (true)
                    {
                        string displayText = "Save as map id:";
                        if (!firstEnterIDAttempt)
                            displayText = "Invalid map id entered. Please enter a numeric value greater than 0." + Environment.NewLine + Environment.NewLine + displayText;

                        firstEnterIDAttempt = false;

                        int newId;
                        if (int.TryParse(InputBox.Show("Save as...", displayText, map.ID.ToString()), out newId))
                        {
                            if (newId >= 0)
                            {
                                try
                                {
                                    map.ChangeID((MapID)newId);
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(string.Format("Failed to change map ID to `{0}`. Exception: {1}", newId, ex));
                                }
                            }
                        }
                    }
                }

                // Add the MapGrh-bound walls
                var mapGrhs = map.Spatial.GetMany<MapGrh>().Distinct();
                var extraWalls = GlobalState.Instance.MapGrhWalls.CreateWallList(mapGrhs);
                foreach (var wall in extraWalls)
                {
                    map.AddEntity(wall);
                }

                // Save the map
                map.Save(ContentPaths.Dev, EditorDynamicEntityFactory.Instance);

                // Update the database
                GlobalState.Instance.DbController.GetQuery<InsertMapQuery>().Execute(map);

                // Pull the MapGrh-bound walls back out
                foreach (var wall in extraWalls)
                {
                    map.RemoveEntity(wall);
                }

                // Save successful
                if (showConfirmation)
                {
                    const string savedMsg = "Successfully saved the changes to map `{0}`!";
                    MessageBox.Show(string.Format(savedMsg, map), "Map saved", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to save map `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, map, ex);
                Debug.Fail(string.Format(errmsg, map, ex));
            }
        }

        /// <summary>
        /// Saves a map as a new map.
        /// </summary>
        /// <param name="map">The map to save.</param>
        /// <param name="showConfirmation">If true, a confirmation will be shown to make sure the user wants to
        /// perform this operation.</param>
        public static void SaveMapAs(EditorMap map, bool showConfirmation = true)
        {
            try
            {
                if (map == null)
                    return;

                var newID = MapBase.GetNextFreeIndex(ContentPaths.Dev);

                var newSetID = 0;
                if (int.TryParse(InputBox.Show("Save as...", "Save map ID as:", newID.ToString()), out newSetID))
                {
                    if (!MapBase.MapIDExists((MapID)newSetID))
                    {
                        newID = ((MapID)newSetID);
                        // Confirm save

                        if (showConfirmation)
                        {
                            const string confirmMsg = "Are you sure you wish to save map `{0}` as a new map (with ID `{1}`)?";
                            if (MessageBox.Show(string.Format(confirmMsg, map, newID), "Save map as?", MessageBoxButtons.YesNo) ==
                                DialogResult.No)
                                return;
                        }
                    }
                    else
                    {
                        const string confirmMsgOverWrite =
                            "Are you sure you wish to save map `{0}` and overwrite map (with ID `{1}`)?";
                        if (
                            MessageBox.Show(string.Format(confirmMsgOverWrite, map.ID, newSetID), "Save map as?",
                                MessageBoxButtons.YesNo) == DialogResult.No)
                            return;

                        newID = ((MapID)newSetID);
                    }
                }

                // Change the map ID
                map.ChangeID(newID);

                // Save
                SaveMap(map, false);

                // Save successful
                if (showConfirmation)
                {
                    const string savedMsg = "Successfully saved the map `{0}` as a new map!";
                    MessageBox.Show(string.Format(savedMsg, map), "Map successfully saved as a new map", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to save map `{0}` as a new map. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, map, ex);
                Debug.Fail(string.Format(errmsg, map, ex));
            }
        }
    }
}