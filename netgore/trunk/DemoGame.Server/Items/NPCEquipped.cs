using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server
{
    public class NPCEquipped : CharacterEquipped
    {
// ReSharper disable SuggestBaseTypeForParameter
        public NPCEquipped(NPC npc) : base(npc)
// ReSharper restore SuggestBaseTypeForParameter
        {
        }
    }
}
