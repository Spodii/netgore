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
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.IO;
using NetGore.World;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `event_counters_map`.
    /// </summary>
    public class EventCountersMapTable : IEventCountersMapTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 3;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "event_counters_map";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "counter", "map_event_counter_id", "map_id" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "map_event_counter_id", "map_id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "counter" };

        /// <summary>
        /// The field that maps onto the database column `counter`.
        /// </summary>
        Int64 _counter;

        /// <summary>
        /// The field that maps onto the database column `map_event_counter_id`.
        /// </summary>
        Byte _mapEventCounterId;

        /// <summary>
        /// The field that maps onto the database column `map_id`.
        /// </summary>
        UInt16 _mapID;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCountersMapTable"/> class.
        /// </summary>
        public EventCountersMapTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCountersMapTable"/> class.
        /// </summary>
        /// <param name="counter">The initial value for the corresponding property.</param>
        /// <param name="mapEventCounterId">The initial value for the corresponding property.</param>
        /// <param name="mapID">The initial value for the corresponding property.</param>
        public EventCountersMapTable(Int64 @counter, Byte @mapEventCounterId, MapID @mapID)
        {
            Counter = @counter;
            MapEventCounterId = @mapEventCounterId;
            MapID = @mapID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventCountersMapTable"/> class.
        /// </summary>
        /// <param name="source">IEventCountersMapTable to copy the initial values from.</param>
        public EventCountersMapTable(IEventCountersMapTable source)
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
        public static void CopyValues(IEventCountersMapTable source, IDictionary<String, Object> dic)
        {
            dic["counter"] = source.Counter;
            dic["map_event_counter_id"] = source.MapEventCounterId;
            dic["map_id"] = source.MapID;
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
        /// Copies the values from the given <paramref name="source"/> into this EventCountersMapTable.
        /// </summary>
        /// <param name="source">The IEventCountersMapTable to copy the values from.</param>
        public void CopyValuesFrom(IEventCountersMapTable source)
        {
            Counter = source.Counter;
            MapEventCounterId = source.MapEventCounterId;
            MapID = source.MapID;
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
                    return new ColumnMetadata("counter", "", "bigint(20)", null, typeof(Int64), false, false, false);

                case "map_event_counter_id":
                    return new ColumnMetadata("map_event_counter_id", "", "tinyint(3) unsigned", null, typeof(Byte), false, true,
                        false);

                case "map_id":
                    return new ColumnMetadata("map_id", "", "smallint(5) unsigned", null, typeof(UInt16), false, true, false);

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

                case "map_event_counter_id":
                    return MapEventCounterId;

                case "map_id":
                    return MapID;

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

                case "map_event_counter_id":
                    MapEventCounterId = (Byte)value;
                    break;

                case "map_id":
                    MapID = (MapID)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IEventCountersMapTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `counter`.
        /// The underlying database type is `bigint(20)`.
        /// </summary>
        [SyncValue]
        public Int64 Counter
        {
            get { return _counter; }
            set { _counter = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `map_event_counter_id`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        [SyncValue]
        public Byte MapEventCounterId
        {
            get { return _mapEventCounterId; }
            set { _mapEventCounterId = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `map_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public MapID MapID
        {
            get { return (MapID)_mapID; }
            set { _mapID = (UInt16)value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public virtual IEventCountersMapTable DeepCopy()
        {
            return new EventCountersMapTable(this);
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