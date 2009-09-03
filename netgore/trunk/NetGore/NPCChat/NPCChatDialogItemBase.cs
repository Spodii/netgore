using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using NetGore.NPCChat.Conditionals;

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
        /// Checks if the conditionals to use this NPCChatDialogItemBase pass for the given <paramref name="user"/>
        /// and <paramref name="npc"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="npc">The NPC.</param>
        /// <returns>True if the conditionals to use this NPCChatDialogItemBase pass for the given <paramref name="user"/>
        /// and <paramref name="npc"/>; otherwise false.</returns>
        public bool CheckConditionals(object user, object npc)
        {
            NPCChatConditionalCollectionBase c = Conditionals;
            if (c == null)
                return true;

            return Conditionals.Evaluate(user, npc);
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatConditionalCollectionBase that contains the
        /// conditionals used to evaluate if this NPCChatDialogItemBase may be used. If this value is null, it
        /// is assumed that there are no conditionals attached to this NPCChatDialogItemBase, and should be treated
        /// the same way as if the conditionals evaluated to true.
        /// </summary>
        public abstract NPCChatConditionalCollectionBase Conditionals { get; }

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
        /// When overridden in the derived class, gets the NPCChatResponseBase of the response with the given
        /// <paramref name="responseIndex"/>.
        /// </summary>
        /// <param name="responseIndex">Index of the response.</param>
        /// <returns>The NPCChatResponseBase for the response at index <paramref name="responseIndex"/>, or null
        /// if the response is invalid or ends the chat dialog.</returns>
        public abstract NPCChatResponseBase GetResponse(byte responseIndex);

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatConditionalCollectionBase.
        /// </summary>
        /// <returns>A new NPCChatConditionalCollectionBase instance, or null if the derived class does not
        /// want to load the conditionals when using Read.</returns>
        protected abstract NPCChatConditionalCollectionBase CreateConditionalCollection();

        /// <summary>
        /// Reads the values for this NPCChatDialogItemBase from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        protected void Read(IValueReader reader)
        {
            ushort index = reader.ReadUShort("Index");
            string title = reader.ReadString("Title");
            string text = reader.ReadString("Text");
            var responses = reader.ReadManyNodes("Responses", x => CreateResponse(x));

            IValueReader cReader = reader.ReadNode("Conditionals");
            bool hasConditionals = cReader.ReadBool("HasConditionals");
            NPCChatConditionalCollectionBase conditionals = null;
            if (hasConditionals)
            {
                conditionals = CreateConditionalCollection();
                if (conditionals != null)
                    conditionals.Read(cReader);
            }

            SetReadValues(index, title, text, responses, conditionals);
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="page">The index.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="conditionals">The conditionals.</param>
        /// <param name="responses">The responses.</param>
        protected abstract void SetReadValues(ushort page, string title, string text, IEnumerable<NPCChatResponseBase> responses, NPCChatConditionalCollectionBase conditionals);

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

        /// <summary>
        /// Writes the NPCChatDialogItemBase's values to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the values to.</param>
        public void Write(IValueWriter writer)
        {
            writer.Write("Index", Index);
            writer.Write("Title", Title ?? string.Empty);
            writer.Write("Text", Text ?? string.Empty);
            writer.WriteManyNodes("Responses", Responses, ((w, item) => item.Write(w)));

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