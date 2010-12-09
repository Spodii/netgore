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
********************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.IO;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `event_counters_item_template`.
    /// </summary>
    public class EventCountersItemTemplateTable : IEventCountersItemTemplateTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 3;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "event_counters_item_template";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "counter", "item_template_event_counter_id", "item_template_id" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "item_template_event_counter_id", "item_template_id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "counter" };

        /// <summary>
        /// The field that maps onto the database column `counter`.
        /// </summary>
        Int64 _counter;

        /// <summary>
        /// The field that maps onto the database column `item_template_event_counter_id`.
        /// </summary>
        Byte _itemTemplateEventCounterId;

        /// <summary>
        /// The field that maps onto the database column `item_template_id`.
        /// </summary>
        UInt16 _itemTemplateID;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCountersItemTemplateTable"/> class.
        /// </summary>
        public EventCountersItemTemplateTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCountersItemTemplateTable"/> class.
        /// </summary>
        /// <param name="counter">The initial value for the corresponding property.</param>
        /// <param name="itemTemplateEventCounterId">The initial value for the corresponding property.</param>
        /// <param name="itemTemplateID">The initial value for the corresponding property.</param>
        public EventCountersItemTemplateTable(Int64 @counter, Byte @itemTemplateEventCounterId, ItemTemplateID @itemTemplateID)
        {
            Counter = @counter;
            ItemTemplateEventCounterId = @itemTemplateEventCounterId;
            ItemTemplateID = @itemTemplateID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCountersItemTemplateTable"/> class.
        /// </summary>
        /// <param name="source">IEventCountersItemTemplateTable to copy the initial values from.</param>
        public EventCountersItemTemplateTable(IEventCountersItemTemplateTable source)
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
        public static void CopyValues(IEventCountersItemTemplateTable source, IDictionary<String, Object> dic)
        {
            dic["counter"] = source.Counter;
            dic["item_template_event_counter_id"] = source.ItemTemplateEventCounterId;
            dic["item_template_id"] = source.ItemTemplateID;
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
        /// Copies the values from the given <paramref name="source"/> into this EventCountersItemTemplateTable.
        /// </summary>
        /// <param name="source">The IEventCountersItemTemplateTable to copy the values from.</param>
        public void CopyValuesFrom(IEventCountersItemTemplateTable source)
        {
            Counter = source.Counter;
            ItemTemplateEventCounterId = source.ItemTemplateEventCounterId;
            ItemTemplateID = source.ItemTemplateID;
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
                case "counter":
                    return new ColumnMetadata("counter", "The event counter.", "bigint(20)", null, typeof(Int64), false, false,
                        false);

                case "item_template_event_counter_id":
                    return new ColumnMetadata("item_template_event_counter_id", "The ID of the event that the counter is for.",
                        "tinyint(3) unsigned", null, typeof(Byte), false, true, false);

                case "item_template_id":
                    return new ColumnMetadata("item_template_id", "The template of the item the event occured on.",
                        "smallint(5) unsigned", null, typeof(UInt16), false, true, false);

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
                case "counter":
                    return Counter;

                case "item_template_event_counter_id":
                    return ItemTemplateEventCounterId;

                case "item_template_id":
                    return ItemTemplateID;

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
                case "counter":
                    Counter = (Int64)value;
                    break;

                case "item_template_event_counter_id":
                    ItemTemplateEventCounterId = (Byte)value;
                    break;

                case "item_template_id":
                    ItemTemplateID = (ItemTemplateID)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IEventCountersItemTemplateTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `counter`.
        /// The underlying database type is `bigint(20)`.The database column contains the comment: 
        /// "The event counter.".
        /// </summary>
        [Description("The event counter.")]
        [SyncValue]
        public Int64 Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `item_template_event_counter_id`.
        /// The underlying database type is `tinyint(3) unsigned`.The database column contains the comment: 
        /// "The ID of the event that the counter is for.".
        /// </summary>
        [Description("The ID of the event that the counter is for.")]
        [SyncValue]
        public Byte ItemTemplateEventCounterId
        {
            get { return _itemTemplateEventCounterId; }
            set { _itemTemplateEventCounterId = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `item_template_id`.
        /// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
        /// "The template of the item the event occured on.".
        /// </summary>
        [Description("The template of the item the event occured on.")]
        [SyncValue]
        public ItemTemplateID ItemTemplateID
        {
            get { return (ItemTemplateID)_itemTemplateID; }
            set { _itemTemplateID = (UInt16)value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public virtual IEventCountersItemTemplateTable DeepCopy()
        {
            return new EventCountersItemTemplateTable(this);
        }

        #endregion

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public virtual void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public virtual void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion
    }
}