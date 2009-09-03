using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Globalization;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public partial class NPCChatConditionalEditorForm : Form
    {
        readonly NPCChatConditionalBase[] _conditionals;
        readonly EditorNPCChatConditionalCollectionItem _item;

        public NPCChatConditionalEditorForm(EditorNPCChatConditionalCollectionItem conditionalItem,
                                            NPCChatConditionalBase[] conditionals)
        {
            if (conditionalItem == null)
                throw new ArgumentNullException("conditionalItem");
            if (conditionals == null)
                throw new ArgumentNullException("conditionals");
            if (conditionals.Length == 0)
                throw new ArgumentException("Conditionals array cannot be empty.", "conditionals");

            _item = conditionalItem;
            _conditionals = conditionals;

            InitializeComponent();
        }

        void cmbConditionalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            NPCChatConditionalBase item = cmbConditionalType.SelectedItem as NPCChatConditionalBase;
            if (item == null)
                return;

            _item.SetConditional(item);
        }

        void InitGUI(EditorNPCChatConditionalCollectionItem item)
        {
            chkNot.Checked = item.Not;
            cmbConditionalType.SelectedItem = item.Conditional;
            lstParameters.ConditionalCollectionItem = item;
        }

        void NPCChatConditionalEditorForm_Load(object sender, EventArgs e)
        {
            cmbConditionalType.Items.Clear();
            cmbConditionalType.Items.AddRange(_conditionals);

            InitGUI(_item);
        }

        private void chkNot_CheckedChanged(object sender, EventArgs e)
        {
            _item.SetNot(chkNot.Checked);
        }
    }
}