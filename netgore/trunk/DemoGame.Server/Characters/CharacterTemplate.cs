using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;
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

        public ushort GiveCash { get; private set; }

        public ushort GiveExp { get; private set; }

        public CharacterTemplateID ID { get; private set; }

        public string Name { get; private set; }

        public ushort RespawnSecs { get; private set; }

        /// <summary>
        /// Gets the default stat values for the CharacterTemplate. This IEnumerable will only contain
        /// IStats that have a non-zero value and exist in the CharacterTemplate database.
        /// </summary>
        public IEnumerable<IStat> Stats { get; private set; }

        public IEnumerable<CharacterTemplateInventoryItem> Inventory { get; private set; }

        public IEnumerable<CharacterTemplateEquipmentItem> Equipment { get; private set; }

        public CharacterTemplate(CharacterTemplateID id, string name, string aiName, Alliance alliance, BodyIndex body,
            ushort respawnSecs, ushort giveExp, ushort giveCash, IEnumerable<IStat> stats,
            IEnumerable<CharacterTemplateInventoryItem> inventory, IEnumerable<CharacterTemplateEquipmentItem> equipment)
        {
            Debug.Assert(!stats.Any(x => x == null));
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

            Stats = stats.Where(x => x.Value != 0).ToArray();
            Inventory = inventory.ToArray();
            Equipment = equipment.ToArray();

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded CharacterTemplate `{0}`.", this);
        }

        /// <summary>
        /// Creates an instance of the CharacterTemplate as a NPC.
        /// </summary>
        /// <param name="world">World to place the NPC in.</param>
        /// <returns>An instance of the CharacterTemplate as a NPC.</returns>
        public NPC CreateNPCInstance(World world)
        {
            NPC ret = new NPC(world, this);

            if (log.IsInfoEnabled)
                log.InfoFormat("Created NPC instance from template `{0}`.", this);

            foreach (var inventoryItem in Inventory)
            {
                var item = inventoryItem.CreateInstance();
                if (item == null)
                    continue;

                var extraItems = ret.Inventory.Add(item);
                if (extraItems != null)
                    extraItems.Dispose();
            }

            foreach (var equippedItem in Equipment)
            {
                var item = equippedItem.CreateInstance();
                if (item == null)
                    continue;

                if (!ret.Equipped.Equip(item))
                    item.Dispose();
            }

            return ret;
        }

        /// <summary>
        /// Creates an instance of the CharacterTemplate as a NPC.
        /// </summary>
        /// <param name="map">Map to place the NPC on.</param>
        /// <returns>An instance of the CharacterTemplate as a NPC.</returns>
        public NPC CreateNPCInstance(Map map)
        {
            NPC ret = CreateNPCInstance(map.World);
            ret.RespawnMap = map;
            ret.ChangeMap(map);
            return ret;
        }

        public override string ToString()
        {
            return string.Format("{0} [{1}]", Name, ID);
        }
    }
}