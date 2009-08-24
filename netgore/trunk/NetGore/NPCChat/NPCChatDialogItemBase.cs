using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

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

        /// <summary>
        /// NPCChatDialogItemBase constructor.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        protected NPCChatDialogItemBase(IValueReader reader)
        {
            Read(reader);
        }

        /// <summary>
        /// NPCChatDialogItemBase constructor.
        /// </summary>
        protected NPCChatDialogItemBase()
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatResponseBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>A NPCChatResponseBase created using the given IValueReader</returns>
        protected abstract NPCChatResponseBase CreateResponse(IValueReader reader);

        /// <summary>
        /// Reads the values for this NPCChatDialogItemBase from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        public void Read(IValueReader reader)
        {
            ushort index = reader.ReadUShort("Index");
            string title = reader.ReadString("Title");
            string text = reader.ReadString("Text");
            byte responseCount = reader.ReadByte("ResponseCount");

            var responseReaders = reader.ReadNodes("Response", responseCount);
            int i = 0;
            var responses = new NPCChatResponseBase[responseCount];
            foreach (IValueReader r in responseReaders)
            {
                responses[i] = CreateResponse(r);
                i++;
            }

            SetReadValues(index, title, text, responses);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="page">The index.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="responses">The responses.</param>
        protected abstract void SetReadValues(ushort page, string title, string text, IEnumerable<NPCChatResponseBase> responses);

        /// <summary>
        /// Writes the NPCChatDialogItemBase's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write("Index", Index);
            writer.Write("Title", Title ?? string.Empty);
            writer.Write("Text", Text ?? string.Empty);
            writer.Write("ResponseCount", (byte)Responses.Count());

            foreach (NPCChatResponseBase response in Responses)
            {
                writer.WriteStartNode("Response");
                response.Write(writer);
                writer.WriteEndNode("Response");
            }
        }

        #region INPCChatDialogItem Members

        public abstract ushort GetNextPage(object user, object npc, byte responseIndex);

        #endregion
    }
}