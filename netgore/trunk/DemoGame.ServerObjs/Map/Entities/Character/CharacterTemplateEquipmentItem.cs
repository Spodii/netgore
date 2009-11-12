using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore;

namespace DemoGame.Server
{
    public class CharacterTemplateEquipmentItem
    {
        public ItemChance Chance { get; private set; }
        public IItemTemplateTable ItemTemplate { get; private set; }

        public CharacterTemplateEquipmentItem(IItemTemplateTable itemTemplate, ItemChance chance)
        {
            ItemTemplate = itemTemplate;
            Chance = chance;
        }
    }
}