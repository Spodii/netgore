using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `server_setting`.
    /// </summary>
    public class ServerSettingTable : IServerSettingTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 1;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "server_setting";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "motd" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "motd" };

        /// <summary>
        /// The field that maps onto the database column `motd`.
        /// </summary>
        String _motd;

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
        /// ServerSettingTable constructor.
        /// </summary>
        public ServerSettingTable()
        {
        }

        /// <summary>
        /// ServerSettingTable constructor.
        /// </summary>
        /// <param name="motd">The initial value for the corresponding property.</param>
        public ServerSettingTable(String @motd)
        {
            Motd = @motd;
        }

        /// <summary>
        /// ServerSettingTable constructor.
        /// </summary>
        /// <param name="source">IServerSettingTable to copy the initial values from.</param>
        public ServerSettingTable(IServerSettingTable source)
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
        public static void CopyValues(IServerSettingTable source, IDictionary<String, Object> dic)
        {
            dic["@motd"] = source.Motd;
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
        /// Copies the values from the given <paramref name="source"/> into this ServerSettingTable.
        /// </summary>
        /// <param name="source">The IServerSettingTable to copy the values from.</param>
        public void CopyValuesFrom(IServerSettingTable source)
        {
            Motd = source.Motd;
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
                case "motd":
                    return new ColumnMetadata("motd", "The message of the day.", "varchar(0)", "", typeof(String), false, false,
                                              false);

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
                case "motd":
                    return Motd;

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
                case "motd":
                    Motd = (String)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IServerSettingTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `motd`.
        /// The underlying database type is `varchar(0)`. The database column contains the comment: 
        /// "The message of the day.".
        /// </summary>
        public String Motd
        {
            get { return _motd; }
            set { _motd = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IServerSettingTable DeepCopy()
        {
            return new ServerSettingTable(this);
        }

        #endregion
    }
}