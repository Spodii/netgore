using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoGame.Server
{
    public class NPCStats : CharacterStats
    {
        readonly NPC _npc;

        public NPC NPC
        {
            get { return _npc; }
        }

        public NPCStats(NPC npc, StatCollectionType statCollectionType)
            : base(statCollectionType)
        {
            _npc = npc;
        }
    }
}