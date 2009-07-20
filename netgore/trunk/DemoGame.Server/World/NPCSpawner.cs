using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Uses the information from a <see cref="MapSpawnValues"/> to spawn NPCs on a Map.
    /// </summary>
    public class NPCSpawner
    {
        static readonly Random _rnd = new Random();

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly Map _map;

        /// <summary>
        /// Gets the Map that this NPCSpawner is on.
        /// </summary>
        public Map Map { get { return _map; } }

        readonly CharacterTemplate _characterTemplate;

        /// <summary>
        /// Gets the CharacterTemplate used for the Characters spawned.
        /// </summary>
        public CharacterTemplate CharacterTemplate { get { return _characterTemplate; } }

        readonly byte _amount;

        /// <summary>
        /// Gets the number of Characters being spawned by this NPCSpawner.
        /// </summary>
        public byte Amount { get { return _amount; } }

        readonly Rectangle _area;

        /// <summary>
        /// Gets the area that the spawning takes place in.
        /// </summary>
        public Rectangle Area { get { return _area; } }

        /// <summary>
        /// NPCSpawner constructor.
        /// </summary>
        /// <param name="mapSpawnValues">The MapSpawnValues containing the values to use to create this NPCSpawner.</param>
        /// <param name="map">The Map instance to do the spawning on. The MapIndex of this Map must be equal to the
        /// MapIndex of the <paramref name="mapSpawnValues"/>.</param>
        /// <exception cref="ArgumentException">The <paramref name="map"/>'s MapIndex does not match the
        /// <paramref name="mapSpawnValues"/>'s MapIndex.</exception>
        public NPCSpawner(MapSpawnValues mapSpawnValues, Map map)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            if (mapSpawnValues == null)
                throw new ArgumentNullException("mapSpawnValues");

            if (map.Index != mapSpawnValues.MapIndex)
                throw new ArgumentException("The map's MapIndex and mapSpawnValues's MapIndex do not match.", "map");

            _map = map;
            _characterTemplate = CharacterTemplateManager.GetTemplate(mapSpawnValues.CharacterTemplateID);
            _amount = mapSpawnValues.SpawnAmount;
            _area = mapSpawnValues.SpawnArea.ToRectangle(map);

            if (_characterTemplate == null)
            {
                const string errmsg = "Failed to find the CharacterTemplate for CharacterTemplateID `{0}`.";
                string err = string.Format(errmsg, mapSpawnValues.CharacterTemplateID);
                if (log.IsFatalEnabled)
                    log.Fatal(err);
                Debug.Fail(err);
                throw new Exception(err);
            }

            SpawnNPCs();
        }

        /// <summary>
        /// Gets a random position that fits inside this NPCSpawner's Area.
        /// </summary>
        /// <returns>A random position that fits inside this NPCSpawner's Area.</returns>
        Vector2 RandomSpawnPosition()
        {
            int x = _rnd.Next(Area.Left, Area.Right + 1);
            int y = _rnd.Next(Area.Top, Area.Bottom + 1);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Spawns the NPCs for this NPCSpawner.
        /// </summary>
        void SpawnNPCs()
        {
            for (int i = 0; i < Amount; i++)
            {
                Vector2 pos = RandomSpawnPosition();
                NPC npc = new NPC(_map.World, _characterTemplate, _map, pos);
                npc.OnKilled += NPC_OnKilled;
            }
        }

        /// <summary>
        /// Handles when a NPC handled by this NPCSpawner is killed.
        /// </summary>
        /// <param name="character">The NPC that was killed.</param>
        void NPC_OnKilled(Character character)
        {
            Vector2 respawnPos = RandomSpawnPosition();

            character.RespawnMapIndex = _map.Index;
            character.RespawnPosition = respawnPos;
        }
    }
}
