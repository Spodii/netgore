using System.Linq;

namespace DemoGame.Server.Queries
{
    public class SelectCharacterTemplateInventoryQueryValues
    {
        public ItemChance Chance { get; private set; }
        public CharacterTemplateID CharacterTemplateID { get; private set; }
        public ItemTemplateID ItemTemplateID { get; private set; }
        public byte Max { get; private set; }
        public byte Min { get; private set; }

        public SelectCharacterTemplateInventoryQueryValues(CharacterTemplateID character, ItemTemplateID item, byte min, byte max,
                                                           ItemChance chance)
        {
            CharacterTemplateID = character;
            ItemTemplateID = item;
            Min = min;
            Max = max;
            Chance = chance;
        }
    }
}