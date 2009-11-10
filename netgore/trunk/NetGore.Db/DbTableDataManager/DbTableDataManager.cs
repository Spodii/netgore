using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for a manager of table data.
    /// </summary>
    /// <typeparam name="TID">The Type of ID.</typeparam>
    /// <typeparam name="TItem">The Type of item.</typeparam>
    public abstract class DbTableDataManager<TID, TItem> : IEnumerable<TItem>
    {
        readonly IDbController _dbController;
        readonly DArray<TItem> _items = new DArray<TItem>(32, false);
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTableDataManager&lt;TID, TItem&gt;"/> class.
        /// </summary>
        /// <param name="dbController">The IDbController.</param>
        protected DbTableDataManager(IDbController dbController)
        {
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _dbController = dbController;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            CacheDbQueries(_dbController);
            // ReSharper restore DoNotCallOverridableMethodsInConstructor

            LoadAll();
        }

        /// <summary>
        /// Gets the item at the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the item to get.</param>
        /// <returns>The item at the given <paramref name="id"/>.</returns>
        public TItem this[TID id]
        {
            get { return _items[IDToInt(id)]; }
        }

        /// <summary>
        /// Gets the <see cref="IDbController"/> used by this DbTableDataManager.
        /// </summary>
        public IDbController DbController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Gets the length of the internal buffer where the greatest item index is equal to the <see cref="Length"/> - 1.
        /// </summary>
        public int Length
        {
            get { return _items.Length; }
        }

        /// <summary>
        /// When overridden in the derived class, provides a chance to cache frequently used queries instead of
        /// having to grab the query from the <see cref="IDbController"/> every time. Caching is completely
        /// optional, but if you do cache any queries, it should be done here. Do not use this method for
        /// anything other than caching queries from the <paramref name="dbController"/>.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/> to grab the queries from.</param>
        protected virtual void CacheDbQueries(IDbController dbController)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets all of the IDs in the table being managed.
        /// </summary>
        /// <returns>An IEnumerable of all of the IDs in the table being managed.</returns>
        protected abstract IEnumerable<TID> GetIDs();

        /// <summary>
        /// When overridden in the derived class, converts the <paramref name="value"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> as an int.</returns>
        protected abstract int IDToInt(TID value);

        /// <summary>
        /// When overridden in the derived class, converts the int to a <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The int as a <paramref name="value"/>.</returns>
        public abstract TID IntToID(int value);

        /// <summary>
        /// Loads all of the items.
        /// </summary>
        void LoadAll()
        {
            var ids = GetIDs();

            foreach (var id in ids)
            {
                var item = LoadItem(id);
                var i = IDToInt(id);
                _items.Insert(i, item);

                if (log.IsDebugEnabled)
                    log.DebugFormat("Loaded item `{0}` at index `{1}`.", item, i);
            }

            _items.Trim();
        }

        /// <summary>
        /// When overridden in the derived class, loads an item from the database.
        /// </summary>
        /// <param name="id">The ID of the item to load.</param>
        /// <returns>The item loaded from the database.</returns>
        protected abstract TItem LoadItem(TID id);

        /// <summary>
        /// Tries to get the item for the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the item to get.</param>
        /// <param name="item">When this method returns true, contains the item with the given <paramref name="id"/>.</param>
        /// <returns>True if the <paramref name="item"/> for the given <paramref name="id"/> was found;
        /// otherwise false.</returns>
        public bool TryGetValue(TID id, out TItem item)
        {
            var i = IDToInt(id);

            if (!_items.CanGet(i))
            {
                item = default(TItem);
                return false;
            }

            item = _items[i];
            return true;
        }

        #region IEnumerable<TItem> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TItem> GetEnumerator()
        {
            foreach (var item in _items)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}