using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;

namespace DemoGame.Server
{
    /// <summary>
    /// Loads and manages a collection of Alliances.
    /// </summary>
    public class AllianceManager : IEnumerable<Alliance>
    {
        /// <summary>
        /// Dictionary of alliances by their name
        /// </summary>
        static readonly Dictionary<string, Alliance> _alliances = new Dictionary<string, Alliance>();

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
        /// Loads the Alliance information for all Alliances.
        /// </summary>
        void LoadAll()
        {
            var allianceData = DBController.SelectAlliances.Execute();

            // Create the alliance objects and store them by their name
            foreach (var data in allianceData)
            {
                string name = (string)data["name"];

                if (Contains(name))
                    throw new Exception(string.Format("Duplicate alliance name found: {0}", name));

                Alliance a = new Alliance(this, name);
                _alliances.Add(name, a);

                if (log.IsInfoEnabled)
                    log.InfoFormat("Loaded alliance `{0}`", name);
            }

            // Add the information for each alliance
            foreach (var data in allianceData)
            {
                string name = (string)data["name"];
                Alliance alliance = this[name];

                if (alliance == null)
                    throw new Exception(string.Format("Failed to find Alliance `{0}`", name));

                alliance.Load(data);
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