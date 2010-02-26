using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.DbObjs;
using log4net;
using NetGore;
using NetGore.Features.Quests;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the information of a Character template to build Character instances from.
    /// </summary>
    public class CharacterTemplate
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IEnumerable<CharacterTemplateEquipmentItem> _equipment;
        readonly IEnumerable<CharacterTemplateInventoryItem> _inventory;
        readonly IEnumerable<IQuest<User>> _quests;
        readonly ICharacterTemplateTable _templateTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplate"/> class.
        /// </summary>
        /// <param name="templateTable">The template table.</param>
        /// <param name="inventory">The inventory.</param>
        /// <param name="equipment">The equipment.</param>
        /// <param name="quests">The quests.</param>
        public CharacterTemplate(ICharacterTemplateTable templateTable, IEnumerable<CharacterTemplateInventoryItem> inventory,
                                 IEnumerable<CharacterTemplateEquipmentItem> equipment, IEnumerable<IQuest<User>> quests)
        {
            if (templateTable == null)
                throw new ArgumentNullException("templateTable");
            if (inventory == null)
                throw new ArgumentNullException("inventory");
            if (equipment == null)
                throw new ArgumentNullException("equipment");

            Debug.Assert(!inventory.Any(x => x == null));
            Debug.Assert(!equipment.Any(x => x == null));
            Debug.Assert(!quests.Any(x => x == null));

            _inventory = inventory.ToCompact();
            _equipment = equipment.ToCompact();
            _quests = quests.ToCompact();

            _templateTable = templateTable;

            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded CharacterTemplate `{0}`.", this);
        }

        /// <summary>
        /// Gets the <see cref="CharacterTemplate"/>'s equipment items.
        /// </summary>
        public IEnumerable<CharacterTemplateEquipmentItem> Equipment
        {
            get { return _equipment; }
        }

        /// <summary>
        /// Gets the <see cref="CharacterTemplate"/>'s inventory items.
        /// </summary>
        public IEnumerable<CharacterTemplateInventoryItem> Inventory
        {
            get { return _inventory; }
        }

        /// <summary>
        /// Gets the <see cref="CharacterTemplate"/>'s quests.
        /// </summary>
        public IEnumerable<IQuest<User>> Quests
        {
            get { return _quests; }
        }

        /// <summary>
        /// Gets the <see cref="CharacterTemplate"/>'s values.
        /// </summary>
        public ICharacterTemplateTable TemplateTable
        {
            get { return _templateTable; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return TemplateTable.Name + " {" + TemplateTable.ID + "}";
        }
    }
}