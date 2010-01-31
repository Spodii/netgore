using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using NetGore.NPCChat;

namespace NetGore.EditorTools.NPCChat
{
    public delegate void NPCChatDialogComboBoxChangeDialogHandler(NPCChatDialogComboBox sender, NPCChatDialogBase dialog);

    public class NPCChatDialogComboBox : ComboBox
    {
        NPCChatDialogBase _lastSelectedDialog;

        /// <summary>
        /// Notifies listeners when the selected <see cref="NPCChatDialogBase"/> has changed.
        /// </summary>
        public event NPCChatDialogComboBoxChangeDialogHandler SelectedDialogChanged;

        public void AddDialog(NPCChatDialogBase dialog)
        {
            var v = new NPCChatDialogComboBoxItem(dialog);
            Items.Add(v);
        }

        public void AddDialog(IEnumerable<NPCChatDialogBase> dialogs)
        {
            foreach (var dialog in dialogs)
            {
                AddDialog(dialog);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ComboBox.SelectedIndexChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            NPCChatDialogBase selectedDialog = null;

            if (SelectedItem != null && SelectedItem is NPCChatDialogComboBoxItem)
                selectedDialog = ((NPCChatDialogComboBoxItem)SelectedItem).Dialog;

            if (selectedDialog != _lastSelectedDialog)
            {
                if (SelectedDialogChanged != null)
                    SelectedDialogChanged(this, selectedDialog);

                _lastSelectedDialog = selectedDialog;
            }
        }

        struct NPCChatDialogComboBoxItem
        {
            public readonly NPCChatDialogBase Dialog;

            readonly string _toString;

            /// <summary>
            /// Initializes a new instance of the <see cref="NPCChatDialogComboBoxItem"/> struct.
            /// </summary>
            /// <param name="dialog">The dialog.</param>
            public NPCChatDialogComboBoxItem(NPCChatDialogBase dialog)
            {
                Dialog = dialog;
                _toString = string.Format("{0}: {1}", Dialog.Index, Dialog.Title);

                // TODO: Can likely get rid of this struct by overriding OnDrawItem
            }

            /// <summary>
            /// Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return _toString;
            }
        }
    }
}