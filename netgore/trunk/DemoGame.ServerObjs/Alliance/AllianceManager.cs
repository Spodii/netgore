using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Collections;
using NetGore.Db;

namespace DemoGame.Server
{
    public class AllianceManager : DbTableDataManager<AllianceID, Alliance>
    {
        /// <summary>
        /// Dictionary of alliances stored by their name.
        /// </summary>
        static readonly Dictionary<string, Alliance> _allianceFromName =
            new Dictionary<string, Alliance>(StringComparer.OrdinalIgnoreCase);

        SelectAllianceQuery _selectAllianceQuery;
        SelectAllianceAttackableQuery _selectAllianceAttackableQuery;
        SelectAllianceHostileQuery _selectAllianceHostileQuery;

        static readonly AllianceManager _instance = new AllianceManager(DbControllerBase.GetInstance());
        
        /// <summary>
        /// Gets the Alliance by the given name.
        /// </summary>
        /// <param name="name">Name of the Alliance to get.</param>
        /// <returns>The Alliance by the given name, or null if none found.</returns>
        public Alliance this[string name]
        {
            get
            {
                Alliance ret;
                if (_allianceFromName.TryGetValue(name, out ret))
                    return ret;
                return null;
            }
        }

        /// <summary>
        /// Gets an instance of the <see cref="AllianceManager"/>.
        /// </summary>
        public static AllianceManager Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTableDataManager&lt;TID, TItem&gt;"/> class.
        /// </summary>
        /// <param name="dbController">The IDbController.</param>
        AllianceManager(IDbController dbController) : base(dbController)
        {
        }

        /// <summary>
        /// When overridden in the derived class, provides a chance to cache frequently used queries instead of
        /// having to grab the query from the <see cref="IDbController"/> every time. Caching is completely
        /// optional, but if you do cache any queries, it should be done here. Do not use this method for
        /// anything other than caching queries from the <paramref name="dbController"/>.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> to grab the queries from.</param>
        protected override void CacheDbQueries(IDbController dbController)
        {
            _selectAllianceQuery = dbController.GetQuery<SelectAllianceQuery>();
            _selectAllianceAttackableQuery = dbController.GetQuery<SelectAllianceAttackableQuery>();
            _selectAllianceHostileQuery = dbController.GetQuery<SelectAllianceHostileQuery>();

            base.CacheDbQueries(dbController);
        }

        /// <summary>
        /// When overridden in the derived class, gets all of the IDs in the table being managed.
        /// </summary>
        /// <returns>An IEnumerable of all of the IDs in the table being managed.</returns>
        protected override IEnumerable<AllianceID> GetIDs()
        {
            return DbController.GetQuery<SelectAllianceIDsQuery>().Execute();
        }

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> as an int.</returns>
        protected override int IDToInt(AllianceID value)
        {
            return (int)value;
        }

        /// <summary>
        /// When overridden in the derived class, converts the int to a <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The int as a <paramref name="value"/>.</returns>
        public override AllianceID IntToID(int value)
        {
            return new AllianceID(value);
        }

        /// <summary>
        /// When overridden in the derived class, loads an item from the database.
        /// </summary>
        /// <param name="id">The ID of the item to load.</param>
        /// <returns>The item loaded from the database.</returns>
        protected override Alliance LoadItem(AllianceID id)
        {
            AllianceTable values = _selectAllianceQuery.Execute(id);
            var attackables = _selectAllianceAttackableQuery.Execute(id);
            var hostiles = _selectAllianceHostileQuery.Execute(id);

            Debug.Assert(id == values.ID);
            Debug.Assert(attackables.All(x => x.AllianceID == id));
            Debug.Assert(hostiles.All(x => x.AllianceID == id));

            var ret = new Alliance(id, values.Name, attackables, hostiles);

            _allianceFromName.Add(ret.Name, ret);

            return ret;
        }
    }

