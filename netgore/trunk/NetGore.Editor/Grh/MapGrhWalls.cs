using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using NetGore.Collections;
using NetGore.Graphics;
using NetGore.IO;
using NetGore.World;

namespace NetGore.Editor.Grhs
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
        /// Initializes a new instance of the <see cref="MapGrhWalls"/> class.
        /// </summary>
        /// <param name="contentPath">The content path.</param>
        /// <param name="createWall">Delegate describing how to create the <see cref="WallEntityBase"/>
        /// from an <see cref="IValueReader"/>.</param>
        public MapGrhWalls(ContentPaths contentPath, CreateWallEntityFromReaderHandler createWall)
        {
            Load(contentPath, createWall);

            // Listen for the addition and removal of walls
            GrhInfo.AddedNew += DeleteGrhDataWalls;
            GrhInfo.Removed += DeleteGrhDataWalls;
        }

        /// <summary>
        /// Gets or sets the List of WallEntities for the given GrhData
        /// </summary>
        /// <param name="index">Index of the GrhData to get/set the walls for</param>
        /// <returns>List of the WallEntities for the given GrhData, or null if the GrhData is
        /// invalid or no bound walls exist for it</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
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
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public List<WallEntityBase> this[GrhData gd]
        {
            get { return this[gd.GrhIndex]; }
            set { this[gd.GrhIndex] = value; }
        }

        /// <summary>
        /// Creates a List of the <see cref="WallEntityBase"/>s used for a set of <see cref="MapGrh"/>s.
        /// </summary>
        /// <param name="mapGrhs">Set of <see cref="MapGrh"/>s to get the walls for</param>
        /// <returns>List of each bound wall for all the <see cref="MapGrh"/>s.</returns>
        public IEnumerable<WallEntityBase> CreateWallList(IEnumerable<MapGrh> mapGrhs)
        {
            var ret = new List<WallEntityBase>();

            // Iterate through the requested MapGrhs
            foreach (var mg in mapGrhs)
            {
                // Grab the List for the given MapGrh
                var mgWalls = this[mg.Grh.GrhData];
                if (mgWalls == null)
                    continue;

                // Create a new instance of each of the walls and add it to the return List
                foreach (var wall in mgWalls)
                {
                    var newWallEntity = wall.DeepCopy();
                    newWallEntity.Position = mg.Position + wall.Position;
                    ret.Add(newWallEntity);
                }
            }

            return ret;
        }

        /// <summary>
        /// Handles when a <see cref="GrhData"/> is removed or a new <see cref="GrhData"/> is added, and deletes
        /// the walls for it.
        /// </summary>
        /// <param name="sender">The <see cref="GrhData"/> that the event is related to.</param>
        void DeleteGrhDataWalls(GrhData sender)
        {
            RemoveWalls(sender);
        }

        public static string GetFilePath(ContentPaths contentPath)
        {
            return contentPath.Data.Join("grhdatawalls" + EngineSettings.DataFileSuffix);
        }

        public void Load(ContentPaths contentPath, CreateWallEntityFromReaderHandler createWall)
        {
            var filePath = GetFilePath(contentPath);

            // Clear the old walls in case this isn't the first load
            _walls.Clear();

            if (!File.Exists(filePath))
                return;

            // Read the values
            var reader = XmlValueReader.CreateFromFile(filePath, _rootNodeName);
            var loadedWalls = reader.ReadManyNodes(_rootNodeName, x => ReadWalls(x, createWall));

            foreach (var wall in loadedWalls)
            {
                _walls[(int)wall.Key] = wall.Value;
            }
        }

        static KeyValuePair<GrhIndex, List<WallEntityBase>> ReadWalls(IValueReader r, CreateWallEntityFromReaderHandler createWall)
        {
            var grhIndex = r.ReadGrhIndex(_grhIndexValueKey);
            var walls = r.ReadManyNodes(_wallsNodeName, x => createWall(x));

            return new KeyValuePair<GrhIndex, List<WallEntityBase>>(grhIndex, walls.ToList());
        }

        /// <summary>
        /// Removes all the walls for a <see cref="GrhData"/>.
        /// </summary>
        /// <param name="grhData">The <see cref="GrhData"/> to remove the walls for.</param>
        public void RemoveWalls(GrhData grhData)
        {
            var index = (int)grhData.GrhIndex;

            if (!_walls.CanGet(index))
                return;

            _walls.RemoveAt(index);
        }

        public void Save(ContentPaths contentPath)
        {
            var filePath = GetFilePath(contentPath);

            // Build up a list of the valid walls, and join them with their GrhIndex
            var wallsToWrite = new List<KeyValuePair<GrhIndex, List<WallEntityBase>>>(_walls.Count);
            for (var i = 0; i < _walls.Length; i++)
            {
                var walls = _walls[i];
                if (walls == null || walls.Count == 0)
                    continue;

                var kvp = new KeyValuePair<GrhIndex, List<WallEntityBase>>(new GrhIndex(i), walls);
                wallsToWrite.Add(kvp);
            }

            // Write the values
            using (var writer = XmlValueWriter.Create(filePath, _rootNodeName))
            {
                writer.WriteManyNodes(_rootNodeName, wallsToWrite, WriteWalls);
            }
        }

        static void WriteWalls(IValueWriter w, KeyValuePair<GrhIndex, List<WallEntityBase>> item)
        {
            var grhIndex = item.Key;
            var walls = item.Value;

            w.Write(_grhIndexValueKey, grhIndex);
            w.WriteManyNodes(_wallsNodeName, walls, ((pwriter, pitem) => pitem.Write(pwriter)));
        }
    }
}