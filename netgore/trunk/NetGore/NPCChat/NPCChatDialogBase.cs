using System;
using System.Linq;
using System.Text;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Describes all the parts of a conversation that can take place with an NPC.
    /// </summary>
    public abstract class NPCChatDialogBase
    {
        /// <summary>
        /// When overridden in the derived class, gets the NPCChatDialogItemBase for the given page number.
        /// </summary>
        /// <param name="page">The page number of the NPCChatDialogItemBase to get.</param>
        /// <returns>The NPCChatDialogItemBase for the given <paramref name="page"/>, or null if no valid
        /// NPCChatDialogItemBase existed for the given <paramref name="page"/>.</returns>
        public abstract NPCChatDialogItemBase GetDialogItem(ushort page);

        /// <summary>
        /// When overridden in the derived class, gets the initial NPCChatDialogItemBase that is used at the
        /// start of a conversation.
        /// </summary>
        /// <returns>The initial NPCChatDialogItemBase that is used at the start of a conversation.</returns>
        public abstract NPCChatDialogItemBase GetInitialDialogItem();
    }
}
