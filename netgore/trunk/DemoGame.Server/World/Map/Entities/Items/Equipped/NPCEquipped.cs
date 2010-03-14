using System.Linq;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains and handles the collection of a single <see cref="NPC"/>'s equipped items.
    /// </summary>
    public class NPCEquipped : CharacterEquipped
    {
        // ReSharper disable SuggestBaseTypeForParameter

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCEquipped"/> class.
        /// </summary>
        /// <param name="npc">The NPC.</param>
        public NPCEquipped(NPC npc) : base(npc)
        {
        }

        // ReSharper restore SuggestBaseTypeForParameter
    }
}