using System.Linq;

namespace DemoGame.Server
{
    public class CharacterTemplateEquipmentItem
    {
        public ItemChance Chance { get; private set; }
        public ItemTemplate ItemTemplate { get; private set; }

        public CharacterTemplateEquipmentItem(ItemTemplate itemTemplate, ItemChance chance)
        {
            ItemTemplate = itemTemplate;
            Chance = chance;
        }
    }
}