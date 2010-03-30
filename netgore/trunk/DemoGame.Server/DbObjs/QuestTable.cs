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

This file was generated on (UTC): 3/30/2010 12:13:03 AM
********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.Features.Quests;
using NetGore.IO;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `quest`.
    /// </summary>
    public class QuestTable : IQuestTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 4;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "quest";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "id", "repeatable", "reward_cash", "reward_exp" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "repeatable", "reward_cash", "reward_exp" };

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        UInt16 _iD;

        /// <summary>
        /// The field that maps onto the database column `repeatable`.
        /// </summary>
        Boolean _repeatable;

        /// <summary>
        /// The field that maps onto the database column `reward_cash`.
        /// </summary>
        Int32 _rewardCash;

        /// <summary>
        /// The field that maps onto the database column `reward_exp`.
        /// </summary>
        Int32 _rewardExp;

        /// <summary>
        /// QuestTable constructor.
        /// </summary>
        public QuestTable()
        {
        }

        /// <summary>
        /// QuestTable constructor.
        /// </summary>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="repeatable">The initial value for the corresponding property.</param>
        /// <param name="rewardCash">The initial value for the corresponding property.</param>
        /// <param name="rewardExp">The initial value for the corresponding property.</param>
        public QuestTable(QuestID @iD, Boolean @repeatable, Int32 @rewardCash, Int32 @rewardExp)
        {
            ID = @iD;
            Repeatable = @repeatable;
            RewardCash = @rewardCash;
            RewardExp = @rewardExp;
        }

        /// <summary>
        /// QuestTable constructor.
        /// </summary>
        /// <param name="source">IQuestTable to copy the initial values from.</param>
        public QuestTable(IQuestTable source)
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
        public static void CopyValues(IQuestTable source, IDictionary<String, Object> dic)
        {
            dic["@id"] = source.ID;
            dic["@repeatable"] = source.Repeatable;
            dic["@reward_cash"] = source.RewardCash;
            dic["@reward_exp"] = source.RewardExp;
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
        /// Copies the values from the given <paramref name="source"/> into this QuestTable.
        /// </summary>
        /// <param name="source">The IQuestTable to copy the values from.</param>
        public void CopyValuesFrom(IQuestTable source)
        {
            ID = source.ID;
            Repeatable = source.Repeatable;
            RewardCash = source.RewardCash;
            RewardExp = source.RewardExp;
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
                case "id":
                    return new ColumnMetadata("id", "", "smallint(5) unsigned", null, typeof(UInt16), false, true, false);

                case "repeatable":
                    return new ColumnMetadata("repeatable", "", "tinyint(1) unsigned", "0", typeof(Boolean), false, false, false);

                case "reward_cash":
                    return new ColumnMetadata("reward_cash", "", "int(11)", "0", typeof(Int32), false, false, false);

                case "reward_exp":
                    return new ColumnMetadata("reward_exp", "", "int(11)", "0", typeof(Int32), false, false, false);

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
                case "id":
                    return ID;

                case "repeatable":
                    return Repeatable;

                case "reward_cash":
                    return RewardCash;

                case "reward_exp":
                    return RewardExp;

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
                case "id":
                    ID = (QuestID)value;
                    break;

                case "repeatable":
                    Repeatable = (Boolean)value;
                    break;

                case "reward_cash":
                    RewardCash = (Int32)value;
                    break;

                case "reward_exp":
                    RewardExp = (Int32)value;
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

        #region IQuestTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public QuestID ID
        {
            get { return (QuestID)_iD; }
            set { _iD = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `repeatable`.
        /// The underlying database type is `tinyint(1) unsigned` with the default value of `0`.
        /// </summary>
        [SyncValue]
        public Boolean Repeatable
        {
            get { return _repeatable; }
            set { _repeatable = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `reward_cash`.
        /// The underlying database type is `int(11)` with the default value of `0`.
        /// </summary>
        [SyncValue]
        public Int32 RewardCash
        {
            get { return _rewardCash; }
            set { _rewardCash = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `reward_exp`.
        /// The underlying database type is `int(11)` with the default value of `0`.
        /// </summary>
        [SyncValue]
        public Int32 RewardExp
        {
            get { return _rewardExp; }
            set { _rewardExp = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IQuestTable DeepCopy()
        {
            return new QuestTable(this);
        }

        #endregion
    }
}