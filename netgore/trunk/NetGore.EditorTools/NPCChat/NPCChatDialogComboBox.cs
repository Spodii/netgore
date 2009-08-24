using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public delegate void NPCChatDialogComboBoxChangeDialogHandler(NPCChatDialogComboBox sender, NPCChatDialogBase dialog);

    public class NPCChatDialogComboBox : ComboBox
    {
        NPCChatDialogBase _lastSelectedDialog;
        public event NPCChatDialogComboBoxChangeDialogHandler OnChangeDialog;

        public void AddDialog(NPCChatDialogBase dialog)
        {
            NPCChatDialogComboBoxItem v = new NPCChatDialogComboBoxItem(dialog);
            Items.Add(v);
        }

        public void AddDialog(IEnumerable<NPCChatDialogBase> dialogs)
        {
            foreach (NPCChatDialogBase dialog in dialogs)
            {
                AddDialog(dialog);
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            NPCChatDialogBase selectedDialog = null;

            if (SelectedItem != null && SelectedItem is NPCChatDialogComboBoxItem)
                selectedDialog = ((NPCChatDialogComboBoxItem)SelectedItem).Dialog;

            if (selectedDialog != _lastSelectedDialog)
            {
                if (OnChangeDialog != null)
                    OnChangeDialog(this, selectedDialog);

                _lastSelectedDialog = selectedDialog;
            }
        }

        struct NPCChatDialogComboBoxItem
        {
            public readonly NPCChatDialogBase Dialog;
            readonly string _toString;

            public NPCChatDialogComboBoxItem(NPCChatDialogBase dialog)
            {
                Dialog = dialog;
                _toString = string.Format("{0}: {1}", Dialog.Index, Dialog.Title);
            }

            public override string ToString()
            {
                return _toString;
            }
        }
    }
}