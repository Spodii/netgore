using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using DemoGame.Server.Db;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Collections;

namespace DemoGame.Server
{
    /// <summary>
    /// Loads and manages a collection of Alliances.
    /// </summary>
    public static class AllianceManager
    {
        /// <summary>
        /// Array of alliances stored by their ID.
        /// </summary>
        static readonly DArray<Alliance> _alliance = new DArray<Alliance>(false);

        /// <summary>
        /// Dictionary of alliances stored by their name.
        /// </summary>
        static readonly Dictionary<string, Alliance> _allianceFromName =
            new Dictionary<string, Alliance>(StringComparer.OrdinalIgnoreCase);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static DBController _dbController;

        /// <summary>
        /// Gets the DBController used by this AllianceManager.
        /// </summary>
        public static DBController DBController
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
        /// <param name="dbController">DBController for the database holding the alliance information.</param>
        public static void Initialize(DBController dbController)
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
            var allianceIDs = DbQueries.Alliance.GetAllianceIDs();

            // Load each alliance
            foreach (AllianceID allianceID in allianceIDs)
            {
                Alliance alliance = LoadAlliance(DBController, allianceID);
                Add(alliance);
            }

            _alliance.Trim();
        }

        /// <summary>
        /// Loads a single Alliance.
        /// </summary>
        /// <param name="dbController">DBController used to communicate with the database.</param>
        /// <param name="id">ID of the Alliance to load.</param>
        /// <returns>The loaded Alliance.</returns>
        public static Alliance LoadAlliance(DBController dbController, AllianceID id)
        {
            var values = DbQueries.Alliance.GetAlliance(id);
            SelectAllianceAttackableQueryValues attackableIDs = dbController.GetQuery<SelectAllianceAttackableQuery>().Execute(id);
            SelectAllianceHostileQueryValues hostileIDs = dbController.GetQuery<SelectAllianceHostileQuery>().Execute(id);

            Debug.Assert(id == attackableIDs.AllianceID);
            Debug.Assert(id == hostileIDs.AllianceID);

            Alliance alliance = new Alliance(id, values.name, attackableIDs.AttackableIDs, hostileIDs.HostileIDs);

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded Alliance `{0}`.", alliance);

            return alliance;
        }
    }
}