using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server.Queries
{
    public class SelectCharacterTemplateEquippedQueryValues
    {
        public ItemTemplateID ItemTemplateID { get; private set; }
        public CharacterTemplateID CharacterTemplateID { get; private set; }
        public EquipmentSlot Slot { get; private set; }
        public ItemChance Chance { get; private set; }

        public SelectCharacterTemplateEquippedQueryValues(CharacterTemplateID character, ItemTemplateID item,
            EquipmentSlot slot, ItemChance chance)
        {
            CharacterTemplateID = character;
            ItemTemplateID = item;
            Slot = slot;
            Chance = chance;
        }
    }
}
