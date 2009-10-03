using System.Collections.Generic;
using System.IO;
using System.Linq;
using DemoGame;
using DemoGame.Client;
using NetGore;
using NetGore.Collections;
using NetGore.Graphics;
using NetGore.IO;

namespace DemoGame.MapEditor
{
    /// <summary>
    /// Contains information on what WallEntities are bound to the given MapGrh
    /// </summary>
    public class MapGrhWalls
    {
        const string _grhIndexValueKey = "GrhData";
        const string _rootNodeName = "GrhDataWalls";
        const string _wallsNodeName = "Walls";

        /// <summary>
        /// Array containing a list of WallEntities for each valid GrhData by index
        /// </summary>
        readonly DArray<List<WallEntityBase>> _walls = new DArray<List<WallEntityBase>>(false);

        /// <summary>
        /// Gets or sets the List of WallEntities for the given GrhData
        /// </summary>
        /// <param name="index">Index of the GrhData to get/set the walls for</param>
        /// <returns>List of the WallEntities for the given GrhData, or null if the GrhData is
        /// invalid or no bound walls exist for it</returns>
        public List<WallEntityBase> this[GrhIndex index]
        {
            get
            {
                if (!_walls.CanGet((int)index))
                    return null;
                return _walls[(int)index];
            }
            set { _walls[(int)index] = value; }
        }

        /// <summary>
        /// Gets or sets the List of WallEntities for the given GrhData
        /// </summary>
        /// <param name="gd">GrhData to get/set the walls for</param>
        /// <returns>List of the WallEntities for the given GrhData, or null if the GrhData is
        /// invalid or does not exist</returns>
        public List<WallEntityBase> this[GrhData gd]
        {
            get { return this[gd.GrhIndex]; }
            set { this[gd.GrhIndex] = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrhWalls"/> class.
        /// </summary>
        /// <param name="contentPath">The content path.</param>
        public MapGrhWalls(ContentPaths contentPath)
        {
            Load(contentPath);
        }

        /// <summary>
        /// Creates a List of the WallEntities used for a set of MapGrhs
        /// </summary>
        /// <param name="mapGrhs">Set of MapGrhs to get the walls for</param>
        /// <returns>List of each bound wall for all the MapGrhs</returns>
        public List<WallEntityBase> CreateWallList(IEnumerable<MapGrh> mapGrhs)
        {
            var ret = new List<WallEntityBase>();

            // Iterate through the requested MapGrhs
            foreach (MapGrh mg in mapGrhs)
            {
                // Grab the List for the given MapGrh
                var mgWalls = this[mg.Grh.GrhData];
                if (mgWalls != null)
                {
                    // Create a new instance of each of the walls and add it to the return List
                    foreach (WallEntityBase wall in mgWalls)
                    {
                        WallEntity w = new WallEntity(mg.Destination + wall.Position, wall.CB.Size, wall.CollisionType);
                        ret.Add(w);
                    }
                }
            }

            return ret;
        }

        public static string GetFilePath(ContentPaths contentPath)
        {
            return contentPath.Data.Join("grhdatawalls.xml");
        }

        public void Load(ContentPaths contentPath)
        {
            string filePath = GetFilePath(contentPath);

            // Clear the old walls in case this isn't the first load
            _walls.Clear();

            if (!File.Exists(filePath))
                return;

            // Read the values
            XmlValueReader reader = new XmlValueReader(filePath, _rootNodeName);
            var loadedWalls = reader.ReadManyNodes<KeyValuePair<GrhIndex, List<WallEntityBase>>>(_rootNodeName, ReadWalls);

            foreach (var wall in loadedWalls)
            {
                _walls[(int)wall.Key] = wall.Value;
            }
        }

        static KeyValuePair<GrhIndex, List<WallEntityBase>> ReadWalls(IValueReader r)
        {
            GrhIndex grhIndex = r.ReadGrhIndex(_grhIndexValueKey);
            var walls = r.ReadManyNodes<WallEntityBase>(_wallsNodeName, x => new WallEntity(x));

            return new KeyValuePair<GrhIndex, List<WallEntityBase>>(grhIndex, walls.ToList());
        }

        public void Save(ContentPaths contentPath)
        {
            string filePath = GetFilePath(contentPath);

            // Build up a list of the valid walls, and join them with their GrhIndex
            var wallsToWrite = new List<KeyValuePair<GrhIndex, List<WallEntityBase>>>(_walls.Count);
            for (int i = 0; i < _walls.Length; i++)
            {
                var walls = _walls[i];
                if (walls == null || walls.Count == 0)
                    continue;

                var kvp = new KeyValuePair<GrhIndex, List<WallEntityBase>>(new GrhIndex(i), walls);
                wallsToWrite.Add(kvp);
            }

            // Write the values
            using (XmlValueWriter writer = new XmlValueWriter(filePath, _rootNodeName))
            {
                writer.WriteManyNodes(_rootNodeName, wallsToWrite, WriteWalls);
            }
        }

        static void WriteWalls(IValueWriter w, KeyValuePair<GrhIndex, List<WallEntityBase>> item)
        {
            GrhIndex grhIndex = item.Key;
            var walls = item.Value;

            w.Write(_grhIndexValueKey, grhIndex);
            w.WriteManyNodes(_wallsNodeName, walls, ((pwriter, pitem) => pitem.Write(pwriter)));
        }
    }
}