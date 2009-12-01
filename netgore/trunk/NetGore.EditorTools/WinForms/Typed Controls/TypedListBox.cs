using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    /// <summary>
    /// A <see cref="ListBox"/> that uses strong typing for the collection items.
    /// </summary>
    /// <typeparam name="T">The Type of collection item.</typeparam>
    public class TypedListBox<T> : ListBox, ITypedControl<T>
    {
        /// <summary>
        /// Notifies listeners when the selected item has changed.
        /// </summary>
        public event TypedListBoxChangeEventHandler<T> TypedSelectedItemChanged;

        /// <summary>
        /// Adds an item to the <see cref="Control"/>'s collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(T item)
        {
            Items.Add(new TypedListControlItem<T>(this, item));
        }

        /// <summary>
        /// Adds an item to the <see cref="Control"/>'s collection.
        /// </summary>
        /// <param name="items">The items to add.</param>
        public void AddItems(IEnumerable<T> items)
        {
            Items.AddRange(items.Select(x => new TypedListControlItem<T>(this, x)).ToArray());
        }

        /// <summary>
        /// Gets the items to initially populate the <see cref="Control"/>'s collection with.
        /// </summary>
        /// <returns>The items to initially populate the <see cref="Control"/>'s collection with.</returns>
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

        /// <summary>
        /// Gets the selected item as type <typeparamref name="T"/>, or the default value for the given
        /// type if the selected item was not of the given type.
        /// </summary>
        public T TypedSelectedItem
        {
            get
            {
                T output;
                if (!TypedControlHelper<T>.TryGetItemAsTyped(SelectedItem, out output))
                    return default(T);

                return output;
            }
        }

        /// <summary>
        /// Handles when the selected value changes.
        /// </summary>
        /// <param name="item">The new selected value.</param>
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