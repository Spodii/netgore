using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// Describes all the parts of a conversation that can take place with an NPC.
    /// This class is immutable and intended for use in the Client only.
    /// </summary>
    public class ClientNPCChatDialog : NPCChatDialogBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        NPCChatDialogID _id;
        NPCChatDialogItemBase[] _items;
        string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientNPCChatDialog"/> class.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        internal ClientNPCChatDialog(IValueReader reader) : base(reader)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the unique index of this NPCChatDialogBase. This is used to
        /// distinguish each NPCChatDialogBase from one another.
        /// </summary>
        public override NPCChatDialogID ID
        {
            get { return _id; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the title for this dialog. The title is primarily
        /// used for debugging and development purposes only.
        /// </summary>
        public override string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// When overridden in the derived class, creates an NPCChatDialogItemBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>An NPCChatDialogItemBase created using the given IValueReader.</returns>
        protected override NPCChatDialogItemBase CreateDialogItem(IValueReader reader)
        {
            return new ClientNPCChatDialogItem(reader);
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatDialogItemBase for the given page number.
        /// </summary>
        /// <param name="chatDialogItemID">The page number of the NPCChatDialogItemBase to get.</param>
        /// <returns>The NPCChatDialogItemBase for the given <paramref name="chatDialogItemID"/>, or null if
        /// no valid NPCChatDialogItemBase existed for the given <paramref name="chatDialogItemID"/> or if
        /// the <paramref name="chatDialogItemID"/> is equal to
        /// <see cref="NPCChatResponseBase.EndConversationPage"/>.</returns>
        public override NPCChatDialogItemBase GetDialogItem(NPCChatDialogItemID chatDialogItemID)
        {
            if (chatDialogItemID == NPCChatResponseBase.EndConversationPage)
                return null;

            if (chatDialogItemID < 0 || chatDialogItemID >= _items.Length)
            {
                const string errmsg = "Invalid NPCChatDialogItemBase ID `{0}`.";
                Debug.Fail(string.Format(errmsg, chatDialogItemID));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, chatDialogItemID);
                return null;
            }

            return _items[(int)chatDialogItemID];
        }

        /// <summary>
        /// When overridden in the derived class, gets an IEnumerable of the NPCChatDialogItemBases in this
        /// NPCChatDialogBase.
        /// </summary>
        /// <returns>An IEnumerable of the NPCChatDialogItemBases in this NPCChatDialogBase.</returns>
        protected override IEnumerable<NPCChatDialogItemBase> GetDialogItems()
        {
            return _items;
        }

        /// <summary>
        /// When overridden in the derived class, gets the initial NPCChatDialogItemBase that is used at the
        /// start of a conversation.
        /// </summary>
        /// <returns>The initial NPCChatDialogItemBase that is used at the start of a conversation.</returns>
        public override NPCChatDialogItemBase GetInitialDialogItem()
        {
            Debug.Assert(_items[0] != null, "Why is the first dialog page null!?");

            return _items[0];
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="title">The title.</param>
        /// <param name="items">The dialog items.</param>
        protected override void SetReadValues(NPCChatDialogID id, string title, IEnumerable<NPCChatDialogItemBase> items)
        {
            Debug.Assert(
                _id == default(NPCChatDialogID) && _title == default(string) &&
                _items == default(IEnumerable<NPCChatDialogItemBase>), "Values were already set?");

            _id = id;
            _title = title;
            _items = items.ToArray();
        }
    }
}