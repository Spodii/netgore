using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Collections;

namespace DemoGame.Server
{
    /// <summary>
    /// Loads and manages a collection of Alliances.
    /// </summary>
    public class AllianceManager : IEnumerable<Alliance>
    {
        /// <summary>
        /// Dictionary of alliances stored by their name.
        /// </summary>
        readonly Dictionary<string, Alliance> _allianceFromName = new Dictionary<string, Alliance>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Array of alliances stored by their ID.
        /// </summary>
        readonly DArray<Alliance> _alliance = new DArray<Alliance>(false);

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly DBController _dbController;

        /// <summary>
        /// Gets the DBController used by this AllianceManager.
        /// </summary>
        public DBController DBController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Gets the Alliance by the given name.
        /// </summary>
        /// <param name="allianceName">Name of the Alliance to get.</param>
        /// <returns>The Alliance by the given name, or null if none found.</returns>
        public Alliance this[string allianceName]
        {
            get 
            {
                Alliance ret;
                if (_allianceFromName.TryGetValue(allianceName, out ret))
                    return ret;
                return null;
            }
        }

        /// <summary>
        /// Gets the Alliance by the specified AllianceID.
        /// </summary>
        /// <param name="allianceID">ID of the Alliance.</param>
        /// <returns>The Alliance with the given ID, or null if none found.</returns>
        public Alliance this[AllianceID allianceID]
        {
            get 
            {
                if (!_alliance.CanGet((int)allianceID))
                    return null;
                return _alliance[(int)allianceID]; 
            }
        }

        /// <summary>
        /// AllianceManager constructor
        /// </summary>
        /// <param name="dbController">DBController for the database holding the alliance information.</param>
        public AllianceManager(DBController dbController)
        {
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _dbController = dbController;

            // Load the alliances
            LoadAll();
        }

        /// <summary>
        /// Gets if this AllianceManager contains an Alliance by the given name.
        /// </summary>
        /// <param name="allianceName">Name of the Alliance.</param>
        /// <returns>True if this AllianceManager contains an Alliance with the name <paramref name="allianceName"/>,
        /// else false.</returns>
        public bool Contains(string allianceName)
        {
            return _allianceFromName.ContainsKey(allianceName);
        }

        /// <summary>
        /// Loads a single Alliance.
        /// </summary>
        /// <param name="dbController">DBController used to communicate with the database.</param>
        /// <param name="id">ID of the Alliance to load.</param>
        /// <returns>The loaded Alliance.</returns>
        public static Alliance LoadAlliance(DBController dbController, AllianceID id)
        {
            var values = dbController.GetQuery<SelectAllianceQuery>().Execute(id);
            var attackableIDs = dbController.GetQuery<SelectAllianceAttackableQuery>().Execute(id);
            var hostileIDs = dbController.GetQuery<SelectAllianceHostileQuery>().Execute(id);

            Debug.Assert(id == values.ID);
            Debug.Assert(id == attackableIDs.AllianceID);
            Debug.Assert(id == hostileIDs.AllianceID);

            var alliance = new Alliance(id, values.Name, attackableIDs.AttackableIDs, hostileIDs.HostileIDs);

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded Alliance `{0}`.", alliance);

            return alliance;
        }

        /// <summary>
        /// Adds an Alliance to this AllianceManager.
        /// </summary>
        /// <param name="alliance">Alliance to add.</param>
        void Add(Alliance alliance)
        {
            if (alliance == null)
                throw new ArgumentNullException("alliance");
            
#if DEBUG
            // Ensure the ID is free
            if (_alliance.CanGet((int)alliance.ID))
            {
                var a = _alliance[(int)alliance.ID];
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
        /// Loads the Alliance information for all Alliances and binds them to this AllianceManager.
        /// </summary>
        void LoadAll()
        {
            // Grab all IDs
            var allianceIDs = DBController.GetQuery<SelectAllianceIDsQuery>().Execute();

            // Load each alliance
            foreach (var allianceID in allianceIDs)
            {
                var alliance = LoadAlliance(DBController, allianceID);
                Add(alliance);
            }

            _alliance.Trim();
        }

        #region IEnumerable<Alliance> Members

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        public IEnumerator<Alliance> GetEnumerator()
        {
            foreach (Alliance alliance in _allianceFromName.Values)
            {
                yield return alliance;
            }
        }

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}