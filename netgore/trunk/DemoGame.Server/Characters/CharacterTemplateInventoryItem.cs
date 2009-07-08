using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DemoGame.Server
{
    public class CharacterTemplateInventoryItem
    {
        static readonly Random _random = new Random();

        public ItemTemplate ItemTemplate { get; private set; }
        public byte Min { get; private set; }
        public byte Max { get; private set; }
        public ItemChance Chance { get; private set; }

        public CharacterTemplateInventoryItem(ItemTemplate itemTemplate, byte min, byte max, ItemChance chance)
        {
            // TODO: If Min > Max, swap the values, but also have a log error + debug fail

            ItemTemplate = itemTemplate;
            Min = min;
            Max = max;
            Chance = chance;
        }

        /// <summary>
        /// Creates an instance of the item from the template.
        /// </summary>
        /// <returns>The instance of the item, or null if the creation chance failed.</returns>
        public ItemEntity CreateInstance()
        {
            if (!Chance.Test())
                return null;

            byte amount = (byte)_random.Next(Min, Max + 1);
            if (amount == 0)
                return null;

            ItemEntity instance = new ItemEntity(ItemTemplate, Vector2.Zero, amount);
            return instance;
        }
    }
}
