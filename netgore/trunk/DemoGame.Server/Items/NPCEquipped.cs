using System.Linq;

namespace DemoGame.Server
{
    public class NPCEquipped : CharacterEquipped
    {
        // ReSharper disable SuggestBaseTypeForParameter
        public NPCEquipped(NPC npc) : base(npc) // ReSharper restore SuggestBaseTypeForParameter
        {
        }
    }
}