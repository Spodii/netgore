using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;
using NetGore.NPCChat;

namespace DemoGame.Server.NPCChat
{
    /// <summary>
    /// Describes all the parts of a conversation that can take place with an NPC.
    /// This class is immutable and intended for use in the Server only.
    /// </summary>
    public class NPCChatDialog : NPCChatDialogBase
    {
        ushort _index;
        NPCChatDialogItemBase[] _items;

        /// <summary>
        /// When overridden in the derived class, gets the unique index of this NPCChatDialogBase. This is used to
        /// distinguish each NPCChatDialogBase from one another.
        /// </summary>
        public override ushort Index
        {
            get { return _index; }
        }

        /// <summary>
        /// This property is not supported by the Server's NPCChatDialog.
        /// </summary>
        public override string Title
        {
            get { throw new NotSupportedException("This property is not supported by the Server's NPCChatDialog."); }
        }

        /// <summary>
        /// NPCChatDialog constructor.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        internal NPCChatDialog(IValueReader reader) : base(reader)
        {
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
        /// <param name="page">The page number of the NPCChatDialogItemBase to get.</param>
        /// <returns>The NPCChatDialogItemBase for the given <paramref name="page"/>, or null if no valid
        /// NPCChatDialogItemBase existed for the given <paramref name="page"/>.</returns>
        public override NPCChatDialogItemBase GetDialogItem(ushort page)
        {
            if (page < 0 || page >= _items.Length)
                throw CreateInvalidResponseIndexException("page", page);

            return _items[page];
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
            Debug.Assert(_index == default(ushort) && _items == default(IEnumerable<NPCChatDialogItemBase>),
                         "Values were already set?");

            _index = index;
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
            return string.Format(string.Format("{0} [Index: {1}]", GetType().Name, Index));
        }
    }
}