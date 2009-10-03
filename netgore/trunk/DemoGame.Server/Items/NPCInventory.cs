using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame.Server
{
    public class NPCInventory : CharacterInventory
    {
        // ReSharper disable SuggestBaseTypeForParameter
        public NPCInventory(NPC npc) : base(npc) // ReSharper restore SuggestBaseTypeForParameter
        {
        }
    }
}