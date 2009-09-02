using System;
using System.Linq;
using System.Windows.Forms;
using NetGore.EditorTools;
using NetGore.EditorTools.NPCChat;
using NetGore.NPCChat;

namespace DemoGame.NPCChatEditor
{
    public delegate void SelectedConditionalItemChangeHandler(
        NPCChatConditionalsListBox sender, EditorNPCChatConditionalCollectionItem item);

    public class NPCChatConditionalsListBox : ListBox
    {
        ComboBox _evaluationTypeComboBox;
        EditorNPCChatConditionalCollectionItem _selectedConditionalItem;

        public event SelectedConditionalItemChangeHandler OnChangeConditionalItem;

        EditorNPCChatConditionalCollection _conditionalCollection;

        public void EditCurrentItem()
        {
            if (SelectedConditionalItem == null)
                return;

            var f = new NPCChatConditionalEditorForm(SelectedConditionalItem);
            f.Show();
        }

        public EditorNPCChatConditionalCollection ConditionalCollection
        {
            get { return _conditionalCollection; }
            set
            {
                if (_conditionalCollection == value)
                    return;

                _conditionalCollection = value;

                Items.Clear();

                if (_conditionalCollection != null)
                {
                    Items.AddRange(_conditionalCollection.ToArray());
                }
            }
        }

        public EditorNPCChatConditionalCollectionItem SelectedConditionalItem
        {
            get { return _selectedConditionalItem; }
        }

        public ComboBox EvaluationTypeComboBox
        {
            get { return _evaluationTypeComboBox; }
            set
            {
                if (_evaluationTypeComboBox == value)
                    return;

                _evaluationTypeComboBox = value;
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            EditorNPCChatConditionalCollectionItem item = SelectedItem as EditorNPCChatConditionalCollectionItem;
            if (item != SelectedConditionalItem)
            {
                _selectedConditionalItem = item;
                if (OnChangeConditionalItem != null)
                    OnChangeConditionalItem(this, item);
            }
        }
    }
}