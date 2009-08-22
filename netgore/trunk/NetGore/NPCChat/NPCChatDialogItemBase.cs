using System.Collections.Generic;
using System.Linq;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Describes a single page of dialog in a NPCChatDialogBase, and the possible responses available for the page.
    /// </summary>
    public abstract class NPCChatDialogItemBase : INPCChatDialogItem
    {
        /// <summary>
        /// When overridden in the derived class, gets the page index of this NPCChatDialogItemBase in the
        /// NPCChatDialogBase. This value is unique to each NPCChatDialogItemBase in the NPCChatDialogBase.
        /// </summary>
        public abstract ushort Index { get; }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of the EditorNPCChatResponses available
        /// for this page of dialog.
        /// </summary>
        public abstract IEnumerable<NPCChatResponseBase> Responses { get; }

        /// <summary>
        /// When overridden in the derived class, gets the main dialog text in this page of dialog.
        /// </summary>
        public abstract string Text { get; }

        /// <summary>
        /// When overridden in the derived class, gets the title for this page of dialog. The title is primarily
        /// used for debugging and development purposes only.
        /// </summary>
        public abstract string Title { get; }

        #region INPCChatDialogItem Members

        public abstract ushort GetNextPage(object user, object npc, byte responseIndex);

        #endregion
    }
}