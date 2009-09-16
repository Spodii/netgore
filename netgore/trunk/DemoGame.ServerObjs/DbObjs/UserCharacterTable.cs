using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `user_character`.
    /// </summary>
    public class UserCharacterTable : IUserCharacterTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 37;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "user_character";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        {
            "acc", "account_id", "agi", "armor", "body_id", "bra", "cash", "character_template_id", "chat_dialog", "defence", "dex",
            "evade", "exp", "hp", "id", "imm", "int", "level", "map_id", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "perc",
            "recov", "regen", "respawn_map", "respawn_x", "respawn_y", "statpoints", "str", "tact", "ws", "x", "y"
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
            "acc", "account_id", "agi", "armor", "body_id", "bra", "cash", "character_template_id", "chat_dialog", "defence", "dex",
            "evade", "exp", "hp", "id", "imm", "int", "level", "map_id", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "perc",
            "recov", "regen", "respawn_map", "respawn_x", "respawn_y", "statpoints", "str", "tact", "ws", "x", "y"
        };

        /// <summary>
        /// The field that maps onto the database column `acc`.
        /// </summary>
        Byte _acc;

        /// <summary>
        /// The field that maps onto the database column `account_id`.
        /// </summary>
        int? _accountID;

        /// <summary>
        /// The field that maps onto the database column `agi`.
        /// </summary>
        Byte _agi;

        /// <summary>
        /// The field that maps onto the database column `armor`.
        /// </summary>
        Byte _armor;

        /// <summary>
        /// The field that maps onto the database column `body_id`.
        /// </summary>
        UInt16 _bodyID;

        /// <summary>
        /// The field that maps onto the database column `bra`.
        /// </summary>
        Byte _bra;

        /// <summary>
        /// The field that maps onto the database column `cash`.
        /// </summary>
        UInt32 _cash;

        /// <summary>
        /// The field that maps onto the database column `character_template_id`.
        /// </summary>
        ushort? _characterTemplateID;

        /// <summary>
        /// The field that maps onto the database column `chat_dialog`.
        /// </summary>
        ushort? _chatDialog;

        /// <summary>
        /// The field that maps onto the database column `defence`.
        /// </summary>
        Byte _defence;

        /// <summary>
        /// The field that maps onto the database column `dex`.
        /// </summary>
        Byte _dex;

        /// <summary>
        /// The field that maps onto the database column `evade`.
        /// </summary>
        Byte _evade;

        /// <summary>
        /// The field that maps onto the database column `exp`.
        /// </summary>
        UInt32 _exp;

        /// <summary>
        /// The field that maps onto the database column `hp`.
        /// </summary>
        Int16 _hP;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        Int32 _iD;

        /// <summary>
        /// The field that maps onto the database column `imm`.
        /// </summary>
        Byte _imm;

        /// <summary>
        /// The field that maps onto the database column `int`.
        /// </summary>
        Byte _int;

        /// <summary>
        /// The field that maps onto the database column `level`.
        /// </summary>
        Byte _level;

        /// <summary>
        /// The field that maps onto the database column `map_id`.
        /// </summary>
        UInt16 _mapID;

        /// <summary>
        /// The field that maps onto the database column `maxhit`.
        /// </summary>
        Byte _maxHit;

        /// <summary>
        /// The field that maps onto the database column `maxhp`.
        /// </summary>
        Int16 _maxHP;

        /// <summary>
        /// The field that maps onto the database column `maxmp`.
        /// </summary>
        Int16 _maxMP;

        /// <summary>
        /// The field that maps onto the database column `minhit`.
        /// </summary>
        Byte _minHit;

        /// <summary>
        /// The field that maps onto the database column `mp`.
        /// </summary>
        Int16 _mP;

        /// <summary>
        /// The field that maps onto the database column `name`.
        /// </summary>
        String _name;

        /// <summary>
        /// The field that maps onto the database column `perc`.
        /// </summary>
        Byte _perc;

        /// <summary>
        /// The field that maps onto the database column `recov`.
        /// </summary>
        Byte _recov;

        /// <summary>
        /// The field that maps onto the database column `regen`.
        /// </summary>
        Byte _regen;

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
        /// The field that maps onto the database column `statpoints`.
        /// </summary>
        UInt32 _statPoints;

        /// <summary>
        /// The field that maps onto the database column `str`.
        /// </summary>
        Byte _str;

        /// <summary>
        /// The field that maps onto the database column `tact`.
        /// </summary>
        Byte _tact;

        /// <summary>
        /// The field that maps onto the database column `ws`.
        /// </summary>
        Byte _wS;

        /// <summary>
        /// The field that maps onto the database column `x`.
        /// </summary>
        Single _x;

        /// <summary>
        /// The field that maps onto the database column `y`.
        /// </summary>
        Single _y;

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
        /// UserCharacterTable constructor.
        /// </summary>
        public UserCharacterTable()
        {
        }

        /// <summary>
        /// UserCharacterTable constructor.
        /// </summary>
        /// <param name="acc">The initial value for the corresponding property.</param>
        /// <param name="accountID">The initial value for the corresponding property.</param>
        /// <param name="agi">The initial value for the corresponding property.</param>
        /// <param name="armor">The initial value for the corresponding property.</param>
        /// <param name="bodyID">The initial value for the corresponding property.</param>
        /// <param name="bra">The initial value for the corresponding property.</param>
        /// <param name="cash">The initial value for the corresponding property.</param>
        /// <param name="characterTemplateID">The initial value for the corresponding property.</param>
        /// <param name="chatDialog">The initial value for the corresponding property.</param>
        /// <param name="defence">The initial value for the corresponding property.</param>
        /// <param name="dex">The initial value for the corresponding property.</param>
        /// <param name="evade">The initial value for the corresponding property.</param>
        /// <param name="exp">The initial value for the corresponding property.</param>
        /// <param name="hP">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="imm">The initial value for the corresponding property.</param>
        /// <param name="int">The initial value for the corresponding property.</param>
        /// <param name="level">The initial value for the corresponding property.</param>
        /// <param name="mapID">The initial value for the corresponding property.</param>
        /// <param name="maxHit">The initial value for the corresponding property.</param>
        /// <param name="maxHP">The initial value for the corresponding property.</param>
        /// <param name="maxMP">The initial value for the corresponding property.</param>
        /// <param name="minHit">The initial value for the corresponding property.</param>
        /// <param name="mP">The initial value for the corresponding property.</param>
        /// <param name="name">The initial value for the corresponding property.</param>
        /// <param name="perc">The initial value for the corresponding property.</param>
        /// <param name="recov">The initial value for the corresponding property.</param>
        /// <param name="regen">The initial value for the corresponding property.</param>
        /// <param name="respawnMap">The initial value for the corresponding property.</param>
        /// <param name="respawnX">The initial value for the corresponding property.</param>
        /// <param name="respawnY">The initial value for the corresponding property.</param>
        /// <param name="statPoints">The initial value for the corresponding property.</param>
        /// <param name="str">The initial value for the corresponding property.</param>
        /// <param name="tact">The initial value for the corresponding property.</param>
        /// <param name="wS">The initial value for the corresponding property.</param>
        /// <param name="x">The initial value for the corresponding property.</param>
        /// <param name="y">The initial value for the corresponding property.</param>
        public UserCharacterTable(Byte @acc, AccountID? @accountID, Byte @agi, Byte @armor, BodyIndex @bodyID, Byte @bra,
                                  UInt32 @cash, CharacterTemplateID? @characterTemplateID, ushort? @chatDialog, Byte @defence,
                                  Byte @dex, Byte @evade, UInt32 @exp, SPValueType @hP, Int32 @iD, Byte @imm, Byte @int,
                                  Byte @level, MapIndex @mapID, Byte @maxHit, Int16 @maxHP, Int16 @maxMP, Byte @minHit,
                                  SPValueType @mP, String @name, Byte @perc, Byte @recov, Byte @regen, MapIndex? @respawnMap,
                                  Single @respawnX, Single @respawnY, UInt32 @statPoints, Byte @str, Byte @tact, Byte @wS,
                                  Single @x, Single @y)
        {
            Acc = @acc;
            AccountID = @accountID;
            Agi = @agi;
            Armor = @armor;
            BodyID = @bodyID;
            Bra = @bra;
            Cash = @cash;
            CharacterTemplateID = @characterTemplateID;
            ChatDialog = @chatDialog;
            Defence = @defence;
            Dex = @dex;
            Evade = @evade;
            Exp = @exp;
            HP = @hP;
            ID = @iD;
            Imm = @imm;
            Int = @int;
            Level = @level;
            MapID = @mapID;
            MaxHit = @maxHit;
            MaxHP = @maxHP;
            MaxMP = @maxMP;
            MinHit = @minHit;
            MP = @mP;
            Name = @name;
            Perc = @perc;
            Recov = @recov;
            Regen = @regen;
            RespawnMap = @respawnMap;
            RespawnX = @respawnX;
            RespawnY = @respawnY;
            StatPoints = @statPoints;
            Str = @str;
            Tact = @tact;
            WS = @wS;
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
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(IUserCharacterTable source, IDictionary<String, Object> dic)
        {
            dic["@acc"] = source.Acc;
            dic["@account_id"] = source.AccountID;
            dic["@agi"] = source.Agi;
            dic["@armor"] = source.Armor;
            dic["@body_id"] = source.BodyID;
            dic["@bra"] = source.Bra;
            dic["@cash"] = source.Cash;
            dic["@character_template_id"] = source.CharacterTemplateID;
            dic["@chat_dialog"] = source.ChatDialog;
            dic["@defence"] = source.Defence;
            dic["@dex"] = source.Dex;
            dic["@evade"] = source.Evade;
            dic["@exp"] = source.Exp;
            dic["@hp"] = source.HP;
            dic["@id"] = source.ID;
            dic["@imm"] = source.Imm;
            dic["@int"] = source.Int;
            dic["@level"] = source.Level;
            dic["@map_id"] = source.MapID;
            dic["@maxhit"] = source.MaxHit;
            dic["@maxhp"] = source.MaxHP;
            dic["@maxmp"] = source.MaxMP;
            dic["@minhit"] = source.MinHit;
            dic["@mp"] = source.MP;
            dic["@name"] = source.Name;
            dic["@perc"] = source.Perc;
            dic["@recov"] = source.Recov;
            dic["@regen"] = source.Regen;
            dic["@respawn_map"] = source.RespawnMap;
            dic["@respawn_x"] = source.RespawnX;
            dic["@respawn_y"] = source.RespawnY;
            dic["@statpoints"] = source.StatPoints;
            dic["@str"] = source.Str;
            dic["@tact"] = source.Tact;
            dic["@ws"] = source.WS;
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
            Acc = source.Acc;
            AccountID = source.AccountID;
            Agi = source.Agi;
            Armor = source.Armor;
            BodyID = source.BodyID;
            Bra = source.Bra;
            Cash = source.Cash;
            CharacterTemplateID = source.CharacterTemplateID;
            ChatDialog = source.ChatDialog;
            Defence = source.Defence;
            Dex = source.Dex;
            Evade = source.Evade;
            Exp = source.Exp;
            HP = source.HP;
            ID = source.ID;
            Imm = source.Imm;
            Int = source.Int;
            Level = source.Level;
            MapID = source.MapID;
            MaxHit = source.MaxHit;
            MaxHP = source.MaxHP;
            MaxMP = source.MaxMP;
            MinHit = source.MinHit;
            MP = source.MP;
            Name = source.Name;
            Perc = source.Perc;
            Recov = source.Recov;
            Regen = source.Regen;
            RespawnMap = source.RespawnMap;
            RespawnX = source.RespawnX;
            RespawnY = source.RespawnY;
            StatPoints = source.StatPoints;
            Str = source.Str;
            Tact = source.Tact;
            WS = source.WS;
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
                case "acc":
                    return new ColumnMetadata("acc", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "account_id":
                    return new ColumnMetadata("account_id", "", "int(11)", null, typeof(int?), true, false, false);

                case "agi":
                    return new ColumnMetadata("agi", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "armor":
                    return new ColumnMetadata("armor", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "body_id":
                    return new ColumnMetadata("body_id", "", "smallint(5) unsigned", "1", typeof(UInt16), false, false, false);

                case "bra":
                    return new ColumnMetadata("bra", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "cash":
                    return new ColumnMetadata("cash", "", "int(10) unsigned", "0", typeof(UInt32), false, false, false);

                case "character_template_id":
                    return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(ushort?), true,
                                              false, false);

                case "chat_dialog":
                    return new ColumnMetadata("chat_dialog", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "defence":
                    return new ColumnMetadata("defence", "", "tinyint(3) unsigned", "0", typeof(Byte), false, false, false);

                case "dex":
                    return new ColumnMetadata("dex", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "evade":
                    return new ColumnMetadata("evade", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "exp":
                    return new ColumnMetadata("exp", "", "int(10) unsigned", "0", typeof(UInt32), false, false, false);

                case "hp":
                    return new ColumnMetadata("hp", "", "smallint(6)", "50", typeof(Int16), false, false, false);

                case "id":
                    return new ColumnMetadata("id", "", "int(11)", null, typeof(Int32), false, false, false);

                case "imm":
                    return new ColumnMetadata("imm", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "int":
                    return new ColumnMetadata("int", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "level":
                    return new ColumnMetadata("level", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "map_id":
                    return new ColumnMetadata("map_id", "", "smallint(5) unsigned", "1", typeof(UInt16), false, false, false);

                case "maxhit":
                    return new ColumnMetadata("maxhit", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "maxhp":
                    return new ColumnMetadata("maxhp", "", "smallint(6)", "50", typeof(Int16), false, false, false);

                case "maxmp":
                    return new ColumnMetadata("maxmp", "", "smallint(6)", "50", typeof(Int16), false, false, false);

                case "minhit":
                    return new ColumnMetadata("minhit", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "mp":
                    return new ColumnMetadata("mp", "", "smallint(6)", "50", typeof(Int16), false, false, false);

                case "name":
                    return new ColumnMetadata("name", "", "varchar(30)", null, typeof(String), false, false, false);

                case "perc":
                    return new ColumnMetadata("perc", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "recov":
                    return new ColumnMetadata("recov", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "regen":
                    return new ColumnMetadata("regen", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "respawn_map":
                    return new ColumnMetadata("respawn_map", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "respawn_x":
                    return new ColumnMetadata("respawn_x", "", "float", "50", typeof(Single), false, false, false);

                case "respawn_y":
                    return new ColumnMetadata("respawn_y", "", "float", "50", typeof(Single), false, false, false);

                case "statpoints":
                    return new ColumnMetadata("statpoints", "", "int(10) unsigned", "0", typeof(UInt32), false, false, false);

                case "str":
                    return new ColumnMetadata("str", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "tact":
                    return new ColumnMetadata("tact", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "ws":
                    return new ColumnMetadata("ws", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

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
                case "acc":
                    return Acc;

                case "account_id":
                    return AccountID;

                case "agi":
                    return Agi;

                case "armor":
                    return Armor;

                case "body_id":
                    return BodyID;

                case "bra":
                    return Bra;

                case "cash":
                    return Cash;

                case "character_template_id":
                    return CharacterTemplateID;

                case "chat_dialog":
                    return ChatDialog;

                case "defence":
                    return Defence;

                case "dex":
                    return Dex;

                case "evade":
                    return Evade;

                case "exp":
                    return Exp;

                case "hp":
                    return HP;

                case "id":
                    return ID;

                case "imm":
                    return Imm;

                case "int":
                    return Int;

                case "level":
                    return Level;

                case "map_id":
                    return MapID;

                case "maxhit":
                    return MaxHit;

                case "maxhp":
                    return MaxHP;

                case "maxmp":
                    return MaxMP;

                case "minhit":
                    return MinHit;

                case "mp":
                    return MP;

                case "name":
                    return Name;

                case "perc":
                    return Perc;

                case "recov":
                    return Recov;

                case "regen":
                    return Regen;

                case "respawn_map":
                    return RespawnMap;

                case "respawn_x":
                    return RespawnX;

                case "respawn_y":
                    return RespawnY;

                case "statpoints":
                    return StatPoints;

                case "str":
                    return Str;

                case "tact":
                    return Tact;

                case "ws":
                    return WS;

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
                case "acc":
                    Acc = (Byte)value;
                    break;

                case "account_id":
                    AccountID = (AccountID?)value;
                    break;

                case "agi":
                    Agi = (Byte)value;
                    break;

                case "armor":
                    Armor = (Byte)value;
                    break;

                case "body_id":
                    BodyID = (BodyIndex)value;
                    break;

                case "bra":
                    Bra = (Byte)value;
                    break;

                case "cash":
                    Cash = (UInt32)value;
                    break;

                case "character_template_id":
                    CharacterTemplateID = (CharacterTemplateID?)value;
                    break;

                case "chat_dialog":
                    ChatDialog = (ushort?)value;
                    break;

                case "defence":
                    Defence = (Byte)value;
                    break;

                case "dex":
                    Dex = (Byte)value;
                    break;

                case "evade":
                    Evade = (Byte)value;
                    break;

                case "exp":
                    Exp = (UInt32)value;
                    break;

                case "hp":
                    HP = (SPValueType)value;
                    break;

                case "id":
                    ID = (Int32)value;
                    break;

                case "imm":
                    Imm = (Byte)value;
                    break;

                case "int":
                    Int = (Byte)value;
                    break;

                case "level":
                    Level = (Byte)value;
                    break;

                case "map_id":
                    MapID = (MapIndex)value;
                    break;

                case "maxhit":
                    MaxHit = (Byte)value;
                    break;

                case "maxhp":
                    MaxHP = (Int16)value;
                    break;

                case "maxmp":
                    MaxMP = (Int16)value;
                    break;

                case "minhit":
                    MinHit = (Byte)value;
                    break;

                case "mp":
                    MP = (SPValueType)value;
                    break;

                case "name":
                    Name = (String)value;
                    break;

                case "perc":
                    Perc = (Byte)value;
                    break;

                case "recov":
                    Recov = (Byte)value;
                    break;

                case "regen":
                    Regen = (Byte)value;
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

                case "statpoints":
                    StatPoints = (UInt32)value;
                    break;

                case "str":
                    Str = (Byte)value;
                    break;

                case "tact":
                    Tact = (Byte)value;
                    break;

                case "ws":
                    WS = (Byte)value;
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

        #region IUserCharacterTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `acc`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Acc
        {
            get { return _acc; }
            set { _acc = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `account_id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        public AccountID? AccountID
        {
            get { return (AccountID?)_accountID; }
            set { _accountID = (int?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `agi`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Agi
        {
            get { return _agi; }
            set { _agi = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `armor`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Armor
        {
            get { return _armor; }
            set { _armor = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `body_id`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
        /// </summary>
        public BodyIndex BodyID
        {
            get { return (BodyIndex)_bodyID; }
            set { _bodyID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `bra`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Bra
        {
            get { return _bra; }
            set { _bra = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `cash`.
        /// The underlying database type is `int(10) unsigned` with the default value of `0`.
        /// </summary>
        public UInt32 Cash
        {
            get { return _cash; }
            set { _cash = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_template_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public CharacterTemplateID? CharacterTemplateID
        {
            get { return (CharacterTemplateID?)_characterTemplateID; }
            set { _characterTemplateID = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `chat_dialog`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public ushort? ChatDialog
        {
            get { return _chatDialog; }
            set { _chatDialog = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `defence`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
        /// </summary>
        public Byte Defence
        {
            get { return _defence; }
            set { _defence = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `dex`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Dex
        {
            get { return _dex; }
            set { _dex = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `evade`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Evade
        {
            get { return _evade; }
            set { _evade = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `exp`.
        /// The underlying database type is `int(10) unsigned` with the default value of `0`.
        /// </summary>
        public UInt32 Exp
        {
            get { return _exp; }
            set { _exp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `hp`.
        /// The underlying database type is `smallint(6)` with the default value of `50`.
        /// </summary>
        public SPValueType HP
        {
            get { return _hP; }
            set { _hP = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(11)`.
        /// </summary>
        public Int32 ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `imm`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Imm
        {
            get { return _imm; }
            set { _imm = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `int`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Int
        {
            get { return _int; }
            set { _int = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `level`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Level
        {
            get { return _level; }
            set { _level = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `map_id`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
        /// </summary>
        public MapIndex MapID
        {
            get { return (MapIndex)_mapID; }
            set { _mapID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `maxhit`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte MaxHit
        {
            get { return _maxHit; }
            set { _maxHit = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `maxhp`.
        /// The underlying database type is `smallint(6)` with the default value of `50`.
        /// </summary>
        public Int16 MaxHP
        {
            get { return _maxHP; }
            set { _maxHP = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `maxmp`.
        /// The underlying database type is `smallint(6)` with the default value of `50`.
        /// </summary>
        public Int16 MaxMP
        {
            get { return _maxMP; }
            set { _maxMP = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `minhit`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte MinHit
        {
            get { return _minHit; }
            set { _minHit = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `mp`.
        /// The underlying database type is `smallint(6)` with the default value of `50`.
        /// </summary>
        public SPValueType MP
        {
            get { return _mP; }
            set { _mP = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `name`.
        /// The underlying database type is `varchar(30)`.
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `perc`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Perc
        {
            get { return _perc; }
            set { _perc = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `recov`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Recov
        {
            get { return _recov; }
            set { _recov = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `regen`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Regen
        {
            get { return _regen; }
            set { _regen = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_map`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public MapIndex? RespawnMap
        {
            get { return (MapIndex?)_respawnMap; }
            set { _respawnMap = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_x`.
        /// The underlying database type is `float` with the default value of `50`.
        /// </summary>
        public Single RespawnX
        {
            get { return _respawnX; }
            set { _respawnX = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_y`.
        /// The underlying database type is `float` with the default value of `50`.
        /// </summary>
        public Single RespawnY
        {
            get { return _respawnY; }
            set { _respawnY = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `statpoints`.
        /// The underlying database type is `int(10) unsigned` with the default value of `0`.
        /// </summary>
        public UInt32 StatPoints
        {
            get { return _statPoints; }
            set { _statPoints = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `str`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Str
        {
            get { return _str; }
            set { _str = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `tact`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Tact
        {
            get { return _tact; }
            set { _tact = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `ws`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte WS
        {
            get { return _wS; }
            set { _wS = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `x`.
        /// The underlying database type is `float` with the default value of `100`.
        /// </summary>
        public Single X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `y`.
        /// The underlying database type is `float` with the default value of `100`.
        /// </summary>
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