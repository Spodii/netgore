using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using NetGore.Collections;

namespace DemoGame.Server
{
    /// <summary>
    /// Manages each individual NPCDrop. This list is immutable, and any attempt to modify it will
    /// throw a MethodAccessException.
    /// </summary>
    public class NPCDropManager : IList<NPCDrop>
    {
        const string _isReadonlyMessage =
            "This collection is read-only and may not be modified. Any IList or ICollection " +
            "method call that may result in the collection being modified will throw this MethodAccessException.";

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly DArray<NPCDrop> _npcDrops = new DArray<NPCDrop>(32, false);

        public NPCDropManager(SelectNPCDropsQuery query, IList<ItemTemplate> itemTemplates)
        {
            if (query == null)
                throw new ArgumentNullException("query");
            if (itemTemplates == null)
                throw new ArgumentNullException("itemTemplates");

            var dropValues = query.Execute();
            foreach (SelectNPCDropQueryValues v in dropValues)
            {
                // Get the template of the item to be dropped
                ItemTemplate itemTemplate = itemTemplates[v.ItemGuid];
                if (itemTemplate == null)
                {
                    // TODO: What will we do when the ItemTemplate is invalid for a NPCDrop?
                    Debug.Fail(":(");
                    continue;
                }

                // Create the NPCDrop and add it to the DArray
                NPCDrop drop = new NPCDrop(v.Guid, itemTemplate, v.Min, v.Max, v.Chance);
                _npcDrops[v.Guid] = drop;

                if (log.IsInfoEnabled)
                    log.InfoFormat("Loaded NPCDrop `{0}` for item `{1}`", v.Guid, v.ItemGuid);
            }

            // Trim down the DArray
            _npcDrops.Trim();
        }

        #region IList<NPCDrop> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A System.Collections.Generic.IEnumerator<T> that can be used to iterate through the collection.</returns>
        public IEnumerator<NPCDrop> GetEnumerator()
        {
            return (IEnumerator<NPCDrop>)_npcDrops;
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
        void ICollection<NPCDrop>.Add(NPCDrop item)
        {
            throw new MethodAccessException(_isReadonlyMessage);
        }

        /// <summary>
        /// Removes all items from the System.Collections.Generic.ICollection<T>.
        /// </summary>
        void ICollection<NPCDrop>.Clear()
        {
            throw new MethodAccessException(_isReadonlyMessage);
        }

        /// <summary>
        /// Determines whether the System.Collections.Generic.ICollection<T> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.ICollection<T>.</param>
        /// <returns>true if item is found in the System.Collections.Generic.ICollection<T>; otherwise, false.</returns>
        public bool Contains(NPCDrop item)
        {
            return _npcDrops.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the System.Collections.Generic.ICollection<T> to an System.Array, 
        /// starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied 
        /// from System.Collections.Generic.ICollection<T>. The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(NPCDrop[] array, int arrayIndex)
        {
            _npcDrops.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection<T>.
        /// </summary>
        /// <param name="item">The object to remove from the System.Collections.Generic.ICollection<T>.</param>
        /// <returns>true if item was successfully removed from the System.Collections.Generic.ICollection<T>; 
        /// otherwise, false. This method also returns false if item is not found in the 
        /// original System.Collections.Generic.ICollection<T>.</returns>
        bool ICollection<NPCDrop>.Remove(NPCDrop item)
        {
            throw new MethodAccessException(_isReadonlyMessage);
        }

        /// <summary>
        /// Gets the number of NPCDropManager in this collection.
        /// </summary>
        public int Count
        {
            get { return _npcDrops.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the System.Collections.Generic.ICollection<T> is read-only.
        /// Always true for NPCDropManager.
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
        public int IndexOf(NPCDrop item)
        {
            return _npcDrops.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the System.Collections.Generic.IList<T> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the System.Collections.Generic.IList<T>.</param>
        void IList<NPCDrop>.Insert(int index, NPCDrop item)
        {
            throw new MethodAccessException(_isReadonlyMessage);
        }

        /// <summary>
        /// Removes the System.Collections.Generic.IList<T> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        void IList<NPCDrop>.RemoveAt(int index)
        {
            throw new MethodAccessException(_isReadonlyMessage);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public NPCDrop this[int index]
        {
            get { return _npcDrops[index]; }
            set { throw new MethodAccessException(_isReadonlyMessage); }
        }

        #endregion
    }
}