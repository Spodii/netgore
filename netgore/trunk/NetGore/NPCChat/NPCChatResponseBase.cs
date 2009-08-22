using System.Linq;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Describes a single response in a NPCChatDialogItemBase.
    /// </summary>
    public abstract class NPCChatResponseBase
    {
        /// <summary>
        /// The page number used for responses to end the conversation.
        /// </summary>
        public const ushort EndConversationPage = ushort.MaxValue;

        /// <summary>
        /// When overridden in the derived class, gets the page of the NPCChatDialogItemBase to go to if this
        /// response is chosen. If this value is equal to the EndConversationPage constant, then the dialog
        /// will end instead of going to a new page.
        /// </summary>
        public abstract ushort Page { get; }

        /// <summary>
        /// When overridden in the derived class, gets the text to display for this response.
        /// </summary>
        public abstract string Text { get; }
    }
}