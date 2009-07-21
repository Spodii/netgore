using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DemoGame.Server
{
    public static class CharacterTemplateEquipmentItemExtensions
    {
        /// <summary>
        /// Creates an instance of the item from the template.
        /// </summary>
        /// <returns>The instance of the item, or null if the creation chance failed.</returns>
        public static ItemEntity CreateInstance(this CharacterTemplateEquipmentItem v)
        {
            if (!v.Chance.Test())
                return null;

            ItemEntity instance = new ItemEntity(v.ItemTemplate, Vector2.Zero, 1);
            return instance;
        }
    }
}
