using System.Linq;

namespace DemoGame.Server
{
    public class NPCEquipped : CharacterEquipped
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NPCEquipped"/> class.
        /// </summary>
        /// <param name="npc">The NPC.</param>
        public NPCEquipped(Character npc) : base(npc)
        {
        }
    }
}