using System.Linq;
using NetGore.IO;

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

        /// <summary>
        /// Writes the NPCChatDialogItemBase's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write("Page", Page);
            writer.Write("Text", Text ?? string.Empty);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="text">The text.</param>
        protected abstract void SetReadValues(ushort page, string text);

        /// <summary>
        /// NPCChatResponseBase constructor.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        protected NPCChatResponseBase(IValueReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// NPCChatResponseBase constructor.
        /// </summary>
        protected NPCChatResponseBase()
        {
        }

        /// <summary>
        /// Reads the values for this NPCChatResponseBase from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        public void Read(IValueReader reader)
        {
            ushort page = reader.ReadUShort("Page");
            string text = reader.ReadString("Text");

            SetReadValues(page, text);
        }
    }
}