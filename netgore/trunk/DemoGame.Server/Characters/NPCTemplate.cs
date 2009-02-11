using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using MySql.Data.MySqlClient;
using Platyform.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the information of a NPC template to build NPC instances from.
    /// </summary>
    public class NPCTemplate
    {
        static readonly NPCTemplateOC _ordinalCache = new NPCTemplateOC();
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly string _aiName;
        readonly Alliance _alliance;
        readonly ushort _bodyIndex;
        readonly IEnumerable<NPCDrop> _drops;
        readonly ushort _giveCash;
        readonly ushort _giveExp;
        readonly string _name;
        readonly ushort _respawnSecs;
        readonly IEnumerable<IStat> _stats;

        /// <summary>
        /// Gets the default AI module's name
        /// </summary>
        public string AIName
        {
            get { return _aiName; }
        }

        /// <summary>
        /// Gets the default alliance name
        /// </summary>
        public Alliance Alliance
        {
            get { return _alliance; }
        }

        /// <summary>
        /// Gets the default BodyIndex
        /// </summary>
        public ushort BodyIndex
        {
            get { return _bodyIndex; }
        }

        public IEnumerable<NPCDrop> Drops
        {
            get { return _drops; }
        }

        public ushort GiveCash
        {
            get { return _giveCash; }
        }

        public ushort GiveExp
        {
            get { return _giveExp; }
        }

        /// <summary>
        /// Gets the default name
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the default respawn time
        /// </summary>
        public ushort RespawnSecs
        {
            get { return _respawnSecs; }
        }

        /// <summary>
        /// Gets the default stat values for the NPCTemplate. This IEnumerable will only contain
        /// IStats that have a non-zero value and exist in the NPCTemplate database.
        /// </summary>
        public IEnumerable<IStat> Stats
        {
            get { return _stats; }
        }

        /// <summary>
        /// NPCTemplate constructor
        /// </summary>
        /// <param name="guid">Unique ID of the NPC template.</param>
        /// <param name="conn">MySqlConnection to load the information from.</param>
        /// <param name="allianceManager">AllianceManager containing the alliances used by the NPCs.</param>
        /// <param name="npcDrops">NPCDropManager collection used to get the NPCDrop reference for each item the
        /// NPC drops.</param>
        public NPCTemplate(int guid, MySqlConnection conn, AllianceManager allianceManager, IList<NPCDrop> npcDrops)
        {
            if (allianceManager == null)
                throw new ArgumentNullException("allianceManager");
            if (npcDrops == null)
                throw new ArgumentNullException("npcDrops");

            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT * FROM `npc_templates` WHERE `guid`='{0}'", guid);
                using (MySqlDataReader r = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    // Ensure we have a valid ID
                    if (!r.Read())
                        throw new ArgumentException(string.Format("NPC template `{0}` does not exist", guid), "guid");

                    // Make sure the ordinal cache is ready
                    _ordinalCache.Initialize(r);

                    // Load everything
                    _bodyIndex = r.GetUInt16(_ordinalCache.Body);
                    _name = r.GetString(_ordinalCache.Name);
                    _respawnSecs = r.GetUInt16(_ordinalCache.Respawn);
                    _aiName = r.GetString(_ordinalCache.AI);
                    _giveExp = r.GetUInt16(_ordinalCache.GiveExp);
                    _giveCash = r.GetUInt16(_ordinalCache.GiveCash);
                    _drops = LoadDrops(r, npcDrops);
                    _alliance = allianceManager[r.GetString(_ordinalCache.Alliance)];
                    _stats = LoadStats(r);
                }
            }
        }

        /// <summary>
        /// Loads the NPCDrop information for a NPC from the database.
        /// </summary>
        /// <param name="dataRecord">IDataRecord to read from.</param>
        /// <param name="npcDrops">Collection of NPCDropManager to read from.</param>
        /// <returns>An IEnumerable of the NPCDropManager for the NPC.</returns>
        static IEnumerable<NPCDrop> LoadDrops(IDataRecord dataRecord, IList<NPCDrop> npcDrops)
        {
            // Read the field
            string rawDropsStr = dataRecord.GetString(dataRecord.GetOrdinal("drops"));

            // If the string is null or empty, return an empty drops list
            if (string.IsNullOrEmpty(rawDropsStr))
                return Enumerable.Empty<NPCDrop>();

            // Split up the string by the comma
            var dropsStrs = rawDropsStr.Split(',');

            // Parse all the strings, and hold only the distinct values
            var distinctValues = dropsStrs.Select(str => int.Parse(str)).Distinct();

            // Select the NPCDropManager and return the result
            return distinctValues.Select(dropGuid => npcDrops[dropGuid]);
        }

        /// <summary>
        /// Loads all of the stats from the database.
        /// </summary>
        /// <param name="dataRecord">IDataRecord to load the stat values from.</param>
        /// <returns>An IEnumerable of all the IStats that had a non-zero value.</returns>
        static IEnumerable<IStat> LoadStats(IDataRecord dataRecord)
        {
            // Create a temporary set of NPCStats to load the values into
            NPCStats stats = new NPCStats();

            // Load all values from the database into the temporary stats collection
            foreach (StatType statType in _ordinalCache.StatTypes)
            {
                IStat stat = stats.GetStat(statType);
                int ordinal = _ordinalCache.GetStatOrdinal(statType);
                stat.Read(dataRecord, ordinal);
            }

            // Return all non-zero stats, but in an array so we no longer reference the temporary NPCStats
            return stats.Where(stat => stat.Value != 0).ToArray();
        }

        class NPCTemplateOC : OrdinalCacheBase
        {
            byte _ai;
            byte _alliance;
            byte _body;
            byte _giveCash;
            byte _giveExp;
            byte _name;
            byte _respawn;
            byte?[] _statOrdinals;

            public int AI
            {
                get { return _ai; }
            }

            public int Alliance
            {
                get { return _alliance; }
            }

            public int Body
            {
                get { return _body; }
            }

            public int GiveCash
            {
                get { return _giveCash; }
            }

            public int GiveExp
            {
                get { return _giveExp; }
            }

            public int Name
            {
                get { return _name; }
            }

            public int Respawn
            {
                get { return _respawn; }
            }

            public IEnumerable<StatType> StatTypes
            {
                get { return GetLoadedStatOrdinalEnumerator(_statOrdinals); }
            }

            public int GetStatOrdinal(StatType statType)
            {
                return GetStatOrdinalHelper(statType, _statOrdinals);
            }

            protected override void LoadCache(IDataRecord dataRecord)
            {
                _body = dataRecord.GetOrdinalAsByte("body");
                _name = dataRecord.GetOrdinalAsByte("name");
                _respawn = dataRecord.GetOrdinalAsByte("respawn");
                _ai = dataRecord.GetOrdinalAsByte("ai");
                _giveExp = dataRecord.GetOrdinalAsByte("give_exp");
                _giveCash = dataRecord.GetOrdinalAsByte("give_cash");
                _alliance = dataRecord.GetOrdinalAsByte("alliance");

                // Try to load every StatType possible
                var statTypes = Enum.GetValues(typeof(StatType)).Cast<StatType>();
                _statOrdinals = TryCreateStatOrdinalCache(dataRecord, statTypes);

                // Display the loaded StatTypes in the log
                if (log.IsInfoEnabled)
                {
                    foreach (StatType statType in StatTypes)
                    {
                        int ordinal = GetStatOrdinal(statType);
                        log.InfoFormat("NPC StatType `{0}` found at ordinal `{1}`", statType, ordinal);
                    }
                }
            }
        }
    }
}