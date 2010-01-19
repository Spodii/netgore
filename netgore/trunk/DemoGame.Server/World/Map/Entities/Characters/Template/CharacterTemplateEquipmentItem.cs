using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;

namespace DemoGame.Server
{
    public class CharacterTemplateEquipmentItem
    {
        public CharacterTemplateEquipmentItem(IItemTemplateTable itemTemplate, ItemChance chance)
        {
            ItemTemplate = itemTemplate;
            Chance = chance;
        }

        public ItemChance Chance { get; private set; }
        public IItemTemplateTable ItemTemplate { get; private set; }
    }
}