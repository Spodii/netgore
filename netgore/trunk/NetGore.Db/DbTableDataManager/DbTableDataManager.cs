using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for a manager of table data.
    /// </summary>
    /// <typeparam name="TID">The Type of ID.</typeparam>
    /// <typeparam name="TItem">The Type of item.</typeparam>
    public abstract class DbTableDataManager<TID, TItem>
    {
        readonly IDbController _dbController;
        readonly DArray<TItem> _items = new DArray<TItem>(32, false);

        /// <summary>
        /// Gets the <see cref="IDbController"/> used by this DbTableDataManager.
        /// </summary>
        public IDbController DbController
        {
            get { return _dbController; }
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
        /// Initializes a new instance of the <see cref="DbTableDataManager&lt;TID, TItem&gt;"/> class.
        /// </summary>
        /// <param name="dbController">The IDbController.</param>
        protected DbTableDataManager(IDbController dbController)
        {
            if (dbController == null)
                throw new ArgumentNullException("dbController");

            _dbController = dbController;

            LoadAll();
        }

        /// <summary>
        /// Loads all of the items.
        /// </summary>
        void LoadAll()
        {
            var ids = GetIDs();

            foreach (var id in ids)
            {
                var item = LoadItem(id);
                int i = IDToInt(id);
                _items.Insert(i, item);
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets all of the IDs in the table being managed.
        /// </summary>
        /// <returns>An IEnumerable of all of the IDs in the table being managed.</returns>
        protected abstract IEnumerable<TID> GetIDs();

        /// <summary>
        /// When overridden in the derived class, converts the <typeparamref name="TID"/> to an int.
        /// </summary>
        /// <param name="value">The <typeparamref name="TID"/> value.</param>
        /// <returns>The <paramref name="value"/> as an int.</returns>
        protected abstract int IDToInt(TID value);

        /// <summary>
        /// When overridden in the derived class, converts the int to a <typeparamref name="TID"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The int as a <paramref name="value"/>.</returns>
        public abstract TID IntToID(int value);

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
            int i = IDToInt(id);

            if (!_items.CanGet(i))
            {
                item = default(TItem);
                return false;
            }

            item = _items[i];
            return true;
        }
    }
}