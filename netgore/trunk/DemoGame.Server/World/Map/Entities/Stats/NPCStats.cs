using System.Linq;

namespace DemoGame.Server
{
    public class NPCStats : CharacterStats
    {
        readonly NPC _npc;

        public NPCStats(NPC npc, StatCollectionType statCollectionType) : base(statCollectionType)
        {
            _npc = npc;
        }

        public NPC NPC
        {
            get { return _npc; }
        }
    }
}