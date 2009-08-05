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
        /// Gets the value for the database column `alliance_id`.
        /// </summary>
        Byte AllianceId { get; }

        /// <summary>
        /// Gets the value for the database column `hostile_id`.
        /// </summary>
        Byte HostileId { get; }

        /// <summary>
        /// Gets the value for the database column `placeholder`.
        /// </summary>
        Byte Placeholder { get; }
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
        Byte _placeholder;

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
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
        public AllianceHostileTable(Byte @allianceId, Byte @hostileId, Byte @placeholder)
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

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void ReadValues(IDataReader dataReader)
        {
            AllianceId = (Byte)dataReader.GetValue(dataReader.GetOrdinal("alliance_id"));
            HostileId = (Byte)dataReader.GetValue(dataReader.GetOrdinal("hostile_id"));
            Placeholder = (Byte)dataReader.GetValue(dataReader.GetOrdinal("placeholder"));
        }

        #region IAllianceHostileTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `alliance_id`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public Byte AllianceId
        {
            get { return _allianceId; }
            set { _allianceId = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `hostile_id`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public Byte HostileId
        {
            get { return _hostileId; }
            set { _hostileId = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `placeholder`.
        /// The underlying database type is `tinyint(3) unsigned`. The database column contains the comment: 
        /// "Unused placeholder column - please do not remove".
        /// </summary>
        public Byte Placeholder
        {
            get { return _placeholder; }
            set { _placeholder = value; }
        }

        #endregion
    }
}