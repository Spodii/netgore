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
    /// This class is immutable.
    /// </summary>
    public class ServerNPCChatDialog : NPCChatDialogBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        NPCChatDialogID _id;
        NPCChatDialogItemBase[] _items;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerNPCChatDialog"/> class.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        internal ServerNPCChatDialog(IValueReader reader) : base(reader)
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
        /// This property is not supported by the Server's NPCChatDialog, and will always return String.Empty.
        /// </summary>
        public override string Title
        {
            get
            {
                Debug.Fail("This property is not supported by the Server.");
                return string.Empty;
            }
        }

        /// <summary>
        /// When overridden in the derived class, creates an NPCChatDialogItemBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>An NPCChatDialogItemBase created using the given IValueReader.</returns>
        protected override NPCChatDialogItemBase CreateDialogItem(IValueReader reader)
        {
            return new ServerNPCChatDialogItem(reader);
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatDialogItemBase for the given page number.
        /// </summary>
        /// <param name="chatDialogItemIndex">The page number of the NPCChatDialogItemBase to get.</param>
        /// <returns>The NPCChatDialogItemBase for the given <paramref name="chatDialogItemIndex"/>, or null if
        /// no valid NPCChatDialogItemBase existed for the given <paramref name="chatDialogItemIndex"/> or if
        /// the <paramref name="chatDialogItemIndex"/> is equal to
        /// <see cref="NPCChatResponseBase.EndConversationPage"/>.</returns>
        public override NPCChatDialogItemBase GetDialogItem(NPCChatDialogItemID chatDialogItemIndex)
        {
            if (chatDialogItemIndex == NPCChatResponseBase.EndConversationPage)
                return null;

            if (chatDialogItemIndex < 0 || chatDialogItemIndex >= _items.Length)
            {
                const string errmsg = "Invalid NPCChatDialogItemBase index `{0}`.";
                Debug.Fail(string.Format(errmsg, chatDialogItemIndex));
                if (log.IsWarnEnabled)
                    log.ErrorFormat(errmsg, chatDialogItemIndex);
                return null;
            }

            return _items[(int)chatDialogItemIndex];
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
            Debug.Assert(_id == default(NPCChatDialogID) && _items == default(IEnumerable<NPCChatDialogItemBase>),
                "Values were already set?");

            _id = id;
            _items = items.ToArray();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(string.Format("{0} [Index: {1}]", GetType().Name, ID));
        }
    }
}