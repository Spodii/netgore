using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `alliance_hostile`.
    /// </summary>
    public class AllianceHostileTable : IAllianceHostileTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 3;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "alliance_hostile";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        {
            "alliance_id", "hostile_id", "placeholder"
        };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[]
        {
            "alliance_id", "hostile_id"
        };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        {
            "placeholder"
        };

        /// <summary>
        /// The field that maps onto the database column `alliance_id`.
        /// </summary>
        Byte _allianceID;

        /// <summary>
        /// The field that maps onto the database column `hostile_id`.
        /// </summary>
        Byte _hostileID;

        /// <summary>
        /// The field that maps onto the database column `placeholder`.
        /// </summary>
        byte? _placeholder;

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
        /// AllianceHostileTable constructor.
        /// </summary>
        public AllianceHostileTable()
        {
        }

        /// <summary>
        /// AllianceHostileTable constructor.
        /// </summary>
        /// <param name="allianceID">The initial value for the corresponding property.</param>
        /// <param name="hostileID">The initial value for the corresponding property.</param>
        /// <param name="placeholder">The initial value for the corresponding property.</param>
        public AllianceHostileTable(AllianceID @allianceID, AllianceID @hostileID, byte? @placeholder)
        {
            AllianceID = @allianceID;
            HostileID = @hostileID;
            Placeholder = @placeholder;
        }

        /// <summary>
        /// AllianceHostileTable constructor.
        /// </summary>
        /// <param name="source">IAllianceHostileTable to copy the initial values from.</param>
        public AllianceHostileTable(IAllianceHostileTable source)
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
        public static void CopyValues(IAllianceHostileTable source, IDictionary<String, Object> dic)
        {
            dic["@alliance_id"] = source.AllianceID;
            dic["@hostile_id"] = source.HostileID;
            dic["@placeholder"] = source.Placeholder;
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
        /// Copies the values from the given <paramref name="source"/> into this AllianceHostileTable.
        /// </summary>
        /// <param name="source">The IAllianceHostileTable to copy the values from.</param>
        public void CopyValuesFrom(IAllianceHostileTable source)
        {
            AllianceID = source.AllianceID;
            HostileID = source.HostileID;
            Placeholder = source.Placeholder;
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
                case "alliance_id":
                    return new ColumnMetadata("alliance_id", "", "tinyint(3) unsigned", null, typeof(Byte), false, true, false);

                case "hostile_id":
                    return new ColumnMetadata("hostile_id", "", "tinyint(3) unsigned", null, typeof(Byte), false, true, false);

                case "placeholder":
                    return new ColumnMetadata("placeholder", "Unused placeholder column - please do not remove",
                                              "tinyint(3) unsigned", null, typeof(byte?), true, false, false);

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
                case "alliance_id":
                    return AllianceID;

                case "hostile_id":
                    return HostileID;

                case "placeholder":
                    return Placeholder;

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
                case "alliance_id":
                    AllianceID = (AllianceID)value;
                    break;

                case "hostile_id":
                    HostileID = (AllianceID)value;
                    break;

                case "placeholder":
                    Placeholder = (byte?)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IAllianceHostileTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `alliance_id`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public AllianceID AllianceID
        {
            get { return (AllianceID)_allianceID; }
            set { _allianceID = (Byte)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `hostile_id`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public AllianceID HostileID
        {
            get { return (AllianceID)_hostileID; }
            set { _hostileID = (Byte)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `placeholder`.
        /// The underlying database type is `tinyint(3) unsigned`. The database column contains the comment: 
        /// "Unused placeholder column - please do not remove".
        /// </summary>
        public byte? Placeholder
        {
            get { return _placeholder; }
            set { _placeholder = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IAllianceHostileTable DeepCopy()
        {
            return new AllianceHostileTable(this);
        }

        #endregion
    }
}