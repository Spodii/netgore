using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public const Int32 ColumnCount = 36;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "character";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
                                              {
                                                  "acc", "agi", "armor", "body_id", "bra", "cash", "character_template_id",
                                                  "defence", "dex", "evade", "exp", "hp", "id", "imm", "int", "level", "map_id",
                                                  "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "password", "perc", "recov",
                                                  "regen", "respawn_map", "respawn_x", "respawn_y", "statpoints", "str", "tact",
                                                  "ws", "x", "y"
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
                                                        "acc", "agi", "armor", "body_id", "bra", "cash", "character_template_id",
                                                        "defence", "dex", "evade", "exp", "hp", "imm", "int", "level", "map_id",
                                                        "maxhit", "maxhp", "maxmp", "minhit", "mp", "name", "password", "perc",
                                                        "recov", "regen", "respawn_map", "respawn_x", "respawn_y", "statpoints",
                                                        "str", "tact", "ws", "x", "y"
                                                    };

        /// <summary>
        /// The fields that are used in the column collection `Stat`.
        /// </summary>
        static readonly String[] _statColumns = new string[]
                                                {
                                                    "acc", "agi", "armor", "bra", "defence", "dex", "evade", "imm", "int", "maxhit",
                                                    "maxhp", "maxmp", "minhit", "perc", "recov", "regen", "str", "tact", "ws"
                                                };

        /// <summary>
        /// Dictionary containing the values for the column collection `Stat`.
        /// </summary>
        readonly StatConstDictionary _stat = new StatConstDictionary();

        /// <summary>
        /// The field that maps onto the database column `body_id`.
        /// </summary>
        UInt16 _bodyID;

        /// <summary>
        /// The field that maps onto the database column `cash`.
        /// </summary>
        UInt32 _cash;

        /// <summary>
        /// The field that maps onto the database column `character_template_id`.
        /// </summary>
        Nullable<UInt16> _characterTemplateID;

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
        /// The field that maps onto the database column `password`.
        /// </summary>
        String _password;

        /// <summary>
        /// The field that maps onto the database column `respawn_map`.
        /// </summary>
        Nullable<UInt16> _respawnMap;

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
            get { return (IEnumerable<String>)_dbColumns; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
        /// </summary>
        public static IEnumerable<String> DbKeyColumns
        {
            get { return (IEnumerable<String>)_dbColumnsKeys; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
        /// </summary>
        public static IEnumerable<String> DbNonKeyColumns
        {
            get { return (IEnumerable<String>)_dbColumnsNonKey; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the name of the database
        /// columns used in the column collection `Stat`.
        /// </summary>
        public static IEnumerable<String> StatColumns
        {
            get { return (IEnumerable<String>)_statColumns; }
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
        /// <param name="agi">The initial value for the corresponding property.</param>
        /// <param name="armor">The initial value for the corresponding property.</param>
        /// <param name="bodyID">The initial value for the corresponding property.</param>
        /// <param name="bra">The initial value for the corresponding property.</param>
        /// <param name="cash">The initial value for the corresponding property.</param>
        /// <param name="characterTemplateID">The initial value for the corresponding property.</param>
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
        /// <param name="password">The initial value for the corresponding property.</param>
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
        public CharacterTable(Byte @acc, Byte @agi, Byte @armor, BodyIndex @bodyID, Byte @bra, UInt32 @cash,
                              Nullable<CharacterTemplateID> @characterTemplateID, Byte @defence, Byte @dex, Byte @evade,
                              UInt32 @exp, SPValueType @hP, CharacterID @iD, Byte @imm, Byte @int, Byte @level, MapIndex @mapID,
                              Byte @maxHit, Int16 @maxHP, Int16 @maxMP, Byte @minHit, SPValueType @mP, String @name,
                              String @password, Byte @perc, Byte @recov, Byte @regen, Nullable<MapIndex> @respawnMap,
                              Single @respawnX, Single @respawnY, UInt32 @statPoints, Byte @str, Byte @tact, Byte @wS, Single @x,
                              Single @y)
        {
            SetStat((StatType)StatType.Acc, (Int32)@acc);
            SetStat((StatType)StatType.Agi, (Int32)@agi);
            SetStat((StatType)StatType.Armor, (Int32)@armor);
            BodyID = (BodyIndex)@bodyID;
            SetStat((StatType)StatType.Bra, (Int32)@bra);
            Cash = (UInt32)@cash;
            CharacterTemplateID = (Nullable<CharacterTemplateID>)@characterTemplateID;
            SetStat((StatType)StatType.Defence, (Int32)@defence);
            SetStat((StatType)StatType.Dex, (Int32)@dex);
            SetStat((StatType)StatType.Evade, (Int32)@evade);
            Exp = (UInt32)@exp;
            HP = (SPValueType)@hP;
            ID = (CharacterID)@iD;
            SetStat((StatType)StatType.Imm, (Int32)@imm);
            SetStat((StatType)StatType.Int, (Int32)@int);
            Level = (Byte)@level;
            MapID = (MapIndex)@mapID;
            SetStat((StatType)StatType.MaxHit, (Int32)@maxHit);
            SetStat((StatType)StatType.MaxHP, (Int32)@maxHP);
            SetStat((StatType)StatType.MaxMP, (Int32)@maxMP);
            SetStat((StatType)StatType.MinHit, (Int32)@minHit);
            MP = (SPValueType)@mP;
            Name = (String)@name;
            Password = (String)@password;
            SetStat((StatType)StatType.Perc, (Int32)@perc);
            SetStat((StatType)StatType.Recov, (Int32)@recov);
            SetStat((StatType)StatType.Regen, (Int32)@regen);
            RespawnMap = (Nullable<MapIndex>)@respawnMap;
            RespawnX = (Single)@respawnX;
            RespawnY = (Single)@respawnY;
            StatPoints = (UInt32)@statPoints;
            SetStat((StatType)StatType.Str, (Int32)@str);
            SetStat((StatType)StatType.Tact, (Int32)@tact);
            SetStat((StatType)StatType.WS, (Int32)@wS);
            X = (Single)@x;
            Y = (Single)@y;
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
            dic["@acc"] = (Byte)source.GetStat((StatType)StatType.Acc);
            dic["@agi"] = (Byte)source.GetStat((StatType)StatType.Agi);
            dic["@armor"] = (Byte)source.GetStat((StatType)StatType.Armor);
            dic["@body_id"] = (BodyIndex)source.BodyID;
            dic["@bra"] = (Byte)source.GetStat((StatType)StatType.Bra);
            dic["@cash"] = (UInt32)source.Cash;
            dic["@character_template_id"] = (Nullable<CharacterTemplateID>)source.CharacterTemplateID;
            dic["@defence"] = (Byte)source.GetStat((StatType)StatType.Defence);
            dic["@dex"] = (Byte)source.GetStat((StatType)StatType.Dex);
            dic["@evade"] = (Byte)source.GetStat((StatType)StatType.Evade);
            dic["@exp"] = (UInt32)source.Exp;
            dic["@hp"] = (SPValueType)source.HP;
            dic["@id"] = (CharacterID)source.ID;
            dic["@imm"] = (Byte)source.GetStat((StatType)StatType.Imm);
            dic["@int"] = (Byte)source.GetStat((StatType)StatType.Int);
            dic["@level"] = (Byte)source.Level;
            dic["@map_id"] = (MapIndex)source.MapID;
            dic["@maxhit"] = (Byte)source.GetStat((StatType)StatType.MaxHit);
            dic["@maxhp"] = (Int16)source.GetStat((StatType)StatType.MaxHP);
            dic["@maxmp"] = (Int16)source.GetStat((StatType)StatType.MaxMP);
            dic["@minhit"] = (Byte)source.GetStat((StatType)StatType.MinHit);
            dic["@mp"] = (SPValueType)source.MP;
            dic["@name"] = (String)source.Name;
            dic["@password"] = (String)source.Password;
            dic["@perc"] = (Byte)source.GetStat((StatType)StatType.Perc);
            dic["@recov"] = (Byte)source.GetStat((StatType)StatType.Recov);
            dic["@regen"] = (Byte)source.GetStat((StatType)StatType.Regen);
            dic["@respawn_map"] = (Nullable<MapIndex>)source.RespawnMap;
            dic["@respawn_x"] = (Single)source.RespawnX;
            dic["@respawn_y"] = (Single)source.RespawnY;
            dic["@statpoints"] = (UInt32)source.StatPoints;
            dic["@str"] = (Byte)source.GetStat((StatType)StatType.Str);
            dic["@tact"] = (Byte)source.GetStat((StatType)StatType.Tact);
            dic["@ws"] = (Byte)source.GetStat((StatType)StatType.WS);
            dic["@x"] = (Single)source.X;
            dic["@y"] = (Single)source.Y;
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
            SetStat((StatType)StatType.Acc, (Int32)source.GetStat((StatType)StatType.Acc));
            SetStat((StatType)StatType.Agi, (Int32)source.GetStat((StatType)StatType.Agi));
            SetStat((StatType)StatType.Armor, (Int32)source.GetStat((StatType)StatType.Armor));
            BodyID = (BodyIndex)source.BodyID;
            SetStat((StatType)StatType.Bra, (Int32)source.GetStat((StatType)StatType.Bra));
            Cash = (UInt32)source.Cash;
            CharacterTemplateID = (Nullable<CharacterTemplateID>)source.CharacterTemplateID;
            SetStat((StatType)StatType.Defence, (Int32)source.GetStat((StatType)StatType.Defence));
            SetStat((StatType)StatType.Dex, (Int32)source.GetStat((StatType)StatType.Dex));
            SetStat((StatType)StatType.Evade, (Int32)source.GetStat((StatType)StatType.Evade));
            Exp = (UInt32)source.Exp;
            HP = (SPValueType)source.HP;
            ID = (CharacterID)source.ID;
            SetStat((StatType)StatType.Imm, (Int32)source.GetStat((StatType)StatType.Imm));
            SetStat((StatType)StatType.Int, (Int32)source.GetStat((StatType)StatType.Int));
            Level = (Byte)source.Level;
            MapID = (MapIndex)source.MapID;
            SetStat((StatType)StatType.MaxHit, (Int32)source.GetStat((StatType)StatType.MaxHit));
            SetStat((StatType)StatType.MaxHP, (Int32)source.GetStat((StatType)StatType.MaxHP));
            SetStat((StatType)StatType.MaxMP, (Int32)source.GetStat((StatType)StatType.MaxMP));
            SetStat((StatType)StatType.MinHit, (Int32)source.GetStat((StatType)StatType.MinHit));
            MP = (SPValueType)source.MP;
            Name = (String)source.Name;
            Password = (String)source.Password;
            SetStat((StatType)StatType.Perc, (Int32)source.GetStat((StatType)StatType.Perc));
            SetStat((StatType)StatType.Recov, (Int32)source.GetStat((StatType)StatType.Recov));
            SetStat((StatType)StatType.Regen, (Int32)source.GetStat((StatType)StatType.Regen));
            RespawnMap = (Nullable<MapIndex>)source.RespawnMap;
            RespawnX = (Single)source.RespawnX;
            RespawnY = (Single)source.RespawnY;
            StatPoints = (UInt32)source.StatPoints;
            SetStat((StatType)StatType.Str, (Int32)source.GetStat((StatType)StatType.Str));
            SetStat((StatType)StatType.Tact, (Int32)source.GetStat((StatType)StatType.Tact));
            SetStat((StatType)StatType.WS, (Int32)source.GetStat((StatType)StatType.WS));
            X = (Single)source.X;
            Y = (Single)source.Y;
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
                    return new ColumnMetadata("character_template_id", "", "smallint(5) unsigned", null, typeof(Nullable<UInt16>),
                                              true, false, true);

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
                    return new ColumnMetadata("name", "", "varchar(50)", null, typeof(String), false, false, true);

                case "password":
                    return new ColumnMetadata("password", "", "varchar(50)", null, typeof(String), false, false, false);

                case "perc":
                    return new ColumnMetadata("perc", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "recov":
                    return new ColumnMetadata("recov", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "regen":
                    return new ColumnMetadata("regen", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "respawn_map":
                    return new ColumnMetadata("respawn_map", "", "smallint(5) unsigned", null, typeof(Nullable<UInt16>), true,
                                              false, true);

                case "respawn_x":
                    return new ColumnMetadata("respawn_x", "", "float", null, typeof(Single), false, false, false);

                case "respawn_y":
                    return new ColumnMetadata("respawn_y", "", "float", null, typeof(Single), false, false, false);

                case "statpoints":
                    return new ColumnMetadata("statpoints", "", "int(10) unsigned", "0", typeof(UInt32), false, false, false);

                case "str":
                    return new ColumnMetadata("str", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "tact":
                    return new ColumnMetadata("tact", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "ws":
                    return new ColumnMetadata("ws", "", "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

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
                    return GetStat((StatType)StatType.Acc);

                case "agi":
                    return GetStat((StatType)StatType.Agi);

                case "armor":
                    return GetStat((StatType)StatType.Armor);

                case "body_id":
                    return BodyID;

                case "bra":
                    return GetStat((StatType)StatType.Bra);

                case "cash":
                    return Cash;

                case "character_template_id":
                    return CharacterTemplateID;

                case "defence":
                    return GetStat((StatType)StatType.Defence);

                case "dex":
                    return GetStat((StatType)StatType.Dex);

                case "evade":
                    return GetStat((StatType)StatType.Evade);

                case "exp":
                    return Exp;

                case "hp":
                    return HP;

                case "id":
                    return ID;

                case "imm":
                    return GetStat((StatType)StatType.Imm);

                case "int":
                    return GetStat((StatType)StatType.Int);

                case "level":
                    return Level;

                case "map_id":
                    return MapID;

                case "maxhit":
                    return GetStat((StatType)StatType.MaxHit);

                case "maxhp":
                    return GetStat((StatType)StatType.MaxHP);

                case "maxmp":
                    return GetStat((StatType)StatType.MaxMP);

                case "minhit":
                    return GetStat((StatType)StatType.MinHit);

                case "mp":
                    return MP;

                case "name":
                    return Name;

                case "password":
                    return Password;

                case "perc":
                    return GetStat((StatType)StatType.Perc);

                case "recov":
                    return GetStat((StatType)StatType.Recov);

                case "regen":
                    return GetStat((StatType)StatType.Regen);

                case "respawn_map":
                    return RespawnMap;

                case "respawn_x":
                    return RespawnX;

                case "respawn_y":
                    return RespawnY;

                case "statpoints":
                    return StatPoints;

                case "str":
                    return GetStat((StatType)StatType.Str);

                case "tact":
                    return GetStat((StatType)StatType.Tact);

                case "ws":
                    return GetStat((StatType)StatType.WS);

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
            _stat[(StatType)key] = (Byte)value;
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
                    SetStat((StatType)StatType.Acc, (Int32)value);
                    break;

                case "agi":
                    SetStat((StatType)StatType.Agi, (Int32)value);
                    break;

                case "armor":
                    SetStat((StatType)StatType.Armor, (Int32)value);
                    break;

                case "body_id":
                    BodyID = (BodyIndex)value;
                    break;

                case "bra":
                    SetStat((StatType)StatType.Bra, (Int32)value);
                    break;

                case "cash":
                    Cash = (UInt32)value;
                    break;

                case "character_template_id":
                    CharacterTemplateID = (Nullable<CharacterTemplateID>)value;
                    break;

                case "defence":
                    SetStat((StatType)StatType.Defence, (Int32)value);
                    break;

                case "dex":
                    SetStat((StatType)StatType.Dex, (Int32)value);
                    break;

                case "evade":
                    SetStat((StatType)StatType.Evade, (Int32)value);
                    break;

                case "exp":
                    Exp = (UInt32)value;
                    break;

                case "hp":
                    HP = (SPValueType)value;
                    break;

                case "id":
                    ID = (CharacterID)value;
                    break;

                case "imm":
                    SetStat((StatType)StatType.Imm, (Int32)value);
                    break;

                case "int":
                    SetStat((StatType)StatType.Int, (Int32)value);
                    break;

                case "level":
                    Level = (Byte)value;
                    break;

                case "map_id":
                    MapID = (MapIndex)value;
                    break;

                case "maxhit":
                    SetStat((StatType)StatType.MaxHit, (Int32)value);
                    break;

                case "maxhp":
                    SetStat((StatType)StatType.MaxHP, (Int32)value);
                    break;

                case "maxmp":
                    SetStat((StatType)StatType.MaxMP, (Int32)value);
                    break;

                case "minhit":
                    SetStat((StatType)StatType.MinHit, (Int32)value);
                    break;

                case "mp":
                    MP = (SPValueType)value;
                    break;

                case "name":
                    Name = (String)value;
                    break;

                case "password":
                    Password = (String)value;
                    break;

                case "perc":
                    SetStat((StatType)StatType.Perc, (Int32)value);
                    break;

                case "recov":
                    SetStat((StatType)StatType.Recov, (Int32)value);
                    break;

                case "regen":
                    SetStat((StatType)StatType.Regen, (Int32)value);
                    break;

                case "respawn_map":
                    RespawnMap = (Nullable<MapIndex>)value;
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
                    SetStat((StatType)StatType.Str, (Int32)value);
                    break;

                case "tact":
                    SetStat((StatType)StatType.Tact, (Int32)value);
                    break;

                case "ws":
                    SetStat((StatType)StatType.WS, (Int32)value);
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
            get { return (IEnumerable<KeyValuePair<StatType, Int32>>)_stat; }
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
            return (Byte)_stat[(StatType)key];
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
        /// The underlying database type is `int(10) unsigned` with the default value of `0`.
        /// </summary>
        public UInt32 Cash
        {
            get { return (UInt32)_cash; }
            set { _cash = (UInt32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `character_template_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public Nullable<CharacterTemplateID> CharacterTemplateID
        {
            get { return (Nullable<CharacterTemplateID>)_characterTemplateID; }
            set { _characterTemplateID = (Nullable<UInt16>)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `exp`.
        /// The underlying database type is `int(10) unsigned` with the default value of `0`.
        /// </summary>
        public UInt32 Exp
        {
            get { return (UInt32)_exp; }
            set { _exp = (UInt32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `hp`.
        /// The underlying database type is `smallint(6)` with the default value of `50`.
        /// </summary>
        public SPValueType HP
        {
            get { return (SPValueType)_hP; }
            set { _hP = (Int16)value; }
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
            get { return (Byte)_level; }
            set { _level = (Byte)value; }
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
            get { return (SPValueType)_mP; }
            set { _mP = (Int16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `name`.
        /// The underlying database type is `varchar(50)`.
        /// </summary>
        public String Name
        {
            get { return (String)_name; }
            set { _name = (String)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `password`.
        /// The underlying database type is `varchar(50)`.
        /// </summary>
        public String Password
        {
            get { return (String)_password; }
            set { _password = (String)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_map`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public Nullable<MapIndex> RespawnMap
        {
            get { return (Nullable<MapIndex>)_respawnMap; }
            set { _respawnMap = (Nullable<UInt16>)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_x`.
        /// The underlying database type is `float`.
        /// </summary>
        public Single RespawnX
        {
            get { return (Single)_respawnX; }
            set { _respawnX = (Single)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_y`.
        /// The underlying database type is `float`.
        /// </summary>
        public Single RespawnY
        {
            get { return (Single)_respawnY; }
            set { _respawnY = (Single)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `statpoints`.
        /// The underlying database type is `int(10) unsigned` with the default value of `0`.
        /// </summary>
        public UInt32 StatPoints
        {
            get { return (UInt32)_statPoints; }
            set { _statPoints = (UInt32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `x`.
        /// The underlying database type is `float` with the default value of `100`.
        /// </summary>
        public Single X
        {
            get { return (Single)_x; }
            set { _x = (Single)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `y`.
        /// The underlying database type is `float` with the default value of `100`.
        /// </summary>
        public Single Y
        {
            get { return (Single)_y; }
            set { _y = (Single)value; }
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

                for (Int32 i = 0; i < _lookupTable.Length; i++)
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
                for (int i = 0; i < _values.Length; i++)
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