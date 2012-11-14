using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetGore.Editor
{
    /// <summary>
    /// Extension methods for the ListBox.
    /// </summary>
    public static class ListBoxExtensions
    {
        /// <summary>
        /// Adds an item to the <see cref="ListBox"/>, then selects the newly added item.
        /// </summary>
        /// <param name="listBox">The <see cref="ListBox"/>.</param>
        /// <param name="item">The item to add.</param>
        public static void AddItemAndReselect(this ListBox listBox, object item)
        {
            listBox.Items.Add(item);
            listBox.SelectedItem = item;
        }

        /// <summary>
        /// Deletes the selected item from the <see cref="ListBox"/> if possible, and selects a new item
        /// after deleting it.
        /// </summary>
        /// <param name="listBox">The <see cref="ListBox"/>.</param>
        /// <returns>True if an item was selected and it was removed; otherwise false.</returns>
        public static bool DeleteSelectedItem(this ListBox listBox)
        {
            var i = listBox.SelectedIndex;
            if (i < 0 || i >= listBox.Items.Count)
                return false;

            listBox.RemoveItemAtAndReselect(i);

            return true;
        }

        /// <summary>
        /// Refreshes the cached text for an item at the specified index.
        /// </summary>
        /// <param name="listBox">The <see cref="ListBox"/>.</param>
        /// <param name="index">The index of the item to refresh the text of.</param>
        public static void RefreshItemAt(this ListBox listBox, int index)
        {
            if (index < 0 || index >= listBox.Items.Count)
                return;

            var region = listBox.GetItemRectangle(index);
            listBox.Invalidate(region);
        }

        /// <summary>
        /// Removes an item from the <see cref="ListBox"/>, then select a new item if item that was removed was selected.
        /// </summary>
        /// <param name="listBox">The <see cref="ListBox"/>.</param>
        /// <param name="item">The item to remove.</param>
        public static void RemoveItemAndReselect(this ListBox listBox, object item)
        {
            if (item == null)
                return;

            RemoveItemAtAndReselect(listBox, listBox.Items.IndexOf(item));
        }

        /// <summary>
        /// Removes an item from the <see cref="ListBox"/>, then select a new item if item that was removed was selected.
        /// </summary>
        /// <param name="listBox">The <see cref="ListBox"/>.</param>
        /// <param name="itemIndex">Index of the item to remove.</param>
        public static void RemoveItemAtAndReselect(this ListBox listBox, int itemIndex)
        {
            if (itemIndex < 0)
                return;

            var selectedIndex = listBox.SelectedIndex;

            var removedSelectedItem = (selectedIndex == itemIndex);

            listBox.Items.RemoveAt(itemIndex);

            if (removedSelectedItem)
            {
                if (listBox.Items.Count == 0)
                    listBox.SelectedItem = null;
                else if (selectedIndex >= listBox.Items.Count)
                    listBox.SelectedIndex = selectedIndex - 1;
                else
                    listBox.SelectedIndex = selectedIndex;
            }
        }

        /// <summary>
        /// Synchronizes the items in the <see cref="ListBox"/> with the given set of <paramref name="values"/>. Only items
        /// that need to be added/removed are changed, preserving the state of the <see cref="ListBox"/> as much as possible.
        /// Any item in the <paramref name="listBox"/> not of type <typeparamref name="T"/> will be removed.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="listBox">The <see cref="ListBox"/>.</param>
        /// <param name="values">The set of values that the <see cref="ListBox"/> should have for its Items.</param>
        public static void SynchronizeItemList<T>(this ListBox listBox, IEnumerable<T> values) where T : class
        {
            var notOfThisType = new List<object>(2);
            var thisItemsCasted = new List<T>();
            foreach (var v in listBox.Items)
            {
                if (v is T)
                    thisItemsCasted.Add((T)v);
                else
                    notOfThisType.Add(v);
            }

            foreach (var v in notOfThisType)
            {
                listBox.Items.Remove(v);
            }

            // Only do something if there are any items missing
            if (values.ContainSameElements(thisItemsCasted))
                return;

            var buffer = new List<T>();

            // Remove
            foreach (var item in thisItemsCasted)
            {
                if (!values.Contains(item))
                    buffer.Add(item);
            }

            listBox.BeginUpdate();

            try
            {
                foreach (var item in buffer)
                {
                    listBox.Items.Remove(item);
                }

                // Add
                buffer.Clear();
                foreach (var item in values)
                {
                    if (!thisItemsCasted.Contains(item))
                        buffer.Add(item);
                }

                listBox.Items.AddRange(buffer.Cast<object>().ToArray());
            }
            finally
            {
                listBox.EndUpdate();
            }
        }
    }
}