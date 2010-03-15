using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using log4net;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Uses the information from a <see cref="MapSpawnValues"/> to spawn non-persistent NPCs on a Map.
    /// </summary>
    public class NPCSpawner
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly CharacterTemplateManager _characterTemplateManager = CharacterTemplateManager.Instance;
        static readonly Random _rnd = new Random();

        readonly byte _amount;
        readonly Rectangle _area;
        readonly CharacterTemplate _characterTemplate;
        readonly Map _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCSpawner"/> class.
        /// </summary>
        /// <param name="mapSpawnValues">The MapSpawnValues containing the values to use to create this NPCSpawner.</param>
        /// <param name="map">The Map instance to do the spawning on. The <see cref="MapID"/> of this Map must be equal to the
        /// <see cref="MapID"/> of the <paramref name="mapSpawnValues"/>.</param>
        /// <exception cref="ArgumentException">The <paramref name="map"/>'s <see cref="MapID"/> does not match the
        /// <paramref name="mapSpawnValues"/>'s <see cref="MapID"/>.</exception>
        public NPCSpawner(IMapSpawnTable mapSpawnValues, Map map)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            if (mapSpawnValues == null)
                throw new ArgumentNullException("mapSpawnValues");

            if (map.ID != mapSpawnValues.MapID)
                throw new ArgumentException("The map's MapID and mapSpawnValues's MapID do not match.", "map");

            _map = map;
            _characterTemplate = _characterTemplateManager[mapSpawnValues.CharacterTemplateID];
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
        /// Gets the number of Characters being spawned by this <see cref="NPCSpawner"/>.
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
        /// Gets the <see cref="CharacterTemplate"/> used for the Characters spawned by this <see cref="NPCSpawner"/>.
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
        /// Loads the NPCSpawners for a Map.
        /// </summary>
        /// <param name="map">Map to load the spawners for.</param>
        /// <returns>IEnumerable of the <see cref="NPCSpawner"/>s that were loaded.</returns>
        public static IEnumerable<NPCSpawner> LoadSpawners(Map map)
        {
            var queryValues = MapSpawnValues.Load(map.DbController, map.ID);
            var ret = new List<NPCSpawner>();

            foreach (MapSpawnValues queryValue in queryValues)
            {
                NPCSpawner spawner = new NPCSpawner(queryValue, map);
                ret.Add(spawner);
            }

            return ret;
        }

        /// <summary>
        /// Handles when a NPC handled by this <see cref="NPCSpawner"/> is killed.
        /// </summary>
        /// <param name="character">The NPC that was killed.</param>
        void NPC_Killed(Character character)
        {
            Debug.Assert(character is NPC, "How is this not a NPC?!");
            Debug.Assert(!character.IsDisposed, "Uhm, why did somebody dispose my precious little map-spawned NPC? :(");

            Vector2 respawnPos = RandomSpawnPosition();

            character.RespawnMapID = _map.ID;
            character.RespawnPosition = respawnPos;
        }

        /// <summary>
        /// Gets a random position that fits inside the <see cref="NPCSpawner.Area"/>.
        /// </summary>
        /// <returns>A random position that fits inside the <see cref="NPCSpawner.Area"/>.</returns>
        Vector2 RandomSpawnPosition()
        {
            int x = _rnd.Next(Area.Left, Area.Right + 1);
            int y = _rnd.Next(Area.Top, Area.Bottom + 1);

            return new Vector2(x, y);
        }

        /// <summary>
        /// Spawns the NPCs for this <see cref="NPCSpawner"/>.
        /// </summary>
        void SpawnNPCs()
        {
            for (int i = 0; i < Amount; i++)
            {
                Vector2 pos = RandomSpawnPosition();
                NPC npc = new NPC(_map.World, _characterTemplate, _map, pos);
                npc.Killed += NPC_Killed;
            }
        }
    }
}