using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.IO;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `quest_reward`.
    /// </summary>
    public class QuestRewardTable : IQuestRewardTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 4;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "quest_reward";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "id", "quest_id", "type", "value" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "quest_id", "type", "value" };

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Int32 _iD;

        /// <summary>
        /// The field that maps onto the database column `quest_id`.
        /// </summary>
        Int32 _questId;

        /// <summary>
        /// The field that maps onto the database column `type`.
        /// </summary>
        Byte _type;

        /// <summary>
        /// The field that maps onto the database column `value`.
        /// </summary>
        String _value;

        /// <summary>
        /// QuestRewardTable constructor.
        /// </summary>
        public QuestRewardTable()
        {
        }

        /// <summary>
        /// QuestRewardTable constructor.
        /// </summary>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="questId">The initial value for the corresponding property.</param>
        /// <param name="type">The initial value for the corresponding property.</param>
        /// <param name="value">The initial value for the corresponding property.</param>
        public QuestRewardTable(Int32 @iD, Int32 @questId, Byte @type, String @value)
        {
            ID = @iD;
            QuestId = @questId;
            Type = @type;
            Value = @value;
        }

        /// <summary>
        /// QuestRewardTable constructor.
        /// </summary>
        /// <param name="source">IQuestRewardTable to copy the initial values from.</param>
        public QuestRewardTable(IQuestRewardTable source)
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
        public static void CopyValues(IQuestRewardTable source, IDictionary<String, Object> dic)
        {
            dic["@id"] = source.ID;
            dic["@quest_id"] = source.QuestId;
            dic["@type"] = source.Type;
            dic["@value"] = source.Value;
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
        /// Copies the values from the given <paramref name="source"/> into this QuestRewardTable.
        /// </summary>
        /// <param name="source">The IQuestRewardTable to copy the values from.</param>
        public void CopyValuesFrom(IQuestRewardTable source)
        {
            ID = source.ID;
            QuestId = source.QuestId;
            Type = source.Type;
            Value = source.Value;
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
                    return new ColumnMetadata("id", "", "int(11)", null, typeof(Int32), false, true, false);

                case "quest_id":
                    return new ColumnMetadata("quest_id", "", "int(11)", null, typeof(Int32), false, false, true);

                case "type":
                    return new ColumnMetadata("type", "", "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                case "value":
                    return new ColumnMetadata("value", "", "text", null, typeof(String), true, false, false);

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

                case "quest_id":
                    return QuestId;

                case "type":
                    return Type;

                case "value":
                    return Value;

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
                    ID = (Int32)value;
                    break;

                case "quest_id":
                    QuestId = (Int32)value;
                    break;

                case "type":
                    Type = (Byte)value;
                    break;

                case "value":
                    Value = (String)value;
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

        #region IQuestRewardTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        [SyncValue]
        public Int32 ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `quest_id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        [SyncValue]
        public Int32 QuestId
        {
            get { return _questId; }
            set { _questId = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `type`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        [SyncValue]
        public Byte Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `value`.
        /// The underlying database type is `text`.
        /// </summary>
        [SyncValue]
        public String Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IQuestRewardTable DeepCopy()
        {
            return new QuestRewardTable(this);
        }

        #endregion
    }
}