    /*
    /// <summary>
    /// Loads and manages a collection of Alliances.
    /// </summary>
    public static class AllianceManager
    {
        /// <summary>
        /// Array of alliances stored by their ID.
        /// </summary>
        static readonly DArray<Alliance> _alliance = new DArray<Alliance>(false);


        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static IDbController _dbController;

        /// <summary>
        /// Gets the <see cref="IDbController"/> used by this AllianceManager.
        /// </summary>
        public static IDbController DbController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Gets if this class has been initialized.
        /// </summary>
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// Adds an Alliance to this AllianceManager.
        /// </summary>
        /// <param name="alliance">Alliance to add.</param>
        static void Add(Alliance alliance)
        {
            if (alliance == null)
                throw new ArgumentNullException("alliance");

#if DEBUG
            // Ensure the ID is free
            if (_alliance.CanGet((int)alliance.ID))
            {
                Alliance a = _alliance[(int)alliance.ID];
                if (a != null)
                {
                    const string errmsg = "Failed to add Alliance `{0}` - ID `{1}` is already occupied by `{2}`!";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, alliance, alliance.ID, a);
                    Debug.Fail(string.Format(errmsg, alliance, alliance.ID, a));
                    return;
                }
            }

            // Make sure we don't already have this Alliance in the collection somehow
            if (_alliance.Contains(alliance))
            {
                const string errmsg = "Failed to add Alliance `{0}` - it is already in the AllianceManager!";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, alliance);
                Debug.Fail(string.Format(errmsg, alliance));
                return;
            }
#endif

            // Add it
            _allianceFromName.Add(alliance.Name, alliance);
            _alliance.Insert((int)alliance.ID, alliance);
        }

        /// <summary>
        /// Gets if this AllianceManager contains an Alliance by the given name.
        /// </summary>
        /// <param name="allianceName">Name of the Alliance.</param>
        /// <returns>True if this AllianceManager contains an Alliance with the name <paramref name="allianceName"/>,
        /// else false.</returns>
        public static bool Contains(string allianceName)
        {
            return _allianceFromName.ContainsKey(allianceName);
        }

        /// <summary>
        /// Gets the Alliance by the given name.
        /// </summary>
        /// <param name="allianceName">Name of the Alliance to get.</param>
        /// <returns>The Alliance by the given name, or null if none found.</returns>
        public static Alliance GetAlliance(string allianceName)
        {
            Alliance ret;
            if (_allianceFromName.TryGetValue(allianceName, out ret))
                return ret;
            return null;
        }

        /// <summary>
        /// Gets the Alliance by the specified AllianceID.
        /// </summary>
        /// <param name="allianceID">ID of the Alliance.</param>
        /// <returns>The Alliance with the given ID, or null if none found.</returns>
        public static Alliance GetAlliance(AllianceID allianceID)
        {
            if (!_alliance.CanGet((int)allianceID))
                return null;
            return _alliance[(int)allianceID];
        }

        /// <summary>
        /// Initializes the AllianceManager. Must be called before anything else.
        /// </summary>
        /// <param name="dbController">IDbController for the database holding the alliance information.</param>
        public static void Initialize(IDbController dbController)
        {
            if (IsInitialized)
                return;

            IsInitialized = true;

            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _dbController = dbController;

            // Load the alliances
            LoadAll();
        }

        /// <summary>
        /// Loads the Alliance information for all Alliances and binds them to this AllianceManager.
        /// </summary>
        static void LoadAll()
        {
            // Grab all IDs
            var allianceIDs = DbController.GetQuery<SelectAllianceIDsQuery>().Execute();

            // Load each alliance
            foreach (AllianceID allianceID in allianceIDs)
            {
                Alliance alliance = LoadAlliance(DbController, allianceID);
                Add(alliance);
            }

            _alliance.Trim();
        }

        /// <summary>
        /// Loads a single Alliance.
        /// </summary>
        /// <param name="dbController">DbController used to communicate with the database.</param>
        /// <param name="id">ID of the Alliance to load.</param>
        /// <returns>The loaded Alliance.</returns>
        public static Alliance LoadAlliance(IDbController dbController, AllianceID id)
        {
            AllianceTable values = dbController.GetQuery<SelectAllianceQuery>().Execute(id);
            var attackables = dbController.GetQuery<SelectAllianceAttackableQuery>().Execute(id);
            var hostiles = dbController.GetQuery<SelectAllianceHostileQuery>().Execute(id);

            Debug.Assert(id == values.ID);
            Debug.Assert(attackables.All(x => x.AllianceID == id));
            Debug.Assert(hostiles.All(x => x.AllianceID == id));

            Alliance alliance = new Alliance(id, values.Name, attackables, hostiles);

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded Alliance `{0}`.", alliance);

            return alliance;
        }
    }
    */
}