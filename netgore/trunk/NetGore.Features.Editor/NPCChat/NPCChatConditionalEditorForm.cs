using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.Features.NPCChat.Conditionals;

namespace NetGore.Features.NPCChat
{
    public partial class NPCChatConditionalEditorForm : Form
    {
        readonly NPCChatConditionalBase[] _conditionals;
        readonly EditorNPCChatConditionalCollectionItem _item;

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCChatConditionalEditorForm"/> class.
        /// </summary>
        /// <param name="conditionalItem">The conditional item.</param>
        /// <param name="conditionals">The conditionals.</param>
        /// <exception cref="ArgumentNullException"><paramref name="conditionalItem" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="conditionals" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="conditionals"/> array cannot be empty.</exception>
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

        void InitGUI(EditorNPCChatConditionalCollectionItem item)
        {
            chkNot.Checked = item.Not;
            cmbConditionalType.SelectedItem = item.Conditional;
            lstParameters.ConditionalCollectionItem = item;
        }

        void NPCChatConditionalEditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }

        void NPCChatConditionalEditorForm_Load(object sender, EventArgs e)
        {
            cmbConditionalType.Items.Clear();
            cmbConditionalType.Items.AddRange(_conditionals.Cast<object>().ToArray());

            InitGUI(_item);
        }

        void chkNot_CheckedChanged(object sender, EventArgs e)
        {
            _item.SetNot(chkNot.Checked);
        }

        void cmbConditionalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = cmbConditionalType.SelectedItem as NPCChatConditionalBase;
            if (item == null)
                return;

            _item.SetConditional(item);
        }
    }
}