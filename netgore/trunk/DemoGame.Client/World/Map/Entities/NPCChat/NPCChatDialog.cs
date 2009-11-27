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
    /// Describes all the parts of a conversation that can take place with an NPC.
    /// This class is immutable and intended for use in the Client only.
    /// </summary>
    public class NPCChatDialog : NPCChatDialogBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ushort _index;
        NPCChatDialogItemBase[] _items;
        string _title;

        /// <summary>
        /// NPCChatDialog constructor.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        internal NPCChatDialog(IValueReader reader) : base(reader)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the unique index of this NPCChatDialogBase. This is used to
        /// distinguish each NPCChatDialogBase from one another.
        /// </summary>
        public override ushort Index
        {
            get { return _index; }
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
            return new NPCChatDialogItem(reader);
        }

        /// <summary>
        /// When overridden in the derived class, gets the NPCChatDialogItemBase for the given page number.
        /// </summary>
        /// <param name="chatDialogItemIndex">The page number of the NPCChatDialogItemBase to get.</param>
        /// <returns>The NPCChatDialogItemBase for the given <paramref name="chatDialogItemIndex"/>, or null if
        /// no valid NPCChatDialogItemBase existed for the given <paramref name="chatDialogItemIndex"/> or if
        /// the <paramref name="chatDialogItemIndex"/> is equal to
        /// <see cref="NPCChatResponseBase.EndConversationPage"/>.</returns>
        public override NPCChatDialogItemBase GetDialogItem(ushort chatDialogItemIndex)
        {
            if (chatDialogItemIndex == NPCChatResponseBase.EndConversationPage)
                return null;

            if (chatDialogItemIndex < 0 || chatDialogItemIndex >= _items.Length)
            {
                const string errmsg = "Invalid NPCChatDialogItemBase index `{0}`.";
                Debug.Fail(string.Format(errmsg, chatDialogItemIndex));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, chatDialogItemIndex);
                return null;
            }

            return _items[chatDialogItemIndex];
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
        /// <param name="index">The index.</param>
        /// <param name="title">The title.</param>
        /// <param name="items">The dialog items.</param>
        protected override void SetReadValues(ushort index, string title, IEnumerable<NPCChatDialogItemBase> items)
        {
            Debug.Assert(
                _index == default(ushort) && _title == default(string) && _items == default(IEnumerable<NPCChatDialogItemBase>),
                "Values were already set?");

            _index = index;
            _title = title;
            _items = items.ToArray();
        }
    }
}