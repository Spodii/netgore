using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `log_account_activity`.
    /// </summary>
    public class LogAccountActivityTable : ILogAccountActivityTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 5;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "log_account_activity";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "account_id", "id", "ip", "time_login", "time_logout" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "account_id", "ip", "time_login", "time_logout" };

        /// <summary>
        /// The field that maps onto the database column `account_id`.
        /// </summary>
        Int32 _accountId;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Int32 _iD;

        /// <summary>
        /// The field that maps onto the database column `ip`.
        /// </summary>
        UInt32 _ip;

        /// <summary>
        /// The field that maps onto the database column `time_login`.
        /// </summary>
        DateTime _timeLogin;

        /// <summary>
        /// The field that maps onto the database column `time_logout`.
        /// </summary>
        Nullable<DateTime> _timeLogout;

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public static IEnumerable<String> DbColumns
        {
            get { return (IEnumerable<String>)_dbColumns; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
        /// </summary>
        public static IEnumerable<String> DbKeyColumns
        {
            get { return (IEnumerable<String>)_dbColumnsKeys; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
        /// </summary>
        public static IEnumerable<String> DbNonKeyColumns
        {
            get { return (IEnumerable<String>)_dbColumnsNonKey; }
        }

        /// <summary>
        /// LogAccountActivityTable constructor.
        /// </summary>
        public LogAccountActivityTable()
        {
        }

        /// <summary>
        /// LogAccountActivityTable constructor.
        /// </summary>
        /// <param name="accountId">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="ip">The initial value for the corresponding property.</param>
        /// <param name="timeLogin">The initial value for the corresponding property.</param>
        /// <param name="timeLogout">The initial value for the corresponding property.</param>
        public LogAccountActivityTable(Int32 @accountId, Int32 @iD, UInt32 @ip, DateTime @timeLogin,
                                       Nullable<DateTime> @timeLogout)
        {
            AccountId = (Int32)@accountId;
            ID = (Int32)@iD;
            Ip = (UInt32)@ip;
            TimeLogin = (DateTime)@timeLogin;
            TimeLogout = (Nullable<DateTime>)@timeLogout;
        }

        /// <summary>
        /// LogAccountActivityTable constructor.
        /// </summary>
        /// <param name="source">ILogAccountActivityTable to copy the initial values from.</param>
        public LogAccountActivityTable(ILogAccountActivityTable source)
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
        public static void CopyValues(ILogAccountActivityTable source, IDictionary<String, Object> dic)
        {
            dic["@account_id"] = (Int32)source.AccountId;
            dic["@id"] = (Int32)source.ID;
            dic["@ip"] = (UInt32)source.Ip;
            dic["@time_login"] = (DateTime)source.TimeLogin;
            dic["@time_logout"] = (Nullable<DateTime>)source.TimeLogout;
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
        /// Copies the values from the given <paramref name="source"/> into this LogAccountActivityTable.
        /// </summary>
        /// <param name="source">The ILogAccountActivityTable to copy the values from.</param>
        public void CopyValuesFrom(ILogAccountActivityTable source)
        {
            AccountId = (Int32)source.AccountId;
            ID = (Int32)source.ID;
            Ip = (UInt32)source.Ip;
            TimeLogin = (DateTime)source.TimeLogin;
            TimeLogout = (Nullable<DateTime>)source.TimeLogout;
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
                case "account_id":
                    return new ColumnMetadata("account_id", "", "int(11)", null, typeof(Int32), false, false, true);

                case "id":
                    return new ColumnMetadata("id", "", "int(11)", null, typeof(Int32), false, true, false);

                case "ip":
                    return new ColumnMetadata("ip", "The IP address used, formatted as an unsigned 32-bit integer",
                                              "int(11) unsigned", null, typeof(UInt32), false, false, false);

                case "time_login":
                    return new ColumnMetadata("time_login", "", "datetime", null, typeof(DateTime), false, false, false);

                case "time_logout":
                    return new ColumnMetadata("time_logout", "", "datetime", null, typeof(Nullable<DateTime>), true, false, false);

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
                case "account_id":
                    return AccountId;

                case "id":
                    return ID;

                case "ip":
                    return Ip;

                case "time_login":
                    return TimeLogin;

                case "time_logout":
                    return TimeLogout;

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
                case "account_id":
                    AccountId = (Int32)value;
                    break;

                case "id":
                    ID = (Int32)value;
                    break;

                case "ip":
                    Ip = (UInt32)value;
                    break;

                case "time_login":
                    TimeLogin = (DateTime)value;
                    break;

                case "time_logout":
                    TimeLogout = (Nullable<DateTime>)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region ILogAccountActivityTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `account_id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        public Int32 AccountId
        {
            get { return (Int32)_accountId; }
            set { _accountId = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        public Int32 ID
        {
            get { return (Int32)_iD; }
            set { _iD = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `ip`.
        /// The underlying database type is `int(11) unsigned`. The database column contains the comment: 
        /// "The IP address used, formatted as an unsigned 32-bit integer".
        /// </summary>
        public UInt32 Ip
        {
            get { return (UInt32)_ip; }
            set { _ip = (UInt32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `time_login`.
        /// The underlying database type is `datetime`.
        /// </summary>
        public DateTime TimeLogin
        {
            get { return (DateTime)_timeLogin; }
            set { _timeLogin = (DateTime)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `time_logout`.
        /// The underlying database type is `datetime`.
        /// </summary>
        public Nullable<DateTime> TimeLogout
        {
            get { return (Nullable<DateTime>)_timeLogout; }
            set { _timeLogout = (Nullable<DateTime>)value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public ILogAccountActivityTable DeepCopy()
        {
            return new LogAccountActivityTable(this);
        }

        #endregion
    }
}