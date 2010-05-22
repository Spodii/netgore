/********************************************************************
                   DO NOT MANUALLY EDIT THIS FILE!

This file was automatically generated using the DbClassCreator
program. The only time you should ever alter this file is if you are
using an automated code formatter. The DbClassCreator will overwrite
this file every time it is run, so all manual changes will be lost.
If there is something in this file that you wish to change, you should
be able to do it through the DbClassCreator arguments.

Make sure that you re-run the DbClassCreator every time you alter your
game's database.

For more information on the DbClassCreator, please see:
    http://www.netgore.com/wiki/dbclasscreator.html

This file was generated on (UTC): 5/21/2010 1:39:24 AM
********************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Features.Shops;
using NetGore.IO;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `world_stats_user_shopping`.
    /// </summary>
    public class WorldStatsUserShoppingTable : IWorldStatsUserShoppingTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 10;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "world_stats_user_shopping";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        { "amount", "character_id", "cost", "item_template_id", "map_id", "sale_type", "shop_id", "when", "x", "y" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        { "amount", "character_id", "cost", "item_template_id", "map_id", "sale_type", "shop_id", "when", "x", "y" };

        /// <summary>
        /// The field that maps onto the database column `amount`.
        /// </summary>
        Byte _amount;

        /// <summary>
        /// The field that maps onto the database column `character_id`.
        /// </summary>
        Int32 _characterID;

        /// <summary>
        /// The field that maps onto the database column `cost`.
        /// </summary>
        Int32 _cost;

        /// <summary>
        /// The field that maps onto the database column `item_template_id`.
        /// </summary>
        ushort? _itemTemplateID;

        /// <summary>
        /// The field that maps onto the database column `map_id`.
        /// </summary>
        ushort? _mapID;

        /// <summary>
        /// The field that maps onto the database column `sale_type`.
        /// </summary>
        SByte _saleType;

        /// <summary>
        /// The field that maps onto the database column `shop_id`.
        /// </summary>
        UInt16 _shopID;

        /// <summary>
        /// The field that maps onto the database column `when`.
        /// </summary>
        DateTime _when;

        /// <summary>
        /// The field that maps onto the database column `x`.
        /// </summary>
        UInt16 _x;

        /// <summary>
        /// The field that maps onto the database column `y`.
        /// </summary>
        UInt16 _y;

        /// <summary>
        /// WorldStatsUserShoppingTable constructor.
        /// </summary>
        public WorldStatsUserShoppingTable()
        {
        }

        /// <summary>
        /// WorldStatsUserShoppingTable constructor.
        /// </summary>
        /// <param name="amount">The initial value for the corresponding property.</param>
        /// <param name="characterID">The initial value for the corresponding property.</param>
        /// <param name="cost">The initial value for the corresponding property.</param>
        /// <param name="itemTemplateID">The initial value for the corresponding property.</param>
        /// <param name="mapID">The initial value for the corresponding property.</param>
        /// <param name="saleType">The initial value for the corresponding property.</param>
        /// <param name="shopID">The initial value for the corresponding property.</param>
        /// <param name="when">The initial value for the corresponding property.</param>
        /// <param name="x">The initial value for the corresponding property.</param>
        /// <param name="y">The initial value for the corresponding property.</param>
        public WorldStatsUserShoppingTable(Byte @amount, CharacterID @characterID, Int32 @cost, ItemTemplateID? @itemTemplateID,
                                           MapID? @mapID, SByte @saleType, ShopID @shopID, DateTime @when, UInt16 @x, UInt16 @y)
        {
            Amount = @amount;
            CharacterID = @characterID;
            Cost = @cost;
            ItemTemplateID = @itemTemplateID;
            MapID = @mapID;
            SaleType = @saleType;
            ShopID = @shopID;
            When = @when;
            X = @x;
            Y = @y;
        }

        /// <summary>
        /// WorldStatsUserShoppingTable constructor.
        /// </summary>
        /// <param name="source">IWorldStatsUserShoppingTable to copy the initial values from.</param>
        public WorldStatsUserShoppingTable(IWorldStatsUserShoppingTable source)
        {
            CopyValuesFrom(source);
        }

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
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(IWorldStatsUserShoppingTable source, IDictionary<String, Object> dic)
        {
            dic["@amount"] = source.Amount;
            dic["@character_id"] = source.CharacterID;
            dic["@cost"] = source.Cost;
            dic["@item_template_id"] = source.ItemTemplateID;
            dic["@map_id"] = source.MapID;
            dic["@sale_type"] = source.SaleType;
            dic["@shop_id"] = source.ShopID;
            dic["@when"] = source.When;
            dic["@x"] = source.X;
            dic["@y"] = source.Y;
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
        /// Copies the values from the given <paramref name="source"/> into this WorldStatsUserShoppingTable.
        /// </summary>
        /// <param name="source">The IWorldStatsUserShoppingTable to copy the values from.</param>
        public void CopyValuesFrom(IWorldStatsUserShoppingTable source)
        {
            Amount = source.Amount;
            CharacterID = source.CharacterID;
            Cost = source.Cost;
            ItemTemplateID = source.ItemTemplateID;
            MapID = source.MapID;
            SaleType = source.SaleType;
            ShopID = source.ShopID;
            When = source.When;
            X = source.X;
            Y = source.Y;
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
                case "amount":
                    return new ColumnMetadata("amount",
                                              "The number of items involved in the transaction. Should always be greater than 0, and should only be greater for 1 for items that can stack.",
                                              "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                case "character_id":
                    return new ColumnMetadata("character_id",
                                              "The ID of the character that performed this transaction with the shop.", "int(11)",
                                              null, typeof(Int32), false, false, true);

                case "cost":
                    return new ColumnMetadata("cost",
                                              "The amount of money that was involved in this transaction (how much the shopper sold the items for, or how much they bought the items for). ",
                                              "int(11)", null, typeof(Int32), false, false, false);

                case "item_template_id":
                    return new ColumnMetadata("item_template_id",
                                              "The ID of the item template that the event relates to. Only valid when the item involved has a set item template ID.",
                                              "smallint(5) unsigned", null, typeof(ushort?), true, false, true);

                case "map_id":
                    return new ColumnMetadata("map_id", "The ID of the map the event took place on.", "smallint(5) unsigned", null,
                                              typeof(ushort?), true, false, true);

                case "sale_type":
                    return new ColumnMetadata("sale_type",
                                              "Whether the shop sold to the user, or vise versa. If 0, the shop sold an item to the shopper. If non-zero, the shopper sold an item to a shop.",
                                              "tinyint(4)", null, typeof(SByte), false, false, false);

                case "shop_id":
                    return new ColumnMetadata("shop_id", "The ID of the shop the event took place at.", "smallint(5) unsigned",
                                              null, typeof(UInt16), false, false, true);

                case "when":
                    return new ColumnMetadata("when", "When this event took place.", "timestamp", "CURRENT_TIMESTAMP",
                                              typeof(DateTime), false, false, false);

                case "x":
                    return new ColumnMetadata("x",
                                              "The map X coordinate of the shopper when this event took place. Only valid when the map_id is not null.",
                                              "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

                case "y":
                    return new ColumnMetadata("y",
                                              "The map Y coordinate of the shopper when this event took place. Only valid when the map_id is not null.",
                                              "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

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
                case "amount":
                    return Amount;

                case "character_id":
                    return CharacterID;

                case "cost":
                    return Cost;

                case "item_template_id":
                    return ItemTemplateID;

                case "map_id":
                    return MapID;

                case "sale_type":
                    return SaleType;

                case "shop_id":
                    return ShopID;

                case "when":
                    return When;

                case "x":
                    return X;

                case "y":
                    return Y;

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
                case "amount":
                    Amount = (Byte)value;
                    break;

                case "character_id":
                    CharacterID = (CharacterID)value;
                    break;

                case "cost":
                    Cost = (Int32)value;
                    break;

                case "item_template_id":
                    ItemTemplateID = (ItemTemplateID?)value;
                    break;

                case "map_id":
                    MapID = (MapID?)value;
                    break;

                case "sale_type":
                    SaleType = (SByte)value;
                    break;

                case "shop_id":
                    ShopID = (ShopID)value;
                    break;

                case "when":
                    When = (DateTime)value;
                    break;

                case "x":
                    X = (UInt16)value;
                    break;

                case "y":
                    Y = (UInt16)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion

        #region IWorldStatsUserShoppingTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `amount`.
        /// The underlying database type is `tinyint(3) unsigned`. The database column contains the comment: 
        /// "The number of items involved in the transaction. Should always be greater than 0, and should only be greater for 1 for items that can stack.".
        /// </summary>
        [Description(
            "The number of items involved in the transaction. Should always be greater than 0, and should only be greater for 1 for items that can stack."
            )]
        [SyncValue]
        public Byte Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_id`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "The ID of the character that performed this transaction with the shop.".
        /// </summary>
        [Description("The ID of the character that performed this transaction with the shop.")]
        [SyncValue]
        public CharacterID CharacterID
        {
            get { return (CharacterID)_characterID; }
            set { _characterID = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `cost`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "The amount of money that was involved in this transaction (how much the shopper sold the items for, or how much they bought the items for). ".
        /// </summary>
        [Description(
            "The amount of money that was involved in this transaction (how much the shopper sold the items for, or how much they bought the items for). "
            )]
        [SyncValue]
        public Int32 Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `item_template_id`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The ID of the item template that the event relates to. Only valid when the item involved has a set item template ID.".
        /// </summary>
        [Description(
            "The ID of the item template that the event relates to. Only valid when the item involved has a set item template ID."
            )]
        [SyncValue]
        public ItemTemplateID? ItemTemplateID
        {
            get { return (Nullable<ItemTemplateID>)_itemTemplateID; }
            set { _itemTemplateID = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `map_id`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The ID of the map the event took place on.".
        /// </summary>
        [Description("The ID of the map the event took place on.")]
        [SyncValue]
        public MapID? MapID
        {
            get { return (Nullable<MapID>)_mapID; }
            set { _mapID = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `sale_type`.
        /// The underlying database type is `tinyint(4)`. The database column contains the comment: 
        /// "Whether the shop sold to the user, or vise versa. If 0, the shop sold an item to the shopper. If non-zero, the shopper sold an item to a shop.".
        /// </summary>
        [Description(
            "Whether the shop sold to the user, or vise versa. If 0, the shop sold an item to the shopper. If non-zero, the shopper sold an item to a shop."
            )]
        [SyncValue]
        public SByte SaleType
        {
            get { return _saleType; }
            set { _saleType = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `shop_id`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The ID of the shop the event took place at.".
        /// </summary>
        [Description("The ID of the shop the event took place at.")]
        [SyncValue]
        public ShopID ShopID
        {
            get { return (ShopID)_shopID; }
            set { _shopID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `when`.
        /// The underlying database type is `timestamp` with the default value of `CURRENT_TIMESTAMP`. The database column contains the comment: 
        /// "When this event took place.".
        /// </summary>
        [Description("When this event took place.")]
        [SyncValue]
        public DateTime When
        {
            get { return _when; }
            set { _when = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `x`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The map X coordinate of the shopper when this event took place. Only valid when the map_id is not null.".
        /// </summary>
        [Description("The map X coordinate of the shopper when this event took place. Only valid when the map_id is not null.")]
        [SyncValue]
        public UInt16 X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `y`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The map Y coordinate of the shopper when this event took place. Only valid when the map_id is not null.".
        /// </summary>
        [Description("The map Y coordinate of the shopper when this event took place. Only valid when the map_id is not null.")]
        [SyncValue]
        public UInt16 Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IWorldStatsUserShoppingTable DeepCopy()
        {
            return new WorldStatsUserShoppingTable(this);
        }

        #endregion
    }
}