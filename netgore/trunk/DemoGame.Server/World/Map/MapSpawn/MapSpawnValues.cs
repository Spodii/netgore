using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.World;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains and internally synchronizes to the database the values used to specify how Characters spawn on a Map.
    /// </summary>
    public class MapSpawnValues : IMapSpawnTable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly MapSpawnValuesID _id;

        CharacterTemplateID _characterTemplateID;
        IDbController _dbController;
        MapID _mapID;
        byte _spawnAmount;
        MapSpawnRect _spawnArea;
        private Direction _spawnDirection;
        ushort _spawnRespawn;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSpawnValues"/> class.
        /// </summary>
        /// <param name="dbController">The IDbController used to synchronize changes to the values.</param>
        /// <param name="mapID">The index of the Map that these values are for.</param>
        /// <param name="characterTemplateID">The CharacterTemplateID of the CharacterTemplate to spawn.</param>
        public MapSpawnValues(IDbController dbController, MapID mapID, CharacterTemplateID characterTemplateID)
            : this(dbController, GetFreeID(dbController), mapID, characterTemplateID, 1, new MapSpawnRect(null, null, null, null), Direction.None, 5)
        {
            DbController.GetQuery<InsertMapSpawnQuery>().Execute(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSpawnValues"/> class.
        /// </summary>
        /// <param name="dbController">The IDbController used to synchronize changes to the values.</param>
        /// <param name="v">The IMapSpawnTable containing the values to use.</param>
        MapSpawnValues(IDbController dbController, IMapSpawnTable v)
            : this(dbController, v.ID, v.MapID, v.CharacterTemplateID, v.Amount, new MapSpawnRect(v), v.DirectionId, v.Respawn)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapSpawnValues"/> class.
        /// </summary>
        /// <param name="dbController">The DbController used to synchronize changes to the values.</param>
        /// <param name="id">The unique ID of this MapSpawnValues.</param>
        /// <param name="mapID">The index of the Map that these values are for.</param>
        /// <param name="characterTemplateID">The CharacterTemplateID of the CharacterTemplate to spawn.</param>
        /// <param name="spawnAmount">The maximum number of Characters that will be spawned by this MapSpawnValues.</param>
        /// <param name="spawnRect">The area on the map the spawning will take place at.</param>
        /// <param name="spawnDirection">The direction on this map to spawn the NPCs.</param>
        /// <param name="spawnRespawn">The time it takes for an NPC to spawn.</param>
        MapSpawnValues(IDbController dbController, MapSpawnValuesID id, MapID mapID, CharacterTemplateID characterTemplateID,
                       byte spawnAmount, MapSpawnRect spawnRect, Direction spawnDirection, ushort spawnRespawn)
        {
            _dbController = dbController;
            _id = id;
            _mapID = mapID;
            _characterTemplateID = characterTemplateID;
            _spawnAmount = spawnAmount;
            _spawnArea = spawnRect;
            _spawnDirection = spawnDirection;
            _spawnRespawn = spawnRespawn;
        }

        /// <summary>
        /// Gets the IDbController used to synchronize changes to the values.
        /// </summary>
        [Browsable(false)]
        public IDbController DbController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Gets or sets the ID of the Map that these values are for.
        /// </summary>
        [Browsable(false)]
        public MapID MapID
        {
            get { return _mapID; }
            set
            {
                if (_mapID == value)
                    return;

                _mapID = value;
                UpdateDB();
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of Characters that will be spawned by this MapSpawnValues.
        /// </summary>
        [Browsable(true)]
        [DisplayName("Amount")]
        [Description("The maximum number of Characters that will be spawned by this MapSpawnValues.")]
        public byte SpawnAmount
        {
            get { return _spawnAmount; }
            set
            {
                if (_spawnAmount == value)
                    return;

                _spawnAmount = value;
                UpdateDB();
            }
        }

        /// <summary>
        /// Gets or sets the area on the map the spawning will take place at.
        /// When possible, its best to use <see cref="SetSpawnArea"/> to ensure the spawn area is validated.
        /// </summary>
        [Browsable(true)]
        [DisplayName("Area")]
        [Description("The area on the map the spawning will take place at.")]
        public MapSpawnRect SpawnArea
        {
            get { return _spawnArea; }
            set
            {
                _spawnArea = value;
                UpdateDB();
            }
        }

        /// <summary>
        /// Gets or sets the time in seconds it takes for the Character to respawn.
        /// </summary>
        [Browsable(true)]
        [DisplayName("Respawn")]
        [Description("How long in seconds to wait after death to be respawned.")]
        public ushort SpawnRespawn
        {
            get { return _spawnRespawn; }
            set
            {
                if (_spawnRespawn == value)
                    return;

                _spawnRespawn = value;
                UpdateDB();
            }
        }

        /// <summary>
        /// Gets or sets the direction of the spawn that will occur here.
        /// Set to null for a random direction.
        /// </summary>
        [Browsable(true)]
        [DisplayName("Spawn Direction")]
        [Description("The direction this mob should spawn in, set to null for a random direction")]
        [DefaultValue(null)]
        public Direction SpawnDirection
        {
            get { return _spawnDirection; }
            set
            {
                _spawnDirection = value;
                UpdateDB();
            }
        }

        /// <summary>
        /// Deletes the MapSpawnValues from the database. After this is called, this MapSpawnValues must be treated
        /// as disposed and not be used at all!
        /// </summary>
        public void Delete()
        {
            if (DbController == null)
            {
                const string errmsg = "Called Delete() on `{0}` when the DbController was already null. Likely already deleted.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Deleting MapSpawnValues `{0}`.", this);

            var id = ID;
            DbController.GetQuery<DeleteMapSpawnQuery>().Execute(id);
            DbController.GetQuery<MapSpawnValuesIDCreator>().FreeID(id);

            _dbController = null;
        }

        static MapSpawnValuesID GetFreeID(IDbController dbController)
        {
            return dbController.GetQuery<MapSpawnValuesIDCreator>().GetNext();
        }

        /// <summary>
        /// Loads a MapSpawnValues from the database.
        /// </summary>
        /// <param name="dbController">DbController used to communicate with the database.</param>
        /// <param name="id">ID of the MapSpawnValues to load.</param>
        /// <returns>The MapSpawnValues with ID <paramref name="id"/>.</returns>
        public static MapSpawnValues Load(IDbController dbController, MapSpawnValuesID id)
        {
            var values = dbController.GetQuery<SelectMapSpawnQuery>().Execute(id);
            Debug.Assert(id == values.ID);
            return new MapSpawnValues(dbController, values);
        }

        /// <summary>
        /// Loads all of the MapSpawnValues for the given <paramref name="mapID"/> from the database.
        /// </summary>
        /// <param name="dbController">DbController used to communicate with the database.</param>
        /// <param name="mapID">Index of the map to load the MapSpawnValues for.</param>
        /// <returns>An IEnumerable of all of the MapSpawnValues for the given <paramref name="mapID"/>.</returns>
        public static IEnumerable<MapSpawnValues> Load(IDbController dbController, MapID mapID)
        {
            var ret = new List<MapSpawnValues>();
            var queryValues = dbController.GetQuery<SelectMapSpawnsOnMapQuery>().Execute(mapID);

            foreach (var v in queryValues)
            {
                Debug.Assert(v.MapID == mapID);
                ret.Add(new MapSpawnValues(dbController, v));
            }

            return ret;
        }

        /// <summary>
        /// Sets the spawn area for this <see cref="MapSpawnValues"/>.
        /// </summary>
        /// <param name="map">Instance of the Map with the <see cref="MapID"/> equal to the <see cref="MapID"/> handled by this
        /// <see cref="MapSpawnValues"/>. This is to ensure that the <paramref name="newSpawnArea"/> given is in a
        /// valid map range.</param>
        /// <param name="newSpawnArea">New MapSpawnRect values.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="newSpawnArea"/> contains one or more
        /// values that are not in range of the <paramref name="map"/>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="map"/>'s <see cref="MapID"/> does not match this
        /// MapSpawnValues's <see cref="MapID"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="map"/> is null.</exception>
        public void SetSpawnArea(MapBase map, MapSpawnRect newSpawnArea)
        {
            if (map == null)
                throw new ArgumentNullException("map");

            if (map.ID != MapID)
                throw new ArgumentException("The ID of the specified map does not match this MapID", "map");

            if (newSpawnArea == SpawnArea)
                return;

            var x = newSpawnArea.X.HasValue ? newSpawnArea.X.Value : (ushort)0;
            var y = newSpawnArea.Y.HasValue ? newSpawnArea.Y.Value : (ushort)0;

            const string errmsg = "One or more of the `newSpawnArea` parameter values are out of range of the map!";

            if (x < 0)
                throw new ArgumentOutOfRangeException("newSpawnArea", errmsg);

            if (y < 0)
                throw new ArgumentOutOfRangeException("newSpawnArea", errmsg);

            if (newSpawnArea.Width.HasValue && (x + newSpawnArea.Width.Value) > map.Width)
                throw new ArgumentOutOfRangeException("newSpawnArea", errmsg);

            if (newSpawnArea.Height.HasValue && (y + newSpawnArea.Height.Value) > map.Height)
                throw new ArgumentOutOfRangeException("newSpawnArea", errmsg);

            SpawnArea = newSpawnArea;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("MapSpawnValues [ID: {0} Map: {1}]", ID, MapID);
        }

        /// <summary>
        /// Updates the MapSpawnValues in the database.
        /// </summary>
        void UpdateDB()
        {
            if (DbController == null)
            {
                const string errmsg =
                    "Tried to call UpdateDB() on `{0}` when the DbController was null." + " Likely means Delete() was called.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Updating MapSpawnValues `{0}`.", this);

            DbController.GetQuery<UpdateMapSpawnQuery>().Execute(this);
        }

        #region IMapSpawnTable Members

        /// <summary>
        /// Gets the value of the database column `amount`.
        /// </summary>
        [Browsable(false)]
        byte IMapSpawnTable.Amount
        {
            get { return SpawnAmount; }
        }

        /// <summary>
        /// Gets or sets the CharacterTemplateID of the CharacterTemplate to spawn.
        /// </summary>
        [Browsable(true)]
        [Description("The ID of the CharacterTemplate to spawn.")]
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
        /// Gets the value of the database column 'direction_id'
        /// </summary>
        [Browsable(false)]
        public Direction DirectionId
        {
            get
            {
                return SpawnDirection;
            }
        }

        /// <summary>
        /// Gets the value of the database column `height`.
        /// </summary>
        [Browsable(false)]
        ushort? IMapSpawnTable.Height
        {
            get { return SpawnArea.Height; }
        }

        /// <summary>
        /// Gets the unique ID of this MapSpawnValues.
        /// </summary>
        [Browsable(false)]
        public MapSpawnValuesID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the value of the database column `map_id`.
        /// </summary>
        [Browsable(false)]
        MapID IMapSpawnTable.MapID
        {
            get { return MapID; }
        }

        /// <summary>
        /// Gets the value of the database column `width`.
        /// </summary>
        [Browsable(false)]
        ushort? IMapSpawnTable.Width
        {
            get { return SpawnArea.Width; }
        }

        /// <summary>
        /// Gets the value of the database column `respawn`.
        /// </summary>
        [Browsable(false)]
        ushort IMapSpawnTable.Respawn
        {
            get { return SpawnRespawn; }
        }

        /// <summary>
        /// Gets the value of the database column `x`.
        /// </summary>
        [Browsable(false)]
        ushort? IMapSpawnTable.X
        {
            get { return SpawnArea.X; }
        }

        /// <summary>
        /// Gets the value of the database column `y`.
        /// </summary>
        [Browsable(false)]
        ushort? IMapSpawnTable.Y
        {
            get { return SpawnArea.Y; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IMapSpawnTable DeepCopy()
        {
            return new MapSpawnTable(this);
        }

        #endregion
    }
}