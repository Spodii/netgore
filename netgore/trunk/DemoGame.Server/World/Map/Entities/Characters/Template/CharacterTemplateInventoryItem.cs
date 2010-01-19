using System.Diagnostics;
using System.Linq;
using DemoGame.DbObjs;

namespace DemoGame.Server
{
    public class CharacterTemplateInventoryItem
    {
        public CharacterTemplateInventoryItem(IItemTemplateTable itemTemplate, byte min, byte max, ItemChance chance)
        {
            if (min > max)
            {
                var tmp = min;
                min = max;
                max = tmp;
                Debug.Fail("min was less than max. Swapped to fix the problem, but could indicate a problem elsewhere.");
            }

            ItemTemplate = itemTemplate;
            Min = min;
            Max = max;
            Chance = chance;
        }

        public ItemChance Chance { get; private set; }
        public IItemTemplateTable ItemTemplate { get; private set; }
        public byte Max { get; private set; }
        public byte Min { get; private set; }
    }
}