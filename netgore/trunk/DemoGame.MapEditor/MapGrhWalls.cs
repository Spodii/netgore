using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using DemoGame.Client;
using DemoGame.Extensions;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    /// <summary>
    /// Contains information on what WallEntities are bound to the given MapGrh
    /// </summary>
    public class MapGrhWalls
    {
        /// <summary>
        /// Array containing a list of WallEntities for each valid GrhData by index
        /// </summary>
        readonly DArray<List<WallEntity>> _walls = new DArray<List<WallEntity>>(false);

        /// <summary>
        /// Gets or sets the List of WallEntities for the given GrhData
        /// </summary>
        /// <param name="index">Index of the GrhData to get/set the walls for</param>
        /// <returns>List of the WallEntities for the given GrhData, or null if the GrhData is
        /// invalid or no bound walls exist for it</returns>
        public List<WallEntity> this[int index]
        {
            get
            {
                if (!_walls.CanGet(index))
                    return null;
                return _walls[index];
            }
            set { _walls[index] = value; }
        }

        /// <summary>
        /// Gets or sets the List of WallEntities for the given GrhData
        /// </summary>
        /// <param name="gd">GrhData to get/set the walls for</param>
        /// <returns>List of the WallEntities for the given GrhData, or null if the GrhData is
        /// invalid or does not exist</returns>
        public List<WallEntity> this[GrhData gd]
        {
            get { return this[gd.GrhIndex]; }
            set { this[gd.GrhIndex] = value; }
        }

        /// <summary>
        /// MapGrhWalls constructor
        /// </summary>
        /// <param name="path">Path to the file containing the bound wall information</param>
        public MapGrhWalls(string path)
        {
            Load(path);
        }

        /// <summary>
        /// Creates a List of the WallEntities used for a set of MapGrhs
        /// </summary>
        /// <param name="mapGrhs">Set of MapGrhs to get the walls for</param>
        /// <returns>List of each bound wall for all the MapGrhs</returns>
        public List<WallEntity> CreateWallList(IEnumerable<MapGrh> mapGrhs)
        {
            var ret = new List<WallEntity>();

            // Iterate through the requested MapGrhs
            foreach (MapGrh mg in mapGrhs)
            {
                // Grab the List for the given MapGrh
                var mgWalls = this[mg.Grh.GrhData.GrhIndex];
                if (mgWalls != null)
                {
                    // Create a new instance of each of the walls and add it to the return List
                    foreach (WallEntity wall in mgWalls)
                    {
                        WallEntity w = Entity.Create<Wall>(mg.Destination + wall.Position, wall.CB.Width, wall.CB.Height);
                        w.CollisionType = wall.CollisionType;
                        ret.Add(w);
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Loads the bound wall information for MapGrhs
        /// </summary>
        /// <param name="path">Path to the file containing the bound wall information</param>
        public void Load(string path)
        {
            if (!File.Exists(path))
                throw new FileLoadException("File requested for the MapGrhWalls does not exist.", path);

            // Clear the old walls in case this isn't the first load
            _walls.Clear();

            // Grab the wall information
            var wallInfoFile = XmlInfoReader.ReadFile(path, true);

            // Store the type for the CollisionType since we will be using it quite a bit
            Type typeofCT = typeof(CollisionType);

            // Iterate through each entry in the WallInfo file
            foreach (var wallInfo in wallInfoFile)
            {
                // Get the list information
                ushort grhIndex = ushort.Parse(wallInfo["Wall.GrhData"]);
                int wallCount = int.Parse(wallInfo["Wall.WallCount"]);
                var walls = new List<WallEntity>(wallCount);
                _walls[grhIndex] = walls;

                // Create the individual walls
                for (int i = 0; i < wallCount; i++)
                {
                    CollisionType ct = (CollisionType)Enum.Parse(typeofCT, wallInfo["Wall.Wall" + i + ".Type"]);
                    float x = float.Parse(wallInfo["Wall.Wall" + i + ".X"]);
                    float y = float.Parse(wallInfo["Wall.Wall" + i + ".Y"]);
                    float w = float.Parse(wallInfo["Wall.Wall" + i + ".W"]);
                    float h = float.Parse(wallInfo["Wall.Wall" + i + ".H"]);

                    WallEntity newWall = Entity.Create<Wall>(new Vector2(x, y), w, h);
                    newWall.CollisionType = ct;
                    walls.Add(newWall);
                }
            }
        }

        /// <summary>
        /// Saves the bound wall information for MapGrhs
        /// </summary>
        /// <param name="path">Path to the file to save to</param>
        public void Save(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter w = XmlWriter.Create(stream, settings))
                {
                    if (w == null)
                        throw new Exception("Failed to create XmlWriter for saving MapGrhWalls.");

                    // Begin
                    w.WriteStartDocument();
                    w.WriteStartElement("Walls");

                    // List of walls
                    for (int i = 0; i < _walls.Length; i++)
                    {
                        var walls = _walls[i];

                        // Ignore if there are no walls
                        if (walls == null || walls.Count == 0)
                            continue;

                        w.WriteStartElement("Wall");
                        w.WriteAttributeString("GrhData", i.ToString());
                        w.WriteAttributeString("WallCount", walls.Count.ToString());

                        // Individual walls
                        int wallIndex = 0;
                        foreach (WallEntity wall in walls)
                        {
                            w.WriteStartElement("Wall" + wallIndex);
                            w.WriteAttributeString("Type", wall.CollisionType.ToString());
                            w.WriteAttributeString("X", wall.Position.X.ToString());
                            w.WriteAttributeString("Y", wall.Position.Y.ToString());
                            w.WriteAttributeString("W", wall.CB.Width.ToString());
                            w.WriteAttributeString("H", wall.CB.Height.ToString());
                            w.WriteEndElement();
                            wallIndex++;
                        }

                        w.WriteEndElement();
                    }

                    // End
                    w.WriteEndElement();
                    w.WriteEndDocument();
                    w.Flush();
                }
            }
        }
    }
}