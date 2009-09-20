using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace DemoGame.Server
{
    public static class CharacterTemplateInventoryItemExtensions
    {
        static readonly Random _random = new Random();

        /// <summary>
        /// Creates an instance of the <see cref="ItemEntity"/> from the template.
        /// </summary>
        /// <returns>The instance of the <see cref="ItemEntity"/>, or null if the creation chance failed.</returns>
        public static ItemEntity CreateInstance(this CharacterTemplateInventoryItem v)
        {
            if (!v.Chance.Test())
                return null;

            byte amount = (byte)_random.Next(v.Min, v.Max + 1);
            if (amount == 0)
                return null;

            ItemEntity instance = new ItemEntity(v.ItemTemplate, Vector2.Zero, amount);
            return instance;
        }
    }
}