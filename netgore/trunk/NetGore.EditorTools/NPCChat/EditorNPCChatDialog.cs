using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public event EditorNPCChatDialogEventHandler OnChange;

        public IEnumerable<EditorNPCChatDialogItem> Items
        {
            get { return _dialogItems.Where(x => x != null); }
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
    }
}
