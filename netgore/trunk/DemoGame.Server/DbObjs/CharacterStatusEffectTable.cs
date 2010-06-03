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

This file was generated on (UTC): 6/2/2010 10:29:24 PM
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
    /// Provides a strongly-typed structure for the database table `character_status_effect`.
    /// </summary>
    public class CharacterStatusEffectTable : ICharacterStatusEffectTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 5;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "character_status_effect";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[] { "character_id", "id", "power", "status_effect_id", "time_left_secs" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[] { "character_id", "power", "status_effect_id", "time_left_secs" };

        /// <summary>
        /// The field that maps onto the database column `character_id`.
        /// </summary>
        Int32 _characterID;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Int32 _iD;

        /// <summary>
        /// The field that maps onto the database column `power`.
        /// </summary>
        UInt16 _power;

        /// <summary>
        /// The field that maps onto the database column `status_effect_id`.
        /// </summary>
        Byte _statusEffect;

        /// <summary>
        /// The field that maps onto the database column `time_left_secs`.
        /// </summary>
        UInt16 _timeLeftSecs;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterStatusEffectTable"/> class.
        /// </summary>
        public CharacterStatusEffectTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterStatusEffectTable"/> class.
        /// </summary>
        /// <param name="characterID">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="power">The initial value for the corresponding property.</param>
        /// <param name="statusEffect">The initial value for the corresponding property.</param>
        /// <param name="timeLeftSecs">The initial value for the corresponding property.</param>
        public CharacterStatusEffectTable(CharacterID @characterID, ActiveStatusEffectID @iD, UInt16 @power,
                                          StatusEffectType @statusEffect, UInt16 @timeLeftSecs)
        {
            CharacterID = @characterID;
            ID = @iD;
            Power = @power;
            StatusEffect = @statusEffect;
            TimeLeftSecs = @timeLeftSecs;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterStatusEffectTable"/> class.
        /// </summary>
        /// <param name="source">ICharacterStatusEffectTable to copy the initial values from.</param>
        public CharacterStatusEffectTable(ICharacterStatusEffectTable source)
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
        public static void CopyValues(ICharacterStatusEffectTable source, IDictionary<String, Object> dic)
        {
            dic["character_id"] = source.CharacterID;
            dic["id"] = source.ID;
            dic["power"] = source.Power;
            dic["status_effect_id"] = source.StatusEffect;
            dic["time_left_secs"] = source.TimeLeftSecs;
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
        /// Copies the values from the given <paramref name="source"/> into this CharacterStatusEffectTable.
        /// </summary>
        /// <param name="source">The ICharacterStatusEffectTable to copy the values from.</param>
        public void CopyValuesFrom(ICharacterStatusEffectTable source)
        {
            CharacterID = source.CharacterID;
            ID = source.ID;
            Power = source.Power;
            StatusEffect = source.StatusEffect;
            TimeLeftSecs = source.TimeLeftSecs;
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
                    return new ColumnMetadata("character_id", "ID of the Character that the status effect is on.", "int(11)", null,
                                              typeof(Int32), false, false, true);

                case "id":
                    return new ColumnMetadata("id", "Unique ID of the status effect instance.", "int(11)", null, typeof(Int32),
                                              false, true, false);

                case "power":
                    return new ColumnMetadata("power", "The power of this status effect instance.", "smallint(5) unsigned", null,
                                              typeof(UInt16), false, false, false);

                case "status_effect_id":
                    return new ColumnMetadata("status_effect_id",
                                              "ID of the status effect that this effect is for. This corresponds to the StatusEffectType enum's value.",
                                              "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                case "time_left_secs":
                    return new ColumnMetadata("time_left_secs", "The amount of time remaining for this status effect in seconds.",
                                              "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

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

                case "id":
                    return ID;

                case "power":
                    return Power;

                case "status_effect_id":
                    return StatusEffect;

                case "time_left_secs":
                    return TimeLeftSecs;

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

                case "id":
                    ID = (ActiveStatusEffectID)value;
                    break;

                case "power":
                    Power = (UInt16)value;
                    break;

                case "status_effect_id":
                    StatusEffect = (StatusEffectType)value;
                    break;

                case "time_left_secs":
                    TimeLeftSecs = (UInt16)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region ICharacterStatusEffectTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_id`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "ID of the Character that the status effect is on.".
        /// </summary>
        [Description("ID of the Character that the status effect is on.")]
        [SyncValue]
        public CharacterID CharacterID
        {
            get { return (CharacterID)_characterID; }
            set { _characterID = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(11)`. The database column contains the comment: 
        /// "Unique ID of the status effect instance.".
        /// </summary>
        [Description("Unique ID of the status effect instance.")]
        [SyncValue]
        public ActiveStatusEffectID ID
        {
            get { return (ActiveStatusEffectID)_iD; }
            set { _iD = (Int32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `power`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The power of this status effect instance.".
        /// </summary>
        [Description("The power of this status effect instance.")]
        [SyncValue]
        public UInt16 Power
        {
            get { return _power; }
            set { _power = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `status_effect_id`.
        /// The underlying database type is `tinyint(3) unsigned`. The database column contains the comment: 
        /// "ID of the status effect that this effect is for. This corresponds to the StatusEffectType enum's value.".
        /// </summary>
        [Description("ID of the status effect that this effect is for. This corresponds to the StatusEffectType enum's value.")]
        [SyncValue]
        public StatusEffectType StatusEffect
        {
            get { return (StatusEffectType)_statusEffect; }
            set { _statusEffect = (Byte)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `time_left_secs`.
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "The amount of time remaining for this status effect in seconds.".
        /// </summary>
        [Description("The amount of time remaining for this status effect in seconds.")]
        [SyncValue]
        public UInt16 TimeLeftSecs
        {
            get { return _timeLeftSecs; }
            set { _timeLeftSecs = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public virtual ICharacterStatusEffectTable DeepCopy()
        {
            return new CharacterStatusEffectTable(this);
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