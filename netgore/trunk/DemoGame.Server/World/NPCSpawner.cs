using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using log4net;
using Microsoft.Xna.Framework;

namespace DemoGame.Server
{
    /// <summary>
    /// Uses the information from a <see cref="MapSpawnValues"/> to spawn non-persistent NPCs on a Map.
    /// </summary>
    public class NPCSpawner
    {
        static readonly Random _rnd = new Random();

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly byte _amount;

        readonly Rectangle _area;
        readonly CharacterTemplate _characterTemplate;
        readonly Map _map;

        /// <summary>
        /// Gets the number of Characters being spawned by this NPCSpawner.
        /// </summary>
        public byte Amount
        {
            get { return _amount; }
        }

        /// <summary>
        /// Gets the area that the spawning takes place in.
        /// </summary>
        public Rectangle Area
        {
            get { return _area; }
        }

        /// <summary>
        /// Gets the CharacterTemplate used for the Characters spawned.
        /// </summary>
        public CharacterTemplate CharacterTemplate
        {
            get { return _characterTemplate; }
        }

        /// <summary>
        /// Gets the Map that this NPCSpawner is on.
        /// </summary>
        public Map Map
        {
            get { return _map; }
        }

        /// <summary>
        /// NPCSpawner constructor.
        /// </summary>
        /// <param name="mapSpawnValues">The MapSpawnValues containing the values to use to create this NPCSpawner.</param>
        /// <param name="map">The Map instance to do the spawning on. The MapIndex of this Map must be equal to the
        /// MapIndex of the <paramref name="mapSpawnValues"/>.</param>
        /// <exception cref="ArgumentException">The <paramref name="map"/>'s MapIndex does not match the
        /// <paramref name="mapSpawnValues"/>'s MapIndex.</exception>
        public NPCSpawner(IMapSpawnTable mapSpawnValues, Map map)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            if (mapSpawnValues == null)
                throw new ArgumentNullException("mapSpawnValues");

            if (map.Index != mapSpawnValues.MapID)
                throw new ArgumentException("The map's MapIndex and mapSpawnValues's MapIndex do not match.", "map");

            _map = map;
            _characterTemplate = CharacterTemplateManager.GetTemplate(mapSpawnValues.CharacterTemplateID);
            _amount = mapSpawnValues.Amount;
            _area = new MapSpawnRect(mapSpawnValues).ToRectangle(map);

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
        /// Loads the NPCSpawners for a Map.
        /// </summary>
        /// <param name="map">Map to load the spawners for.</param>
        /// <returns>IEnumerable of the NPCSpawners that were loaded.</returns>
        public static IEnumerable<NPCSpawner> LoadSpawners(Map map)
        {
            var queryValues = MapSpawnValues.Load(map.DBController, map.Index);
            var ret = new List<NPCSpawner>();

            foreach (MapSpawnValues queryValue in queryValues)
            {
                NPCSpawner spawner = new NPCSpawner(queryValue, map);
                ret.Add(spawner);
            }

            return ret;
        }

        /// <summary>
        /// Handles when a NPC handled by this NPCSpawner is killed.
        /// </summary>
        /// <param name="character">The NPC that was killed.</param>
        void NPC_OnKilled(Character character)
        {
            Debug.Assert(character is NPC, "How is this not a NPC?!");
            Debug.Assert(!character.IsDisposed, "Uhm, why did somebody dispose my precious little map-spawned NPC? :(");

            Vector2 respawnPos = RandomSpawnPosition();

            character.RespawnMapIndex = _map.Index;
            character.RespawnPosition = respawnPos;
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
    }
}