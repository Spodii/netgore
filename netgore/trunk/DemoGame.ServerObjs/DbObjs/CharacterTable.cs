using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `character`.
    /// </summary>
    public interface ICharacterTable
    {
        /// <summary>
        /// Gets the value for the database column `body`.
        /// </summary>
        UInt16 Body { get; }

        /// <summary>
        /// Gets the value for the database column `cash`.
        /// </summary>
        UInt32 Cash { get; }

        /// <summary>
        /// Gets the value for the database column `exp`.
        /// </summary>
        UInt32 Exp { get; }

        /// <summary>
        /// Gets the value for the database column `hp`.
        /// </summary>
        UInt16 Hp { get; }

        /// <summary>
        /// Gets the value for the database column `id`.
        /// </summary>
        UInt32 Id { get; }

        /// <summary>
        /// Gets the value for the database column `level`.
        /// </summary>
        Byte Level { get; }

        /// <summary>
        /// Gets the value for the database column `map_id`.
        /// </summary>
        UInt16 MapId { get; }

        /// <summary>
        /// Gets the value for the database column `mp`.
        /// </summary>
        UInt16 Mp { get; }

        /// <summary>
        /// Gets the value for the database column `name`.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the value for the database column `password`.
        /// </summary>
        String Password { get; }

        /// <summary>
        /// Gets the value for the database column `respawn_map`.
        /// </summary>
        UInt16 RespawnMap { get; }

        /// <summary>
        /// Gets the value for the database column `respawn_x`.
        /// </summary>
        Single RespawnX { get; }

        /// <summary>
        /// Gets the value for the database column `respawn_y`.
        /// </summary>
        Single RespawnY { get; }

        /// <summary>
        /// Gets the value for the database column `statpoints`.
        /// </summary>
        UInt32 Statpoints { get; }

        /// <summary>
        /// Gets the value for the database column `template_id`.
        /// </summary>
        UInt16 TemplateId { get; }

        /// <summary>
        /// Gets the value for the database column `x`.
        /// </summary>
        Single X { get; }

        /// <summary>
        /// Gets the value for the database column `y`.
        /// </summary>
        Single Y { get; }

        Int32 GetStat(StatType key);

        Void SetStat(StatType key, Int32 value);
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
        UInt16 _hp;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        UInt32 _id;

        /// <summary>
        /// The field that maps onto the database column `level`.
        /// </summary>
        Byte _level;

        /// <summary>
        /// The field that maps onto the database column `map_id`.
        /// </summary>
        UInt16 _mapId;

        /// <summary>
        /// The field that maps onto the database column `mp`.
        /// </summary>
        UInt16 _mp;

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
        UInt16 _respawnMap;

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
        UInt32 _statpoints;

        /// <summary>
        /// The field that maps onto the database column `template_id`.
        /// </summary>
        UInt16 _templateId;

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
        public IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
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
        /// <param name="hp">The initial value for the corresponding property.</param>
        /// <param name="id">The initial value for the corresponding property.</param>
        /// <param name="imm">The initial value for the corresponding property.</param>
        /// <param name="int">The initial value for the corresponding property.</param>
        /// <param name="level">The initial value for the corresponding property.</param>
        /// <param name="mapId">The initial value for the corresponding property.</param>
        /// <param name="maxhit">The initial value for the corresponding property.</param>
        /// <param name="maxhp">The initial value for the corresponding property.</param>
        /// <param name="maxmp">The initial value for the corresponding property.</param>
        /// <param name="minhit">The initial value for the corresponding property.</param>
        /// <param name="mp">The initial value for the corresponding property.</param>
        /// <param name="name">The initial value for the corresponding property.</param>
        /// <param name="password">The initial value for the corresponding property.</param>
        /// <param name="perc">The initial value for the corresponding property.</param>
        /// <param name="recov">The initial value for the corresponding property.</param>
        /// <param name="regen">The initial value for the corresponding property.</param>
        /// <param name="respawnMap">The initial value for the corresponding property.</param>
        /// <param name="respawnX">The initial value for the corresponding property.</param>
        /// <param name="respawnY">The initial value for the corresponding property.</param>
        /// <param name="statpoints">The initial value for the corresponding property.</param>
        /// <param name="str">The initial value for the corresponding property.</param>
        /// <param name="tact">The initial value for the corresponding property.</param>
        /// <param name="templateId">The initial value for the corresponding property.</param>
        /// <param name="ws">The initial value for the corresponding property.</param>
        /// <param name="x">The initial value for the corresponding property.</param>
        /// <param name="y">The initial value for the corresponding property.</param>
        public CharacterTable(Byte @acc, Byte @agi, Byte @armor, UInt16 @body, Byte @bra, UInt32 @cash, Byte @defence, Byte @dex,
                              Byte @evade, UInt32 @exp, UInt16 @hp, UInt32 @id, Byte @imm, Byte @int, Byte @level, UInt16 @mapId,
                              Byte @maxhit, UInt16 @maxhp, UInt16 @maxmp, Byte @minhit, UInt16 @mp, String @name, String @password,
                              Byte @perc, Byte @recov, Byte @regen, UInt16 @respawnMap, Single @respawnX, Single @respawnY,
                              UInt32 @statpoints, Byte @str, Byte @tact, UInt16 @templateId, Byte @ws, Single @x, Single @y)
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
            Hp = @hp;
            Id = @id;
            SetStat(StatType.Imm, @imm);
            SetStat(StatType.Int, @int);
            Level = @level;
            MapId = @mapId;
            SetStat(StatType.MaxHit, @maxhit);
            SetStat(StatType.MaxHP, @maxhp);
            SetStat(StatType.MaxMP, @maxmp);
            SetStat(StatType.MinHit, @minhit);
            Mp = @mp;
            Name = @name;
            Password = @password;
            SetStat(StatType.Perc, @perc);
            SetStat(StatType.Recov, @recov);
            SetStat(StatType.Regen, @regen);
            RespawnMap = @respawnMap;
            RespawnX = @respawnX;
            RespawnY = @respawnY;
            Statpoints = @statpoints;
            SetStat(StatType.Str, @str);
            SetStat(StatType.Tact, @tact);
            TemplateId = @templateId;
            SetStat(StatType.WS, @ws);
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
        ///  this method will not create them if they are missing.
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
            dic["@hp"] = source.Hp;
            dic["@id"] = source.Id;
            dic["@imm"] = (Byte)source.GetStat(StatType.Imm);
            dic["@int"] = (Byte)source.GetStat(StatType.Int);
            dic["@level"] = source.Level;
            dic["@map_id"] = source.MapId;
            dic["@maxhit"] = (Byte)source.GetStat(StatType.MaxHit);
            dic["@maxhp"] = (UInt16)source.GetStat(StatType.MaxHP);
            dic["@maxmp"] = (UInt16)source.GetStat(StatType.MaxMP);
            dic["@minhit"] = (Byte)source.GetStat(StatType.MinHit);
            dic["@mp"] = source.Mp;
            dic["@name"] = source.Name;
            dic["@password"] = source.Password;
            dic["@perc"] = (Byte)source.GetStat(StatType.Perc);
            dic["@recov"] = (Byte)source.GetStat(StatType.Recov);
            dic["@regen"] = (Byte)source.GetStat(StatType.Regen);
            dic["@respawn_map"] = source.RespawnMap;
            dic["@respawn_x"] = source.RespawnX;
            dic["@respawn_y"] = source.RespawnY;
            dic["@statpoints"] = source.Statpoints;
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
            paramValues["@hp"] = source.Hp;
            paramValues["@id"] = source.Id;
            paramValues["@imm"] = (Byte)source.GetStat(StatType.Imm);
            paramValues["@int"] = (Byte)source.GetStat(StatType.Int);
            paramValues["@level"] = source.Level;
            paramValues["@map_id"] = source.MapId;
            paramValues["@maxhit"] = (Byte)source.GetStat(StatType.MaxHit);
            paramValues["@maxhp"] = (UInt16)source.GetStat(StatType.MaxHP);
            paramValues["@maxmp"] = (UInt16)source.GetStat(StatType.MaxMP);
            paramValues["@minhit"] = (Byte)source.GetStat(StatType.MinHit);
            paramValues["@mp"] = source.Mp;
            paramValues["@name"] = source.Name;
            paramValues["@password"] = source.Password;
            paramValues["@perc"] = (Byte)source.GetStat(StatType.Perc);
            paramValues["@recov"] = (Byte)source.GetStat(StatType.Recov);
            paramValues["@regen"] = (Byte)source.GetStat(StatType.Regen);
            paramValues["@respawn_map"] = source.RespawnMap;
            paramValues["@respawn_x"] = source.RespawnX;
            paramValues["@respawn_y"] = source.RespawnY;
            paramValues["@statpoints"] = source.Statpoints;
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
        ///  this method will not create them if they are missing.
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
            Hp = source.Hp;
            Id = source.Id;
            SetStat(StatType.Imm, source.GetStat(StatType.Imm));
            SetStat(StatType.Int, source.GetStat(StatType.Int));
            Level = source.Level;
            MapId = source.MapId;
            SetStat(StatType.MaxHit, source.GetStat(StatType.MaxHit));
            SetStat(StatType.MaxHP, source.GetStat(StatType.MaxHP));
            SetStat(StatType.MaxMP, source.GetStat(StatType.MaxMP));
            SetStat(StatType.MinHit, source.GetStat(StatType.MinHit));
            Mp = source.Mp;
            Name = source.Name;
            Password = source.Password;
            SetStat(StatType.Perc, source.GetStat(StatType.Perc));
            SetStat(StatType.Recov, source.GetStat(StatType.Recov));
            SetStat(StatType.Regen, source.GetStat(StatType.Regen));
            RespawnMap = source.RespawnMap;
            RespawnX = source.RespawnX;
            RespawnY = source.RespawnY;
            Statpoints = source.Statpoints;
            SetStat(StatType.Str, source.GetStat(StatType.Str));
            SetStat(StatType.Tact, source.GetStat(StatType.Tact));
            TemplateId = source.TemplateId;
            SetStat(StatType.WS, source.GetStat(StatType.WS));
            X = source.X;
            Y = source.Y;
        }

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void ReadValues(IDataReader dataReader)
        {
            SetStat(StatType.Acc, dataReader.GetByte(dataReader.GetOrdinal("acc")));
            SetStat(StatType.Agi, dataReader.GetByte(dataReader.GetOrdinal("agi")));
            SetStat(StatType.Armor, dataReader.GetByte(dataReader.GetOrdinal("armor")));
            Body = dataReader.GetUInt16(dataReader.GetOrdinal("body"));
            SetStat(StatType.Bra, dataReader.GetByte(dataReader.GetOrdinal("bra")));
            Cash = dataReader.GetUInt32(dataReader.GetOrdinal("cash"));
            SetStat(StatType.Defence, dataReader.GetByte(dataReader.GetOrdinal("defence")));
            SetStat(StatType.Dex, dataReader.GetByte(dataReader.GetOrdinal("dex")));
            SetStat(StatType.Evade, dataReader.GetByte(dataReader.GetOrdinal("evade")));
            Exp = dataReader.GetUInt32(dataReader.GetOrdinal("exp"));
            Hp = dataReader.GetUInt16(dataReader.GetOrdinal("hp"));
            Id = dataReader.GetUInt32(dataReader.GetOrdinal("id"));
            SetStat(StatType.Imm, dataReader.GetByte(dataReader.GetOrdinal("imm")));
            SetStat(StatType.Int, dataReader.GetByte(dataReader.GetOrdinal("int")));
            Level = dataReader.GetByte(dataReader.GetOrdinal("level"));
            MapId = dataReader.GetUInt16(dataReader.GetOrdinal("map_id"));
            SetStat(StatType.MaxHit, dataReader.GetByte(dataReader.GetOrdinal("maxhit")));
            SetStat(StatType.MaxHP, dataReader.GetUInt16(dataReader.GetOrdinal("maxhp")));
            SetStat(StatType.MaxMP, dataReader.GetUInt16(dataReader.GetOrdinal("maxmp")));
            SetStat(StatType.MinHit, dataReader.GetByte(dataReader.GetOrdinal("minhit")));
            Mp = dataReader.GetUInt16(dataReader.GetOrdinal("mp"));
            Name = dataReader.GetString(dataReader.GetOrdinal("name"));
            Password = dataReader.GetString(dataReader.GetOrdinal("password"));
            SetStat(StatType.Perc, dataReader.GetByte(dataReader.GetOrdinal("perc")));
            SetStat(StatType.Recov, dataReader.GetByte(dataReader.GetOrdinal("recov")));
            SetStat(StatType.Regen, dataReader.GetByte(dataReader.GetOrdinal("regen")));
            RespawnMap = dataReader.GetUInt16(dataReader.GetOrdinal("respawn_map"));
            RespawnX = dataReader.GetFloat(dataReader.GetOrdinal("respawn_x"));
            RespawnY = dataReader.GetFloat(dataReader.GetOrdinal("respawn_y"));
            Statpoints = dataReader.GetUInt32(dataReader.GetOrdinal("statpoints"));
            SetStat(StatType.Str, dataReader.GetByte(dataReader.GetOrdinal("str")));
            SetStat(StatType.Tact, dataReader.GetByte(dataReader.GetOrdinal("tact")));
            TemplateId = dataReader.GetUInt16(dataReader.GetOrdinal("template_id"));
            SetStat(StatType.WS, dataReader.GetByte(dataReader.GetOrdinal("ws")));
            X = dataReader.GetFloat(dataReader.GetOrdinal("x"));
            Y = dataReader.GetFloat(dataReader.GetOrdinal("y"));
        }

        #region ICharacterTable Members

        public Int32 GetStat(StatType key)
        {
            return _stat[key];
        }

        public void SetStat(StatType key, Int32 value)
        {
            _stat[key] = value;
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
        public UInt16 Hp
        {
            get { return _hp; }
            set { _hp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(10) unsigned`.
        /// </summary>
        public UInt32 Id
        {
            get { return _id; }
            set { _id = value; }
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
        public UInt16 MapId
        {
            get { return _mapId; }
            set { _mapId = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `mp`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `50`.
        /// </summary>
        public UInt16 Mp
        {
            get { return _mp; }
            set { _mp = value; }
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
        public UInt16 RespawnMap
        {
            get { return _respawnMap; }
            set { _respawnMap = value; }
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
        public UInt32 Statpoints
        {
            get { return _statpoints; }
            set { _statpoints = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `template_id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public UInt16 TemplateId
        {
            get { return _templateId; }
            set { _templateId = value; }
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