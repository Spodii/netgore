using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the information of a NPC template to build NPC instances from.
    /// </summary>
    public class NPCTemplate
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly string _aiName;
        readonly Alliance _alliance;
        readonly ushort _bodyIndex;
        readonly IEnumerable<NPCDrop> _drops;
        readonly ushort _giveCash;
        readonly ushort _giveExp;
        readonly int _id;
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
        /// Gets the ID of the NPCTemplate.
        /// </summary>
        public int ID
        {
            get { return _id; }
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
        /// <param name="id">Unique ID of the NPC template.</param>
        /// <param name="query">SelectNPCTemplateQuery to load the information from.</param>
        /// <param name="allianceManager">AllianceManager containing the alliances used by the NPCs.</param>
        /// <param name="itemTemplates">ItemTemplates containing the templates for items the NPCs will drop.</param>
        public NPCTemplate(int id, SelectNPCTemplateQuery query, SelectNPCTemplateDropsQuery dropsQuery,
            AllianceManager allianceManager, ItemTemplates itemTemplates)
        {
            if (allianceManager == null)
                throw new ArgumentNullException("allianceManager");

            // Drops and Stats are converted to an array just because they are the most compact IEnumerable collection
            SelectNPCTemplateQueryValues values = query.Execute(id);
            _id = values.ID;
            _bodyIndex = values.BodyIndex;
            _name = values.Name;
            _respawnSecs = values.Respawn;
            _aiName = values.AIName;
            _giveExp = values.GiveExp;
            _giveCash = values.GiveCash;
            _alliance = allianceManager[values.AllianceID];

            // Get the drops
            var drops = GetNPCDrops(dropsQuery, id, itemTemplates);
            _drops = drops.ToArray();

            // Only store stats that have a non-zero value, and assume the rest are zero
            _stats = values.Stats.Where(x => x.Value != 0).ToArray();

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded NPCTemplate `{0}` [{1}]", ID, Name);
        }

        public static IEnumerable<NPCDrop> GetNPCDrops(SelectNPCTemplateDropsQuery dropsQuery, int characterID, ItemTemplates itemTemplates)
        {
            var dropValues = dropsQuery.Execute(characterID);

            var ret = new List<NPCDrop>(dropValues.Count());
            foreach (var value in dropValues)
            {
                // TODO: Error handling for when the ItemTemplate does not exist
                var itemTemplate = itemTemplates[value.ItemID];
                var drop = new NPCDrop(itemTemplate, value.DropChance);
                ret.Add(drop);
            }

            return ret;
        }
    }
}