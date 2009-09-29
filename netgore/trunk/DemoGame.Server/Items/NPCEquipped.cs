using System.Linq;
using DemoGame;
using NetGore;
using NetGore.RPGComponents;

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