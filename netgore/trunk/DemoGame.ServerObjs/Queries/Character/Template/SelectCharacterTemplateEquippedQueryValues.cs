using System.Linq;

namespace DemoGame.Server.Queries
{
    public class SelectCharacterTemplateEquippedQueryValues
    {
        public ItemChance Chance { get; private set; }
        public CharacterTemplateID CharacterTemplateID { get; private set; }
        public ItemTemplateID ItemTemplateID { get; private set; }

        public SelectCharacterTemplateEquippedQueryValues(CharacterTemplateID character, ItemTemplateID item, ItemChance chance)
        {
            CharacterTemplateID = character;
            ItemTemplateID = item;
            Chance = chance;
        }
    }
}