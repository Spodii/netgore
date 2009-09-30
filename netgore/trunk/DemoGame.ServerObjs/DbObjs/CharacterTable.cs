using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DemoGame;
using DemoGame.DbObjs;
using NetGore;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `character`.
    /// </summary>
    public class CharacterTable : ICharacterTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 38;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "character";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        {
            "acc", "account_id", "agi", "armor", "body_id", "bra", "cash", "character_template_id", "chat_dialog", "defence", "dex",
            "evade", "exp", "hp", "id", "imm", "int", "level", "map_id", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "perc",
            "recov", "regen", "respawn_map", "respawn_x", "respawn_y", "shop_id", "statpoints", "str", "tact", "ws", "x", "y"
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
            "acc", "account_id", "agi", "armor", "body_id", "bra", "cash", "character_template_id", "chat_dialog", "defence", "dex",
            "evade", "exp", "hp", "imm", "int", "level", "map_id", "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "perc",
            "recov", "regen", "respawn_map", "respawn_x", "respawn_y", "shop_id", "statpoints", "str", "tact", "ws", "x", "y"
        };

        /// <summary>
        /// The fields that are used in the column collection `Stat`.
        /// </summary>
        static readonly String[] _statColumns = new string[]
        {
            "acc", "agi", "armor", "bra", "defence", "dex", "evade", "imm", "int", "maxhit", "maxhp", "maxmp", "minhit", "perc",
            "recov", "regen", "str", "tact", "ws"
        };

        /// <summary>
        /// Dictionary containing the values for the column collection `Stat`.
        /// </summary>
        readonly StatConstDictionary _stat = new StatConstDictionary();

        /// <summary>
        /// The field that maps onto the database column `account_id`.
        /// </summary>
        int? _accountID;

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
        /// The field that maps onto the database column `statpoints`.
        /// </summary>
        Int32 _statPoints;

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
        /// Gets an IEnumerable of strings containing the name of the database
        /// columns used in the column collection `Stat`.
        /// </summary>
        public static IEnumerable<String> StatColumns
        {
            get { return _statColumns; }
        }

        /// <summary>
        /// CharacterTable constructor.
        /// </summary>
        public CharacterTable()
        {
        }

        /// <summary>
        /// CharacterTable constructor.
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
        /// <param name="shopID">The initial value for the corresponding property.</param>
        /// <param name="statPoints">The initial value for the corresponding property.</param>
        /// <param name="str">The initial value for the corresponding property.</param>
        /// <param name="tact">The initial value for the corresponding property.</param>
        /// <param name="wS">The initial value for the corresponding property.</param>
        /// <param name="x">The initial value for the corresponding property.</param>
        /// <param name="y">The initial value for the corresponding property.</param>
        public CharacterTable(Byte @acc, AccountID? @accountID, Byte @agi, Byte @armor, BodyIndex @bodyID, Byte @bra, Int32 @cash,
                              CharacterTemplateID? @characterTemplateID, ushort? @chatDialog, Byte @defence, Byte @dex,
                              Byte @evade, Int32 @exp, SPValueType @hP, CharacterID @iD, Byte @imm, Byte @int, Byte @level,
                              MapIndex @mapID, Byte @maxHit, Int16 @maxHP, Int16 @maxMP, Byte @minHit, SPValueType @mP,
                              String @name, Byte @perc, Byte @recov, Byte @regen, MapIndex? @respawnMap, Single @respawnX,
                              Single @respawnY, ShopID? @shopID, Int32 @statPoints, Byte @str, Byte @tact, Byte @wS, Single @x,
                              Single @y)
        {
            SetStat(StatType.Acc, @acc);
            AccountID = @accountID;
            SetStat(StatType.Agi, @agi);
            SetStat(StatType.Armor, @armor);
            BodyID = @bodyID;
            SetStat(StatType.Bra, @bra);
            Cash = @cash;
            CharacterTemplateID = @characterTemplateID;
            ChatDialog = @chatDialog;
            SetStat(StatType.Defence, @defence);
            SetStat(StatType.Dex, @dex);
            SetStat(StatType.Evade, @evade);
            Exp = @exp;
            HP = @hP;
            ID = @iD;
            SetStat(StatType.Imm, @imm);
            SetStat(StatType.Int, @int);
            Level = @level;
            MapID = @mapID;
            SetStat(StatType.MaxHit, @maxHit);
            SetStat(StatType.MaxHP, @maxHP);
            SetStat(StatType.MaxMP, @maxMP);
            SetStat(StatType.MinHit, @minHit);
            MP = @mP;
            Name = @name;
            SetStat(StatType.Perc, @perc);
            SetStat(StatType.Recov, @recov);
            SetStat(StatType.Regen, @regen);
            RespawnMap = @respawnMap;
            RespawnX = @respawnX;
            RespawnY = @respawnY;
            ShopID = @shopID;
            StatPoints = @statPoints;
            SetStat(StatType.Str, @str);
            SetStat(StatType.Tact, @tact);
            SetStat(StatType.WS, @wS);
            X = @x;
            Y = @y;
        }

        /// <summary>
        /// CharacterTable constructor.
        /// </summary>
        /// <param name="source">ICharacterTable to copy the initial values from.</param>
        public CharacterTable(ICharacterTable source)
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
        public static void CopyValues(ICharacterTable source, IDictionary<String, Object> dic)
        {
            dic["@acc"] = (Byte)source.GetStat(StatType.Acc);
            dic["@account_id"] = source.AccountID;
            dic["@agi"] = (Byte)source.GetStat(StatType.Agi);
            dic["@armor"] = (Byte)source.GetStat(StatType.Armor);
            dic["@body_id"] = source.BodyID;
            dic["@bra"] = (Byte)source.GetStat(StatType.Bra);
            dic["@cash"] = source.Cash;
            dic["@character_template_id"] = source.CharacterTemplateID;
            dic["@chat_dialog"] = source.ChatDialog;
            dic["@defence"] = (Byte)source.GetStat(StatType.Defence);
            dic["@dex"] = (Byte)source.GetStat(StatType.Dex);
            dic["@evade"] = (Byte)source.GetStat(StatType.Evade);
            dic["@exp"] = source.Exp;
            dic["@hp"] = source.HP;
            dic["@id"] = source.ID;
            dic["@imm"] = (Byte)source.GetStat(StatType.Imm);
            dic["@int"] = (Byte)source.GetStat(StatType.Int);
            dic["@level"] = source.Level;
            dic["@map_id"] = source.MapID;
            dic["@maxhit"] = (Byte)source.GetStat(StatType.MaxHit);
            dic["@maxhp"] = (Int16)source.GetStat(StatType.MaxHP);
            dic["@maxmp"] = (Int16)source.GetStat(StatType.MaxMP);
            dic["@minhit"] = (Byte)source.GetStat(StatType.MinHit);
            dic["@mp"] = source.MP;
            dic["@name"] = source.Name;
            dic["@perc"] = (Byte)source.GetStat(StatType.Perc);
            dic["@recov"] = (Byte)source.GetStat(StatType.Recov);
            dic["@regen"] = (Byte)source.GetStat(StatType.Regen);
            dic["@respawn_map"] = source.RespawnMap;
            dic["@respawn_x"] = source.RespawnX;
            dic["@respawn_y"] = source.RespawnY;
            dic["@shop_id"] = source.ShopID;
            dic["@statpoints"] = source.StatPoints;
            dic["@str"] = (Byte)source.GetStat(StatType.Str);
            dic["@tact"] = (Byte)source.GetStat(StatType.Tact);
            dic["@ws"] = (Byte)source.GetStat(StatType.WS);
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
        /// Copies the values from the given <paramref name="source"/> into this CharacterTable.
        /// </summary>
        /// <param name="source">The ICharacterTable to copy the values from.</param>
        public void CopyValuesFrom(ICharacterTable source)
        {
            SetStat(StatType.Acc, source.GetStat(StatType.Acc));
            AccountID = source.AccountID;
            SetStat(StatType.Agi, source.GetStat(StatType.Agi));
            SetStat(StatType.Armor, source.GetStat(StatType.Armor));
            BodyID = source.BodyID;
            SetStat(StatType.Bra, source.GetStat(StatType.Bra));
            Cash = source.Cash;
            CharacterTemplateID = source.CharacterTemplateID;
            ChatDialog = source.ChatDialog;
            SetStat(StatType.Defence, source.GetStat(StatType.Defence));
            SetStat(StatType.Dex, source.GetStat(StatType.Dex));
            SetStat(StatType.Evade, source.GetStat(StatType.Evade));
            Exp = source.Exp;
            HP = source.HP;
            ID = source.ID;
            SetStat(StatType.Imm, source.GetStat(StatType.Imm));
            SetStat(StatType.Int, source.GetStat(StatType.Int));
            Level = source.Level;
            MapID = source.MapID;
            SetStat(StatType.MaxHit, source.GetStat(StatType.MaxHit));
            SetStat(StatType.MaxHP, source.GetStat(StatType.MaxHP));
            SetStat(StatType.MaxMP, source.GetStat(StatType.MaxMP));
            SetStat(StatType.MinHit, source.GetStat(StatType.MinHit));
            MP = source.MP;
            Name = source.Name;
            SetStat(StatType.Perc, source.GetStat(StatType.Perc));
            SetStat(StatType.Recov, source.GetStat(StatType.Recov));
            SetStat(StatType.Regen, source.GetStat(StatType.Regen));
            RespawnMap = source.RespawnMap;
            RespawnX = source.RespawnX;
            RespawnY = source.RespawnY;
            ShopID = source.ShopID;
            StatPoints = source.StatPoints;
            SetStat(StatType.Str, source.GetStat(StatType.Str));
            SetStat(StatType.Tact, source.GetStat(StatType.Tact));
            SetStat(StatType.WS, source.GetStat(StatType.WS));
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
                    return new ColumnMetadata("account_id", "", "int(11)", null, typeof(int?), true, false, true);

                case "agi":
                    return new ColumnMetadata("agi", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "armor":
                    return new ColumnMetadata("armor", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "body_id":
                    return new ColumnMetadata("body_id", "", "smallint(5) unsigned", "1", typeof(UInt16), false, false, false);

                case "bra":
                    return new ColumnMetadata("bra", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "cash":
                    return new ColumnMetadata("cash", "", "int(11)", "0", typeof(Int32), false, false, false);

                case "character_template_id":
                    return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(ushort?), true,
                                              false, true);

                case "chat_dialog":
                    return new ColumnMetadata("chat_dialog", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, false);

                case "defence":
                    return new ColumnMetadata("defence", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "dex":
                    return new ColumnMetadata("dex", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "evade":
                    return new ColumnMetadata("evade", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "exp":
                    return new ColumnMetadata("exp", "", "int(11)", "0", typeof(Int32), false, false, false);

                case "hp":
                    return new ColumnMetadata("hp", "", "smallint(6)", "50", typeof(Int16), false, false, false);

                case "id":
                    return new ColumnMetadata("id", "", "int(11)", null, typeof(Int32), false, true, false);

                case "imm":
                    return new ColumnMetadata("imm", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "int":
                    return new ColumnMetadata("int", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "level":
                    return new ColumnMetadata("level", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "map_id":
                    return new ColumnMetadata("map_id", "", "smallint(5) unsigned", "1", typeof(UInt16), false, false, true);

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
                    return new ColumnMetadata("name", "", "varchar(30)", null, typeof(String), false, false, true);

                case "perc":
                    return new ColumnMetadata("perc", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "recov":
                    return new ColumnMetadata("recov", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "regen":
                    return new ColumnMetadata("regen", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "respawn_map":
                    return new ColumnMetadata("respawn_map", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, true);

                case "respawn_x":
                    return new ColumnMetadata("respawn_x", "", "float", "50", typeof(Single), false, false, false);

                case "respawn_y":
                    return new ColumnMetadata("respawn_y", "", "float", "50", typeof(Single), false, false, false);

                case "shop_id":
                    return new ColumnMetadata("shop_id", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, true);

                case "statpoints":
                    return new ColumnMetadata("statpoints", "", "int(11)", "0", typeof(Int32), false, false, false);

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
                    return GetStat(StatType.Acc);

                case "account_id":
                    return AccountID;

                case "agi":
                    return GetStat(StatType.Agi);

                case "armor":
                    return GetStat(StatType.Armor);

                case "body_id":
                    return BodyID;

                case "bra":
                    return GetStat(StatType.Bra);

                case "cash":
                    return Cash;

                case "character_template_id":
                    return CharacterTemplateID;

                case "chat_dialog":
                    return ChatDialog;

                case "defence":
                    return GetStat(StatType.Defence);

                case "dex":
                    return GetStat(StatType.Dex);

                case "evade":
                    return GetStat(StatType.Evade);

                case "exp":
                    return Exp;

                case "hp":
                    return HP;

                case "id":
                    return ID;

                case "imm":
                    return GetStat(StatType.Imm);

                case "int":
                    return GetStat(StatType.Int);

                case "level":
                    return Level;

                case "map_id":
                    return MapID;

                case "maxhit":
                    return GetStat(StatType.MaxHit);

                case "maxhp":
                    return GetStat(StatType.MaxHP);

                case "maxmp":
                    return GetStat(StatType.MaxMP);

                case "minhit":
                    return GetStat(StatType.MinHit);

                case "mp":
                    return MP;

                case "name":
                    return Name;

                case "perc":
                    return GetStat(StatType.Perc);

                case "recov":
                    return GetStat(StatType.Recov);

                case "regen":
                    return GetStat(StatType.Regen);

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

                case "str":
                    return GetStat(StatType.Str);

                case "tact":
                    return GetStat(StatType.Tact);

                case "ws":
                    return GetStat(StatType.WS);

                case "x":
                    return X;

                case "y":
                    return Y;

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
            _stat[key] = (Byte)value;
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
                    SetStat(StatType.Acc, (Int32)value);
                    break;

                case "account_id":
                    AccountID = (AccountID?)value;
                    break;

                case "agi":
                    SetStat(StatType.Agi, (Int32)value);
                    break;

                case "armor":
                    SetStat(StatType.Armor, (Int32)value);
                    break;

                case "body_id":
                    BodyID = (BodyIndex)value;
                    break;

                case "bra":
                    SetStat(StatType.Bra, (Int32)value);
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

                case "defence":
                    SetStat(StatType.Defence, (Int32)value);
                    break;

                case "dex":
                    SetStat(StatType.Dex, (Int32)value);
                    break;

                case "evade":
                    SetStat(StatType.Evade, (Int32)value);
                    break;

                case "exp":
                    Exp = (Int32)value;
                    break;

                case "hp":
                    HP = (SPValueType)value;
                    break;

                case "id":
                    ID = (CharacterID)value;
                    break;

                case "imm":
                    SetStat(StatType.Imm, (Int32)value);
                    break;

                case "int":
                    SetStat(StatType.Int, (Int32)value);
                    break;

                case "level":
                    Level = (Byte)value;
                    break;

                case "map_id":
                    MapID = (MapIndex)value;
                    break;

                case "maxhit":
                    SetStat(StatType.MaxHit, (Int32)value);
                    break;

                case "maxhp":
                    SetStat(StatType.MaxHP, (Int32)value);
                    break;

                case "maxmp":
                    SetStat(StatType.MaxMP, (Int32)value);
                    break;

                case "minhit":
                    SetStat(StatType.MinHit, (Int32)value);
                    break;

                case "mp":
                    MP = (SPValueType)value;
                    break;

                case "name":
                    Name = (String)value;
                    break;

                case "perc":
                    SetStat(StatType.Perc, (Int32)value);
                    break;

                case "recov":
                    SetStat(StatType.Recov, (Int32)value);
                    break;

                case "regen":
                    SetStat(StatType.Regen, (Int32)value);
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

                case "str":
                    SetStat(StatType.Str, (Int32)value);
                    break;

                case "tact":
                    SetStat(StatType.Tact, (Int32)value);
                    break;

                case "ws":
                    SetStat(StatType.WS, (Int32)value);
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

        #region ICharacterTable Members

        /// <summary>
        /// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
        /// key is the collection's key and the value is the value for that corresponding key.
        /// </summary>
        public IEnumerable<KeyValuePair<StatType, Int32>> Stats
        {
            get { return _stat; }
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
            return (Byte)_stat[key];
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
        /// Gets or sets the value for the field that maps onto the database column `body_id`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
        /// </summary>
        public BodyIndex BodyID
        {
            get { return (BodyIndex)_bodyID; }
            set { _bodyID = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `cash`.
        /// The underlying database type is `int(11)` with the default value of `0`.
        /// </summary>
        public Int32 Cash
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
        /// Gets or sets the value for the field that maps onto the database column `exp`.
        /// The underlying database type is `int(11)` with the default value of `0`.
        /// </summary>
        public Int32 Exp
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
        public CharacterID ID
        {
            get { return (CharacterID)_iD; }
            set { _iD = (Int32)value; }
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
        /// Gets or sets the value for the field that maps onto the database column `shop_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public ShopID? ShopID
        {
            get { return (ShopID?)_shopID; }
            set { _shopID = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `statpoints`.
        /// The underlying database type is `int(11)` with the default value of `0`.
        /// </summary>
        public Int32 StatPoints
        {
            get { return _statPoints; }
            set { _statPoints = value; }
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
        public ICharacterTable DeepCopy()
        {
            return new CharacterTable(this);
        }

        #endregion

        /// <summary>
        /// A Dictionary-like lookup table for the Enum values of the type collection `Stat` for the
        /// table that this class represents. Majority of the code for this class was automatically generated and
        /// only other automatically generated code should be using this class.
        /// </summary>
        class StatConstDictionary : IEnumerable<KeyValuePair<StatType, Int32>>
        {
            /// <summary>
            /// Array that maps the integer value of key type to the index of the _values array.
            /// </summary>
            static readonly Int32[] _lookupTable;

            /// <summary>
            /// Array containing the actual values. The index of this array is found through the value returned
            /// from the _lookupTable.
            /// </summary>
            readonly Int32[] _values;

            /// <summary>
            /// Gets or sets an item's value using the <paramref name="key"/>.
            /// </summary>
            /// <param name="key">The key for the value to get or set.</param>
            /// <returns>The item's value for the corresponding <paramref name="key"/>.</returns>
            public Int32 this[StatType key]
            {
                get { return _values[_lookupTable[(Int32)key]]; }
                set { _values[_lookupTable[(Int32)key]] = value; }
            }

            /// <summary>
            /// StatConstDictionary static constructor.
            /// </summary>
            static StatConstDictionary()
            {
                var asArray = Enum.GetValues(typeof(StatType)).Cast<StatType>().ToArray();
                _lookupTable = new Int32[asArray.Length];

                for (var i = 0; i < _lookupTable.Length; i++)
                {
                    _lookupTable[i] = (Int32)asArray[i];
                }
            }

            /// <summary>
            /// StatConstDictionary constructor.
            /// </summary>
            public StatConstDictionary()
            {
                _values = new Int32[_lookupTable.Length];
            }

            #region IEnumerable<KeyValuePair<StatType,int>> Members

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>1</filterpriority>
            public IEnumerator<KeyValuePair<StatType, Int32>> GetEnumerator()
            {
                for (var i = 0; i < _values.Length; i++)
                {
                    yield return new KeyValuePair<StatType, Int32>((StatType)i, _values[i]);
                }
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }
    }
}