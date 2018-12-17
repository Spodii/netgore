using System.Linq;
using SFML.Graphics;

namespace DemoGame.Server
{
    public static class CharacterTemplateEquipmentItemExtensions
    {
        /// <summary>
        /// Creates an instance of the <see cref="ItemEntity"/> from the template.
        /// </summary>
        /// <returns>The instance of the <see cref="ItemEntity"/>, or null if the creation chance failed.</returns>
        public static ItemEntity CreateInstance(this CharacterTemplateEquipmentItem v)
        {
            if (!v.Chance.Test())
                return null;

            var instance = new ItemEntity(v.ItemTemplate, Vector2.Zero, 1);
            return instance;
        }
    }
}