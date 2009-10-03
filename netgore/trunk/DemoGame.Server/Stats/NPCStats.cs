using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame.Server
{
    public class NPCStats : CharacterStats
    {
        readonly NPC _npc;

        public NPC NPC
        {
            get { return _npc; }
        }

        public NPCStats(NPC npc, StatCollectionType statCollectionType) : base(statCollectionType)
        {
            _npc = npc;
        }
    }
}