using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.IO;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public delegate void EditorNPCChatDialogEventHandler(EditorNPCChatDialog dialog);

    /// <summary>
    /// Describes all the parts of a conversation that can take place with an NPC.
    /// </summary>
    public class EditorNPCChatDialog : NPCChatDialogBase
    {
        EditorNPCChatDialogItem[] _dialogItems = new EditorNPCChatDialogItem[8];
        ushort _index;
        string _title;

        /// <summary>
        /// Notifies listeners when the EditorNPCChatDialog has changed.
        /// </summary>
        public event EditorNPCChatDialogEventHandler OnChange;

        /// <summary>
        /// When overridden in the derived class, gets the unique index of this NPCChatDialogBase. This is used to
        /// distinguish each NPCChatDialogBase from one another.
        /// </summary>
        public override ushort Index
        {
            get { return _index; }
        }

        public IEnumerable<EditorNPCChatDialogItem> Items
        {
            get { return _dialogItems.Where(x => x != null); }
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
        /// EditorNPCChatDialog constructor.
        /// </summary>
        public EditorNPCChatDialog()
        {
        }

        /// <summary>
        /// EditorNPCChatDialog constructor.
        /// </summary>
        /// <param name="reader">IValueReader to read the values from.</param>
        public EditorNPCChatDialog(IValueReader reader) : base(reader)
        {
        }

        public void Add(IEnumerable<EditorNPCChatDialogItem> items)
        {
            foreach (EditorNPCChatDialogItem item in items)
            {
                Add(item);
            }
        }

        public void Add(EditorNPCChatDialogItem item)
        {
            ResizeArrayToFitIndex(ref _dialogItems, item.Index);

            if (_dialogItems[item.Index] == item)
                return;

            _dialogItems[item.Index] = item;

            if (OnChange != null)
                OnChange(this);
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
        /// <param name="page">The page number of the NPCChatDialogItemBase to get.</param>
        /// <returns>The NPCChatDialogItemBase for the given <paramref name="page"/>, or null if no valid
        /// NPCChatDialogItemBase existed for the given <paramref name="page"/>.</returns>
        public override NPCChatDialogItemBase GetDialogItem(ushort page)
        {
            return GetDialogItemCasted(page);
        }

        public EditorNPCChatDialogItem GetDialogItemCasted(ushort page)
        {
            if (page == EditorNPCChatResponse.EndConversationPage)
                return null;

            return _dialogItems[page];
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

        public ushort GetFreeDialogItemIndex()
        {
            for (int i = 0; i < _dialogItems.Length; i++)
            {
                if (_dialogItems[i] == null)
                    return (ushort)i;
            }

            return (ushort)_dialogItems.Length;
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
            if (_dialogItems[dialogItem.Index] != dialogItem)
                return false;

            _dialogItems[dialogItem.Index] = null;

            // Remove references to the dialog
            foreach (EditorNPCChatResponse r in sourceResponses)
            {
                r.SetPage(EditorNPCChatResponse.EndConversationPage);
            }

            return true;
        }

        static void ResizeArrayToFitIndex<T>(ref T[] array, int index)
        {
            if (array.Length > index)
                return;

            int newSize = array.Length;
            while (newSize <= index)
            {
                newSize <<= 1;
            }

            Array.Resize(ref array, newSize);
        }

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
            for (int i = 0; i < _dialogItems.Length; i++)
            {
                _dialogItems[i] = null;
            }

            // Set the new items
            foreach (NPCChatDialogItemBase item in items)
            {
                Add((EditorNPCChatDialogItem)item);
            }
        }

        public void SetTitle(string value)
        {
            _title = value;
        }
    }
}