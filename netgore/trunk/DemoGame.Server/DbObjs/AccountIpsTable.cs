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
    /// Provides a strongly-typed structure for the database table `account_ips`.
    /// </summary>
    public class AccountIpsTable : IAccountIpsTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 4;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "account_ips";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "account_id", "id", "ip", "time" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "account_id", "ip", "time" };

        /// <summary>
        /// The field that maps onto the database column `account_id`.
        /// </summary>
        Int32 _accountID;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        UInt32 _iD;

        /// <summary>
        /// The field that maps onto the database column `ip`.
        /// </summary>
        UInt32 _ip;

        /// <summary>
        /// The field that maps onto the database column `time`.
        /// </summary>
        DateTime _time;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountIpsTable"/> class.
        /// </summary>
        public AccountIpsTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountIpsTable"/> class.
        /// </summary>
        /// <param name="accountID">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="ip">The initial value for the corresponding property.</param>
        /// <param name="time">The initial value for the corresponding property.</param>
        public AccountIpsTable(AccountID @accountID, UInt32 @iD, UInt32 @ip, DateTime @time)
        {
            AccountID = @accountID;
            ID = @iD;
            Ip = @ip;
            Time = @time;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountIpsTable"/> class.
        /// </summary>
        /// <param name="source">IAccountIpsTable to copy the initial values from.</param>
        public AccountIpsTable(IAccountIpsTable source)
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
        public static void CopyValues(IAccountIpsTable source, IDictionary<String, Object> dic)
        {
            dic["account_id"] = source.AccountID;
            dic["id"] = source.ID;
            dic["ip"] = source.Ip;
            dic["time"] = source.Time;
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
        /// Copies the values from the given <paramref name="source"/> into this AccountIpsTable.
        /// </summary>
        /// <param name="source">The IAccountIpsTable to copy the values from.</param>
        public void CopyValuesFrom(IAccountIpsTable source)
        {
            AccountID = source.AccountID;
            ID = source.ID;
            Ip = source.Ip;
            Time = source.Time;
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
                    return new ColumnMetadata("account_id", "The ID of the account.", "int(11)", null, typeof(Int32), false, false,
                        true);

                case "id":
                    return new ColumnMetadata("id", "", "int(10) unsigned", null, typeof(UInt32), false, true, false);

                case "ip":
                    return new ColumnMetadata("ip", "The IP that logged into the account.", "int(10) unsigned", null,
                        typeof(UInt32), false, false, false);

                case "time":
                    return new ColumnMetadata("time", "When this IP last logged into this account.", "datetime", null,
                        typeof(DateTime), false, false, false);

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
                    return AccountID;

                case "id":
                    return ID;

                case "ip":
                    return Ip;

                case "time":
                    return Time;

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
                    AccountID = (AccountID)value;
                    break;

                case "id":
                    ID = (UInt32)value;
                    break;

                case "ip":
                    Ip = (UInt32)value;
                    break;

                case "time":
                    Time = (DateTime)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IAccountIpsTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `account_id`.
        /// The underlying database type is `int(11)`.The database column contains the comment: 
        /// "The ID of the account.".
        /// </summary>
        [Description("The ID of the account.")]
        [SyncValue]
        public AccountID AccountID
        {
            get { return (AccountID)_accountID; }
            set { _accountID = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(10) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt32 ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `ip`.
        /// The underlying database type is `int(10) unsigned`.The database column contains the comment: 
        /// "The IP that logged into the account.".
        /// </summary>
        [Description("The IP that logged into the account.")]
        [SyncValue]
        public UInt32 Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `time`.
        /// The underlying database type is `datetime`.The database column contains the comment: 
        /// "When this IP last logged into this account.".
        /// </summary>
        [Description("When this IP last logged into this account.")]
        [SyncValue]
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public virtual IAccountIpsTable DeepCopy()
        {
            return new AccountIpsTable(this);
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