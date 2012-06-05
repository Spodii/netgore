using System.Linq;

namespace DemoGame.Server
{
    public class NPCInventory : CharacterInventory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NPCInventory"/> class.
        /// </summary>
        /// <param name="npc">The NPC.</param>
        public NPCInventory(Character npc) : base(npc)
        {
        }
    }
}