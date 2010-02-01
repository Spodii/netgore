using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using NetGore.NPCChat;

namespace NetGore.EditorTools.NPCChat
{
    /// <summary>
    /// Describes all the parts of a conversation that can take place with an NPC.
    /// </summary>
    public class EditorNPCChatDialog : NPCChatDialogBase
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        ushort _index;
        EditorNPCChatDialogItem[] _items = new EditorNPCChatDialogItem[8];
        string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatDialog"/> class.
        /// </summary>
        public EditorNPCChatDialog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorNPCChatDialog"/> class.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public EditorNPCChatDialog(IValueReader reader) : base(reader)
        {
        }

        /// <summary>
        /// Notifies listeners when the <see cref="EditorNPCChatDialog"/> has changed.
        /// </summary>
        public event EditorNPCChatDialogEventHandler Changed;

        /// <summary>
        /// When overridden in the derived class, gets the unique index of this NPCChatDialogBase. This is used to
        /// distinguish each NPCChatDialogBase from one another.
        /// </summary>
        public override ushort Index
        {
            get { return _index; }
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="EditorNPCChatDialogItem"/>s in this <see cref="EditorNPCChatDialog"/>.
        /// </summary>
        public IEnumerable<EditorNPCChatDialogItem> Items
        {
            get { return _items.Where(x => x != null); }
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
        /// Adds multiple <see cref="EditorNPCChatDialogItem"/>s to this <see cref="EditorNPCChatDialog"/>.
        /// </summary>
        /// <param name="items">The <see cref="EditorNPCChatDialogItem"/>s to add.</param>
        public void Add(IEnumerable<EditorNPCChatDialogItem> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        /// <summary>
        /// Adds a <see cref="EditorNPCChatDialogItem"/> to this <see cref="EditorNPCChatDialog"/>.
        /// </summary>
        /// <param name="item">The <see cref="EditorNPCChatDialogItem"/> to add.</param>
        public void Add(EditorNPCChatDialogItem item)
        {
            ResizeArrayToFitIndex(ref _items, item.Index);

            if (_items[item.Index] == item)
                return;

            _items[item.Index] = item;

            if (Changed != null)
                Changed(this);
        }

        /// <summary>
        /// When overridden in the derived class, creates an NPCChatDialogItemBase using the given IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        /// <returns>An NPCChatDialogItemBase created using the given IValueReader.</returns>
        protected override NPCChatDialogItemBase CreateDialogItem(IValueReader reader)
        {
            return new EditorNPCChatDialogItem(reader);
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
            return GetDialogItemCasted(chatDialogItemIndex);
        }

        /// <summary>
        /// Same as <see cref="GetDialogItem"/>, but gets the <see cref="NPCChatDialogItemBase"/> as a
        /// <see cref="EditorNPCChatDialogItem"/>.
        /// </summary>
        /// <param name="chatDialogItemIndex">The <see cref="EditorNPCChatDialogItem"/> index.</param>
        /// <returns>The <see cref="EditorNPCChatDialogItem"/> with the given index
        /// <paramref name="chatDialogItemIndex"/>.</returns>
        public EditorNPCChatDialogItem GetDialogItemCasted(ushort chatDialogItemIndex)
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
            return Items.Cast<NPCChatDialogItemBase>();
        }

        /// <summary>
        /// Gets the next free index for a <see cref="EditorNPCChatDialogItem"/>.
        /// </summary>
        /// <returns>The next free index for a <see cref="EditorNPCChatDialogItem"/>.</returns>
        public ushort GetFreeDialogItemIndex()
        {
            for (var i = 0; i < _items.Length; i++)
            {
                if (_items[i] == null)
                    return (ushort)i;
            }

            return (ushort)_items.Length;
        }

        /// <summary>
        /// When overridden in the derived class, gets the initial NPCChatDialogItemBase that is used at the
        /// start of a conversation.
        /// </summary>
        /// <returns>The initial NPCChatDialogItemBase that is used at the start of a conversation.</returns>
        public override NPCChatDialogItemBase GetInitialDialogItem()
        {
            return GetInitialDialogItemCasted();
        }

        /// <summary>
        /// Same as <see cref="GetInitialDialogItem"/> but gets the <see cref="NPCChatDialogItemBase"/> as
        /// a <see cref="EditorNPCChatDialogItem"/>.
        /// </summary>
        /// <returns>The initial EditorNPCChatDialogItem that is used at the start of a conversation.</returns>
        public EditorNPCChatDialogItem GetInitialDialogItemCasted()
        {
            return GetDialogItemCasted(0);
        }

        /// <summary>
        /// Removes a EditorNPCChatDialogItem.
        /// </summary>
        /// <param name="dialogItem">EditorNPCChatDialogItem to remove.</param>
        /// <returns>True if the <paramref name="dialogItem"/> was successfully removed; otherwise false.</returns>
        public bool RemoveDialogItem(EditorNPCChatDialogItem dialogItem)
        {
            // Find the responses that reference this dialog
            var sourceResponses = GetSourceResponses(dialogItem).Cast<EditorNPCChatResponse>();

            // Remove the dialog from the collection
            if (_items[dialogItem.Index] != dialogItem)
                return false;

            _items[dialogItem.Index] = null;

            // Remove references to the dialog
            foreach (var r in sourceResponses)
            {
                r.SetPage(NPCChatResponseBase.EndConversationPage);
            }

            return true;
        }

        /// <summary>
        /// Ensures an array is large enough to fit the given <paramref name="index"/>.
        /// </summary>
        /// <typeparam name="T">The Type of array.</typeparam>
        /// <param name="array">The array.</param>
        /// <param name="index">The index needed to fit in the array.</param>
        static void ResizeArrayToFitIndex<T>(ref T[] array, int index)
        {
            if (array.Length > index)
                return;

            var newSize = array.Length;
            while (newSize <= index)
            {
                newSize <<= 1;
            }

            Array.Resize(ref array, newSize);
        }

        /// <summary>
        /// Sets the <see cref="Index"/>.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void SetIndex(ushort value)
        {
            _index = value;
        }

        /// <summary>
        /// When overridden in the derived class, sets the values read from the Read method.
        /// </summary>
        protected override void SetReadValues(ushort index, string title, IEnumerable<NPCChatDialogItemBase> items)
        {
            _index = index;
            _title = title;

            // Clear the array
            for (var i = 0; i < _items.Length; i++)
            {
                _items[i] = null;
            }

            // Set the new items
            foreach (var item in items)
            {
                Add((EditorNPCChatDialogItem)item);
            }
        }

        /// <summary>
        /// Sets the <see cref="Title"/>.
        /// </summary>
        /// <param name="value">The new value.</param>
        public void SetTitle(string value)
        {
            _title = value;
        }
    }
}