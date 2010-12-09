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
    /// Provides a strongly-typed structure for the database table `character_template_skill`.
    /// </summary>
    public class CharacterTemplateSkillTable : ICharacterTemplateSkillTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 2;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "character_template_skill";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "character_template_id", "skill_id" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "character_template_id", "skill_id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { };

        /// <summary>
        /// The field that maps onto the database column `character_template_id`.
        /// </summary>
        UInt16 _characterTemplateID;

        /// <summary>
        /// The field that maps onto the database column `skill_id`.
        /// </summary>
        Byte _skillID;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateSkillTable"/> class.
        /// </summary>
        public CharacterTemplateSkillTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateSkillTable"/> class.
        /// </summary>
        /// <param name="characterTemplateID">The initial value for the corresponding property.</param>
        /// <param name="skillID">The initial value for the corresponding property.</param>
        public CharacterTemplateSkillTable(CharacterTemplateID @characterTemplateID, SkillType @skillID)
        {
            CharacterTemplateID = @characterTemplateID;
            SkillID = @skillID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateSkillTable"/> class.
        /// </summary>
        /// <param name="source">ICharacterTemplateSkillTable to copy the initial values from.</param>
        public CharacterTemplateSkillTable(ICharacterTemplateSkillTable source)
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
        public static void CopyValues(ICharacterTemplateSkillTable source, IDictionary<String, Object> dic)
        {
            dic["character_template_id"] = source.CharacterTemplateID;
            dic["skill_id"] = source.SkillID;
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
        /// Copies the values from the given <paramref name="source"/> into this CharacterTemplateSkillTable.
        /// </summary>
        /// <param name="source">The ICharacterTemplateSkillTable to copy the values from.</param>
        public void CopyValuesFrom(ICharacterTemplateSkillTable source)
        {
            CharacterTemplateID = source.CharacterTemplateID;
            SkillID = source.SkillID;
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
                case "character_template_id":
                    return new ColumnMetadata("character_template_id", "The character template that knows the skill.",
                        "smallint(5) unsigned", null, typeof(UInt16), false, true, false);

                case "skill_id":
                    return new ColumnMetadata("skill_id", "The skill the character template knows.", "tinyint(5) unsigned", null,
                        typeof(Byte), false, true, false);

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
                case "character_template_id":
                    return CharacterTemplateID;

                case "skill_id":
                    return SkillID;

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
                case "character_template_id":
                    CharacterTemplateID = (CharacterTemplateID)value;
                    break;

                case "skill_id":
                    SkillID = (SkillType)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region ICharacterTemplateSkillTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_template_id`.
        /// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
        /// "The character template that knows the skill.".
        /// </summary>
        [Description("The character template that knows the skill.")]
        [SyncValue]
        public CharacterTemplateID CharacterTemplateID
        {
            get { return (CharacterTemplateID)_characterTemplateID; }
            set { _characterTemplateID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `skill_id`.
        /// The underlying database type is `tinyint(5) unsigned`.The database column contains the comment: 
        /// "The skill the character template knows.".
        /// </summary>
        [Description("The skill the character template knows.")]
        [SyncValue]
        public SkillType SkillID
        {
            get { return (SkillType)_skillID; }
            set { _skillID = (Byte)value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public virtual ICharacterTemplateSkillTable DeepCopy()
        {
            return new CharacterTemplateSkillTable(this);
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