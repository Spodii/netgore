using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.Server
{
    public class NPCStats : CharacterStatsBase
    {
        readonly NPC _npc;

        public NPC NPC
        {
            get { return _npc; }
        }

        public NPCStats()
        {
            CreateStats();
        }

        public NPCStats(NPC npc)
        {
            _npc = npc;
            CreateStats();
        }

        protected override IStat CreateBaseStat<T>(StatType statType)
        {
            return new Stat<T>(statType);
        }

        protected override IStat CreateModStat<T>(StatType statType)
        {
            ModStatHandler modStatHandler = GetModStatHandler(statType);
            return new ModStat<T>(statType, modStatHandler);
        }
    }
}