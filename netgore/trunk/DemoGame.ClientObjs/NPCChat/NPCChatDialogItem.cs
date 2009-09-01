using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using NetGore.NPCChat;

namespace DemoGame.Client.NPCChat
{
    /// <summary>
    /// Describes a single page of dialog in a NPCChatDialogBase, and the possible responses available for the page.
    /// This class is immutable and intended for use in the Client only.
    /// </summary>
    public class NPCChatDialogItem : NPCChatDialogItemBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ushort _index;
        NPCChatResponseBase[] _responses;
        string _text;
        string _title;

        /// <summary>
        /// When overridden in the derived class, gets the page index of this NPCChatDialogItemBase in the
        /// NPCChatDialogBase. This value is unique to each NPCChatDialogItemBase in the NPCChatDialogBase.
        /// </summary>
        public override ushort Index
        {
            get { return _index; }
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of the EditorNPCChatResponses available
        /// for this page of dialog.
        /// </summary>
        public override IEnumerable<NPCChatResponseBase> Responses
        {
            get { return _responses; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the main dialog text in this page of dialog.
        /// </summary>
        public override string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the title for this page of dialog. The title is primarily
        /// used for debugging and development purposes only.
        /// </summary>
        public override string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// NPCChatDialogItem constructor.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        internal NPCChatDialogItem(IValueReader r) : base(r)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatResponseBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>A NPCChatResponseBase created using the given IValueReader</returns>
        protected override NPCChatResponseBase CreateResponse(IValueReader reader)
        {
            return new NPCChatResponse(reader);
        }

        /// <summary>
        /// When overridden in the derived class, gets the index of the next NPCChatDialogItemBase to use from
        /// the given response.
        /// </summary>
        /// <param name="user">The user that is participating in the chatting.</param>
        /// <param name="npc">The NPC chat is participating in the chatting.</param>
        /// <param name="responseIndex">The index of the response used.</param>
        /// <returns>The index of the NPCChatDialogItemBase to go to based off of the given response.</returns>
        public override ushort GetNextPage(object user, object npc, byte responseIndex)
        {
            if (responseIndex < 0 || responseIndex >= _responses.Length)
                throw CreateInvalidResponseIndexException("responseIndex", responseIndex);

            return _responses[responseIndex].Page;
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatResponseBase of the response with the given
        /// <paramref name="responseIndex"/>.
        /// </summary>
        /// <param name="responseIndex">Index of the response.</param>
        /// <returns>The NPCChatResponseBase for the response at index <paramref name="responseIndex"/>, or null
        /// if the response is invalid.</returns>
        public override NPCChatResponseBase GetResponse(byte responseIndex)
        {
            if (responseIndex >= _responses.Length)
            {
                const string errmsg = "Invalid response index `{0}` for page `{1}`. Max response index is `{2}`.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, responseIndex, Index, _responses.Length - 1);
                return null;
            }

            return _responses[responseIndex];
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="page">The index.</param>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="responses">The responses.</param>
        protected override void SetReadValues(ushort page, string title, string text, IEnumerable<NPCChatResponseBase> responses)
        {
            Debug.Assert(
                _index == default(ushort) && _title == default(string) && _text == default(string) &&
                _responses == default(IEnumerable<NPCChatResponseBase>), "Values were already set?");

            _index = page;
            _title = title;
            _text = text;
            _responses = responses.ToArray();
        }
    }
}