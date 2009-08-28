using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.NPCChat
{
    /// <summary>
    /// Describes a single page of dialog in a NPCChatDialogBase, and the possible responses available for the page.
    /// </summary>
    public abstract class NPCChatDialogItemBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
        /// When overridden in the derived class, gets the NPCChatResponseBase of the response with the given
        /// <paramref name="responseIndex"/>.
        /// </summary>
        /// <param name="responseIndex">Index of the response.</param>
        /// <returns>The NPCChatResponseBase for the response at index <paramref name="responseIndex"/>, or null
        /// if the response is invalid or ends the chat dialog.</returns>
        public abstract NPCChatResponseBase GetResponse(byte responseIndex);

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
        /// Creates an ArgumentOutOfRangeException for the response index being out of range.
        /// </summary>
        /// <param name="parameterName">The name of the response index parameter.</param>
        /// <param name="value">The specified, invalid response index value.</param>
        /// <returns>An ArgumentOutOfRangeException for the response index being out of range.</returns>
        protected ArgumentOutOfRangeException CreateInvalidResponseIndexException(string parameterName, ushort value)
        {
            const string errmsg = "Response index `{0}` was out of range for dialog item `{1}`.";
            string err = string.Format(errmsg, value, Index);
            if (log.IsErrorEnabled)
                log.Error(err);

            return new ArgumentOutOfRangeException(err, parameterName);
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatResponseBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>A NPCChatResponseBase created using the given IValueReader</returns>
        protected abstract NPCChatResponseBase CreateResponse(IValueReader reader);

        /// <summary>
        /// When overridden in the derived class, gets the index of the next NPCChatDialogItemBase to use from
        /// the given response.
        /// </summary>
        /// <param name="user">The user that is participating in the chatting.</param>
        /// <param name="npc">The NPC chat is participating in the chatting.</param>
        /// <param name="responseIndex">The index of the response used.</param>
        /// <returns>The index of the NPCChatDialogItemBase to go to based off of the given response.</returns>
        public abstract ushort GetNextPage(object user, object npc, byte responseIndex);

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

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(string.Format("{0} [Index: {1}, Title: {2}]", GetType().Name, Index, Title));
        }
    }
}