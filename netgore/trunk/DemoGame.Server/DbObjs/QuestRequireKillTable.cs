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
    /// Provides a strongly-typed structure for the database table `quest_require_kill`.
    /// </summary>
    public class QuestRequireKillTable : IQuestRequireKillTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 3;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "quest_require_kill";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "amount", "character_template_id", "quest_id" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "character_template_id", "quest_id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "amount" };

        /// <summary>
        /// The field that maps onto the database column `amount`.
        /// </summary>
        UInt16 _amount;

        /// <summary>
        /// The field that maps onto the database column `character_template_id`.
        /// </summary>
        UInt16 _characterTemplateID;

        /// <summary>
        /// The field that maps onto the database column `quest_id`.
        /// </summary>
        UInt16 _questID;

        /// <summary>
        /// QuestRequireKillTable constructor.
        /// </summary>
        public QuestRequireKillTable()
        {
        }

        /// <summary>
        /// QuestRequireKillTable constructor.
        /// </summary>
        /// <param name="amount">The initial value for the corresponding property.</param>
        /// <param name="characterTemplateID">The initial value for the corresponding property.</param>
        /// <param name="questID">The initial value for the corresponding property.</param>
        public QuestRequireKillTable(UInt16 @amount, CharacterTemplateID @characterTemplateID, QuestID @questID)
        {
            Amount = @amount;
            CharacterTemplateID = @characterTemplateID;
            QuestID = @questID;
        }

        /// <summary>
        /// QuestRequireKillTable constructor.
        /// </summary>
        /// <param name="source">IQuestRequireKillTable to copy the initial values from.</param>
        public QuestRequireKillTable(IQuestRequireKillTable source)
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
        public static void CopyValues(IQuestRequireKillTable source, IDictionary<String, Object> dic)
        {
            dic["@amount"] = source.Amount;
            dic["@character_template_id"] = source.CharacterTemplateID;
            dic["@quest_id"] = source.QuestID;
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
        /// Copies the values from the given <paramref name="source"/> into this QuestRequireKillTable.
        /// </summary>
        /// <param name="source">The IQuestRequireKillTable to copy the values from.</param>
        public void CopyValuesFrom(IQuestRequireKillTable source)
        {
            Amount = source.Amount;
            CharacterTemplateID = source.CharacterTemplateID;
            QuestID = source.QuestID;
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
                case "amount":
                    return new ColumnMetadata("amount", "", "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

                case "character_template_id":
                    return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(UInt16), false,
                                              true, false);

                case "quest_id":
                    return new ColumnMetadata("quest_id", "", "smallint(5) unsigned", null, typeof(UInt16), false, true, false);

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
                case "amount":
                    return Amount;

                case "character_template_id":
                    return CharacterTemplateID;

                case "quest_id":
                    return QuestID;

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
                case "amount":
                    Amount = (UInt16)value;
                    break;

                case "character_template_id":
                    CharacterTemplateID = (CharacterTemplateID)value;
                    break;

                case "quest_id":
                    QuestID = (QuestID)value;
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

        #region IQuestRequireKillTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `amount`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt16 Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_template_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public CharacterTemplateID CharacterTemplateID
        {
            get { return (CharacterTemplateID)_characterTemplateID; }
            set { _characterTemplateID = (UInt16)value; }
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
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IQuestRequireKillTable DeepCopy()
        {
            return new QuestRequireKillTable(this);
        }

        #endregion
    }
}