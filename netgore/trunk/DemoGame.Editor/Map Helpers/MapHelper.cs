using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Server.Queries;
using NetGore.Editor.EditorTool;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Editor.WinForms;
using NetGore.Graphics;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;
using ToolBar = NetGore.Editor.EditorTool.ToolBar;

namespace DemoGame.Editor
{
    public static class MapHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the currently active EditorMap being edited, if any.
        /// </summary>
        public static EditorMap ActiveMap
        {
            get
            {
                var tb = ToolBar.GetToolBar(ToolBarVisibility.Map);
                if (tb == null)
                    return null;

                var map = tb.DisplayObject as EditorMap;
                if (map == null || map.IsDisposed)
                    return null;

                return map;
            }
        }

        /// <summary>
        /// Goes through each map and removes invalid GrhData references.
        /// </summary>
        public static List<KeyValuePair<MapID, int>> RemoveInvalidGrhDatasFromMaps()
        {
            var ret = new List<KeyValuePair<MapID, int>>();

            var grhIndexes = new HashSet<int>(GrhInfo.GrhDatas.Select(x => (int)x.GrhIndex));

            foreach (var mapId in FindAllMaps().Select(x => x.ID))
            {
                // Load the map
                EditorMap map = new EditorMap(mapId, new Camera2D(new Vector2(800, 600)), GetTimeDummy.Instance);
                map.Load(ContentPaths.Dev, true, EditorDynamicEntityFactory.Instance);
                RemoveBoundWalls(map);

                // Remove invalid
                var removed = RemoveInvalidGrhDatasFromMap(map, grhIndexes);

                if (removed > 0)
                {
                    // Save
                    SaveMap(map, false);
                    SaveMiniMap(map.ID, ContentPaths.Dev.Grhs + "MiniMap");
                    ret.Add(new KeyValuePair<MapID, int>(mapId, removed));
                }
            }

            return ret;
        }

        /// <summary>
        /// Removes the invalid GrhData references in a map.
        /// </summary>
        /// <param name="map">The map to remove the invalid GrhData references from.</param>
        /// <returns>Number of MapGrhs removed.</returns>
        public static int RemoveInvalidGrhDatasFromMap(EditorMap map)
        {
            return RemoveInvalidGrhDatasFromMap(map, new HashSet<int>(GrhInfo.GrhDatas.Select(x => (int)x.GrhIndex)));
        }

