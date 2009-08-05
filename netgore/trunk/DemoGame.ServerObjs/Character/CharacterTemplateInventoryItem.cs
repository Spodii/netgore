using System.Linq;

namespace DemoGame.Server
{
    public class CharacterTemplateInventoryItem
    {
        public ItemChance Chance { get; private set; }
        public ItemTemplate ItemTemplate { get; private set; }
        public byte Max { get; private set; }
        public byte Min { get; private set; }

        public CharacterTemplateInventoryItem(ItemTemplate itemTemplate, byte min, byte max, ItemChance chance)
        {
            // TODO: If Min > Max, swap the values, but also have a log error + debug fail

            ItemTemplate = itemTemplate;
            Min = min;
            Max = max;
            Chance = chance;
        }
    }
}