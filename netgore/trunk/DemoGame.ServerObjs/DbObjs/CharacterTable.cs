using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `character`.
    /// </summary>
    public interface ICharacterTable
    {
        /// <summary>
        /// Gets the value of the database column `body`.
        /// </summary>
        UInt16 Body { get; }

        /// <summary>
        /// Gets the value of the database column `cash`.
        /// </summary>
        UInt32 Cash { get; }

        /// <summary>
        /// Gets the value of the database column `exp`.
        /// </summary>
        UInt32 Exp { get; }

        /// <summary>
        /// Gets the value of the database column `hp`.
        /// </summary>
        UInt16 HP { get; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        CharacterID ID { get; }

        /// <summary>
        /// Gets the value of the database column `level`.
        /// </summary>
        Byte Level { get; }

        /// <summary>
        /// Gets the value of the database column `map_id`.
        /// </summary>
        MapIndex MapID { get; }

        /// <summary>
        /// Gets the value of the database column `mp`.
        /// </summary>
        UInt16 MP { get; }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the value of the database column `password`.
        /// </summary>
        String Password { get; }

        /// <summary>
        /// Gets the value of the database column `respawn_map`.
        /// </summary>
        MapIndex RespawnMap { get; }

        /// <summary>
        /// Gets the value of the database column `respawn_x`.
        /// </summary>
        Single RespawnX { get; }

        /// <summary>
        /// Gets the value of the database column `respawn_y`.
        /// </summary>
        Single RespawnY { get; }

        /// <summary>
        /// Gets the value of the database column `statpoints`.
        /// </summary>
        UInt32 StatPoints { get; }

        /// <summary>
        /// Gets the value of the database column `template_id`.
        /// </summary>
        CharacterTemplateID TemplateId { get; }

        /// <summary>
        /// Gets the value of the database column `x`.
        /// </summary>
        Single X { get; }

        /// <summary>
        /// Gets the value of the database column `y`.
        /// </summary>
        Single Y { get; }

        /// <summary>
        /// Gets the value of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        Int32 GetStat(StatType key);

        /// <summary>
        /// Gets the <paramref name="value"/> of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key that represents the column in this column collection.</param>
        /// <param name="value">The value to assign to the column with the corresponding <paramref name="key"/>.</param>
        void SetStat(StatType key, Int32 value);
    }

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
                                                  "acc", "agi", "armor", "body", "bra", "cash", "defence", "dex", "evade", "exp",
                                                  "hp", "id", "imm", "int", "level", "map_id", "maxhit", "maxhp", "maxmp", "minhit",
                                                  "mp", "name", "password", "perc", "recov", "regen", "respawn_map", "respawn_x",
                                                  "respawn_y", "statpoints", "str", "tact", "template_id", "ws", "x", "y"
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
                                                        "acc", "agi", "armor", "body", "bra", "cash", "defence", "dex", "evade",
                                                        "exp", "hp", "imm", "int", "level", "map_id", "maxhit", "maxhp", "maxmp",
                                                        "minhit", "mp", "name", "password", "perc", "recov", "regen", "respawn_map",
                                                        "respawn_x", "respawn_y", "statpoints", "str", "tact", "template_id", "ws",
                                                        "x", "y"
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
        /// The field that maps onto the database column `body`.
        /// </summary>
        UInt16 _body;

        /// <summary>
        /// The field that maps onto the database column `cash`.
        /// </summary>
        UInt32 _cash;

        /// <summary>
        /// The field that maps onto the database column `exp`.
        /// </summary>
        UInt32 _exp;

        /// <summary>
        /// The field that maps onto the database column `hp`.
        /// </summary>
        UInt16 _hP;

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
        UInt16 _mP;

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
        /// The field that maps onto the database column `template_id`.
        /// </summary>
        ushort? _templateId;

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
        /// <param name="agi">The initial value for the corresponding property.</param>
        /// <param name="armor">The initial value for the corresponding property.</param>
        /// <param name="body">The initial value for the corresponding property.</param>
        /// <param name="bra">The initial value for the corresponding property.</param>
        /// <param name="cash">The initial value for the corresponding property.</param>
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
        /// <param name="templateId">The initial value for the corresponding property.</param>
        /// <param name="wS">The initial value for the corresponding property.</param>
        /// <param name="x">The initial value for the corresponding property.</param>
        /// <param name="y">The initial value for the corresponding property.</param>
        public CharacterTable(Byte @acc, Byte @agi, Byte @armor, UInt16 @body, Byte @bra, UInt32 @cash, Byte @defence, Byte @dex,
                              Byte @evade, UInt32 @exp, UInt16 @hP, CharacterID @iD, Byte @imm, Byte @int, Byte @level,
                              MapIndex @mapID, Byte @maxHit, UInt16 @maxHP, UInt16 @maxMP, Byte @minHit, UInt16 @mP, String @name,
                              String @password, Byte @perc, Byte @recov, Byte @regen, MapIndex @respawnMap, Single @respawnX,
                              Single @respawnY, UInt32 @statPoints, Byte @str, Byte @tact, CharacterTemplateID @templateId,
                              Byte @wS, Single @x, Single @y)
        {
            SetStat(StatType.Acc, @acc);
            SetStat(StatType.Agi, @agi);
            SetStat(StatType.Armor, @armor);
            Body = @body;
            SetStat(StatType.Bra, @bra);
            Cash = @cash;
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
            Password = @password;
            SetStat(StatType.Perc, @perc);
            SetStat(StatType.Recov, @recov);
            SetStat(StatType.Regen, @regen);
            RespawnMap = @respawnMap;
            RespawnX = @respawnX;
            RespawnY = @respawnY;
            StatPoints = @statPoints;
            SetStat(StatType.Str, @str);
            SetStat(StatType.Tact, @tact);
            TemplateId = @templateId;
            SetStat(StatType.WS, @wS);
            X = @x;
            Y = @y;
        }

        /// <summary>
        /// CharacterTable constructor.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
        public CharacterTable(IDataReader dataReader)
        {
            ReadValues(dataReader);
        }

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
            dic["@agi"] = (Byte)source.GetStat(StatType.Agi);
            dic["@armor"] = (Byte)source.GetStat(StatType.Armor);
            dic["@body"] = source.Body;
            dic["@bra"] = (Byte)source.GetStat(StatType.Bra);
            dic["@cash"] = source.Cash;
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
            dic["@maxhp"] = (UInt16)source.GetStat(StatType.MaxHP);
            dic["@maxmp"] = (UInt16)source.GetStat(StatType.MaxMP);
            dic["@minhit"] = (Byte)source.GetStat(StatType.MinHit);
            dic["@mp"] = source.MP;
            dic["@name"] = source.Name;
            dic["@password"] = source.Password;
            dic["@perc"] = (Byte)source.GetStat(StatType.Perc);
            dic["@recov"] = (Byte)source.GetStat(StatType.Recov);
            dic["@regen"] = (Byte)source.GetStat(StatType.Regen);
            dic["@respawn_map"] = source.RespawnMap;
            dic["@respawn_x"] = source.RespawnX;
            dic["@respawn_y"] = source.RespawnY;
            dic["@statpoints"] = source.StatPoints;
            dic["@str"] = (Byte)source.GetStat(StatType.Str);
            dic["@tact"] = (Byte)source.GetStat(StatType.Tact);
            dic["@template_id"] = source.TemplateId;
            dic["@ws"] = (Byte)source.GetStat(StatType.WS);
            dic["@x"] = source.X;
            dic["@y"] = source.Y;
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(ICharacterTable source, DbParameterValues paramValues)
        {
            paramValues["@acc"] = (Byte)source.GetStat(StatType.Acc);
            paramValues["@agi"] = (Byte)source.GetStat(StatType.Agi);
            paramValues["@armor"] = (Byte)source.GetStat(StatType.Armor);
            paramValues["@body"] = source.Body;
            paramValues["@bra"] = (Byte)source.GetStat(StatType.Bra);
            paramValues["@cash"] = source.Cash;
            paramValues["@defence"] = (Byte)source.GetStat(StatType.Defence);
            paramValues["@dex"] = (Byte)source.GetStat(StatType.Dex);
            paramValues["@evade"] = (Byte)source.GetStat(StatType.Evade);
            paramValues["@exp"] = source.Exp;
            paramValues["@hp"] = source.HP;
            paramValues["@id"] = source.ID;
            paramValues["@imm"] = (Byte)source.GetStat(StatType.Imm);
            paramValues["@int"] = (Byte)source.GetStat(StatType.Int);
            paramValues["@level"] = source.Level;
            paramValues["@map_id"] = source.MapID;
            paramValues["@maxhit"] = (Byte)source.GetStat(StatType.MaxHit);
            paramValues["@maxhp"] = (UInt16)source.GetStat(StatType.MaxHP);
            paramValues["@maxmp"] = (UInt16)source.GetStat(StatType.MaxMP);
            paramValues["@minhit"] = (Byte)source.GetStat(StatType.MinHit);
            paramValues["@mp"] = source.MP;
            paramValues["@name"] = source.Name;
            paramValues["@password"] = source.Password;
            paramValues["@perc"] = (Byte)source.GetStat(StatType.Perc);
            paramValues["@recov"] = (Byte)source.GetStat(StatType.Recov);
            paramValues["@regen"] = (Byte)source.GetStat(StatType.Regen);
            paramValues["@respawn_map"] = source.RespawnMap;
            paramValues["@respawn_x"] = source.RespawnX;
            paramValues["@respawn_y"] = source.RespawnY;
            paramValues["@statpoints"] = source.StatPoints;
            paramValues["@str"] = (Byte)source.GetStat(StatType.Str);
            paramValues["@tact"] = (Byte)source.GetStat(StatType.Tact);
            paramValues["@template_id"] = source.TemplateId;
            paramValues["@ws"] = (Byte)source.GetStat(StatType.WS);
            paramValues["@x"] = source.X;
            paramValues["@y"] = source.Y;
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
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public void CopyValues(DbParameterValues paramValues)
        {
            CopyValues(this, paramValues);
        }

        public void CopyValuesFrom(ICharacterTable source)
        {
            SetStat(StatType.Acc, source.GetStat(StatType.Acc));
            SetStat(StatType.Agi, source.GetStat(StatType.Agi));
            SetStat(StatType.Armor, source.GetStat(StatType.Armor));
            Body = source.Body;
            SetStat(StatType.Bra, source.GetStat(StatType.Bra));
            Cash = source.Cash;
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
            Password = source.Password;
            SetStat(StatType.Perc, source.GetStat(StatType.Perc));
            SetStat(StatType.Recov, source.GetStat(StatType.Recov));
            SetStat(StatType.Regen, source.GetStat(StatType.Regen));
            RespawnMap = source.RespawnMap;
            RespawnX = source.RespawnX;
            RespawnY = source.RespawnY;
            StatPoints = source.StatPoints;
            SetStat(StatType.Str, source.GetStat(StatType.Str));
            SetStat(StatType.Tact, source.GetStat(StatType.Tact));
            TemplateId = source.TemplateId;
            SetStat(StatType.WS, source.GetStat(StatType.WS));
            X = source.X;
            Y = source.Y;
        }

        public static ColumnMetadata GetColumnData(String fieldName)
        {
            switch (fieldName)
            {
                case "acc":
                    return new ColumnMetadata("acc", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "agi":
                    return new ColumnMetadata("agi", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "armor":
                    return new ColumnMetadata("armor", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "body":
                    return new ColumnMetadata("body", "", "smallint(5) unsigned", "1", typeof(UInt16), false, false, false);

                case "bra":
                    return new ColumnMetadata("bra", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "cash":
                    return new ColumnMetadata("cash", "", "int(10) unsigned", "0", typeof(UInt32), false, false, false);

                case "defence":
                    return new ColumnMetadata("defence", "", "tinyint(3) unsigned", "0", typeof(Byte), false, false, false);

                case "dex":
                    return new ColumnMetadata("dex", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "evade":
                    return new ColumnMetadata("evade", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "exp":
                    return new ColumnMetadata("exp", "", "int(10) unsigned", "0", typeof(UInt32), false, false, false);

                case "hp":
                    return new ColumnMetadata("hp", "", "smallint(5) unsigned", "50", typeof(UInt16), false, false, false);

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
                    return new ColumnMetadata("maxhp", "", "smallint(5) unsigned", "50", typeof(UInt16), false, false, false);

                case "maxmp":
                    return new ColumnMetadata("maxmp", "", "smallint(5) unsigned", "50", typeof(UInt16), false, false, false);

                case "minhit":
                    return new ColumnMetadata("minhit", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "mp":
                    return new ColumnMetadata("mp", "", "smallint(5) unsigned", "50", typeof(UInt16), false, false, false);

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
                    return new ColumnMetadata("respawn_map", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, true);

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

                case "template_id":
                    return new ColumnMetadata("template_id", "", "smallint(5) unsigned", null, typeof(ushort?), true, false, true);

                case "ws":
                    return new ColumnMetadata("ws", "", "tinyint(3) unsigned", null, typeof(Byte), false, false, false);

                case "x":
                    return new ColumnMetadata("x", "", "float", "100", typeof(Single), false, false, false);

                case "y":
                    return new ColumnMetadata("y", "", "float", "100", typeof(Single), false, false, false);

                default:
                    throw new ArgumentException("Field not found.", "fieldName");
            }
        }

        public Object GetValue(String columnName)
        {
            switch (columnName)
            {
                case "acc":
                    return GetStat(StatType.Acc);

                case "agi":
                    return GetStat(StatType.Agi);

                case "armor":
                    return GetStat(StatType.Armor);

                case "body":
                    return Body;

                case "bra":
                    return GetStat(StatType.Bra);

                case "cash":
                    return Cash;

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

                case "password":
                    return Password;

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

                case "statpoints":
                    return StatPoints;

                case "str":
                    return GetStat(StatType.Str);

                case "tact":
                    return GetStat(StatType.Tact);

                case "template_id":
                    return TemplateId;

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
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void ReadValues(IDataReader dataReader)
        {
            Int32 i;

            i = dataReader.GetOrdinal("acc");
            SetStat(StatType.Acc, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("agi");
            SetStat(StatType.Agi, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("armor");
            SetStat(StatType.Armor, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("body");
            Body = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("bra");
            SetStat(StatType.Bra, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("cash");
            Cash = dataReader.GetUInt32(i);

            i = dataReader.GetOrdinal("defence");
            SetStat(StatType.Defence, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("dex");
            SetStat(StatType.Dex, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("evade");
            SetStat(StatType.Evade, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("exp");
            Exp = dataReader.GetUInt32(i);

            i = dataReader.GetOrdinal("hp");
            HP = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("id");
            ID = (CharacterID)dataReader.GetInt32(i);

            i = dataReader.GetOrdinal("imm");
            SetStat(StatType.Imm, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("int");
            SetStat(StatType.Int, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("level");
            Level = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("map_id");
            MapID = (MapIndex)dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("maxhit");
            SetStat(StatType.MaxHit, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("maxhp");
            SetStat(StatType.MaxHP, dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("maxmp");
            SetStat(StatType.MaxMP, dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("minhit");
            SetStat(StatType.MinHit, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("mp");
            MP = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("name");
            Name = dataReader.GetString(i);

            i = dataReader.GetOrdinal("password");
            Password = dataReader.GetString(i);

            i = dataReader.GetOrdinal("perc");
            SetStat(StatType.Perc, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("recov");
            SetStat(StatType.Recov, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("regen");
            SetStat(StatType.Regen, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("respawn_map");
            RespawnMap = (MapIndex)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("respawn_x");
            RespawnX = dataReader.GetFloat(i);

            i = dataReader.GetOrdinal("respawn_y");
            RespawnY = dataReader.GetFloat(i);

            i = dataReader.GetOrdinal("statpoints");
            StatPoints = dataReader.GetUInt32(i);

            i = dataReader.GetOrdinal("str");
            SetStat(StatType.Str, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("tact");
            SetStat(StatType.Tact, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("template_id");
            TemplateId = (CharacterTemplateID)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("ws");
            SetStat(StatType.WS, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("x");
            X = dataReader.GetFloat(i);

            i = dataReader.GetOrdinal("y");
            Y = dataReader.GetFloat(i);
        }

        public void SetValue(String columnName, Object value)
        {
            switch (columnName)
            {
                case "acc":
                    SetStat(StatType.Acc, (Int32)value);
                    break;

                case "agi":
                    SetStat(StatType.Agi, (Int32)value);
                    break;

                case "armor":
                    SetStat(StatType.Armor, (Int32)value);
                    break;

                case "body":
                    Body = (UInt16)value;
                    break;

                case "bra":
                    SetStat(StatType.Bra, (Int32)value);
                    break;

                case "cash":
                    Cash = (UInt32)value;
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
                    Exp = (UInt32)value;
                    break;

                case "hp":
                    HP = (UInt16)value;
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
                    MP = (UInt16)value;
                    break;

                case "name":
                    Name = (String)value;
                    break;

                case "password":
                    Password = (String)value;
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
                    RespawnMap = (MapIndex)value;
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
                    SetStat(StatType.Str, (Int32)value);
                    break;

                case "tact":
                    SetStat(StatType.Tact, (Int32)value);
                    break;

                case "template_id":
                    TemplateId = (CharacterTemplateID)value;
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

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The key must already exist in the DbParameterValues
        /// for the value to be copied over. If any of the keys in the DbParameterValues do not
        /// match one of the column names, or if there is no field for a key, then it will be
        /// ignored. Because of this, it is important to be careful when using this method
        /// since columns or keys can be skipped without any indication.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void TryCopyValues(ICharacterTable source, DbParameterValues paramValues)
        {
            for (int i = 0; i < paramValues.Count; i++)
            {
                switch (paramValues.GetParameterName(i))
                {
                    case "@acc":
                        paramValues[i] = source.GetStat(StatType.Acc);
                        break;

                    case "@agi":
                        paramValues[i] = source.GetStat(StatType.Agi);
                        break;

                    case "@armor":
                        paramValues[i] = source.GetStat(StatType.Armor);
                        break;

                    case "@body":
                        paramValues[i] = source.Body;
                        break;

                    case "@bra":
                        paramValues[i] = source.GetStat(StatType.Bra);
                        break;

                    case "@cash":
                        paramValues[i] = source.Cash;
                        break;

                    case "@defence":
                        paramValues[i] = source.GetStat(StatType.Defence);
                        break;

                    case "@dex":
                        paramValues[i] = source.GetStat(StatType.Dex);
                        break;

                    case "@evade":
                        paramValues[i] = source.GetStat(StatType.Evade);
                        break;

                    case "@exp":
                        paramValues[i] = source.Exp;
                        break;

                    case "@hp":
                        paramValues[i] = source.HP;
                        break;

                    case "@id":
                        paramValues[i] = source.ID;
                        break;

                    case "@imm":
                        paramValues[i] = source.GetStat(StatType.Imm);
                        break;

                    case "@int":
                        paramValues[i] = source.GetStat(StatType.Int);
                        break;

                    case "@level":
                        paramValues[i] = source.Level;
                        break;

                    case "@map_id":
                        paramValues[i] = source.MapID;
                        break;

                    case "@maxhit":
                        paramValues[i] = source.GetStat(StatType.MaxHit);
                        break;

                    case "@maxhp":
                        paramValues[i] = source.GetStat(StatType.MaxHP);
                        break;

                    case "@maxmp":
                        paramValues[i] = source.GetStat(StatType.MaxMP);
                        break;

                    case "@minhit":
                        paramValues[i] = source.GetStat(StatType.MinHit);
                        break;

                    case "@mp":
                        paramValues[i] = source.MP;
                        break;

                    case "@name":
                        paramValues[i] = source.Name;
                        break;

                    case "@password":
                        paramValues[i] = source.Password;
                        break;

                    case "@perc":
                        paramValues[i] = source.GetStat(StatType.Perc);
                        break;

                    case "@recov":
                        paramValues[i] = source.GetStat(StatType.Recov);
                        break;

                    case "@regen":
                        paramValues[i] = source.GetStat(StatType.Regen);
                        break;

                    case "@respawn_map":
                        paramValues[i] = source.RespawnMap;
                        break;

                    case "@respawn_x":
                        paramValues[i] = source.RespawnX;
                        break;

                    case "@respawn_y":
                        paramValues[i] = source.RespawnY;
                        break;

                    case "@statpoints":
                        paramValues[i] = source.StatPoints;
                        break;

                    case "@str":
                        paramValues[i] = source.GetStat(StatType.Str);
                        break;

                    case "@tact":
                        paramValues[i] = source.GetStat(StatType.Tact);
                        break;

                    case "@template_id":
                        paramValues[i] = source.TemplateId;
                        break;

                    case "@ws":
                        paramValues[i] = source.GetStat(StatType.WS);
                        break;

                    case "@x":
                        paramValues[i] = source.X;
                        break;

                    case "@y":
                        paramValues[i] = source.Y;
                        break;
                }
            }
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The key must already exist in the DbParameterValues
        /// for the value to be copied over. If any of the keys in the DbParameterValues do not
        /// match one of the column names, or if there is no field for a key, then it will be
        /// ignored. Because of this, it is important to be careful when using this method
        /// since columns or keys can be skipped without any indication.
        /// </summary>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public void TryCopyValues(DbParameterValues paramValues)
        {
            TryCopyValues(this, paramValues);
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. Unlike ReadValues(), this method not only doesn't require
        /// all values to be in the IDataReader, but also does not require the values in
        /// the IDataReader to be a defined field for the table this class represents.
        /// Because of this, you need to be careful when using this method because values
        /// can easily be skipped without any indication.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void TryReadValues(IDataReader dataReader)
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                switch (dataReader.GetName(i))
                {
                    case "acc":
                        SetStat(StatType.Acc, dataReader.GetByte(i));
                        break;

                    case "agi":
                        SetStat(StatType.Agi, dataReader.GetByte(i));
                        break;

                    case "armor":
                        SetStat(StatType.Armor, dataReader.GetByte(i));
                        break;

                    case "body":
                        Body = dataReader.GetUInt16(i);
                        break;

                    case "bra":
                        SetStat(StatType.Bra, dataReader.GetByte(i));
                        break;

                    case "cash":
                        Cash = dataReader.GetUInt32(i);
                        break;

                    case "defence":
                        SetStat(StatType.Defence, dataReader.GetByte(i));
                        break;

                    case "dex":
                        SetStat(StatType.Dex, dataReader.GetByte(i));
                        break;

                    case "evade":
                        SetStat(StatType.Evade, dataReader.GetByte(i));
                        break;

                    case "exp":
                        Exp = dataReader.GetUInt32(i);
                        break;

                    case "hp":
                        HP = dataReader.GetUInt16(i);
                        break;

                    case "id":
                        ID = (CharacterID)dataReader.GetInt32(i);
                        break;

                    case "imm":
                        SetStat(StatType.Imm, dataReader.GetByte(i));
                        break;

                    case "int":
                        SetStat(StatType.Int, dataReader.GetByte(i));
                        break;

                    case "level":
                        Level = dataReader.GetByte(i);
                        break;

                    case "map_id":
                        MapID = (MapIndex)dataReader.GetUInt16(i);
                        break;

                    case "maxhit":
                        SetStat(StatType.MaxHit, dataReader.GetByte(i));
                        break;

                    case "maxhp":
                        SetStat(StatType.MaxHP, dataReader.GetUInt16(i));
                        break;

                    case "maxmp":
                        SetStat(StatType.MaxMP, dataReader.GetUInt16(i));
                        break;

                    case "minhit":
                        SetStat(StatType.MinHit, dataReader.GetByte(i));
                        break;

                    case "mp":
                        MP = dataReader.GetUInt16(i);
                        break;

                    case "name":
                        Name = dataReader.GetString(i);
                        break;

                    case "password":
                        Password = dataReader.GetString(i);
                        break;

                    case "perc":
                        SetStat(StatType.Perc, dataReader.GetByte(i));
                        break;

                    case "recov":
                        SetStat(StatType.Recov, dataReader.GetByte(i));
                        break;

                    case "regen":
                        SetStat(StatType.Regen, dataReader.GetByte(i));
                        break;

                    case "respawn_map":
                        RespawnMap = (MapIndex)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "respawn_x":
                        RespawnX = dataReader.GetFloat(i);
                        break;

                    case "respawn_y":
                        RespawnY = dataReader.GetFloat(i);
                        break;

                    case "statpoints":
                        StatPoints = dataReader.GetUInt32(i);
                        break;

                    case "str":
                        SetStat(StatType.Str, dataReader.GetByte(i));
                        break;

                    case "tact":
                        SetStat(StatType.Tact, dataReader.GetByte(i));
                        break;

                    case "template_id":
                        TemplateId = (CharacterTemplateID)(dataReader.IsDBNull(i) ? (ushort?)null : dataReader.GetUInt16(i));
                        break;

                    case "ws":
                        SetStat(StatType.WS, dataReader.GetByte(i));
                        break;

                    case "x":
                        X = dataReader.GetFloat(i);
                        break;

                    case "y":
                        Y = dataReader.GetFloat(i);
                        break;
                }
            }
        }

        #region ICharacterTable Members

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
        /// Gets the <paramref name="value"/> of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
        /// </summary>
        /// <param name="key">The key of the column to get.</param>
        /// <param name="value">The value to assign to the column for the corresponding <paramref name="key"/>.</param>
        public void SetStat(StatType key, Int32 value)
        {
            _stat[key] = (Byte)value;
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `body`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
        /// </summary>
        public UInt16 Body
        {
            get { return _body; }
            set { _body = value; }
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
        /// The underlying database type is `smallint(5) unsigned` with the default value of `50`.
        /// </summary>
        public UInt16 HP
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
        /// The underlying database type is `smallint(5) unsigned` with the default value of `50`.
        /// </summary>
        public UInt16 MP
        {
            get { return _mP; }
            set { _mP = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `name`.
        /// The underlying database type is `varchar(50)`.
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `password`.
        /// The underlying database type is `varchar(50)`.
        /// </summary>
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_map`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public MapIndex RespawnMap
        {
            get { return (MapIndex)_respawnMap; }
            set { _respawnMap = (ushort?)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_x`.
        /// The underlying database type is `float`.
        /// </summary>
        public Single RespawnX
        {
            get { return _respawnX; }
            set { _respawnX = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn_y`.
        /// The underlying database type is `float`.
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
        /// Gets or sets the value for the field that maps onto the database column `template_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public CharacterTemplateID TemplateId
        {
            get { return (CharacterTemplateID)_templateId; }
            set { _templateId = (ushort?)value; }
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

        #endregion

        /// <summary>
        /// A Dictionary-like lookup table for the Enum values of the type collection `Stat` for the
        /// table that this class represents. Majority of the code for this class was automatically generated and
        /// only other automatically generated code should be using this class.
        /// </summary>
        class StatConstDictionary
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
        }
    }
}