using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NetGore.Editor;
using NetGore.Features.NPCChat.Conditionals;

namespace NetGore.Features.NPCChat
{
    /// <summary>
    /// A <see cref="ListBox"/> specially for managing the NPC chat conditional for editors.
    /// </summary>
    public class NPCChatConditionalsListBox : ListBox
    {
        EditorNPCChatConditionalCollection _conditionalCollection;
        ComboBox _evaluationTypeComboBox;
        EditorNPCChatConditionalCollectionItem _selectedConditionalItem;

        /// <summary>
        /// Notifies listeners when the <see cref="NPCChatConditionalsListBox.SelectedConditionalItem"/> has changed.
        /// </summary>
        public event TypedEventHandler<NPCChatConditionalsListBox, EventArgs<EditorNPCChatConditionalCollectionItem>>
            ConditionalItemChanged;

        /// <summary>
        /// Gets or sets the current <see cref="EditorNPCChatConditionalCollection"/> being used.
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
                    _conditionalCollection.Changed -= ConditionalCollection_Changed;

                _conditionalCollection = value;

                Items.Clear();

                if (_conditionalCollection != null)
                {
                    Items.AddRange(_conditionalCollection.Cast<object>().ToArray());

                    _conditionalCollection.Changed -= ConditionalCollection_Changed;
                    _conditionalCollection.Changed += ConditionalCollection_Changed;

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

                    _evaluationTypeComboBox.SelectedIndexChanged -= EvaluationTypeComboBox_SelectedIndexChanged;
                    _evaluationTypeComboBox.SelectedIndexChanged += EvaluationTypeComboBox_SelectedIndexChanged;
                }
            }
        }

        /// <summary>
        /// Gets the currently selected EditorNPCChatConditionalCollectionItem.
        /// </summary>
        [Browsable(false)]
        public EditorNPCChatConditionalCollectionItem SelectedConditionalItem
        {
            get { return _selectedConditionalItem; }
        }

        void ConditionalCollection_Changed(EditorNPCChatConditionalCollection source, EventArgs e)
        {
            RebuildItemList();
        }

        /// <summary>
        /// Edits the SelectedConditionalItem.
        /// </summary>
        /// <param name="npcChatConditionals">The NPCChatConditionalBases that can be chosen.</param>
        public void EditCurrentItem(NPCChatConditionalBase[] npcChatConditionals)
        {
            if (SelectedConditionalItem == null)
                return;

            var conditionalEditorForm = new NPCChatConditionalEditorForm(SelectedConditionalItem, npcChatConditionals) { StartPosition = FormStartPosition.CenterScreen };
            conditionalEditorForm.Show();
            conditionalEditorForm.FormClosed += conditionalEditorForm_FormClosed;
        }

        void EvaluationTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ConditionalCollection == null || EvaluationTypeComboBox == null)
                return;

            var item = EvaluationTypeComboBox.SelectedItem;
            if (item == null)
                return;

            var t = (NPCChatConditionalEvaluationType)item;
            if (!EnumHelper<NPCChatConditionalEvaluationType>.IsDefined(t))
            {
                EvaluationTypeComboBox.SelectedItem = ConditionalCollection.EvaluationType;
                return;
            }

            ConditionalCollection.SetEvaluationType(t);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            var item = SelectedItem as EditorNPCChatConditionalCollectionItem;
            if (item != SelectedConditionalItem)
            {
                _selectedConditionalItem = item;
                if (ConditionalItemChanged != null)
                    ConditionalItemChanged.Raise(this, EventArgsHelper.Create(item));
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
            var asEditorCollection = value as EditorNPCChatConditionalCollection;
            if (asEditorCollection != null)
            {
                ConditionalCollection = asEditorCollection;
                return;
            }

            var newCollection = new EditorNPCChatConditionalCollection(value);
            ConditionalCollection = newCollection;
        }

        /// <summary>
        /// Tries to add a <see cref="NPCChatConditionalCollectionItemBase"/> to the <see cref="ConditionalCollection"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>True if the <paramref name="item"/> was successfully added; otherwise false.</returns>
        public bool TryAddToConditionalCollection(NPCChatConditionalCollectionItemBase item)
        {
            if (ConditionalCollection == null)
                return false;

            return ConditionalCollection.TryAddItem(item);
        }

        /// <summary>
        /// Tries to remove a <see cref="EditorNPCChatConditionalCollectionItem"/> from the
        /// <see cref="ConditionalCollection"/>.
        /// </summary>
        /// <param name="item">The conditional item to remove.</param>
        /// <returns>True if the <paramref name="item"/> was successfully removed; otherwise false.</returns>
        public bool TryDeleteConditionalItem(EditorNPCChatConditionalCollectionItem item)
        {
            if (item == null)
                return false;

            var cc = ConditionalCollection;
            if (cc == null)
                return false;

            return cc.TryRemoveItem(item);
        }

        /// <summary>
        /// Tries to remove a <see cref="EditorNPCChatConditionalCollectionItem"/> from the
        /// <see cref="ConditionalCollection"/>.
        /// </summary>
        /// <returns>True if the item was successfully removed; otherwise false.</returns>
        public bool TryDeleteSelectedConditionalItem()
        {
            return TryDeleteConditionalItem(SelectedConditionalItem);
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
    }
}