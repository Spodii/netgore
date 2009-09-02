using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.EditorTools.NPCChat;
using NetGore.Globalization;
using NetGore.NPCChat;

namespace NetGore.EditorTools
{
    public partial class NPCChatConditionalEditorForm : Form
    {
        readonly EditorNPCChatConditionalCollectionItem _item;
        readonly NPCChatConditionalBase[] _conditionals;

        public NPCChatConditionalEditorForm(EditorNPCChatConditionalCollectionItem conditionalItem, NPCChatConditionalBase[] conditionals)
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

        void InitGUI(NPCChatConditionalCollectionItemBase item)
        {
            chkNot.Checked = item.Not;
            cmbConditionalType.SelectedItem = item.Conditional;
            InitParameters(item.Parameters);
        }

        void InitParameters(IEnumerable<NPCChatConditionalParameter> parameters)
        {
            var items = parameters.Select(x => new ParameterListItem(x)).ToArray();
            lstParameters.Items.Clear();
            lstParameters.Items.AddRange(items);
        }

        private void NPCChatConditionalEditorForm_Load(object sender, EventArgs e)
        {
            cmbConditionalType.Items.Clear();
            cmbConditionalType.Items.AddRange(_conditionals);

            InitGUI(_item);
        }

        class ParameterListItem
        {
            readonly NPCChatConditionalParameter _parameter;

            public NPCChatConditionalParameter Parameter { get { return _parameter; } }

            public ParameterListItem(NPCChatConditionalParameter parameter)
            {
                if (parameter == null)
                    throw new ArgumentNullException("parameter");

                _parameter = parameter;
            }

            public static implicit operator NPCChatConditionalParameter(ParameterListItem v)
            {
                return v.Parameter;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Parameter.ValueType);
                sb.Append(" \t");
                sb.Append(Parameter.Value.ToString());
                return sb.ToString();
            }
        }

        NPCChatConditionalParameter _currentParameter;
        bool _doNotUpdateParameterValue;

        private void lstParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstParameters.SelectedItem == _currentParameter)
                return;

            _currentParameter = ((ParameterListItem)lstParameters.SelectedItem).Parameter;
            _doNotUpdateParameterValue = true;

            try
            {
                if (_currentParameter == null)
                {
                    txtValue.Text = string.Empty;
                    txtValue.Enabled = false;
                }
                else
                {
                    txtValue.Text = _currentParameter.Value.ToString();
                    txtValue.Enabled = true;
                }
            }
            finally
            {
                _doNotUpdateParameterValue = false;
            }
        }

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            if (_currentParameter == null)
                return;
            if (_doNotUpdateParameterValue)
                return;

            bool wasValueSet = _currentParameter.TrySetValue(txtValue.Text, Parser.Current);

            if (!wasValueSet)
                txtValue.BackColor = EditorColors.Error;
            else
                txtValue.BackColor = EditorColors.Normal;
        }

        private void cmbConditionalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = cmbConditionalType.SelectedItem as NPCChatConditionalBase;
            if (item == null)
                return;

            _item.SetConditional(item);

            InitParameters(_item.Parameters);
        }
    }
}
