using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;
using log4net;
using NetGore.Collections;

namespace DemoGame.Server
{
    public class ItemTemplates : IList<ItemTemplate>
    {
        const string _isReadonlyMessage =
            "This collection is read-only and may not be modified. Any IList or ICollection " +
            "method call that may result in the collection being modified will throw this MethodAccessException.";

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly DArray<ItemTemplate> _itemTemplates = new DArray<ItemTemplate>(32, false);

        public ItemTemplates(SelectItemTemplatesQuery query)
        {
            if (query == null)
                throw new ArgumentNullException("query");

            // Load the item templates
            var itemTemplates = query.Execute();
            foreach (ItemTemplate it in itemTemplates)
            {
                _itemTemplates[(int)it.ID] = it;

                if (log.IsInfoEnabled)
                    log.InfoFormat("Loaded ItemTemplate `{0}`", it);
            }

            // Trim the DArray
            _itemTemplates.Trim();
        }

        #region IList<ItemTemplate> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A System.Collections.Generic.IEnumerator<T> that can be used to iterate through the collection.</returns>
        public IEnumerator<ItemTemplate> GetEnumerator()
        {
            return ((IEnumerable<ItemTemplate>)_itemTemplates).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An System.Collections.IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the System.Collections.Generic.ICollection<T>.
        /// </summary>
        /// <param name="item">The object to add to the System.Collections.Generic.ICollection<T>.</param>
        void ICollection<ItemTemplate>.Add(ItemTemplate item)
        {
            throw new MethodAccessException(_isReadonlyMessage);
        }

        /// <summary>
        /// Removes all items from the System.Collections.Generic.ICollection<T>.
        /// </summary>
        void ICollection<ItemTemplate>.Clear()
        {
            throw new MethodAccessException(_isReadonlyMessage);
        }

        /// <summary>
        /// Determines whether the System.Collections.Generic.ICollection<T> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.ICollection<T>.</param>
        /// <returns>true if item is found in the System.Collections.Generic.ICollection<T>; otherwise, false.</returns>
        public bool Contains(ItemTemplate item)
        {
            return _itemTemplates.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the System.Collections.Generic.ICollection<T> to an System.Array, 
        /// starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied 
        /// from System.Collections.Generic.ICollection<T>. The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ItemTemplate[] array, int arrayIndex)
        {
            _itemTemplates.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection<T>.
        /// </summary>
        /// <param name="item">The object to remove from the System.Collections.Generic.ICollection<T>.</param>
        /// <returns>true if item was successfully removed from the System.Collections.Generic.ICollection<T>; 
        /// otherwise, false. This method also returns false if item is not found in the 
        /// original System.Collections.Generic.ICollection<T>.</returns>
        bool ICollection<ItemTemplate>.Remove(ItemTemplate item)
        {
            throw new MethodAccessException(_isReadonlyMessage);
        }

        /// <summary>
        /// Gets the number of ItemTemplates in this collection.
        /// </summary>
        public int Count
        {
            get { return _itemTemplates.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the System.Collections.Generic.ICollection<T> is read-only.
        /// Always true for ItemTemplates.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Determines the index of a specific item in the System.Collections.Generic.IList<T>.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.IList<T>.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(ItemTemplate item)
        {
            return _itemTemplates.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the System.Collections.Generic.IList<T> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the System.Collections.Generic.IList<T>.</param>
        void IList<ItemTemplate>.Insert(int index, ItemTemplate item)
        {
            throw new MethodAccessException(_isReadonlyMessage);
        }

        /// <summary>
        /// Removes the System.Collections.Generic.IList<T> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        void IList<ItemTemplate>.RemoveAt(int index)
        {
            throw new MethodAccessException(_isReadonlyMessage);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        ItemTemplate IList<ItemTemplate>.this[int index]
        {
            get { return _itemTemplates[index]; }
            set { throw new MethodAccessException(_isReadonlyMessage); }
        }

        #endregion

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The ID of the Item to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public ItemTemplate this[ItemTemplateID index]
        {
            get { return _itemTemplates[(int)index]; }
            set { throw new MethodAccessException(_isReadonlyMessage); }
        }
    }
}