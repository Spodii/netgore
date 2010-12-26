using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Features.NPCChat.Conditionals;
using NetGore.IO;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Describes a single page of dialog in a <see cref="NPCChatDialogBase"/>, and the possible responses available for the page.
    /// This class is immutable and intended for use in the Client only.
    /// </summary>
    public class ClientNPCChatDialogItem : NPCChatDialogItemBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        NPCChatDialogItemID _id;
        NPCChatResponseBase[] _responses;
        string _text;
        string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientNPCChatDialogItem"/> class.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        internal ClientNPCChatDialogItem(IValueReader reader) : base(reader)
        {
        }

        /// <summary>
        /// Not used by the Client and always returns null.
        /// </summary>
        public override NPCChatConditionalCollectionBase Conditionals
        {
            get { return null; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the page index of this <see cref="NPCChatDialogItemBase"/> in the
        /// <see cref="NPCChatDialogBase"/>. This value is unique to each <see cref="NPCChatDialogItemBase"/> in the
        /// <see cref="NPCChatDialogBase"/>.
        /// </summary>
        public override NPCChatDialogItemID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// Unused by the Client and will always return false.
        /// </summary>
        public override bool IsBranch
        {
            get { return false; }
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of the <see cref="NPCChatResponseBase"/>s available
        /// for this page of dialog.
        /// </summary>
        public override IEnumerable<NPCChatResponseBase> Responses
        {
            get { return _responses; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the main dialog text in this page of dialog. If
        /// <see cref="NPCChatDialogItemBase.IsBranch"/> is true, this should return an empty string.
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
        /// When overridden in the derived class, creates a NPCChatConditionalCollectionBase.
        /// </summary>
        /// <returns>A new NPCChatConditionalCollectionBase instance, or null if the derived class does not
        /// want to load the conditionals when using Read.</returns>
        protected override NPCChatConditionalCollectionBase CreateConditionalCollection()
        {
            return null;
        }

        /// <summary>
        /// When overridden in the derived class, creates a NPCChatResponseBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>A NPCChatResponseBase created using the given IValueReader</returns>
        protected override NPCChatResponseBase CreateResponse(IValueReader reader)
        {
            return new ClientNPCChatResponse(reader);
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
                    log.ErrorFormat(errmsg, responseIndex, ID, _responses.Length - 1);
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
        /// <param name="isBranch">The IsBranch value.</param>
        /// <param name="responses">The responses.</param>
        /// <param name="conditionals">The conditionals.</param>
        protected override void SetReadValues(NPCChatDialogItemID page, string title, string text, bool isBranch,
                                              IEnumerable<NPCChatResponseBase> responses,
                                              NPCChatConditionalCollectionBase conditionals)
        {
            Debug.Assert(
                _id == default(NPCChatDialogItemID) && _title == default(string) && _text == default(string) &&
                _responses == default(IEnumerable<NPCChatResponseBase>), "Values were already set?");

            _id = page;
            _title = title;
            _text = text;
            _responses = responses.ToArray();
        }
    }
}