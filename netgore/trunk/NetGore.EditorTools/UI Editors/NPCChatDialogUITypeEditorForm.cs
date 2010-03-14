using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public class NPCChatDialogUITypeEditorForm : UITypeEditorListForm<NPCChatDialogBase>
    {
        readonly object _selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatDialogUITypeEditorForm"/> class.
        /// </summary>
        /// <param name="selected">The currently selected <see cref="NPCChatDialogBase"/>.
        /// Multiple different types are supported. Can be null.</param>
        public NPCChatDialogUITypeEditorForm(object selected)
        {
            _selected = selected;

            if (!EnsureNPCChatManagerSet())
                MessageBox.Show("Could not load the NPC chat dialog UITypeEditor form - NPCChatManager is not set.");
        }

        /// <summary>
        /// Gets or sets the <see cref="NPCChatManagerBase"/> used to display the items in this form. Needs
        /// to be set before using the <see cref="NPCChatDialogUITypeEditorForm"/>.
        /// </summary>
        public static NPCChatManagerBase NPCChatManager { get; set; }

        /// <summary>
        /// When overridden in the derived class, draws the <paramref name="item"/>.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.DrawItemEventArgs"/> instance containing the event data.</param>
        /// <param name="item">The item being drawn.</param>
        protected override void DrawListItem(DrawItemEventArgs e, NPCChatDialogBase item)
        {
            e.DrawBackground();

            if (item == null)
                return;

            using (var brush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString(item.ID + ". " + item.Title, e.Font, brush, e.Bounds);
            }
        }

        /// <summary>
        /// Ensures the <see cref="NPCChatDialogUITypeEditorForm.NPCChatManager"/> is set.
        /// </summary>
        /// <returns>If false, the form will be unloaded.</returns>
        bool EnsureNPCChatManagerSet()
        {
            if (NPCChatManager == null)
            {
                DialogResult = DialogResult.Abort;
                Close();
                return false;
            }

            return true;
        }

        /// <summary>
        /// When overridden in the derived class, gets the items to add to the list.
        /// </summary>
        /// <returns>The items to add to the list.</returns>
        protected override IEnumerable<NPCChatDialogBase> GetListItems()
        {
            if (!EnsureNPCChatManagerSet())
                return base.GetListItems();

            return NPCChatManager.OrderBy(x => (int)x.ID);
        }

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="item"/> is valid to be
        /// used as the returned item.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        /// <returns>
        /// If the given <paramref name="item"/> is valid to be used as the returned item.
        /// </returns>
        protected override bool IsItemValid(NPCChatDialogBase item)
        {
            return item != null;
        }

        /// <summary>
        /// When overridden in the derived class, sets the item that will be selected by default.
        /// </summary>
        /// <param name="items">The items to choose from.</param>
        /// <returns>
        /// The item that will be selected by default.
        /// </returns>
        protected override NPCChatDialogBase SetDefaultSelectedItem(IEnumerable<NPCChatDialogBase> items)
        {
            if (_selected == null)
                return base.SetDefaultSelectedItem(items);

            if (_selected is string)
            {
                var stringComp = StringComparer.Ordinal;
                var asString = (string)_selected;
                return items.FirstOrDefault(x => stringComp.Equals(x.ID, asString));
            }

            if (_selected is NPCChatDialogID)
            {
                var asID = (NPCChatDialogID)_selected;
                return items.FirstOrDefault(x => x.ID == asID);
            }

            if (_selected is NPCChatDialogBase)
            {
                var asDialog = (NPCChatDialogBase)_selected;
                return items.FirstOrDefault(x => x.ID == asDialog.ID);
            }

            return base.SetDefaultSelectedItem(items);
        }
    }
}