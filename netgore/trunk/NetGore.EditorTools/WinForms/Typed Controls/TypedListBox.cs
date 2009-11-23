using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    public class TypedListBox<T> : ListBox, ITypedControl<T>
    {
        public event TypedListBoxChangeEventHandler<T> TypedSelectedItemChanged;

        public void AddItem(T item)
        {
            Items.Add(new TypedListControlItem<T>(this, item));
        }

        public void AddItems(IEnumerable<T> items)
        {
            Items.AddRange(items.Select(x => new TypedListControlItem<T>(this, x)).ToArray());
        }

        /// <summary>
        /// Gets the items to initially populate the <see cref="ListBox"/> with.
        /// </summary>
        /// <returns>The items to initially populate the <see cref="ListBox"/> with.</returns>
        protected virtual IEnumerable<T> GetInitialItems()
        {
            return Enumerable.Empty<T>();
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.CreateControl"/> method.
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            // Populate the list
            var items = GetInitialItems();

            if (items != null && !items.IsEmpty())
            {
                Items.Clear();
                AddItems(items);
            }

            if (Items.Count > 0)
                SelectedIndex = 0;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ListControl.SelectedValueChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e);

            T item;
            if (TypedControlHelper<T>.TryGetItemAsTyped(SelectedItem, out item))
            {
                OnTypedSelectedValueChanged(item);
                if (TypedSelectedItemChanged != null)
                    TypedSelectedItemChanged(this, item);
            }
        }

        protected virtual void OnTypedSelectedValueChanged(T item)
        {
        }

        #region ITypedControl<T> Members

        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The string to display.</returns>
        public virtual string ItemToString(T item)
        {
            return item.ToString();
        }

        #endregion
    }
}