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

This file was generated on (UTC): 3/22/2010 2:25:30 AM
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
    /// Provides a strongly-typed structure for the database table `character_quest_status`.
    /// </summary>
    public class CharacterQuestStatusTable : ICharacterQuestStatusTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 4;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "character_quest_status";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "character_id", "completed_on", "quest_id", "started_on" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "character_id", "quest_id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "completed_on", "started_on" };

        /// <summary>
        /// The field that maps onto the database column `character_id`.
        /// </summary>
        Int32 _characterID;

        /// <summary>
        /// The field that maps onto the database column `completed_on`.
        /// </summary>
        DateTime? _completedOn;

        /// <summary>
        /// The field that maps onto the database column `quest_id`.
        /// </summary>
        UInt16 _questID;

        /// <summary>
        /// The field that maps onto the database column `started_on`.
        /// </summary>
        DateTime _startedOn;

        /// <summary>
        /// CharacterQuestStatusTable constructor.
        /// </summary>
        public CharacterQuestStatusTable()
        {
        }

        /// <summary>
        /// CharacterQuestStatusTable constructor.
        /// </summary>
        /// <param name="characterID">The initial value for the corresponding property.</param>
        /// <param name="completedOn">The initial value for the corresponding property.</param>
        /// <param name="questID">The initial value for the corresponding property.</param>
        /// <param name="startedOn">The initial value for the corresponding property.</param>
        public CharacterQuestStatusTable(CharacterID @characterID, DateTime? @completedOn, QuestID @questID, DateTime @startedOn)
        {
            CharacterID = @characterID;
            CompletedOn = @completedOn;
            QuestID = @questID;
            StartedOn = @startedOn;
        }

        /// <summary>
        /// CharacterQuestStatusTable constructor.
        /// </summary>
        /// <param name="source">ICharacterQuestStatusTable to copy the initial values from.</param>
        public CharacterQuestStatusTable(ICharacterQuestStatusTable source)
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
        public static void CopyValues(ICharacterQuestStatusTable source, IDictionary<String, Object> dic)
        {
            dic["@character_id"] = source.CharacterID;
            dic["@completed_on"] = source.CompletedOn;
            dic["@quest_id"] = source.QuestID;
            dic["@started_on"] = source.StartedOn;
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
        /// Copies the values from the given <paramref name="source"/> into this CharacterQuestStatusTable.
        /// </summary>
        /// <param name="source">The ICharacterQuestStatusTable to copy the values from.</param>
        public void CopyValuesFrom(ICharacterQuestStatusTable source)
        {
            CharacterID = source.CharacterID;
            CompletedOn = source.CompletedOn;
            QuestID = source.QuestID;
            StartedOn = source.StartedOn;
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
                case "character_id":
                    return new ColumnMetadata("character_id", "", "int(11)", null, typeof(Int32), false, true, false);

                case "completed_on":
                    return new ColumnMetadata("completed_on", "", "datetime", null, typeof(DateTime?), true, false, false);

                case "quest_id":
                    return new ColumnMetadata("quest_id", "", "smallint(5) unsigned", null, typeof(UInt16), false, true, false);

                case "started_on":
                    return new ColumnMetadata("started_on", "", "datetime", null, typeof(DateTime), false, false, false);

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
                case "character_id":
                    return CharacterID;

                case "completed_on":
                    return CompletedOn;

                case "quest_id":
                    return QuestID;

                case "started_on":
                    return StartedOn;

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
                case "character_id":
                    CharacterID = (CharacterID)value;
                    break;

                case "completed_on":
                    CompletedOn = (DateTime?)value;
                    break;

                case "quest_id":
                    QuestID = (QuestID)value;
                    break;

                case "started_on":
                    StartedOn = (DateTime)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region ICharacterQuestStatusTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        [SyncValue]
        public CharacterID CharacterID
        {
            get { return (CharacterID)_characterID; }
            set { _characterID = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `completed_on`.
        /// The underlying database type is `datetime`.
        /// </summary>
        [SyncValue]
        public DateTime? CompletedOn
        {
            get { return _completedOn; }
            set { _completedOn = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `quest_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public QuestID QuestID
        {
            get { return (QuestID)_questID; }
            set { _questID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `started_on`.
        /// The underlying database type is `datetime`.
        /// </summary>
        [SyncValue]
        public DateTime StartedOn
        {
            get { return _startedOn; }
            set { _startedOn = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public ICharacterQuestStatusTable DeepCopy()
        {
            return new CharacterQuestStatusTable(this);
        }

        #endregion

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
    }
}