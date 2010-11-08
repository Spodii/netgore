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
    /// Provides a strongly-typed structure for the database table `account_ban`.
    /// </summary>
    public class AccountBanTable : IAccountBanTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 7;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "account_ban";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        { "account_id", "end_time", "expired", "id", "issued_by", "reason", "start_time" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        { "account_id", "end_time", "expired", "issued_by", "reason", "start_time" };

        /// <summary>
        /// The field that maps onto the database column `account_id`.
        /// </summary>
        Int32 _accountID;

        /// <summary>
        /// The field that maps onto the database column `end_time`.
        /// </summary>
        DateTime _endTime;

        /// <summary>
        /// The field that maps onto the database column `expired`.
        /// </summary>
        Boolean _expired;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Int32 _iD;

        /// <summary>
        /// The field that maps onto the database column `issued_by`.
        /// </summary>
        String _issuedBy;

        /// <summary>
        /// The field that maps onto the database column `reason`.
        /// </summary>
        String _reason;

        /// <summary>
        /// The field that maps onto the database column `start_time`.
        /// </summary>
        DateTime _startTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountBanTable"/> class.
        /// </summary>
        public AccountBanTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountBanTable"/> class.
        /// </summary>
        /// <param name="accountID">The initial value for the corresponding property.</param>
        /// <param name="endTime">The initial value for the corresponding property.</param>
        /// <param name="expired">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="issuedBy">The initial value for the corresponding property.</param>
        /// <param name="reason">The initial value for the corresponding property.</param>
        /// <param name="startTime">The initial value for the corresponding property.</param>
        public AccountBanTable(AccountID @accountID, DateTime @endTime, Boolean @expired, Int32 @iD, String @issuedBy,
                               String @reason, DateTime @startTime)
        {
            AccountID = @accountID;
            EndTime = @endTime;
            Expired = @expired;
            ID = @iD;
            IssuedBy = @issuedBy;
            Reason = @reason;
            StartTime = @startTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountBanTable"/> class.
        /// </summary>
        /// <param name="source">IAccountBanTable to copy the initial values from.</param>
        public AccountBanTable(IAccountBanTable source)
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
        public static void CopyValues(IAccountBanTable source, IDictionary<String, Object> dic)
        {
            dic["account_id"] = source.AccountID;
            dic["end_time"] = source.EndTime;
            dic["expired"] = source.Expired;
            dic["id"] = source.ID;
            dic["issued_by"] = source.IssuedBy;
            dic["reason"] = source.Reason;
            dic["start_time"] = source.StartTime;
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
        /// Copies the values from the given <paramref name="source"/> into this AccountBanTable.
        /// </summary>
        /// <param name="source">The IAccountBanTable to copy the values from.</param>
        public void CopyValuesFrom(IAccountBanTable source)
        {
            AccountID = source.AccountID;
            EndTime = source.EndTime;
            Expired = source.Expired;
            ID = source.ID;
            IssuedBy = source.IssuedBy;
            Reason = source.Reason;
            StartTime = source.StartTime;
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
                    return new ColumnMetadata("account_id", "The account that this ban is for.", "int(11)", null, typeof(Int32),
                                              false, false, true);

                case "end_time":
                    return new ColumnMetadata("end_time", "When this ban ends.", "datetime", null, typeof(DateTime), false, false,
                                              false);

                case "expired":
                    return new ColumnMetadata("expired", "If the ban is expired. A non-zero value means true.",
                                              "tinyint(1) unsigned", "0", typeof(Boolean), false, false, true);

                case "id":
                    return new ColumnMetadata("id", "The unique ban ID.", "int(11)", null, typeof(Int32), false, true, false);

                case "issued_by":
                    return new ColumnMetadata("issued_by", "Name of the person or system that issued this ban.", "varchar(255)",
                                              null, typeof(String), true, false, false);

                case "reason":
                    return new ColumnMetadata("reason", "The reason why this account was banned.", "varchar(255)", null,
                                              typeof(String), false, false, false);

                case "start_time":
                    return new ColumnMetadata("start_time", "When this ban started.", "datetime", null, typeof(DateTime), false,
                                              false, false);

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

                case "end_time":
                    return EndTime;

                case "expired":
                    return Expired;

                case "id":
                    return ID;

                case "issued_by":
                    return IssuedBy;

                case "reason":
                    return Reason;

                case "start_time":
                    return StartTime;

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

                case "end_time":
                    EndTime = (DateTime)value;
                    break;

                case "expired":
                    Expired = (Boolean)value;
                    break;

                case "id":
                    ID = (Int32)value;
                    break;

                case "issued_by":
                    IssuedBy = (String)value;
                    break;

                case "reason":
                    Reason = (String)value;
                    break;

                case "start_time":
                    StartTime = (DateTime)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IAccountBanTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `account_id`.
        /// The underlying database type is `int(11)`.The database column contains the comment: 
        /// "The account that this ban is for.".
        /// </summary>
        [Description("The account that this ban is for.")]
        [SyncValue]
        public AccountID AccountID
        {
            get { return (AccountID)_accountID; }
            set { _accountID = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `end_time`.
        /// The underlying database type is `datetime`.The database column contains the comment: 
        /// "When this ban ends.".
        /// </summary>
        [Description("When this ban ends.")]
        [SyncValue]
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `expired`.
        /// The underlying database type is `tinyint(1) unsigned` with the default value of `0`.The database column contains the comment: 
        /// "If the ban is expired. A non-zero value means true.".
        /// </summary>
        [Description("If the ban is expired. A non-zero value means true.")]
        [SyncValue]
        public Boolean Expired
        {
            get { return _expired; }
            set { _expired = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(11)`.The database column contains the comment: 
        /// "The unique ban ID.".
        /// </summary>
        [Description("The unique ban ID.")]
        [SyncValue]
        public Int32 ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `issued_by`.
        /// The underlying database type is `varchar(255)`.The database column contains the comment: 
        /// "Name of the person or system that issued this ban.".
        /// </summary>
        [Description("Name of the person or system that issued this ban.")]
        [SyncValue]
        public String IssuedBy
        {
            get { return _issuedBy; }
            set { _issuedBy = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `reason`.
        /// The underlying database type is `varchar(255)`.The database column contains the comment: 
        /// "The reason why this account was banned.".
        /// </summary>
        [Description("The reason why this account was banned.")]
        [SyncValue]
        public String Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `start_time`.
        /// The underlying database type is `datetime`.The database column contains the comment: 
        /// "When this ban started.".
        /// </summary>
        [Description("When this ban started.")]
        [SyncValue]
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public virtual IAccountBanTable DeepCopy()
        {
            return new AccountBanTable(this);
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