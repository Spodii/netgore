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
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.AI;
using NetGore.Features.Shops;
using NetGore.IO;
using NetGore.NPCChat;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `character_template`.
    /// </summary>
    public class CharacterTemplateTable : ICharacterTemplateTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 22;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "character_template";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        {
            "ai_id", "alliance_id", "body_id", "chat_dialog", "exp", "give_cash", "give_exp", "id", "level", "move_speed", "name",
            "respawn", "shop_id", "statpoints", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp",
            "stat_minhit", "stat_str"
        };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        {
            "ai_id", "alliance_id", "body_id", "chat_dialog", "exp", "give_cash", "give_exp", "level", "move_speed", "name",
            "respawn", "shop_id", "statpoints", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp",
            "stat_minhit", "stat_str"
        };

        /// <summary>
        /// The fields that are used in the column collection `Stat`.
        /// </summary>
        static readonly String[] _statColumns = new string[]
        { "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str" };

        /// <summary>
        /// Dictionary containing the values for the column collection `Stat`.
        /// </summary>
        readonly StatTypeConstDictionary _stat = new StatTypeConstDictionary();

        /// <summary>
        /// The field that maps onto the database column `ai_id`.
        /// </summary>
        ushort? _aIID;

        /// <summary>
        /// The field that maps onto the database column `alliance_id`.
        /// </summary>
        Byte _allianceID;

        /// <summary>
        /// The field that maps onto the database column `body_id`.
        /// </summary>
        UInt16 _bodyID;

        /// <summary>
        /// The field that maps onto the database column `chat_dialog`.
        /// </summary>
        ushort? _chatDialog;

        /// <summary>
        /// The field that maps onto the database column `exp`.
        /// </summary>
        Int32 _exp;

        /// <summary>
        /// The field that maps onto the database column `give_cash`.
        /// </summary>
        UInt16 _giveCash;

        /// <summary>
        /// The field that maps onto the database column `give_exp`.
        /// </summary>
        UInt16 _giveExp;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        UInt16 _iD;

        /// <summary>
        /// The field that maps onto the database column `level`.
        /// </summary>
        Byte _level;

        /// <summary>
        /// The field that maps onto the database column `move_speed`.
        /// </summary>
        UInt16 _moveSpeed;

        /// <summary>
        /// The field that maps onto the database column `name`.
        /// </summary>
        String _name;

        /// <summary>
        /// The field that maps onto the database column `respawn`.
        /// </summary>
        UInt16 _respawn;

        /// <summary>
        /// The field that maps onto the database column `shop_id`.
        /// </summary>
        ushort? _shopID;

        /// <summary>
        /// The field that maps onto the database column `statpoints`.
        /// </summary>
        Int32 _statPoints;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateTable"/> class.
        /// </summary>
        public CharacterTemplateTable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateTable"/> class.
        /// </summary>
        /// <param name="aIID">The initial value for the corresponding property.</param>
        /// <param name="allianceID">The initial value for the corresponding property.</param>
        /// <param name="bodyID">The initial value for the corresponding property.</param>
        /// <param name="chatDialog">The initial value for the corresponding property.</param>
        /// <param name="exp">The initial value for the corresponding property.</param>
        /// <param name="giveCash">The initial value for the corresponding property.</param>
        /// <param name="giveExp">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="level">The initial value for the corresponding property.</param>
        /// <param name="moveSpeed">The initial value for the corresponding property.</param>
        /// <param name="name">The initial value for the corresponding property.</param>
        /// <param name="respawn">The initial value for the corresponding property.</param>
        /// <param name="shopID">The initial value for the corresponding property.</param>
        /// <param name="statPoints">The initial value for the corresponding property.</param>
        /// <param name="statAgi">The initial value for the corresponding property.</param>
        /// <param name="statDefence">The initial value for the corresponding property.</param>
        /// <param name="statInt">The initial value for the corresponding property.</param>
        /// <param name="statMaxhit">The initial value for the corresponding property.</param>
        /// <param name="statMaxhp">The initial value for the corresponding property.</param>
        /// <param name="statMaxmp">The initial value for the corresponding property.</param>
        /// <param name="statMinhit">The initial value for the corresponding property.</param>
        /// <param name="statStr">The initial value for the corresponding property.</param>
        public CharacterTemplateTable(AIID? @aIID, AllianceID @allianceID, BodyID @bodyID, NPCChatDialogID? @chatDialog,
                                      Int32 @exp, UInt16 @giveCash, UInt16 @giveExp, CharacterTemplateID @iD, Byte @level,
                                      UInt16 @moveSpeed, String @name, UInt16 @respawn, ShopID? @shopID, Int32 @statPoints,
                                      Int16 @statAgi, Int16 @statDefence, Int16 @statInt, Int16 @statMaxhit, Int16 @statMaxhp,
                                      Int16 @statMaxmp, Int16 @statMinhit, Int16 @statStr)
        {
            AIID = @aIID;
            AllianceID = @allianceID;
            BodyID = @bodyID;
            ChatDialog = @chatDialog;
            Exp = @exp;
            GiveCash = @giveCash;
            GiveExp = @giveExp;
            ID = @iD;
            Level = @level;
            MoveSpeed = @moveSpeed;
            Name = @name;
            Respawn = @respawn;
            ShopID = @shopID;
            StatPoints = @statPoints;
            SetStat(StatType.Agi, @statAgi);
            SetStat(StatType.Defence, @statDefence);
            SetStat(StatType.Int, @statInt);
            SetStat(StatType.MaxHit, @statMaxhit);
            SetStat(StatType.MaxHP, @statMaxhp);
            SetStat(StatType.MaxMP, @statMaxmp);
            SetStat(StatType.MinHit, @statMinhit);
            SetStat(StatType.Str, @statStr);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterTemplateTable"/> class.
        /// </summary>
        /// <param name="source">ICharacterTemplateTable to copy the initial values from.</param>
        public CharacterTemplateTable(ICharacterTemplateTable source)
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
        /// Gets an IEnumerable of strings containing the name of the database
        /// columns used in the column collection `Stat`.
        /// </summary>
        public static IEnumerable<String> StatColumns
        {
            get { return _statColumns; }
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(ICharacterTemplateTable source, IDictionary<String, Object> dic)
        {
            dic["ai_id"] = source.AIID;
            dic["alliance_id"] = source.AllianceID;
            dic["body_id"] = source.BodyID;
            dic["chat_dialog"] = source.ChatDialog;
            dic["exp"] = source.Exp;
            dic["give_cash"] = source.GiveCash;
            dic["give_exp"] = source.GiveExp;
            dic["id"] = source.ID;
            dic["level"] = source.Level;
            dic["move_speed"] = source.MoveSpeed;
            dic["name"] = source.Name;
            dic["respawn"] = source.Respawn;
            dic["shop_id"] = source.ShopID;
            dic["statpoints"] = source.StatPoints;
            dic["stat_agi"] = (Int16)source.GetStat(StatType.Agi);
            dic["stat_defence"] = (Int16)source.GetStat(StatType.Defence);
            dic["stat_int"] = (Int16)source.GetStat(StatType.Int);
            dic["stat_maxhit"] = (Int16)source.GetStat(StatType.MaxHit);
            dic["stat_maxhp"] = (Int16)source.GetStat(StatType.MaxHP);
            dic["stat_maxmp"] = (Int16)source.GetStat(StatType.MaxMP);
            dic["stat_minhit"] = (Int16)source.GetStat(StatType.MinHit);
            dic["stat_str"] = (Int16)source.GetStat(StatType.Str);
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
        /// Copies the values from the given <paramref name="source"/> into this CharacterTemplateTable.
        /// </summary>
        /// <param name="source">The ICharacterTemplateTable to copy the values from.</param>
        public void CopyValuesFrom(ICharacterTemplateTable source)
        {
            AIID = source.AIID;
            AllianceID = source.AllianceID;
            BodyID = source.BodyID;
            ChatDialog = source.ChatDialog;
            Exp = source.Exp;
            GiveCash = source.GiveCash;
            GiveExp = source.GiveExp;
            ID = source.ID;
            Level = source.Level;
            MoveSpeed = source.MoveSpeed;
            Name = source.Name;
            Respawn = source.Respawn;
            ShopID = source.ShopID;
            StatPoints = source.StatPoints;
            SetStat(StatType.Agi, source.GetStat(StatType.Agi));
            SetStat(StatType.Defence, source.GetStat(StatType.Defence));
            SetStat(StatType.Int, source.GetStat(StatType.Int));
            SetStat(StatType.MaxHit, source.GetStat(StatType.MaxHit));
            SetStat(StatType.MaxHP, source.GetStat(StatType.MaxHP));
            SetStat(StatType.MaxMP, source.GetStat(StatType.MaxMP));
            SetStat(StatType.MinHit, source.GetStat(StatType.MinHit));
            SetStat(StatType.Str, source.GetStat(StatType.Str));
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
                case "ai_id":
                    return new ColumnMetadata("ai_id", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "alliance_id":
                    return new ColumnMetadata("alliance_id", "", "tinyint(3) unsigned", "0", typeof(Byte), false, false, true);

                case "body_id":
                    return new ColumnMetadata("body_id", "", "smallint(5) unsigned", "1", typeof(UInt16), false, false, false);

                case "chat_dialog":
                    return new ColumnMetadata("chat_dialog", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "exp":
                    return new ColumnMetadata("exp", "", "int(11)", "0", typeof(Int32), false, false, false);

                case "give_cash":
                    return new ColumnMetadata("give_cash", "", "smallint(5) unsigned", "0", typeof(UInt16), false, false, false);

                case "give_exp":
                    return new ColumnMetadata("give_exp", "", "smallint(5) unsigned", "0", typeof(UInt16), false, false, false);

                case "id":
                    return new ColumnMetadata("id", "", "smallint(5) unsigned", null, typeof(UInt16), false, true, false);

                case "level":
                    return new ColumnMetadata("level", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "move_speed":
                    return new ColumnMetadata("move_speed", "", "smallint(5) unsigned", "1800", typeof(UInt16), false, false,
                                              false);

                case "name":
                    return new ColumnMetadata("name", "", "varchar(50)", "New NPC", typeof(String), false, false, false);

                case "respawn":
                    return new ColumnMetadata("respawn", "", "smallint(5) unsigned", "5", typeof(UInt16), false, false, false);

                case "shop_id":
                    return new ColumnMetadata("shop_id", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, true);

                case "statpoints":
                    return new ColumnMetadata("statpoints", "", "int(11)", "0", typeof(Int32), false, false, false);

                case "stat_agi":
                    return new ColumnMetadata("stat_agi", "", "smallint(6)", "1", typeof(Int16), false, false, false);

                case "stat_defence":
                    return new ColumnMetadata("stat_defence", "", "smallint(6)", "1", typeof(Int16), false, false, false);

                case "stat_int":
                    return new ColumnMetadata("stat_int", "", "smallint(6)", "1", typeof(Int16), false, false, false);

                case "stat_maxhit":
                    return new ColumnMetadata("stat_maxhit", "", "smallint(6)", "1", typeof(Int16), false, false, false);

                case "stat_maxhp":
                    return new ColumnMetadata("stat_maxhp", "", "smallint(6)", "50", typeof(Int16), false, false, false);

                case "stat_maxmp":
                    return new ColumnMetadata("stat_maxmp", "", "smallint(6)", "50", typeof(Int16), false, false, false);

                case "stat_minhit":
                    return new ColumnMetadata("stat_minhit", "", "smallint(6)", "1", typeof(Int16), false, false, false);

                case "stat_str":
                    return new ColumnMetadata("stat_str", "", "smallint(6)", "1", typeof(Int16), false, false, false);

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
                case "ai_id":
                    return AIID;

                case "alliance_id":
                    return AllianceID;

                case "body_id":
                    return BodyID;

                case "chat_dialog":
                    return ChatDialog;

                case "exp":
                    return Exp;

                case "give_cash":
                    return GiveCash;

                case "give_exp":
                    return GiveExp;

                case "id":
                    return ID;

                case "level":
                    return Level;

                case "move_speed":
                    return MoveSpeed;

                case "name":
                    return Name;

                case "respawn":
                    return Respawn;

                case "shop_id":
                    return ShopID;

                case "statpoints":
                    return StatPoints;

                case "stat_agi":
                    return GetStat(StatType.Agi);

                case "stat_defence":
                    return GetStat(StatType.Defence);

                case "stat_int":
                    return GetStat(StatType.Int);

                case "stat_maxhit":
                    return GetStat(StatType.MaxHit);

                case "stat_maxhp":
                    return GetStat(StatType.MaxHP);

                case "stat_maxmp":
                    return GetStat(StatType.MaxMP);

                case "stat_minhit":
                    return GetStat(StatType.MinHit);

                case "stat_str":
                    return GetStat(StatType.Str);

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Gets the <paramref name="value"/> of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
        /// </summary>
        /// <param name="key">The key of the column to get.</param>
        /// <param name="value">The value to assign to the column for the corresponding <paramref name="key"/>.</param>
        public void SetStat(StatType key, Int32 value)
        {
            _stat[key] = (Int16)value;
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
                case "ai_id":
                    AIID = (AIID?)value;
                    break;

                case "alliance_id":
                    AllianceID = (AllianceID)value;
                    break;

                case "body_id":
                    BodyID = (BodyID)value;
                    break;

                case "chat_dialog":
                    ChatDialog = (NPCChatDialogID?)value;
                    break;

                case "exp":
                    Exp = (Int32)value;
                    break;

                case "give_cash":
                    GiveCash = (UInt16)value;
                    break;

                case "give_exp":
                    GiveExp = (UInt16)value;
                    break;

                case "id":
                    ID = (CharacterTemplateID)value;
                    break;

                case "level":
                    Level = (Byte)value;
                    break;

                case "move_speed":
                    MoveSpeed = (UInt16)value;
                    break;

                case "name":
                    Name = (String)value;
                    break;

                case "respawn":
                    Respawn = (UInt16)value;
                    break;

                case "shop_id":
                    ShopID = (ShopID?)value;
                    break;

                case "statpoints":
                    StatPoints = (Int32)value;
                    break;

                case "stat_agi":
                    SetStat(StatType.Agi, (Int32)value);
                    break;

                case "stat_defence":
                    SetStat(StatType.Defence, (Int32)value);
                    break;

                case "stat_int":
                    SetStat(StatType.Int, (Int32)value);
                    break;

                case "stat_maxhit":
                    SetStat(StatType.MaxHit, (Int32)value);
                    break;

                case "stat_maxhp":
                    SetStat(StatType.MaxHP, (Int32)value);
                    break;

                case "stat_maxmp":
                    SetStat(StatType.MaxMP, (Int32)value);
                    break;

                case "stat_minhit":
                    SetStat(StatType.MinHit, (Int32)value);
                    break;

                case "stat_str":
                    SetStat(StatType.Str, (Int32)value);
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region ICharacterTemplateTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `ai_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public AIID? AIID
        {
            get { return (Nullable<AIID>)_aIID; }
            set { _aIID = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `alliance_id`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
        /// </summary>
        [SyncValue]
        public AllianceID AllianceID
        {
            get { return (AllianceID)_allianceID; }
            set { _allianceID = (Byte)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `body_id`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
        /// </summary>
        [SyncValue]
        public BodyID BodyID
        {
            get { return (BodyID)_bodyID; }
            set { _bodyID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `chat_dialog`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public NPCChatDialogID? ChatDialog
        {
            get { return (Nullable<NPCChatDialogID>)_chatDialog; }
            set { _chatDialog = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `exp`.
        /// The underlying database type is `int(11)` with the default value of `0`.
        /// </summary>
        [SyncValue]
        public Int32 Exp
        {
            get { return _exp; }
            set { _exp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `give_cash`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        [SyncValue]
        public UInt16 GiveCash
        {
            get { return _giveCash; }
            set { _giveCash = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `give_exp`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        [SyncValue]
        public UInt16 GiveExp
        {
            get { return _giveExp; }
            set { _giveExp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public CharacterTemplateID ID
        {
            get { return (CharacterTemplateID)_iD; }
            set { _iD = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `level`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        [SyncValue]
        public Byte Level
        {
            get { return _level; }
            set { _level = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `move_speed`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `1800`.
        /// </summary>
        [SyncValue]
        public UInt16 MoveSpeed
        {
            get { return _moveSpeed; }
            set { _moveSpeed = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `name`.
        /// The underlying database type is `varchar(50)` with the default value of `New NPC`.
        /// </summary>
        [SyncValue]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `5`.
        /// </summary>
        [SyncValue]
        public UInt16 Respawn
        {
            get { return _respawn; }
            set { _respawn = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `shop_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public ShopID? ShopID
        {
            get { return (Nullable<ShopID>)_shopID; }
            set { _shopID = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `statpoints`.
        /// The underlying database type is `int(11)` with the default value of `0`.
        /// </summary>
        [SyncValue]
        public Int32 StatPoints
        {
            get { return _statPoints; }
            set { _statPoints = value; }
        }

        /// <summary>
        /// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
        /// key is the collection's key and the value is the value for that corresponding key.
        /// </summary>
        public IEnumerable<KeyValuePair<StatType, Int32>> Stats
        {
            get { return _stat; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public virtual ICharacterTemplateTable DeepCopy()
        {
            return new CharacterTemplateTable(this);
        }

        /// <summary>
        /// Gets the value of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
        /// </summary>
        /// <param name="key">The key of the column to get.</param>
        /// <returns>
        /// The value of the database column for the corresponding <paramref name="key"/>.
        /// </returns>
        public Int32 GetStat(StatType key)
        {
            return _stat[key];
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