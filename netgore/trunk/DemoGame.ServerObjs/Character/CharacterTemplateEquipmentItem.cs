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

        public CharacterTemplateEquipmentItem(ItemTemplate itemTemplate, ItemChance chance)
        {
            ItemTemplate = itemTemplate;
            Chance = chance;
        }
    }
}
