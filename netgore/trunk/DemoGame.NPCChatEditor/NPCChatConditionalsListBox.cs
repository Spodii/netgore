using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                {
                    _conditionalCollection.OnChange -= ConditionalCollection_OnChange;
                }

                _conditionalCollection = value;

                Items.Clear();

                if (_conditionalCollection != null)
                {
                    Items.AddRange(_conditionalCollection.ToArray());
                    _conditionalCollection.OnChange += ConditionalCollection_OnChange;
                }
            }
        }

        static bool CollectionsContainSameItems<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            if (a.Count() != b.Count())
                return false;

            foreach (var item in a)
            {
                if (!b.Contains(item))
                    return false;
            }

            return true;
        }

        void RebuildItemList()
        {
            // No conditional collection? Clear the list
            if (ConditionalCollection == null)
            {
                Items.Clear();
                return;
            }

            var thisItemsCasted = Items.Cast<NPCChatConditionalCollectionItemBase>();

            // Only do something if there are any items missing
            if (CollectionsContainSameItems(ConditionalCollection, thisItemsCasted))
                return;

            var buffer = new List<NPCChatConditionalCollectionItemBase>();

            // Remove
            foreach (var item in thisItemsCasted)
            {
                if (!ConditionalCollection.Contains(item))
                    buffer.Add(item);
            }

            foreach (var item in buffer)
                Items.Remove(item);

            // Add
            buffer.Clear();
            foreach (var item in ConditionalCollection)
            {
                if (!thisItemsCasted.Contains(item))
                    buffer.Add(item);
            }

            Items.AddRange(buffer.ToArray());
        }

        void ConditionalCollection_OnChange(EditorNPCChatConditionalCollection source)
        {
            RebuildItemList();
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

                _evaluationTypeComboBox = value;
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

        public void EditCurrentItem(NPCChatConditionalBase[] npcChatConditionals)
        {
            if (SelectedConditionalItem == null)
                return;

            NPCChatConditionalEditorForm f = new NPCChatConditionalEditorForm(SelectedConditionalItem, npcChatConditionals);
            f.Show();
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
    }
}