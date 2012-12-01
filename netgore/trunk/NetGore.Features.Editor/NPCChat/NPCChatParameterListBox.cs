using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetGore.Editor;
using NetGore.Features.NPCChat.Conditionals;

namespace NetGore.Features.NPCChat
{
    public class NPCChatParameterListBox : ListBox
    {
        EditorNPCChatConditionalCollectionItem _conCollectionItem;
        bool _doNotUpdateParameterValue;
        TextBox _valueTextBox;

        [Browsable(false)]
        public EditorNPCChatConditionalCollectionItem ConditionalCollectionItem
        {
            get { return _conCollectionItem; }
            set
            {
                if (_conCollectionItem == value)
                    return;

                _conCollectionItem = value;

                Items.Clear();

                if (_conCollectionItem != null)
                {
                    _conCollectionItem.Changed -= ConditionalCollectionItem_Changed;
                    _conCollectionItem.Changed += ConditionalCollectionItem_Changed;

                    RebuildItemList();
                }
            }
        }

        /// <summary>
        /// Gets the selected NPCChatConditionalParameter.
        /// </summary>
        /// <value>The selected NPCChatConditionalParameter.</value>
        [Browsable(false)]
        public NPCChatConditionalParameter SelectedParameter
        {
            get
            {
                var o = SelectedItem as ParameterListItem;
                if (o == null)
                    return null;

                return o.Parameter;
            }
        }

        /// <summary>
        /// Gets or sets the TextBox used to edit the value of the SelectedParameter.
        /// </summary>
        [Browsable(true)]
        [Description("The TextBox used to edit the value of the SelectedParameter")]
        public TextBox ValueTextBox
        {
            get { return _valueTextBox; }
            set
            {
                if (_valueTextBox == value)
                    return;

                if (_valueTextBox != null)
                {
                    _valueTextBox.TextChanged -= ValueTextBox_TextChanged;
                }

                _valueTextBox = value;

                if (_valueTextBox != null)
                {
                    _valueTextBox.TextChanged -= ValueTextBox_TextChanged;
                    _valueTextBox.TextChanged += ValueTextBox_TextChanged;
                }
            }
        }

        /// <summary>
        /// Handles the corresponding event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void ConditionalCollectionItem_Changed(EditorNPCChatConditionalCollectionItem sender, EventArgs e)
        {
            RebuildItemList();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            _doNotUpdateParameterValue = true;

            try
            {
                if (ValueTextBox != null)
                {
                    if (SelectedParameter == null)
                    {
                        ValueTextBox.Text = string.Empty;
                        ValueTextBox.Enabled = false;
                    }
                    else
                    {
                        ValueTextBox.Text = SelectedParameter.Value.ToString();
                        ValueTextBox.Enabled = true;
                    }
                }
            }
            finally
            {
                _doNotUpdateParameterValue = false;
            }
        }

        void RebuildItemList()
        {
            var si = SelectedIndex;
            Items.Clear();
            if (ConditionalCollectionItem != null)
            {
                Items.AddRange(ConditionalCollectionItem.Parameters.Select(x => new ParameterListItem(x)).Cast<object>().ToArray());
                if (si < Items.Count && si >= 0)
                    SelectedIndex = si;
            }
        }

        void RefreshSelectedItem()
        {
            this.RefreshItemAt(SelectedIndex);
        }

        /// <summary>
        /// Handles the TextChanged event of the ValueTextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void ValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (SelectedParameter == null)
                return;
            if (_doNotUpdateParameterValue)
                return;

            var wasValueSet = SelectedParameter.TrySetValue(ValueTextBox.Text, Parser.Current);

            var selStart = ValueTextBox.SelectionStart;
            var selLen = ValueTextBox.SelectionLength;

            if (!wasValueSet)
                ValueTextBox.BackColor = EditorColors.Error;
            else
            {
                ValueTextBox.BackColor = EditorColors.Normal;
                RefreshSelectedItem();
            }

            // Restore focus to the ValueTextBox
            ValueTextBox.Focus();
            ValueTextBox.SelectionLength = selLen;
            ValueTextBox.SelectionStart = selStart;
        }

        class ParameterListItem
        {
            readonly NPCChatConditionalParameter _parameter;

            /// <summary>
            /// Initializes a new instance of the <see cref="ParameterListItem"/> class.
            /// </summary>
            /// <param name="parameter">The <see cref="NPCChatConditionalParameter"/>.</param>
            /// <exception cref="ArgumentNullException"><paramref name="parameter" /> is <c>null</c>.</exception>
            public ParameterListItem(NPCChatConditionalParameter parameter)
            {
                if (parameter == null)
                    throw new ArgumentNullException("parameter");

                _parameter = parameter;
            }

            public NPCChatConditionalParameter Parameter
            {
                get { return _parameter; }
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.Append(Parameter.ValueType);
                sb.Append(" \t");
                sb.Append(Parameter.Value.ToString());
                return sb.ToString();
            }

            public static implicit operator NPCChatConditionalParameter(ParameterListItem v)
            {
                return v.Parameter;
            }
        }
    }
}