using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using log4net;

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
        readonly Dictionary<string, Alliance> _alliances = new Dictionary<string, Alliance>();

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
        /// <returns>The Alliance by the given name.</returns>
        public Alliance this[string allianceName]
        {
            get { return _alliances[allianceName]; }
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
            return _alliances.ContainsKey(allianceName);
        }

        /// <summary>
        /// Loads a single Alliance.
        /// </summary>
        /// <param name="dbController">DBController used to communicate with the database.</param>
        /// <param name="id">ID of the Alliance to load.</param>
        /// <returns>The loaded Alliance.</returns>
        public static Alliance Load(DBController dbController, byte id)
        {
            var values = dbController.SelectAlliance.Execute(id);
            var attackableIDs = dbController.SelectAllianceAttackable.Execute(id);
            var hostileIDs = dbController.SelectAllianceHostile.Execute(id);

            Debug.Assert(id == values.ID);
            Debug.Assert(id == attackableIDs.AllianceID);
            Debug.Assert(id == hostileIDs.AllianceID);

            var alliance = new Alliance(id, values.Name, attackableIDs.AttackableIDs, hostileIDs.HostileIDs);

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded Alliance `{0}`.", alliance);

            return alliance;
        }

        void Add(Alliance alliance)
        {
            if (alliance == null)
                throw new ArgumentNullException("alliance");

            _alliances.Add(alliance.Name, alliance);
        }

        /// <summary>
        /// Loads the Alliance information for all Alliances and binds them to this AllianceManager.
        /// </summary>
        void LoadAll()
        {
            var allianceIDs = DBController.SelectAllianceIDs.Execute();

            foreach (var allianceID in allianceIDs)
            {
                var alliance = Load(DBController, allianceID);
                Add(alliance);
            }
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
            foreach (Alliance alliance in _alliances.Values)
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