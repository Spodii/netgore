using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.AI;
using NetGore.Features.Shops;
using NetGore.IO;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `user_character`.
    /// </summary>
    public class UserCharacterTable : IUserCharacterTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 28;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "user_character";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        {
            "account_id", "ai_id", "body_id", "cash", "character_template_id", "chat_dialog", "exp", "hp", "id", "level", "map_id",
            "mp", "name", "respawn_map", "respawn_x", "respawn_y", "shop_id", "statpoints", "stat_agi", "stat_defence", "stat_int",
            "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str", "x", "y"
        };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        {
            "account_id", "ai_id", "body_id", "cash", "character_template_id", "chat_dialog", "exp", "hp", "id", "level", "map_id",
            "mp", "name", "respawn_map", "respawn_x", "respawn_y", "shop_id", "statpoints", "stat_agi", "stat_defence", "stat_int",
            "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str", "x", "y"
        };

        /// <summary>
        /// The field that maps onto the database column `account_id`.
        /// </summary>
        int? _accountID;

        /// <summary>
        /// The field that maps onto the database column `ai_id`.
        /// </summary>
        ushort? _aIID;

        /// <summary>
        /// The field that maps onto the database column `body_id`.
        /// </summary>
        UInt16 _bodyID;

        /// <summary>
        /// The field that maps onto the database column `cash`.
        /// </summary>
        Int32 _cash;

        /// <summary>
        /// The field that maps onto the database column `character_template_id`.
        /// </summary>
        ushort? _characterTemplateID;

        /// <summary>
        /// The field that maps onto the database column `chat_dialog`.
        /// </summary>
        ushort? _chatDialog;

        /// <summary>
        /// The field that maps onto the database column `exp`.
        /// </summary>
        Int32 _exp;

        /// <summary>
        /// The field that maps onto the database column `hp`.
        /// </summary>
        Int16 _hP;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Int32 _iD;

        /// <summary>
        /// The field that maps onto the database column `level`.
        /// </summary>
        Byte _level;

        /// <summary>
        /// The field that maps onto the database column `map_id`.
        /// </summary>
        UInt16 _mapID;

        /// <summary>
        /// The field that maps onto the database column `mp`.
        /// </summary>
        Int16 _mP;

        /// <summary>
        /// The field that maps onto the database column `name`.
        /// </summary>
        String _name;

        /// <summary>
        /// The field that maps onto the database column `respawn_map`.
        /// </summary>
        ushort? _respawnMap;

        /// <summary>
        /// The field that maps onto the database column `respawn_x`.
        /// </summary>
        Single _respawnX;

        /// <summary>
        /// The field that maps onto the database column `respawn_y`.
        /// </summary>
        Single _respawnY;

        /// <summary>
        /// The field that maps onto the database column `shop_id`.
        /// </summary>
        ushort? _shopID;

        /// <summary>
        /// The field that maps onto the database column `stat_agi`.
        /// </summary>
        Int16 _statAgi;

        /// <summary>
        /// The field that maps onto the database column `stat_defence`.
        /// </summary>
        Int16 _statDefence;

        /// <summary>
        /// The field that maps onto the database column `stat_int`.
        /// </summary>
        Int16 _statInt;

        /// <summary>
        /// The field that maps onto the database column `stat_maxhit`.
        /// </summary>
        Int16 _statMaxhit;

        /// <summary>
        /// The field that maps onto the database column `stat_maxhp`.
        /// </summary>
        Int16 _statMaxhp;

        /// <summary>
        /// The field that maps onto the database column `stat_maxmp`.
        /// </summary>
        Int16 _statMaxmp;

        /// <summary>
        /// The field that maps onto the database column `stat_minhit`.
        /// </summary>
        Int16 _statMinhit;

        /// <summary>
        /// The field that maps onto the database column `statpoints`.
        /// </summary>
        Int32 _statPoints;

        /// <summary>
        /// The field that maps onto the database column `stat_str`.
        /// </summary>
        Int16 _statStr;

        /// <summary>
        /// The field that maps onto the database column `x`.
        /// </summary>
        Single _x;

        /// <summary>
        /// The field that maps onto the database column `y`.
        /// </summary>
        Single _y;

        /// <summary>
        /// UserCharacterTable constructor.
        /// </summary>
        public UserCharacterTable()
        {
        }

        /// <summary>
        /// UserCharacterTable constructor.
        /// </summary>
        /// <param name="accountID">The initial value for the corresponding property.</param>
        /// <param name="aIID">The initial value for the corresponding property.</param>
        /// <param name="bodyID">The initial value for the corresponding property.</param>
        /// <param name="cash">The initial value for the corresponding property.</param>
        /// <param name="characterTemplateID">The initial value for the corresponding property.</param>
        /// <param name="chatDialog">The initial value for the corresponding property.</param>
        /// <param name="exp">The initial value for the corresponding property.</param>
        /// <param name="hP">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="level">The initial value for the corresponding property.</param>
        /// <param name="mapID">The initial value for the corresponding property.</param>
        /// <param name="mP">The initial value for the corresponding property.</param>
        /// <param name="name">The initial value for the corresponding property.</param>
        /// <param name="respawnMap">The initial value for the corresponding property.</param>
        /// <param name="respawnX">The initial value for the corresponding property.</param>
        /// <param name="respawnY">The initial value for the corresponding property.</param>
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
        /// <param name="x">The initial value for the corresponding property.</param>
        /// <param name="y">The initial value for the corresponding property.</param>
        public UserCharacterTable(AccountID? @accountID, AIID? @aIID, BodyIndex @bodyID, Int32 @cash,
                                  CharacterTemplateID? @characterTemplateID, ushort? @chatDialog, Int32 @exp, SPValueType @hP,
                                  Int32 @iD, Byte @level, MapIndex @mapID, SPValueType @mP, String @name, MapIndex? @respawnMap,
                                  Single @respawnX, Single @respawnY, ShopID? @shopID, Int32 @statPoints, Int16 @statAgi,
                                  Int16 @statDefence, Int16 @statInt, Int16 @statMaxhit, Int16 @statMaxhp, Int16 @statMaxmp,
                                  Int16 @statMinhit, Int16 @statStr, Single @x, Single @y)
        {
            AccountID = @accountID;
            AIID = @aIID;
            BodyID = @bodyID;
            Cash = @cash;
            CharacterTemplateID = @characterTemplateID;
            ChatDialog = @chatDialog;
            Exp = @exp;
            HP = @hP;
            ID = @iD;
            Level = @level;
            MapID = @mapID;
            MP = @mP;
            Name = @name;
            RespawnMap = @respawnMap;
            RespawnX = @respawnX;
            RespawnY = @respawnY;
            ShopID = @shopID;
            StatPoints = @statPoints;
            StatAgi = @statAgi;
            StatDefence = @statDefence;
            StatInt = @statInt;
            StatMaxhit = @statMaxhit;
            StatMaxhp = @statMaxhp;
            StatMaxmp = @statMaxmp;
            StatMinhit = @statMinhit;
            StatStr = @statStr;
            X = @x;
            Y = @y;
        }

        /// <summary>
        /// UserCharacterTable constructor.
        /// </summary>
        /// <param name="source">IUserCharacterTable to copy the initial values from.</param>
        public UserCharacterTable(IUserCharacterTable source)
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
        public static void CopyValues(IUserCharacterTable source, IDictionary<String, Object> dic)
        {
            dic["@account_id"] = source.AccountID;
            dic["@ai_id"] = source.AIID;
            dic["@body_id"] = source.BodyID;
            dic["@cash"] = source.Cash;
            dic["@character_template_id"] = source.CharacterTemplateID;
            dic["@chat_dialog"] = source.ChatDialog;
            dic["@exp"] = source.Exp;
            dic["@hp"] = source.HP;
            dic["@id"] = source.ID;
            dic["@level"] = source.Level;
            dic["@map_id"] = source.MapID;
            dic["@mp"] = source.MP;
            dic["@name"] = source.Name;
            dic["@respawn_map"] = source.RespawnMap;
            dic["@respawn_x"] = source.RespawnX;
            dic["@respawn_y"] = source.RespawnY;
            dic["@shop_id"] = source.ShopID;
            dic["@statpoints"] = source.StatPoints;
            dic["@stat_agi"] = source.StatAgi;
            dic["@stat_defence"] = source.StatDefence;
            dic["@stat_int"] = source.StatInt;
            dic["@stat_maxhit"] = source.StatMaxhit;
            dic["@stat_maxhp"] = source.StatMaxhp;
            dic["@stat_maxmp"] = source.StatMaxmp;
            dic["@stat_minhit"] = source.StatMinhit;
            dic["@stat_str"] = source.StatStr;
            dic["@x"] = source.X;
            dic["@y"] = source.Y;
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
        /// Copies the values from the given <paramref name="source"/> into this UserCharacterTable.
        /// </summary>
        /// <param name="source">The IUserCharacterTable to copy the values from.</param>
        public void CopyValuesFrom(IUserCharacterTable source)
        {
            AccountID = source.AccountID;
            AIID = source.AIID;
            BodyID = source.BodyID;
            Cash = source.Cash;
            CharacterTemplateID = source.CharacterTemplateID;
            ChatDialog = source.ChatDialog;
            Exp = source.Exp;
            HP = source.HP;
            ID = source.ID;
            Level = source.Level;
            MapID = source.MapID;
            MP = source.MP;
            Name = source.Name;
            RespawnMap = source.RespawnMap;
            RespawnX = source.RespawnX;
            RespawnY = source.RespawnY;
            ShopID = source.ShopID;
            StatPoints = source.StatPoints;
            StatAgi = source.StatAgi;
            StatDefence = source.StatDefence;
            StatInt = source.StatInt;
            StatMaxhit = source.StatMaxhit;
            StatMaxhp = source.StatMaxhp;
            StatMaxmp = source.StatMaxmp;
            StatMinhit = source.StatMinhit;
            StatStr = source.StatStr;
            X = source.X;
            Y = source.Y;
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
                    return new ColumnMetadata("account_id", "", "int(11)", null, typeof(int?), true, false, false);

                case "ai_id":
                    return new ColumnMetadata("ai_id", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "body_id":
                    return new ColumnMetadata("body_id", "", "smallint(5) unsigned", "1", typeof(UInt16), false, false, false);

                case "cash":
                    return new ColumnMetadata("cash", "", "int(11)", "0", typeof(Int32), false, false, false);

                case "character_template_id":
                    return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(ushort?), true,
                                              false, false);

                case "chat_dialog":
                    return new ColumnMetadata("chat_dialog", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "exp":
                    return new ColumnMetadata("exp", "", "int(11)", "0", typeof(Int32), false, false, false);

                case "hp":
                    return new ColumnMetadata("hp", "", "smallint(6)", "50", typeof(Int16), false, false, false);

                case "id":
                    return new ColumnMetadata("id", "", "int(11)", null, typeof(Int32), false, false, false);

                case "level":
                    return new ColumnMetadata("level", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "map_id":
                    return new ColumnMetadata("map_id", "", "smallint(5) unsigned", "1", typeof(UInt16), false, false, false);

                case "mp":
                    return new ColumnMetadata("mp", "", "smallint(6)", "50", typeof(Int16), false, false, false);

                case "name":
                    return new ColumnMetadata("name", "", "varchar(30)", null, typeof(String), false, false, false);

                case "respawn_map":
                    return new ColumnMetadata("respawn_map", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "respawn_x":
                    return new ColumnMetadata("respawn_x", "", "float", "50", typeof(Single), false, false, false);

                case "respawn_y":
                    return new ColumnMetadata("respawn_y", "", "float", "50", typeof(Single), false, false, false);

                case "shop_id":
                    return new ColumnMetadata("shop_id", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

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

                case "x":
                    return new ColumnMetadata("x", "", "float", "100", typeof(Single), false, false, false);

                case "y":
                    return new ColumnMetadata("y", "", "float", "100", typeof(Single), false, false, false);

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

                case "ai_id":
                    return AIID;

                case "body_id":
                    return BodyID;

                case "cash":
                    return Cash;

                case "character_template_id":
                    return CharacterTemplateID;

                case "chat_dialog":
                    return ChatDialog;

                case "exp":
                    return Exp;

                case "hp":
                    return HP;

                case "id":
                    return ID;

                case "level":
                    return Level;

                case "map_id":
                    return MapID;

                case "mp":
                    return MP;

                case "name":
                    return Name;

                case "respawn_map":
                    return RespawnMap;

                case "respawn_x":
                    return RespawnX;

                case "respawn_y":
                    return RespawnY;

                case "shop_id":
                    return ShopID;

                case "statpoints":
                    return StatPoints;

                case "stat_agi":
                    return StatAgi;

                case "stat_defence":
                    return StatDefence;

                case "stat_int":
                    return StatInt;

                case "stat_maxhit":
                    return StatMaxhit;

                case "stat_maxhp":
                    return StatMaxhp;

                case "stat_maxmp":
                    return StatMaxmp;

                case "stat_minhit":
                    return StatMinhit;

                case "stat_str":
                    return StatStr;

                case "x":
                    return X;

                case "y":
                    return Y;

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
                    AccountID = (AccountID?)value;
                    break;

                case "ai_id":
                    AIID = (AIID?)value;
                    break;

                case "body_id":
                    BodyID = (BodyIndex)value;
                    break;

                case "cash":
                    Cash = (Int32)value;
                    break;

                case "character_template_id":
                    CharacterTemplateID = (CharacterTemplateID?)value;
                    break;

                case "chat_dialog":
                    ChatDialog = (ushort?)value;
                    break;

                case "exp":
                    Exp = (Int32)value;
                    break;

                case "hp":
                    HP = (SPValueType)value;
                    break;

                case "id":
                    ID = (Int32)value;
                    break;

                case "level":
                    Level = (Byte)value;
                    break;

                case "map_id":
                    MapID = (MapIndex)value;
                    break;

                case "mp":
                    MP = (SPValueType)value;
                    break;

                case "name":
                    Name = (String)value;
                    break;

                case "respawn_map":
                    RespawnMap = (MapIndex?)value;
                    break;

                case "respawn_x":
                    RespawnX = (Single)value;
                    break;

                case "respawn_y":
                    RespawnY = (Single)value;
                    break;

                case "shop_id":
                    ShopID = (ShopID?)value;
                    break;

                case "statpoints":
                    StatPoints = (Int32)value;
                    break;

                case "stat_agi":
                    StatAgi = (Int16)value;
                    break;

                case "stat_defence":
                    StatDefence = (Int16)value;
                    break;

                case "stat_int":
                    StatInt = (Int16)value;
                    break;

                case "stat_maxhit":
                    StatMaxhit = (Int16)value;
                    break;

                case "stat_maxhp":
                    StatMaxhp = (Int16)value;
                    break;

                case "stat_maxmp":
                    StatMaxmp = (Int16)value;
                    break;

                case "stat_minhit":
                    StatMinhit = (Int16)value;
                    break;

                case "stat_str":
                    StatStr = (Int16)value;
                    break;

                case "x":
                    X = (Single)value;
                    break;

                case "y":
                    Y = (Single)value;
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

        #region IUserCharacterTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `account_id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        [SyncValue]
        public AccountID? AccountID
        {
            get { return (AccountID?)_accountID; }
            set { _accountID = (int?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `ai_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public AIID? AIID
        {
            get { return (AIID?)_aIID; }
            set { _aIID = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `body_id`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
        /// </summary>
        [SyncValue]
        public BodyIndex BodyID
        {
            get { return (BodyIndex)_bodyID; }
            set { _bodyID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `cash`.
        /// The underlying database type is `int(11)` with the default value of `0`.
        /// </summary>
        [SyncValue]
        public Int32 Cash
        {
            get { return _cash; }
            set { _cash = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_template_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public CharacterTemplateID? CharacterTemplateID
        {
            get { return (CharacterTemplateID?)_characterTemplateID; }
            set { _characterTemplateID = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `chat_dialog`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public ushort? ChatDialog
        {
            get { return _chatDialog; }
            set { _chatDialog = value; }
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
        /// Gets or sets the value for the field that maps onto the database column `hp`.
        /// The underlying database type is `smallint(6)` with the default value of `50`.
        /// </summary>
        [SyncValue]
        public SPValueType HP
        {
            get { return _hP; }
            set { _hP = value; }
        }

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
        /// Gets or sets the value for the field that maps onto the database column `map_id`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
        /// </summary>
        [SyncValue]
        public MapIndex MapID
        {
            get { return (MapIndex)_mapID; }
            set { _mapID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `mp`.
        /// The underlying database type is `smallint(6)` with the default value of `50`.
        /// </summary>
        [SyncValue]
        public SPValueType MP
        {
            get { return _mP; }
            set { _mP = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `name`.
        /// The underlying database type is `varchar(30)`.
        /// </summary>
        [SyncValue]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_map`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public MapIndex? RespawnMap
        {
            get { return (MapIndex?)_respawnMap; }
            set { _respawnMap = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_x`.
        /// The underlying database type is `float` with the default value of `50`.
        /// </summary>
        [SyncValue]
        public Single RespawnX
        {
            get { return _respawnX; }
            set { _respawnX = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_y`.
        /// The underlying database type is `float` with the default value of `50`.
        /// </summary>
        [SyncValue]
        public Single RespawnY
        {
            get { return _respawnY; }
            set { _respawnY = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `shop_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        [SyncValue]
        public ShopID? ShopID
        {
            get { return (ShopID?)_shopID; }
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
        /// Gets or sets the value for the field that maps onto the database column `stat_agi`.
        /// The underlying database type is `smallint(6)` with the default value of `1`.
        /// </summary>
        [SyncValue]
        public Int16 StatAgi
        {
            get { return _statAgi; }
            set { _statAgi = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `stat_defence`.
        /// The underlying database type is `smallint(6)` with the default value of `1`.
        /// </summary>
        [SyncValue]
        public Int16 StatDefence
        {
            get { return _statDefence; }
            set { _statDefence = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `stat_int`.
        /// The underlying database type is `smallint(6)` with the default value of `1`.
        /// </summary>
        [SyncValue]
        public Int16 StatInt
        {
            get { return _statInt; }
            set { _statInt = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `stat_maxhit`.
        /// The underlying database type is `smallint(6)` with the default value of `1`.
        /// </summary>
        [SyncValue]
        public Int16 StatMaxhit
        {
            get { return _statMaxhit; }
            set { _statMaxhit = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `stat_maxhp`.
        /// The underlying database type is `smallint(6)` with the default value of `50`.
        /// </summary>
        [SyncValue]
        public Int16 StatMaxhp
        {
            get { return _statMaxhp; }
            set { _statMaxhp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `stat_maxmp`.
        /// The underlying database type is `smallint(6)` with the default value of `50`.
        /// </summary>
        [SyncValue]
        public Int16 StatMaxmp
        {
            get { return _statMaxmp; }
            set { _statMaxmp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `stat_minhit`.
        /// The underlying database type is `smallint(6)` with the default value of `1`.
        /// </summary>
        [SyncValue]
        public Int16 StatMinhit
        {
            get { return _statMinhit; }
            set { _statMinhit = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `stat_str`.
        /// The underlying database type is `smallint(6)` with the default value of `1`.
        /// </summary>
        [SyncValue]
        public Int16 StatStr
        {
            get { return _statStr; }
            set { _statStr = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `x`.
        /// The underlying database type is `float` with the default value of `100`.
        /// </summary>
        [SyncValue]
        public Single X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `y`.
        /// The underlying database type is `float` with the default value of `100`.
        /// </summary>
        [SyncValue]
        public Single Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Creates a deep copy of this table. All the values will be the same
        /// but they will be contained in a different object instance.
        /// </summary>
        /// <returns>
        /// A deep copy of this table.
        /// </returns>
        public IUserCharacterTable DeepCopy()
        {
            return new UserCharacterTable(this);
        }

        #endregion
    }
}