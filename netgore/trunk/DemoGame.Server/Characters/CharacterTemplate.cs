using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the information of a Character template to build Character instances from.
    /// </summary>
    public class CharacterTemplate
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string AIName { get; private set; }

        public Alliance Alliance { get; private set; }

        public BodyIndex BodyIndex { get; private set; }
        public IEnumerable<CharacterTemplateEquipmentItem> Equipment { get; private set; }

        public ushort GiveCash { get; private set; }

        public ushort GiveExp { get; private set; }

        public CharacterTemplateID ID { get; private set; }
        public IEnumerable<CharacterTemplateInventoryItem> Inventory { get; private set; }

        public string Name { get; private set; }

        public ushort RespawnSecs { get; private set; }

        public uint Exp { get; private set; }
        public uint StatPoints { get; private set; }
        public byte Level { get; private set; }

        /// <summary>
        /// Gets the default stat values for the CharacterTemplate. This IEnumerable will only contain
        /// IStats that have a non-zero value and exist in the CharacterTemplate database.
        /// </summary>
        public IEnumerable<StatTypeValue> StatValues { get; private set; }

        public CharacterTemplate(CharacterTemplateID id, string name, string aiName, Alliance alliance, BodyIndex body,
                                 ushort respawnSecs, ushort giveExp, ushort giveCash, uint exp, uint statPoints, byte level,
                                 IEnumerable<StatTypeValue> statValues,
                                 IEnumerable<CharacterTemplateInventoryItem> inventory,
                                 IEnumerable<CharacterTemplateEquipmentItem> equipment)
        {
            Debug.Assert(!inventory.Any(x => x == null));
            Debug.Assert(!equipment.Any(x => x == null));

            ID = id;
            Name = name;
            AIName = aiName;
            Alliance = alliance;
            BodyIndex = body;
            RespawnSecs = respawnSecs;
            GiveExp = giveExp;
            GiveCash = giveCash;
            Exp = exp;
            StatPoints = statPoints;
            Level = level;

            StatValues = statValues.Where(x => x.Value != 0).ToArray();
            Inventory = inventory.ToArray();
            Equipment = equipment.ToArray();

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded CharacterTemplate `{0}`.", this);
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, ID);
        }
    }
}