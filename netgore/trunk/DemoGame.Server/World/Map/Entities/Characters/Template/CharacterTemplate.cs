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
        readonly IEnumerable<SkillType> _knownSkills;
        readonly IEnumerable<IQuest<User>> _quests;
        readonly ICharacterTemplateTable _templateTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplate"/> class.
        /// </summary>
        /// <param name="templateTable">The template table.</param>
        /// <param name="inventory">The inventory.</param>
        /// <param name="equipment">The equipment.</param>
        /// <param name="quests">The quests.</param>
        /// <param name="knownSkills">The known skills.</param>
        /// <exception cref="ArgumentNullException"><paramref name="templateTable" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="inventory" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="equipment" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="knownSkills" /> is <c>null</c>.</exception>
        public CharacterTemplate(ICharacterTemplateTable templateTable, IEnumerable<CharacterTemplateInventoryItem> inventory,
                                 IEnumerable<CharacterTemplateEquipmentItem> equipment, IEnumerable<IQuest<User>> quests,
                                 IEnumerable<SkillType> knownSkills)
        {
            _templateTable = templateTable;

            if (templateTable == null)
                throw new ArgumentNullException("templateTable");
            if (inventory == null)
                throw new ArgumentNullException("inventory");
            if (equipment == null)
                throw new ArgumentNullException("equipment");
            if (knownSkills == null)
                throw new ArgumentNullException("knownSkills");

            // Compact all the collections given to us to ensure they are immutable and have minimal overhead
            _inventory = inventory.ToCompact();
            _equipment = equipment.ToCompact();
            _quests = quests.ToCompact();
            _knownSkills = knownSkills.ToCompact();

            // Assert values are valid
            Debug.Assert(_inventory.All(x => x != null));
            Debug.Assert(_equipment.All(x => x != null));
            Debug.Assert(_quests.All(x => x != null));
            Debug.Assert(_knownSkills.All(EnumHelper<SkillType>.IsDefined),
                string.Format("One or more SkillTypes for CharacterTemplate `{0}` are invalid!", this));

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
        /// Gets the skills known by the <see cref="CharacterTemplate"/>.
        /// </summary>
        public IEnumerable<SkillType> KnownSkills
        {
            get { return _knownSkills; }
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
            return TemplateTable.Name + " [" + TemplateTable.ID + "]";
        }
    }
}