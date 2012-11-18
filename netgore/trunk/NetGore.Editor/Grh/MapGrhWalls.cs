using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using NetGore.Collections;
using NetGore.Graphics;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Editor.Grhs
{
    /// <summary>
    /// Contains information on what WallEntities are bound to the given MapGrh
    /// </summary>
    public class MapGrhWalls
    {
        readonly Dictionary<GrhIndex, List<WallEntityBase>> _walls = new Dictionary<GrhIndex, List<WallEntityBase>>();

        public void Clear()
        {
            _walls.Clear();
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
                List<WallEntityBase> ret;
                if (!_walls.TryGetValue(index, out ret))
                    return null;
                return ret;
            }
            set 
            {
                _walls[index] = value;
            }
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
            foreach (var mgTmp in mapGrhs)
            {
                var mg = mgTmp;

                if (mg.Grh == null || mg.Grh.GrhData == null)
                    continue;

                // Grab the List for the given MapGrh
                var mgWalls = this[mg.Grh.GrhData];
                if (mgWalls == null)
                    continue;

                // Create a new instance of each of the walls and add it to the return List
                foreach (var wallTmp in mgWalls)
                {
                    var wall = wallTmp;

                    var newWallEntity = wall.DeepCopy();
                    newWallEntity.Size = wall.Size != Vector2.Zero ? wall.Size * mg.Scale : mg.Size;
                    newWallEntity.Position = mg.Position + (wall.Position * mg.Scale);
                    newWallEntity.BoundGrhIndex = mg.Grh.GrhData.GrhIndex;
                    newWallEntity.IsPlatform = wall.IsPlatform;
                    ret.Add(newWallEntity);
                }
            }

            return ret;
        }
    }
}