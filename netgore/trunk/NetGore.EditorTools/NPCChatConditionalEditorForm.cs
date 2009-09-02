using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public partial class NPCChatConditionalEditorForm : Form
    {
        readonly EditorNPCChatConditionalCollectionItem _conditionalItem;

        public NPCChatConditionalEditorForm(EditorNPCChatConditionalCollectionItem conditionalItem)
        {
            if (conditionalItem == null)
                throw new ArgumentNullException("conditionalItem");

            _conditionalItem = conditionalItem;

            InitializeComponent();
        }

        private void NPCChatConditionalEditorForm_Load(object sender, EventArgs e)
        {

        }
    }
}
