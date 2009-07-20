using System;
using System.Collections.Generic;
using System.Diagnostics;
using DemoGame.Server.Queries;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains and internally synchronizes to the database the values used to specify how Characters spawn on a Map.
    /// </summary>
    public class MapSpawnValues
    {
        readonly DBController _dbController;
        readonly MapSpawnValuesID _id;

        CharacterTemplateID _characterTemplateID;
        MapIndex _mapIndex;
        byte _npcAmount;
        MapSpawnRect _spawnArea;

        /// <summary>
        /// Gets or sets the CharacterTemplateID of the CharacterTemplate to spawn.
        /// </summary>
        public CharacterTemplateID CharacterTemplateID
        {
            get { return _characterTemplateID; }
            set
            {
                if (_characterTemplateID == value)
                    return;

                _characterTemplateID = value;
                UpdateDB();
            }
        }

        /// <summary>
        /// Gets the DBController used to synchronize changes to the values.
        /// </summary>
        public DBController DBController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Gets the unique ID of this MapSpawnValues.
        /// </summary>
        public MapSpawnValuesID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets or sets the index of the Map that these values are for.
        /// </summary>
        public MapIndex MapIndex
        {
            get { return _mapIndex; }
            set
            {
                if (_mapIndex == value)
                    return;

                _mapIndex = value;
                UpdateDB();
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of Characters that will be spawned.
        /// </summary>
        public byte NPCAmount
        {
            get { return _npcAmount; }
            set
            {
                if (_npcAmount == value)
                    return;

                _npcAmount = value;
                UpdateDB();
            }
        }

        /// <summary>
        /// Gets the area on the map the spawning will take place at.
        /// </summary>
        public MapSpawnRect SpawnArea
        {
            get { return _spawnArea; }
            private set
            {
                _spawnArea = value;
                UpdateDB();
            }
        }

        /// <summary>
        /// MapSpawnValues constructor.
        /// </summary>
        /// <param name="dbController">The DBController used to synchronize changes to the values.</param>
        /// <param name="id">The unique ID of this MapSpawnValues.</param>
        /// <param name="mapIndex">The index of the Map that these values are for.</param>
        /// <param name="characterTemplateID">The CharacterTemplateID of the CharacterTemplate to spawn.</param>
        public MapSpawnValues(DBController dbController, MapSpawnValuesID id, MapIndex mapIndex, CharacterTemplateID characterTemplateID)
            : this(dbController, id, mapIndex, characterTemplateID, new MapSpawnRect(null, null, null, null))
        {
        }

        /// <summary>
        /// MapSpawnValues constructor.
        /// </summary>
        /// <param name="dbController">The DBController used to synchronize changes to the values.</param>
        /// <param name="v">The SelectMapSpawnQueryValues containing the values to use.</param>
        MapSpawnValues(DBController dbController, SelectMapSpawnQueryValues v)
            : this(dbController, v.ID, v.MapIndex, v.CharacterTemplateID, v.MapSpawnRect)
        {
        }

        /// <summary>
        /// MapSpawnValues constructor.
        /// </summary>
        /// <param name="dbController">The DBController used to synchronize changes to the values.</param>
        /// <param name="id">The unique ID of this MapSpawnValues.</param>
        /// <param name="mapIndex">The index of the Map that these values are for.</param>
        /// <param name="characterTemplateID">The CharacterTemplateID of the CharacterTemplate to spawn.</param>
        /// <param name="spawnRect">The area on the map the spawning will take place at.</param>
        MapSpawnValues(DBController dbController, MapSpawnValuesID id, MapIndex mapIndex, CharacterTemplateID characterTemplateID,
                       MapSpawnRect spawnRect)
        {
            _dbController = dbController;
            _id = id;
            _mapIndex = mapIndex;
            _characterTemplateID = characterTemplateID;
            _spawnArea = spawnRect;
        }

        /// <summary>
        /// Loads a MapSpawnValues from the database.
        /// </summary>
        /// <param name="dbController">DBController used to communicate with the database.</param>
        /// <param name="id">ID of the MapSpawnValues to load.</param>
        /// <returns>The MapSpawnValues with ID <paramref name="id"/>.</returns>
        public static MapSpawnValues Load(DBController dbController, MapSpawnValuesID id)
        {
            SelectMapSpawnQueryValues values = dbController.GetQuery<SelectMapSpawnQuery>().Execute(id);
            Debug.Assert(id == values.ID);
            return new MapSpawnValues(dbController, values);
        }

        /// <summary>
        /// Loads all of the MapSpawnValues for the given <paramref name="mapIndex"/> from the database.
        /// </summary>
        /// <param name="dbController">DBController used to communicate with the database.</param>
        /// <param name="mapIndex">Index of the map to load the MapSpawnValues for.</param>
        /// <returns>An IEnumerable of all of the MapSpawnValues for the given <paramref name="mapIndex"/>.</returns>
        public static IEnumerable<MapSpawnValues> Load(DBController dbController, MapIndex mapIndex)
        {
            var ret = new List<MapSpawnValues>();
            var queryValues = dbController.GetQuery<SelectMapSpawnsQuery>().Execute(mapIndex);

            foreach (SelectMapSpawnQueryValues v in queryValues)
            {
                Debug.Assert(v.MapIndex == mapIndex);
                ret.Add(new MapSpawnValues(dbController, v));
            }

            return ret;
        }

        /// <summary>
        /// Sets the spawn area for this MapSpawnValues.
        /// </summary>
        /// <param name="map">Instance of the Map with the MapIndex equal to the MapIndex handled by this MapSpawnValues.
        /// This is to ensure that the <paramref name="newSpawnArea"/> given is in a valid map range.</param>
        /// <param name="newSpawnArea">New MapSpawnRect values.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="newSpawnArea"/> contains one or more
        /// values that are not in range of the <paramref name="map"/>.</exception>
        public void SetSpawnArea(Map map, MapSpawnRect newSpawnArea)
        {
            if (newSpawnArea == SpawnArea)
                return;

            ushort x = newSpawnArea.X.HasValue ? newSpawnArea.X.Value : (ushort)0;
            ushort y = newSpawnArea.Y.HasValue ? newSpawnArea.Y.Value : (ushort)0;

            if (x < 0)
                throw new ArgumentOutOfRangeException("newSpawnArea");

            if (y < 0)
                throw new ArgumentOutOfRangeException("newSpawnArea");

            if (newSpawnArea.Width.HasValue && (x + newSpawnArea.Width.Value) > map.Width)
                throw new ArgumentOutOfRangeException("newSpawnArea");

            if (newSpawnArea.Height.HasValue && (y + newSpawnArea.Height.Value) > map.Height)
                throw new ArgumentOutOfRangeException("newSpawnArea");

            SpawnArea = newSpawnArea;
        }

        /// <summary>
        /// Updates the MapSpawnValues in the database.
        /// </summary>
        void UpdateDB()
        {
            _dbController.GetQuery<UpdateMapSpawnQuery>().Execute(this);
        }
    }
}