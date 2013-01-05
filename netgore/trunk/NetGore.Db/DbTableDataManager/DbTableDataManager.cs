using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for a manager of table data.
    /// </summary>
    /// <typeparam name="TID">The Type of ID.</typeparam>
    /// <typeparam name="TItem">The Type of item.</typeparam>
    public abstract class DbTableDataManager<TID, TItem> : IEnumerable<TItem> where TItem : class
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly object _loadSync = new object();
        readonly IDbController _dbController;
        readonly Dictionary<TID, TItem> _items = new Dictionary<TID, TItem>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTableDataManager{TID, TItem}"/> class.
        /// </summary>
        /// <param name="dbController">The <see cref="IDbController"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dbController" /> is <c>null</c>.</exception>
        protected DbTableDataManager(IDbController dbController)
        {
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _dbController = dbController;

            CacheDbQueries(_dbController);

            foreach (var id in GetIDs())
                _items.Add(id, null);
        }

        /// <summary>
        /// Gets the item at the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The ID of the item to get.</param>
        /// <returns>The item at the given <paramref name="id"/>, or the default value of of <typeparamref name="TItem"/> if the
        /// the <paramref name="id"/> is invalid or not item exists for the <paramref name="id"/>.</returns>
        public TItem this[TID id]
        {
            get
            {
                TItem value;
                if (!_items.TryGetValue(id, out value))
                    return default(TItem);

                // Load if the value is null, which means the id exists but is not loaded
                if (value == null)
                {
                    // Synchronize loads to prevent multiple loads of the same item
                    lock (_loadSync)
                    {
                        value = _items[id]; // Since we weren't locked until now, it could have loaded already
                        if (value == null)
                            value = Reload(id);
                    }
                }

                return value;
            }
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
            get { return _items.Count; }
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
        /// When overridden in the derived class, loads an item from the database.
        /// </summary>
        /// <param name="id">The ID of the item to load.</param>
        /// <returns>The item loaded from the database.</returns>
        protected abstract TItem LoadItem(TID id);

        /// <summary>
        /// Forces and item in this collection to be reloaded from the database.
        /// Note that reloading an object will create a new object, not update the existing object. As a result, anything referencing
        /// the old object will continue to reference the old values instead of the newly loaded values.
        /// </summary>
        /// <param name="id">The ID of the item to reload from the database.</param>
        /// <returns>The loaded item.</returns>
        public virtual TItem Reload(TID id)
        {
            var item = LoadItem(id);
            _items[id] = item;

            if (log.IsDebugEnabled)
                log.DebugFormat("Loaded item `{0}` at index `{1}`.", item, id);

            return item;
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
            foreach (var key in _items.Keys.ToImmutable())
            {
                yield return this[key];
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