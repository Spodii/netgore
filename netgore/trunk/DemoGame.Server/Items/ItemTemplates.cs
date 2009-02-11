using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;
using MySql.Data.MySqlClient;
using NetGore;
using NetGore.Extensions;

namespace DemoGame.Server
{
    public class ItemTemplates : IList<ItemTemplate>
    {
        /// <summary>
        /// The table name for the item templates.
        /// </summary>
        public const string TableName = "item_templates";

        const string _isReadonlyMessage =
            "This collection is read-only and may not be modified. Any IList or ICollection " +
            "method call that may result in the collection being modified will throw this MethodAccessException.";

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly DArray<ItemTemplate> _itemTemplates = new DArray<ItemTemplate>(32, false);

        public ItemTemplates(MySqlConnection conn, string tableName)
        {
            if (conn == null)
                throw new ArgumentNullException("conn");
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException("tableName");

            // Create the command
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                // Select all the rows
                cmd.CommandText = string.Format("SELECT * FROM `{0}`", tableName);
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    // Set up the ordinal cache
                    ItemTemplateOC ordinalCache = new ItemTemplateOC();
                    ordinalCache.Initialize(r);

                    // Read each of the individual rows
                    while (r.Read())
                    {
                        // Create the item template
                        ItemTemplate titem = LoadItemTemplate(r, ordinalCache);
                        _itemTemplates[titem.Guid] = titem;

                        if (log.IsInfoEnabled)
                            log.InfoFormat("Loaded ItemTemplate `{0}`", titem);
                    }
                }
            }

            // Trim the DArray
            _itemTemplates.Trim();
        }

        static ItemTemplate LoadItemTemplate(MySqlDataReader r, ItemTemplateOC oc)
        {
            // Load the item template's values
            ushort guid = r.GetUInt16(oc.Guid);
            string name = r.GetString(oc.Name);
            string desc = r.GetString(oc.Description);
            ushort graphic = r.GetUInt16(oc.Graphic);
            int value = r.GetInt32(oc.Value);
            byte width = r.GetByte(oc.Width);
            byte height = r.GetByte(oc.Height);
            ItemType type = (ItemType)r.GetByte(oc.Type);

            // Make sure the ItemType is defined
            if (!type.IsDefined())
            {
                const string errmsg = "Invalid ItemType `{0}` for ItemTemplate guid `{1}`.";
                throw new InvalidCastException(string.Format(errmsg, type, guid));
            }

            // Load the stat modifiers
            ItemStats stats = new ItemStats();
            foreach (StatType statType in oc.StatTypes)
            {
                IStat stat = stats.GetStat(statType);
                int ordinal = oc.GetStatOrdinal(statType);
                stat.Read(r, ordinal);
            }

            // Return the new ItemTemplate
            return new ItemTemplate(guid, name, desc, type, graphic, value, width, height, stats);
        }

        #region IList<ItemTemplate> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A System.Collections.Generic.IEnumerator<T> that can be used to iterate through the collection.</returns>
        public IEnumerator<ItemTemplate> GetEnumerator()
        {
            return (IEnumerator<ItemTemplate>)_itemTemplates;
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
        public ItemTemplate this[int index]
        {
            get { return _itemTemplates[index]; }
            set { throw new MethodAccessException(_isReadonlyMessage); }
        }

        #endregion

        class ItemTemplateOC : OrdinalCacheBase
        {
            byte _description;
            byte _graphic;
            byte _guid;
            byte _height;
            byte _name;
            byte?[] _statOrdinals;
            byte _type;
            byte _value;
            byte _width;

            public int Description
            {
                get { return _description; }
            }

            public int Graphic
            {
                get { return _graphic; }
            }

            public int Guid
            {
                get { return _guid; }
            }

            public int Height
            {
                get { return _height; }
            }

            public int Name
            {
                get { return _name; }
            }

            public IEnumerable<StatType> StatTypes
            {
                get { return GetLoadedStatOrdinalEnumerator(_statOrdinals); }
            }

            public byte Type
            {
                get { return _type; }
            }

            public int Value
            {
                get { return _value; }
            }

            public int Width
            {
                get { return _width; }
            }

            public int GetStatOrdinal(StatType statType)
            {
                return GetStatOrdinalHelper(statType, _statOrdinals);
            }

            /// <summary>
            /// Loads the ordinal caches from a given IDataRecord.
            /// </summary>
            /// <param name="dataRecord">An IDataRecord containing all of the items
            /// that need to be cached.</param>
            protected override void LoadCache(IDataRecord dataRecord)
            {
                _guid = dataRecord.GetOrdinalAsByte("guid");
                _graphic = dataRecord.GetOrdinalAsByte("graphic");
                _name = dataRecord.GetOrdinalAsByte("name");
                _description = dataRecord.GetOrdinalAsByte("description");
                _width = dataRecord.GetOrdinalAsByte("width");
                _height = dataRecord.GetOrdinalAsByte("height");
                _value = dataRecord.GetOrdinalAsByte("value");
                _type = dataRecord.GetOrdinalAsByte("type");

                // Try to load every StatType possible
                var statTypes = Enum.GetValues(typeof(StatType)).Cast<StatType>();
                _statOrdinals = TryCreateStatOrdinalCache(dataRecord, statTypes);

                // Display the loaded StatTypes in the log
                if (log.IsInfoEnabled)
                {
                    foreach (StatType statType in StatTypes)
                    {
                        int ordinal = GetStatOrdinal(statType);
                        log.InfoFormat("ItemTemplate StatType `{0}` found at ordinal `{1}`", statType, ordinal);
                    }
                }
            }
        }
    }
}