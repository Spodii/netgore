using System.Linq;
using NetGore;
using SFML.Graphics;

namespace DemoGame.Server
{
    public static class CharacterTemplateInventoryItemExtensions
    {
        static readonly SafeRandom _random = new SafeRandom();

        /// <summary>
        /// Creates an instance of the <see cref="ItemEntity"/> from the template.
        /// </summary>
        /// <returns>The instance of the <see cref="ItemEntity"/>, or null if the creation chance failed.</returns>
        public static ItemEntity CreateInstance(this CharacterTemplateInventoryItem v)
        {
            if (!v.Chance.Test())
                return null;

            var amount = (byte)_random.Next(v.Min, v.Max + 1);
            if (amount == 0)
                return null;

            var instance = new ItemEntity(v.ItemTemplate, Vector2.Zero, amount);
            return instance;
        }
    }
}