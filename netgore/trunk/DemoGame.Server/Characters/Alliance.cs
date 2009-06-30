using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using log4net;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains information about the Alliance of any Entity in the game, defining Alliances they are allowed
    /// to attack, Alliances they are hostile towards, etc. Every Character should follow these rules strictly.
    /// </summary>
    public class Alliance
    {
        static readonly string[] _splitStr = new string[] { "\r\n" };
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly AllianceManager _allianceManager;
        readonly List<Alliance> _attackable = new List<Alliance>();
        readonly List<Alliance> _hostile = new List<Alliance>();
        readonly string _name;
        bool _loaded;

        /// <summary>
        /// Gets the AllianceManager that manages this Alliance.
        /// </summary>
        public AllianceManager AllianceManager
        {
            get { return _allianceManager; }
        }

        /// <summary>
        /// Gets the list of Alliances that this Alliance can attack
        /// </summary>
        public IEnumerable<Alliance> Attackable
        {
            get { return _attackable; }
        }

        /// <summary>
        /// Gets the list of Alliances that this Alliance is hostile towards
        /// </summary>
        public IEnumerable<Alliance> Hostile
        {
            get { return _hostile; }
        }

        /// <summary>
        /// Gets the unique name of the alliance
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Alliance constructor
        /// </summary>
        /// <param name="allianceManager">AllianceManager that this Alliance will belong to.</param>
        /// <param name="name">Name of the Alliance.</param>
        public Alliance(AllianceManager allianceManager, string name)
        {
            if (allianceManager == null)
                throw new ArgumentNullException("allianceManager");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _allianceManager = allianceManager;
            _name = name;
        }

        /// <summary>
        /// Checks this alliance can attack the given alliance
        /// </summary>
        /// <param name="alliance">Alliance to check against</param>
        /// <returns>True if can attack the given alliance, else false</returns>
        public bool CanAttack(Alliance alliance)
        {
            if (alliance == null)
            {
                Debug.Fail("alliance is null.");
                return false;
            }

            return Attackable.Contains(alliance);
        }

        /// <summary>
        /// Checks this alliance is hostile towards the given alliance
        /// </summary>
        /// <param name="alliance">Alliance to check against</param>
        /// <returns>True if hostile towards the given alliance, else false</returns>
        public bool IsHostile(Alliance alliance)
        {
            if (alliance == null)
            {
                Debug.Fail("alliance is null.");
                if (log.IsWarnEnabled)
                    log.Warn("alliance is null.");
                return false;
            }

            return Hostile.Contains(alliance);
        }

        /// <summary>
        /// Loads the Alliance's data. This must be called once, and only once, before the Alliance is used.
        /// </summary>
        /// <param name="data">Dictionary containing the alliance data.</param>
        public void Load(IDictionary<string, object> data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (_loaded)
                throw new MethodAccessException("Alliance data has already been loaded.");

            _loaded = true;

            // Grab the data values
            string fieldName = (string)data["name"];
            string fieldHostile = (string)data["hostile"];
            string fieldAttackable = (string)data["attackable"];

            // Ensure that this data is even for us, and that this AllianceManager knows about us
            if (AllianceManager[fieldName] != this)
                throw new DataException("Alliance name reference mismatch.");

            // Add the hostile alliances
            var hostile = fieldHostile.Split(_splitStr, StringSplitOptions.RemoveEmptyEntries);

            foreach (string otherName in hostile)
            {
                _hostile.Add(AllianceManager[otherName]);
            }

            _hostile.TrimExcess();

            // Add the attackable alliances
            var attackable = fieldAttackable.Split(_splitStr, StringSplitOptions.RemoveEmptyEntries);

            foreach (string otherName in attackable)
            {
                _attackable.Add(AllianceManager[otherName]);
            }

            _hostile.TrimExcess();
        }
    }
}