        /// <summary>
        /// Removes the invalid GrhData references in a map.
        /// </summary>
        /// <param name="map">The map to remove the invalid GrhData references from.</param>
        /// <param name="grhIndexes">HashSet of the GrhIndexes that exist.</param>
        /// <returns>Number of MapGrhs removed.</returns>
        static int RemoveInvalidGrhDatasFromMap(EditorMap map, HashSet<int> grhIndexes)
        {
            int removed = 0;

            foreach (MapGrh mg in map.Spatial.GetMany<MapGrh>().Distinct().ToArray())
            {
                if (mg.Grh == null || mg.Grh.GrhData == null || !grhIndexes.Contains((int)mg.Grh.GrhData.GrhIndex))
                {
                    // Remove
                    map.Spatial.Remove(mg);
                    ++removed;
                }
            }

            return removed;
        }

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
                using (var map = new EditorMap(id, new Camera2D(new Vector2(800, 600)), GetTimeDummy.Instance) { Name = "New map" })
                {
                    map.SetDimensions(new Vector2(960, 960));
                    SaveMap(map, false);
                    SaveMiniMap(map.ID, ContentPaths.Dev.Grhs + "MiniMap");
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

        static void DeleteMap(MapID mapId, DeleteMapQuery deleteMapQuery)
        {
            try
            {
                // Delete file
                string filePathDev = MapBase.GetMapFilePath(ContentPaths.Dev, mapId);
                if (File.Exists(filePathDev))
                    File.Delete(filePathDev);
            
                try
                {
                    string filePathBuild = MapBase.GetMapFilePath(ContentPaths.Build, mapId);
                    if (File.Exists(filePathBuild))
                        File.Delete(filePathBuild);
                }
                catch   
                {
                }

                // Delete from db
                deleteMapQuery.Execute(mapId);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to delete mapId `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, mapId, ex);
                Debug.Fail(string.Format(errmsg, mapId, ex));

                throw;
            }
        }

        /// <summary>
        /// Deletes a map.
        /// </summary>
        /// <param name="map">The map to delete.</param>
        /// <param name="showConfirmation">If true, a confirmation will be shown to make sure the user wants to
        /// perform this operation.</param>
        /// <returns>True if the map was deleted successfully; otherwise false.</returns>
        public static bool DeleteMap(EditorMap map, bool showConfirmation = true)
        {
            try
            {
                if (map == null)
                    return false;

                // Show the confirmation message
                if (showConfirmation)
                {
                    const string confirmMsg = "Are you sure you wish to delete map `{0}`? This cannot be undone!";
                    if (MessageBox.Show(string.Format(confirmMsg, map), "Delete map?", MessageBoxButtons.YesNo) == DialogResult.No)
                        return false;
                }
                
                // If a MapScreenControl is open for this map, set the map on it to null then dispose of the control
                var msc = MapScreenControl.TryFindInstance(map);
                if (msc != null)
                {
                    msc.Map = null;
                    msc.Dispose();
                }

                DeleteMap(map.ID, GlobalState.Instance.DbController.GetQuery<DeleteMapQuery>());

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
                return false;
            }

            return true;
        }

        static void DeleteMiniMap(MapID mapId)
        {
            try
            {
                // Delete file
                string filePathDev = MapBase.GetMiniMapFilePath(ContentPaths.Dev, mapId);
                if (File.Exists(filePathDev))
                    File.Delete(filePathDev);

                try
                {
                    string filePathBuild = MapBase.GetMiniMapFilePath(ContentPaths.Build, mapId);
                    if (File.Exists(filePathBuild))
                        File.Delete(filePathBuild);
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to delete the mini-map for mapId `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, mapId, ex);
                Debug.Fail(string.Format(errmsg, mapId, ex));

                throw;
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
        /// Removes the MapGrh bound walls from a map.
        /// </summary>
        public static void RemoveBoundWalls(EditorMap map)
        {
            var toRemove = map.Spatial.GetMany<WallEntityBase>(x => x.BoundGrhIndex > 0).Distinct().ToArray();
            foreach (var wall in toRemove)
                map.RemoveEntity(wall);
        }

        /// <summary>
        /// Checks if the given map differs from the copy saved on disk.
        /// </summary>
        public static bool DiffersFromSaved(EditorMap map)
        {
            // Add the MapGrh-bound walls
            var mapGrhs = map.Spatial.GetMany<MapGrh>().Distinct();
            var extraWalls = GlobalState.Instance.MapGrhWalls.CreateWallList(mapGrhs);
            foreach (var wall in extraWalls)
            {
                map.AddEntity(wall);
            }

            bool differs;
            try
            {
                // Compare
                differs = map.DiffersFromSaved(ContentPaths.Dev, EditorDynamicEntityFactory.Instance);
            }
            finally
            {
                // Pull the MapGrh-bound walls back out
                foreach (var wall in extraWalls)
                {
                    map.RemoveEntity(wall);
                }
            }

            return differs;
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

                try
                {
                    // Save the map
                    map.Save(ContentPaths.Dev, EditorDynamicEntityFactory.Instance);
                    SaveMiniMap(map.ID, ContentPaths.Dev.Grhs + "MiniMap");

                    // Update the database
                    GlobalState.Instance.DbController.GetQuery<InsertMapQuery>().Execute(map);
                }
                finally
                {
                    // Pull the MapGrh-bound walls back out
                    foreach (var wall in extraWalls)
                    {
                        Debug.Assert(wall.BoundGrhIndex > 0);
                        map.RemoveEntity(wall);
                    }
                }

                // Save successful
                const string savedMsg = "Successfully saved the changes to map id {0}: {1}";
                MainForm.SetStatusMessage(string.Format(savedMsg, map.ID, map.Name ?? string.Empty));
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
        /// Saves a map as a new mini-map.
        /// </summary>
        /// <param name="mapID">The map ID to save.</param>
        /// <param name="folder">The name of the folder to save to.</param>
        public static void SaveMiniMap(MapID mapID, string folder)
        {
            var tb = ToolBar.GetToolBar(ToolBarVisibility.Map);
            var tbItems = tb.Items;
            tbItems["Map Preview"].PerformClick();
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

                int newSetID;
                if (!int.TryParse(InputBox.Show("Save as...", "Save map ID as:", newID.ToString()), out newSetID))
                    return;

                if (!MapBase.MapIDExists((MapID)newSetID))
                {
                    newID = ((MapID)newSetID);

                    // Confirm save
                    if (showConfirmation)
                    {
                        const string confirmMsg = "Are you sure you wish to save map `{0}` as a new map (with ID `{1}`)?";
                        if (MessageBox.Show(string.Format(confirmMsg, map, newID), "Save map as?", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }
                }
                else
                {
                    const string confirmMsgOverWrite = "Are you sure you wish to save map `{0}` and overwrite map (with ID `{1}`)?";
                    if (MessageBox.Show(string.Format(confirmMsgOverWrite, map.ID, newSetID), "Save map as?", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;

                    newID = ((MapID)newSetID);
                }

                // Change the map ID
                map.ChangeID(newID);

                // Save
                SaveMap(map, false);
                SaveMiniMap(map.ID, ContentPaths.Dev.Grhs + "MiniMap");
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to save map `{0}` as a new map. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, map, ex);
                Debug.Fail(string.Format(errmsg, map, ex));
            }
        }

        /// <summary>
        /// Finds the MapIds for maps that exist in the database but not on file, or vise versa.
        /// </summary>
        public static MapID[] FindMissingMapIds()
        {
            // Get ids from file & db
            MapID[] idsFromFile = MapBase.GetUsedMapIds(ContentPaths.Dev);
            MapID[] idsFromDb = FindAllMaps().Select(x => x.ID).ToArray();

            return idsFromFile.NotIntersect(idsFromDb).ToArray();
        }

        /// <summary>
        /// Finds and deletes the MapIds for maps that exist in the database but not on file, or vise versa.
        /// </summary>
        public static MapID[] DeleteMissingMapIds(bool showConfirmation = true)
        {
            MapID[] missingIds = FindMissingMapIds().OrderBy(x => x).ToArray();

            if (missingIds.Length == 0)
                return new MapID[0];

            if (showConfirmation)
            {
                const string confirmMsgDelete = "The following MapIds exist on file but not in the database, or vise versa." + 
                    " This normally will happen because you deleted or moved the map files directory instead of through the editor." + 
                    " Do you wish to delete them completely?{0}{0}{1}";
                if (MessageBox.Show(string.Format(confirmMsgDelete, Environment.NewLine, string.Join(", ", missingIds)), "Delete incomplete maps", MessageBoxButtons.YesNo) == DialogResult.No)
                    return new MapID[0];
            }
            
            var deleteMapQuery = GlobalState.Instance.DbController.GetQuery<DeleteMapQuery>();

            // Delete
            List<MapID> deletedIds = new List<MapID>();
            foreach (MapID id in missingIds)
            {
                try
                {
                    DeleteMap(id, deleteMapQuery);
                    deletedIds.Add(id);
                    DeleteMiniMap(id);
                }
                catch
                {
                }
            }

            MainForm.SetStatusMessage(string.Format("Deleted {0} missing maps: {1}", deletedIds.Count, string.Join(", ", deletedIds)));

            return deletedIds.ToArray();
        }
    }
}