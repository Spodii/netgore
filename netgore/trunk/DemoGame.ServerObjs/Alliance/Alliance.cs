using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame;
using DemoGame.Server.DbObjs;
using log4net;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains information about the Alliance of any Entity in the game, defining Alliances they are allowed
    /// to attack, Alliances they are hostile towards, etc. Every Character should follow these rules strictly.
    /// </summary>
    public class Alliance
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly AllianceID[] _attackable;
        readonly AllianceID[] _hostile;
        readonly AllianceID _id;
        readonly string _name;

        /// <summary>
        /// Gets the list of Alliance IDs that this Alliance can attack
        /// </summary>
        public IEnumerable<AllianceID> Attackable
        {
            get { return _attackable; }
        }

        /// <summary>
        /// Gets the list of Alliance IDs that this Alliance is hostile towards
        /// </summary>
        public IEnumerable<AllianceID> Hostile
        {
            get { return _hostile; }
        }

        /// <summary>
        /// Gets the ID of this Alliance.
        /// </summary>
        public AllianceID ID
        {
            get { return _id; }
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
        /// <param name="id">The ID of this Alliance.</param>
        /// <param name="name">Name of the Alliance.</param>
        /// <param name="attackables">Information for Alliances that this Alliance can attack.</param>
        /// <param name="hostiles">Information for Alliances that this Alliance is hostile towards.</param>
        public Alliance(AllianceID id, string name, IEnumerable<AllianceAttackableTable> attackables,
                        IEnumerable<AllianceHostileTable> hostiles)
        {
            Debug.Assert(attackables.All(x => x.AllianceID == id));
            Debug.Assert(hostiles.All(x => x.AllianceID == id));

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            _id = id;
            _name = name;
            _attackable = attackables.Select(x => x.AttackableID).ToArray();
            _hostile = hostiles.Select(x => x.HostileID).ToArray();
        }

        /// <summary>
        /// Checks this alliance can attack the given alliance.
        /// </summary>
        /// <param name="alliance">Alliance to check against.</param>
        /// <returns>True if can attack the given alliance, else false.</returns>
        public bool CanAttack(Alliance alliance)
        {
            if (alliance == null)
            {
                const string errmsg = "Parameter `alliance` is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return false;
            }

            return CanAttack(alliance.ID);
        }

        /// <summary>
        /// Checks this alliance can attack the given alliance.
        /// </summary>
        /// <param name="allianceID">Alliance ID to check against.</param>
        /// <returns>True if can attack the given alliance, else false.</returns>
        public bool CanAttack(AllianceID allianceID)
        {
            return Attackable.Contains(allianceID);
        }

        /// <summary>
        /// Checks this Alliance is hostile towards the given Alliance.
        /// </summary>
        /// <param name="alliance">Alliance to check against.</param>
        /// <returns>True if hostile towards the given Alliance, else false.</returns>
        public bool IsHostile(Alliance alliance)
        {
            if (alliance == null)
            {
                const string errmsg = "Parameter `alliance` is null.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                return false;
            }

            return IsHostile(alliance.ID);
        }

        /// <summary>
        /// Checks this Alliance is hostile towards the given Alliance.
        /// </summary>
        /// <param name="allianceID">Alliance ID to check against.</param>
        /// <returns>True if hostile towards the given Alliance, else false.</returns>
        public bool IsHostile(AllianceID allianceID)
        {
            return Hostile.Contains(allianceID);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, ID);
        }
    }
}