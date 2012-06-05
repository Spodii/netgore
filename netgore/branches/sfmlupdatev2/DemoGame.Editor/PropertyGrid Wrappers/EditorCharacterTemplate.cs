using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using DemoGame.DbObjs;
using DemoGame.Server;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using NetGore;
using NetGore.Db;
using NetGore.Features.Quests;

namespace DemoGame.Editor
{
    /// <summary>
    /// A <see cref="CharacterTemplate"/> that is to be used in editors in a <see cref="PropertyGrid"/>.
    /// </summary>
    public class EditorCharacterTemplate : CharacterTemplateTable
    {
        const string _category = "Character Template";

        List<CharacterTemplateEquippedItem> _equipped = new List<CharacterTemplateEquippedItem>();
        List<CharacterTemplateInventoryItem> _inventory = new List<CharacterTemplateInventoryItem>();
        List<SkillType> _knownSkills = new List<SkillType>();
        List<QuestID> _quests = new List<QuestID>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorCharacterTemplate"/> class.
        /// </summary>
        /// <param name="charTemplateTable">The <see cref="ICharacterTemplateTable"/>.</param>
        /// <param name="dbController">The <see cref="IDbController"/>.</param>
        public EditorCharacterTemplate(ICharacterTemplateTable charTemplateTable, IDbController dbController)
        {
            var id = charTemplateTable.ID;

            // Load the main table values
            CopyValuesFrom(charTemplateTable);

            // Load values from other tables
            // Equipped
            var equippedRaw = dbController.GetQuery<SelectCharacterTemplateEquippedQuery>().Execute(id);
            if (equippedRaw != null)
            {
                var equipped = equippedRaw.Select(x => new CharacterTemplateEquippedItem(x.ItemTemplateID, x.Chance));
                _equipped.AddRange(equipped);
            }

            // Inventory
            var inventoryRaw = dbController.GetQuery<SelectCharacterTemplateInventoryQuery>().Execute(id);
            if (inventoryRaw != null)
            {
                var inventory =
                    inventoryRaw.Select(x => new CharacterTemplateInventoryItem(x.ItemTemplateID, x.Chance, x.Min, x.Max));
                _inventory.AddRange(inventory);
            }

            // Known skills
            var knownSkills = dbController.GetQuery<SelectCharacterTemplateSkillsQuery>().Execute(id);
            if (knownSkills != null)
                _knownSkills.AddRange(knownSkills);

            // Quests
            var quests = dbController.GetQuery<SelectCharacterTemplateQuestsQuery>().Execute(id);
            if (quests != null)
                _quests.AddRange(quests);
        }

        /// <summary>
        /// Gets or sets the equipped items that a character can spawn with.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The equipped items that a character can spawn with.")]
        [Category(_category)]
        public List<CharacterTemplateEquippedItem> Equipped
        {
            get
            {
                _equipped.RemoveDuplicates((x, y) => x == y);
                return _equipped;
            }
            set
            {
                _equipped = value ?? new List<CharacterTemplateEquippedItem>();
                _equipped.RemoveDuplicates((x, y) => x == y);
            }
        }

        /// <summary>
        /// Gets or sets the quests that the character may give.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The quests that the character may give.")]
        [Category(_category)]
        public List<QuestID> GiveQuests
        {
            get
            {
                _quests.RemoveDuplicates((x, y) => x == y);
                return _quests;
            }
            set
            {
                _quests = value ?? new List<QuestID>();
                _quests.RemoveDuplicates((x, y) => x == y);
            }
        }

        /// <summary>
        /// Gets or sets the inventory items that a character can spawn with.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The inventory items that a character can spawn with.")]
        [Category(_category)]
        public List<CharacterTemplateInventoryItem> Inventory
        {
            get
            {
                _inventory.RemoveDuplicates((x, y) => x == y);
                return _inventory;
            }
            set
            {
                _inventory = value ?? new List<CharacterTemplateInventoryItem>();
                _inventory.RemoveDuplicates((x, y) => x == y);
            }
        }

        /// <summary>
        /// Gets or sets the skills that the character knows.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Browsable(true)]
        [Description("The skills that the character knows.")]
        [Category(_category)]
        public List<SkillType> KnownSkills
        {
            get
            {
                _knownSkills.RemoveDuplicates((x, y) => x == y);
                return _knownSkills;
            }
            set
            {
                _knownSkills = value ?? new List<SkillType>();
                _knownSkills.RemoveDuplicates((x, y) => x == y);
            }
        }
    }
}