using System.Linq;
using NetGore.IO;
using NetGore.NPCChat.Conditionals;

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
        /// When overridden in the derived class, gets the NPCChatConditionalCollectionBase that contains the
        /// conditionals used to evaluate if this NPCChatResponseBase may be used. If this value is null, it
        /// is assumed that there are no conditionals attached to this NPCChatResponseBase, and should be treated
        /// the same way as if the conditionals evaluated to true.
        /// </summary>
        public abstract NPCChatConditionalCollectionBase Conditionals { get; }

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
        /// When overridden in the derived class, gets the 0-based response index value for this response.
        /// </summary>
        public abstract byte Value { get; }

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
        /// Checks if the conditionals to use this NPCChatResponseBase pass for the given <paramref name="user"/>
        /// and <paramref name="npc"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="npc">The NPC.</param>
        /// <returns>True if the conditionals to use this NPCChatResponseBase pass for the given <paramref name="user"/>
        /// and <paramref name="npc"/>; otherwise false.</returns>
        public bool CheckConditionals(object user, object npc)
        {
            NPCChatConditionalCollectionBase c = Conditionals;
            if (c == null)
                return true;

            return Conditionals.Evaluate(user, npc);
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatConditionalCollectionBase.
        /// </summary>
        /// <returns>A new NPCChatConditionalCollectionBase instance, or null if the derived class does not
        /// want to load the conditionals when using Read.</returns>
        protected abstract NPCChatConditionalCollectionBase CreateConditionalCollection();

        /// <summary>
        /// Reads the values for this NPCChatResponseBase from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        protected void Read(IValueReader reader)
        {
            byte value = reader.ReadByte("Value");
            ushort page = reader.ReadUShort("Page");
            string text = reader.ReadString("Text");

            IValueReader cReader = reader.ReadNode("Conditionals");
            bool hasConditionals = cReader.ReadBool("HasConditionals");
            NPCChatConditionalCollectionBase conditionals = null;
            if (hasConditionals)
            {
                conditionals = CreateConditionalCollection();
                if (conditionals != null)
                    conditionals.Read(cReader);
            }

            SetReadValues(value, page, text, conditionals);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="page">The page.</param>
        /// <param name="text">The text.</param>
        /// <param name="conditionals">The conditionals.</param>
        protected abstract void SetReadValues(byte value, ushort page, string text, NPCChatConditionalCollectionBase conditionals);

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(string.Format("{0} [Value: {1}, Text: {2}]", GetType().Name, Value, Text));
        }

        /// <summary>
        /// Writes the NPCChatDialogItemBase's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write("Value", Value);
            writer.Write("Page", Page);
            writer.Write("Text", Text ?? string.Empty);

            writer.WriteStartNode("Conditionals");
            {
                NPCChatConditionalCollectionBase c = Conditionals;
                bool hasConditionals = (c != null) && (c.Count() > 0);
                writer.Write("HasConditionals", hasConditionals);
                if (hasConditionals)
                    c.Write(writer);
            }
            writer.WriteEndNode("Conditionals");
        }
    }
}