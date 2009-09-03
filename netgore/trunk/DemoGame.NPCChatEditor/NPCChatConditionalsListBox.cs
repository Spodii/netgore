using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore;
using NetGore.EditorTools;
using NetGore.EditorTools.NPCChat;
using NetGore.NPCChat;
using NetGore.NPCChat.Conditionals;

namespace DemoGame.NPCChatEditor
{
    public delegate void SelectedConditionalItemChangeHandler(
        NPCChatConditionalsListBox sender, EditorNPCChatConditionalCollectionItem item);

    public class NPCChatConditionalsListBox : ListBox
    {
        EditorNPCChatConditionalCollection _conditionalCollection;
        ComboBox _evaluationTypeComboBox;
        EditorNPCChatConditionalCollectionItem _selectedConditionalItem;

        /// <summary>
        /// Notifies listeners when the SelectedConditionalItem has changed.
        /// </summary>
        public event SelectedConditionalItemChangeHandler OnChangeConditionalItem;

        /// <summary>
        /// Gets or sets the current EditorNPCChatConditionalCollection being used.
        /// </summary>
        [Browsable(false)]
        public EditorNPCChatConditionalCollection ConditionalCollection
        {
            get { return _conditionalCollection; }
            set
            {
                if (_conditionalCollection == value)
                    return;

                if (_conditionalCollection != null)
                    _conditionalCollection.OnChange -= ConditionalCollection_OnChange;

                _conditionalCollection = value;

                Items.Clear();

                if (_conditionalCollection != null)
                {
                    Items.AddRange(_conditionalCollection.ToArray());
                    _conditionalCollection.OnChange += ConditionalCollection_OnChange;

                    if (EvaluationTypeComboBox != null)
                        EvaluationTypeComboBox.SelectedItem = ConditionalCollection.EvaluationType;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ComboBox used to change the EvaluationType for the current ConditionalCollection.
        /// </summary>
        [Description("The ComboBox used to change the EvaluationType for the current ConditionalCollection.")]
        public ComboBox EvaluationTypeComboBox
        {
            get { return _evaluationTypeComboBox; }
            set
            {
                if (_evaluationTypeComboBox == value)
                    return;

                if (_evaluationTypeComboBox != null)
                    _evaluationTypeComboBox.SelectedIndexChanged -= EvaluationTypeComboBox_SelectedIndexChanged;

                _evaluationTypeComboBox = value;

                if (_evaluationTypeComboBox != null)
                {
                    if (ConditionalCollection != null)
                        _evaluationTypeComboBox.SelectedItem = ConditionalCollection.EvaluationType;

                    _evaluationTypeComboBox.SelectedIndexChanged += EvaluationTypeComboBox_SelectedIndexChanged;
                }
            }
        }

        void EvaluationTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ConditionalCollection == null || EvaluationTypeComboBox == null)
                return;

            var item = EvaluationTypeComboBox.SelectedItem;
            if (item == null)
                return;

            var t = (NPCChatConditionalEvaluationType)item;
            if (!EnumHelper.IsDefined(t))
            {
                EvaluationTypeComboBox.SelectedItem = ConditionalCollection.EvaluationType;
                return;
            }

            ConditionalCollection.SetEvaluationType(t);
        }

        /// <summary>
        /// Gets the currently selected EditorNPCChatConditionalCollectionItem.
        /// </summary>
        [Browsable(false)]
        public EditorNPCChatConditionalCollectionItem SelectedConditionalItem
        {
            get { return _selectedConditionalItem; }
        }

        void ConditionalCollection_OnChange(EditorNPCChatConditionalCollection source)
        {
            RebuildItemList();
        }

        /// <summary>
        /// Handles the FormClosed event of the conditionalEditorForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.FormClosedEventArgs"/> instance containing the event data.</param>
        void conditionalEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RefreshItems();
        }

        /// <summary>
        /// Edits the SelectedConditionalItem.
        /// </summary>
        /// <param name="npcChatConditionals">The NPCChatConditionalBases that can be chosen.</param>
        public void EditCurrentItem(NPCChatConditionalBase[] npcChatConditionals)
        {
            if (SelectedConditionalItem == null)
                return;

            NPCChatConditionalEditorForm conditionalEditorForm = new NPCChatConditionalEditorForm(SelectedConditionalItem,
                                                                                                  npcChatConditionals)
            { StartPosition = FormStartPosition.CenterScreen };
            conditionalEditorForm.Show();
            conditionalEditorForm.FormClosed += conditionalEditorForm_FormClosed;
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

        void RebuildItemList()
        {
            // No conditional collection? Clear the list
            if (ConditionalCollection == null)
            {
                Items.Clear();
                return;
            }

            this.SynchronizeItemList(ConditionalCollection);
        }

        /// <summary>
        /// Sets the ConditionalCollection.
        /// </summary>
        /// <param name="value">The NPCChatConditionalCollectionBase to set the ConditionalCollection as.
        /// If null, an empty NPCChatConditionalCollectionBase will be used.</param>
        public void SetConditionalCollection(NPCChatConditionalCollectionBase value)
        {
            EditorNPCChatConditionalCollection asEditorCollection = value as EditorNPCChatConditionalCollection;
            if (asEditorCollection != null)
            {
                ConditionalCollection = asEditorCollection;
                return;
            }

            EditorNPCChatConditionalCollection newCollection = new EditorNPCChatConditionalCollection(value);
            ConditionalCollection = newCollection;
        }

        /// <summary>
        /// Tries to add a NPCChatConditionalCollectionItemBase to the ConditionalCollection.
        /// </summary>
        /// <param name="item">The NPCChatConditionalCollectionItemBase to add.</param>
        /// <returns>True if the <paramref name="item"/> was successfully added; otherwise false.</returns>
        public bool TryAddToConditionalCollection(NPCChatConditionalCollectionItemBase item)
        {
            if (ConditionalCollection == null)
                return false;

            return ConditionalCollection.TryAddItem(item);
        }
    }
}