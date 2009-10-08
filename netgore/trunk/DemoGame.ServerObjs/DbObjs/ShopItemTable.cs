using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `shop_item`.
    /// </summary>
    public class ShopItemTable : IShopItemTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 2;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "shop_item";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "item_template_id", "shop_id" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "item_template_id", "shop_id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { };

        /// <summary>
        /// The field that maps onto the database column `item_template_id`.
        /// </summary>
        UInt16 _itemTemplateID;

        /// <summary>
        /// The field that maps onto the database column `shop_id`.
        /// </summary>
        UInt16 _shopID;

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public static IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
        /// </summary>
        public static IEnumerable<String> DbKeyColumns
        {
            get { return _dbColumnsKeys; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
        /// </summary>
        public static IEnumerable<String> DbNonKeyColumns
        {
            get { return _dbColumnsNonKey; }
        }

        /// <summary>
        /// ShopItemTable constructor.
        /// </summary>
        public ShopItemTable()
        {
        }

        /// <summary>
        /// ShopItemTable constructor.
        /// </summary>
        /// <param name="itemTemplateID">The initial value for the corresponding property.</param>
        /// <param name="shopID">The initial value for the corresponding property.</param>
        public ShopItemTable(ItemTemplateID @itemTemplateID, ShopID @shopID)
        {
            ItemTemplateID = @itemTemplateID;
            ShopID = @shopID;
        }

        /// <summary>
        /// ShopItemTable constructor.
        /// </summary>
        /// <param name="source">IShopItemTable to copy the initial values from.</param>
        public ShopItemTable(IShopItemTable source)
        {
            CopyValuesFrom(source);
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(IShopItemTable source, IDictionary<String, Object> dic)
        {
            dic["@item_template_id"] = source.ItemTemplateID;
            dic["@shop_id"] = source.ShopID;
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public void CopyValues(IDictionary<String, Object> dic)
        {
            CopyValues(this, dic);
        }

        /// <summary>
        /// Copies the values from the given <paramref name="source"/> into this ShopItemTable.
        /// </summary>
        /// <param name="source">The IShopItemTable to copy the values from.</param>
        public void CopyValuesFrom(IShopItemTable source)
        {
            ItemTemplateID = source.ItemTemplateID;
            ShopID = source.ShopID;
        }

        /// <summary>
        /// Gets the data for the database column that this table represents.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the data for.</param>
        /// <returns>
        /// The data for the database column with the name <paramref name="columnName"/>.
        /// </returns>
        public static ColumnMetadata GetColumnData(String columnName)
        {
            switch (columnName)
            {
                case "item_template_id":
                    return new ColumnMetadata("item_template_id", "", "smallint(5) unsigned", null, typeof(UInt16), false, true,
                                              false);

                case "shop_id":
                    return new ColumnMetadata("shop_id", "", "smallint(5) unsigned", null, typeof(UInt16), false, true, false);

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Gets the value of a column by the database column's name.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the value for.</param>
        /// <returns>
        /// The value of the column with the name <paramref name="columnName"/>.
        /// </returns>
        public Object GetValue(String columnName)
        {
            switch (columnName)
            {
                case "item_template_id":
                    return ItemTemplateID;

                case "shop_id":
                    return ShopID;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Sets the <paramref name="value"/> of a column by the database column's name.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
        /// <param name="value">Value to assign to the column.</param>
        public void SetValue(String columnName, Object value)
        {
            switch (columnName)
            {
                case "item_template_id":
                    ItemTemplateID = (ItemTemplateID)value;
                    break;

                case "shop_id":
                    ShopID = (ShopID)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IShopItemTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `item_template_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public ItemTemplateID ItemTemplateID
        {
            get { return (ItemTemplateID)_itemTemplateID; }
            set { _itemTemplateID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `shop_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public ShopID ShopID
        {
            get { return (ShopID)_shopID; }
            set { _shopID = (UInt16)value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IShopItemTable DeepCopy()
        {
            return new ShopItemTable(this);
        }

        #endregion
    }
}