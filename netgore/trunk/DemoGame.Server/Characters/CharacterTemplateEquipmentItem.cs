using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DemoGame.Server
{
    public class CharacterTemplateEquipmentItem
    {
        public ItemTemplate ItemTemplate { get; private set; }
        public ItemChance Chance { get; private set; }
        public EquipmentSlot Slot { get; private set; }

        public CharacterTemplateEquipmentItem(ItemTemplate itemTemplate, ItemChance chance, EquipmentSlot slot)
        {
            ItemTemplate = itemTemplate;
            Chance = chance;
            Slot = slot;
        }

        /// <summary>
        /// Creates an instance of the item from the template.
        /// </summary>
        /// <returns>The instance of the item, or null if the creation chance failed.</returns>
        public ItemEntity CreateInstance()
        {
            if (!Chance.Test())
                return null;

            ItemEntity instance = new ItemEntity(ItemTemplate, Vector2.Zero, 1);
            return instance;
        }
    }
}
