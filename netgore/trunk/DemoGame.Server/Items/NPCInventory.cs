using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoGame.Server
{
    class NPCInventory : Inventory
    {
// ReSharper disable SuggestBaseTypeForParameter
        public NPCInventory(NPC npc) : base(npc)
// ReSharper restore SuggestBaseTypeForParameter
        {
        }
    }
}