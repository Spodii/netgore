using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `alliance_hostile`.
    /// </summary>
    public interface IAllianceHostileTable
    {
        /// <summary>
        /// Gets the value of the database column `alliance_id`.
        /// </summary>
        AllianceID AllianceId { get; }

        /// <summary>
        /// Gets the value of the database column `hostile_id`.
        /// </summary>
        AllianceID HostileId { get; }

        /// <summary>
        /// Gets the value of the database column `placeholder`.
        /// </summary>
        byte? Placeholder { get; }
    }

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
        static readonly String[] _dbColumns = new string[] { "alliance_id", "hostile_id", "placeholder" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "alliance_id", "hostile_id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "placeholder" };

        /// <summary>
        /// The field that maps onto the database column `alliance_id`.
        /// </summary>
        Byte _allianceId;

        /// <summary>
        /// The field that maps onto the database column `hostile_id`.
        /// </summary>
        Byte _hostileId;

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
        /// <param name="allianceId">The initial value for the corresponding property.</param>
        /// <param name="hostileId">The initial value for the corresponding property.</param>
        /// <param name="placeholder">The initial value for the corresponding property.</param>
        public AllianceHostileTable(AllianceID @allianceId, AllianceID @hostileId, byte? @placeholder)
        {
            AllianceId = @allianceId;
            HostileId = @hostileId;
            Placeholder = @placeholder;
        }

        /// <summary>
        /// AllianceHostileTable constructor.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
        public AllianceHostileTable(IDataReader dataReader)
        {
            ReadValues(dataReader);
        }

        public AllianceHostileTable(IAllianceHostileTable source)
        {
            CopyValuesFrom(source);
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(IAllianceHostileTable source, IDictionary<String, Object> dic)
        {
            dic["@alliance_id"] = source.AllianceId;
            dic["@hostile_id"] = source.HostileId;
            dic["@placeholder"] = source.Placeholder;
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(IAllianceHostileTable source, DbParameterValues paramValues)
        {
            paramValues["@alliance_id"] = source.AllianceId;
            paramValues["@hostile_id"] = source.HostileId;
            paramValues["@placeholder"] = source.Placeholder;
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public void CopyValues(IDictionary<String, Object> dic)
        {
            CopyValues(this, dic);
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public void CopyValues(DbParameterValues paramValues)
        {
            CopyValues(this, paramValues);
        }

        public void CopyValuesFrom(IAllianceHostileTable source)
        {
            AllianceId = source.AllianceId;
            HostileId = source.HostileId;
            Placeholder = source.Placeholder;
        }

        public static ColumnMetadata GetColumnData(String fieldName)
        {
            switch (fieldName)
            {
                case "alliance_id":
                    return new ColumnMetadata("alliance_id", "", "tinyint(3) unsigned", null, typeof(Byte), false, true, false);

                case "hostile_id":
                    return new ColumnMetadata("hostile_id", "", "tinyint(3) unsigned", null, typeof(Byte), false, true, false);

                case "placeholder":
                    return new ColumnMetadata("placeholder", "Unused placeholder column - please do not remove",
                                              "tinyint(3) unsigned", null, typeof(byte?), true, false, false);

                default:
                    throw new ArgumentException("Field not found.", "fieldName");
            }
        }

        public Object GetValue(String columnName)
        {
            switch (columnName)
            {
                case "alliance_id":
                    return AllianceId;

                case "hostile_id":
                    return HostileId;

                case "placeholder":
                    return Placeholder;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void ReadValues(IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("alliance_id");
            AllianceId = (AllianceID)dataReader.GetByte(i);

            i = dataReader.GetOrdinal("hostile_id");
            HostileId = (AllianceID)dataReader.GetByte(i);

            i = dataReader.GetOrdinal("placeholder");
            Placeholder = (dataReader.IsDBNull(i) ? (byte?)null : dataReader.GetByte(i));
        }

        public void SetValue(String columnName, Object value)
        {
            switch (columnName)
            {
                case "alliance_id":
                    AllianceId = (AllianceID)value;
                    break;

                case "hostile_id":
                    HostileId = (AllianceID)value;
                    break;

                case "placeholder":
                    Placeholder = (byte?)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The key must already exist in the DbParameterValues
        /// for the value to be copied over. If any of the keys in the DbParameterValues do not
        /// match one of the column names, or if there is no field for a key, then it will be
        /// ignored. Because of this, it is important to be careful when using this method
        /// since columns or keys can be skipped without any indication.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void TryCopyValues(IAllianceHostileTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@alliance_id":
                        paramValues[i] = source.AllianceId;
                        break;

                    case "@hostile_id":
                        paramValues[i] = source.HostileId;
                        break;

                    case "@placeholder":
                        paramValues[i] = source.Placeholder;
                        break;
                }
            }
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The key must already exist in the DbParameterValues
        /// for the value to be copied over. If any of the keys in the DbParameterValues do not
        /// match one of the column names, or if there is no field for a key, then it will be
        /// ignored. Because of this, it is important to be careful when using this method
        /// since columns or keys can be skipped without any indication.
        /// </summary>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public void TryCopyValues(DbParameterValues paramValues)
        {
            TryCopyValues(this, paramValues);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. Unlike ReadValues(), this method not only doesn't require
        /// all values to be in the IDataReader, but also does not require the values in
        /// the IDataReader to be a defined field for the table this class represents.
        /// Because of this, you need to be careful when using this method because values
        /// can easily be skipped without any indication.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void TryReadValues(IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "alliance_id":
                        AllianceId = (AllianceID)dataReader.GetByte(i);
                        break;

                    case "hostile_id":
                        HostileId = (AllianceID)dataReader.GetByte(i);
                        break;

                    case "placeholder":
                        Placeholder = (dataReader.IsDBNull(i) ? (byte?)null : dataReader.GetByte(i));
                        break;
                }
            }
        }

        #region IAllianceHostileTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `alliance_id`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public AllianceID AllianceId
        {
            get { return (AllianceID)_allianceId; }
            set { _allianceId = (Byte)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `hostile_id`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public AllianceID HostileId
        {
            get { return (AllianceID)_hostileId; }
            set { _hostileId = (Byte)value; }
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

        #endregion
    }
}