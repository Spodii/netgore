using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.Db;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Interface for a class that can be used to serialize values to the database table `character_template`.
    /// </summary>
    public interface ICharacterTemplateTable
    {
        /// <summary>
        /// Gets the value for the database column `acc`.
        /// </summary>
        Byte Acc { get; }

        /// <summary>
        /// Gets the value for the database column `agi`.
        /// </summary>
        Byte Agi { get; }

        /// <summary>
        /// Gets the value for the database column `ai`.
        /// </summary>
        String Ai { get; }

        /// <summary>
        /// Gets the value for the database column `alliance_id`.
        /// </summary>
        Byte AllianceId { get; }

        /// <summary>
        /// Gets the value for the database column `armor`.
        /// </summary>
        Byte Armor { get; }

        /// <summary>
        /// Gets the value for the database column `body`.
        /// </summary>
        UInt16 Body { get; }

        /// <summary>
        /// Gets the value for the database column `bra`.
        /// </summary>
        Byte Bra { get; }

        /// <summary>
        /// Gets the value for the database column `defence`.
        /// </summary>
        Byte Defence { get; }

        /// <summary>
        /// Gets the value for the database column `dex`.
        /// </summary>
        Byte Dex { get; }

        /// <summary>
        /// Gets the value for the database column `evade`.
        /// </summary>
        Byte Evade { get; }

        /// <summary>
        /// Gets the value for the database column `exp`.
        /// </summary>
        UInt32 Exp { get; }

        /// <summary>
        /// Gets the value for the database column `give_cash`.
        /// </summary>
        UInt16 GiveCash { get; }

        /// <summary>
        /// Gets the value for the database column `give_exp`.
        /// </summary>
        UInt16 GiveExp { get; }

        /// <summary>
        /// Gets the value for the database column `id`.
        /// </summary>
        UInt16 Id { get; }

        /// <summary>
        /// Gets the value for the database column `imm`.
        /// </summary>
        Byte Imm { get; }

        /// <summary>
        /// Gets the value for the database column `int`.
        /// </summary>
        Byte Int { get; }

        /// <summary>
        /// Gets the value for the database column `level`.
        /// </summary>
        Byte Level { get; }

        /// <summary>
        /// Gets the value for the database column `maxhit`.
        /// </summary>
        Byte Maxhit { get; }

        /// <summary>
        /// Gets the value for the database column `maxhp`.
        /// </summary>
        UInt16 Maxhp { get; }

        /// <summary>
        /// Gets the value for the database column `maxmp`.
        /// </summary>
        UInt16 Maxmp { get; }

        /// <summary>
        /// Gets the value for the database column `minhit`.
        /// </summary>
        Byte Minhit { get; }

        /// <summary>
        /// Gets the value for the database column `name`.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the value for the database column `perc`.
        /// </summary>
        Byte Perc { get; }

        /// <summary>
        /// Gets the value for the database column `recov`.
        /// </summary>
        Byte Recov { get; }

        /// <summary>
        /// Gets the value for the database column `regen`.
        /// </summary>
        Byte Regen { get; }

        /// <summary>
        /// Gets the value for the database column `respawn`.
        /// </summary>
        UInt16 Respawn { get; }

        /// <summary>
        /// Gets the value for the database column `statpoints`.
        /// </summary>
        UInt32 Statpoints { get; }

        /// <summary>
        /// Gets the value for the database column `str`.
        /// </summary>
        Byte Str { get; }

        /// <summary>
        /// Gets the value for the database column `tact`.
        /// </summary>
        Byte Tact { get; }

        /// <summary>
        /// Gets the value for the database column `ws`.
        /// </summary>
        Byte Ws { get; }
    }

    /// <summary>
    /// Provides a strongly-typed structure for the database table `character_template`.
    /// </summary>
    public class CharacterTemplateTable : ICharacterTemplateTable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 30;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "character_template";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
                                              {
                                                  "acc", "agi", "ai", "alliance_id", "armor", "body", "bra", "defence", "dex",
                                                  "evade", "exp", "give_cash", "give_exp", "id", "imm", "int", "level", "maxhit",
                                                  "maxhp", "maxmp", "minhit", "name", "perc", "recov", "regen", "respawn",
                                                  "statpoints", "str", "tact", "ws"
                                              };

        /// <summary>
        /// The field that maps onto the database column `acc`.
        /// </summary>
        Byte _acc;

        /// <summary>
        /// The field that maps onto the database column `agi`.
        /// </summary>
        Byte _agi;

        /// <summary>
        /// The field that maps onto the database column `ai`.
        /// </summary>
        String _ai;

        /// <summary>
        /// The field that maps onto the database column `alliance_id`.
        /// </summary>
        Byte _allianceId;

        /// <summary>
        /// The field that maps onto the database column `armor`.
        /// </summary>
        Byte _armor;

        /// <summary>
        /// The field that maps onto the database column `body`.
        /// </summary>
        UInt16 _body;

        /// <summary>
        /// The field that maps onto the database column `bra`.
        /// </summary>
        Byte _bra;

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
        UInt16 _id;

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
        /// The field that maps onto the database column `maxhit`.
        /// </summary>
        Byte _maxhit;

        /// <summary>
        /// The field that maps onto the database column `maxhp`.
        /// </summary>
        UInt16 _maxhp;

        /// <summary>
        /// The field that maps onto the database column `maxmp`.
        /// </summary>
        UInt16 _maxmp;

        /// <summary>
        /// The field that maps onto the database column `minhit`.
        /// </summary>
        Byte _minhit;

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
        /// The field that maps onto the database column `respawn`.
        /// </summary>
        UInt16 _respawn;

        /// <summary>
        /// The field that maps onto the database column `statpoints`.
        /// </summary>
        UInt32 _statpoints;

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
        Byte _ws;

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
        }

        /// <summary>
        /// CharacterTemplateTable constructor.
        /// </summary>
        public CharacterTemplateTable()
        {
        }

        /// <summary>
        /// CharacterTemplateTable constructor.
        /// </summary>
        /// <param name="acc">The initial value for the corresponding property.</param>
        /// <param name="agi">The initial value for the corresponding property.</param>
        /// <param name="ai">The initial value for the corresponding property.</param>
        /// <param name="allianceId">The initial value for the corresponding property.</param>
        /// <param name="armor">The initial value for the corresponding property.</param>
        /// <param name="body">The initial value for the corresponding property.</param>
        /// <param name="bra">The initial value for the corresponding property.</param>
        /// <param name="defence">The initial value for the corresponding property.</param>
        /// <param name="dex">The initial value for the corresponding property.</param>
        /// <param name="evade">The initial value for the corresponding property.</param>
        /// <param name="exp">The initial value for the corresponding property.</param>
        /// <param name="giveCash">The initial value for the corresponding property.</param>
        /// <param name="giveExp">The initial value for the corresponding property.</param>
        /// <param name="id">The initial value for the corresponding property.</param>
        /// <param name="imm">The initial value for the corresponding property.</param>
        /// <param name="int">The initial value for the corresponding property.</param>
        /// <param name="level">The initial value for the corresponding property.</param>
        /// <param name="maxhit">The initial value for the corresponding property.</param>
        /// <param name="maxhp">The initial value for the corresponding property.</param>
        /// <param name="maxmp">The initial value for the corresponding property.</param>
        /// <param name="minhit">The initial value for the corresponding property.</param>
        /// <param name="name">The initial value for the corresponding property.</param>
        /// <param name="perc">The initial value for the corresponding property.</param>
        /// <param name="recov">The initial value for the corresponding property.</param>
        /// <param name="regen">The initial value for the corresponding property.</param>
        /// <param name="respawn">The initial value for the corresponding property.</param>
        /// <param name="statpoints">The initial value for the corresponding property.</param>
        /// <param name="str">The initial value for the corresponding property.</param>
        /// <param name="tact">The initial value for the corresponding property.</param>
        /// <param name="ws">The initial value for the corresponding property.</param>
        public CharacterTemplateTable(Byte @acc, Byte @agi, String @ai, Byte @allianceId, Byte @armor, UInt16 @body, Byte @bra,
                                      Byte @defence, Byte @dex, Byte @evade, UInt32 @exp, UInt16 @giveCash, UInt16 @giveExp,
                                      UInt16 @id, Byte @imm, Byte @int, Byte @level, Byte @maxhit, UInt16 @maxhp, UInt16 @maxmp,
                                      Byte @minhit, String @name, Byte @perc, Byte @recov, Byte @regen, UInt16 @respawn,
                                      UInt32 @statpoints, Byte @str, Byte @tact, Byte @ws)
        {
            Acc = @acc;
            Agi = @agi;
            Ai = @ai;
            AllianceId = @allianceId;
            Armor = @armor;
            Body = @body;
            Bra = @bra;
            Defence = @defence;
            Dex = @dex;
            Evade = @evade;
            Exp = @exp;
            GiveCash = @giveCash;
            GiveExp = @giveExp;
            Id = @id;
            Imm = @imm;
            Int = @int;
            Level = @level;
            Maxhit = @maxhit;
            Maxhp = @maxhp;
            Maxmp = @maxmp;
            Minhit = @minhit;
            Name = @name;
            Perc = @perc;
            Recov = @recov;
            Regen = @regen;
            Respawn = @respawn;
            Statpoints = @statpoints;
            Str = @str;
            Tact = @tact;
            Ws = @ws;
        }

        /// <summary>
        /// CharacterTemplateTable constructor.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
        public CharacterTemplateTable(IDataReader dataReader)
        {
            ReadValues(dataReader);
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(ICharacterTemplateTable source, IDictionary<String, Object> dic)
        {
            dic["@acc"] = source.Acc;
            dic["@agi"] = source.Agi;
            dic["@ai"] = source.Ai;
            dic["@alliance_id"] = source.AllianceId;
            dic["@armor"] = source.Armor;
            dic["@body"] = source.Body;
            dic["@bra"] = source.Bra;
            dic["@defence"] = source.Defence;
            dic["@dex"] = source.Dex;
            dic["@evade"] = source.Evade;
            dic["@exp"] = source.Exp;
            dic["@give_cash"] = source.GiveCash;
            dic["@give_exp"] = source.GiveExp;
            dic["@id"] = source.Id;
            dic["@imm"] = source.Imm;
            dic["@int"] = source.Int;
            dic["@level"] = source.Level;
            dic["@maxhit"] = source.Maxhit;
            dic["@maxhp"] = source.Maxhp;
            dic["@maxmp"] = source.Maxmp;
            dic["@minhit"] = source.Minhit;
            dic["@name"] = source.Name;
            dic["@perc"] = source.Perc;
            dic["@recov"] = source.Recov;
            dic["@regen"] = source.Regen;
            dic["@respawn"] = source.Respawn;
            dic["@statpoints"] = source.Statpoints;
            dic["@str"] = source.Str;
            dic["@tact"] = source.Tact;
            dic["@ws"] = source.Ws;
        }

        /// <summary>
        /// Copies the column values into the given DbParameterValues using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="paramValues">The DbParameterValues to copy the values into.</param>
        public static void CopyValues(ICharacterTemplateTable source, DbParameterValues paramValues)
        {
            paramValues["@acc"] = source.Acc;
            paramValues["@agi"] = source.Agi;
            paramValues["@ai"] = source.Ai;
            paramValues["@alliance_id"] = source.AllianceId;
            paramValues["@armor"] = source.Armor;
            paramValues["@body"] = source.Body;
            paramValues["@bra"] = source.Bra;
            paramValues["@defence"] = source.Defence;
            paramValues["@dex"] = source.Dex;
            paramValues["@evade"] = source.Evade;
            paramValues["@exp"] = source.Exp;
            paramValues["@give_cash"] = source.GiveCash;
            paramValues["@give_exp"] = source.GiveExp;
            paramValues["@id"] = source.Id;
            paramValues["@imm"] = source.Imm;
            paramValues["@int"] = source.Int;
            paramValues["@level"] = source.Level;
            paramValues["@maxhit"] = source.Maxhit;
            paramValues["@maxhp"] = source.Maxhp;
            paramValues["@maxmp"] = source.Maxmp;
            paramValues["@minhit"] = source.Minhit;
            paramValues["@name"] = source.Name;
            paramValues["@perc"] = source.Perc;
            paramValues["@recov"] = source.Recov;
            paramValues["@regen"] = source.Regen;
            paramValues["@respawn"] = source.Respawn;
            paramValues["@statpoints"] = source.Statpoints;
            paramValues["@str"] = source.Str;
            paramValues["@tact"] = source.Tact;
            paramValues["@ws"] = source.Ws;
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

        /// <summary>
        /// Reads the values from an IDataReader and assigns the read values to this
        /// object's properties. The database column's name is used to as the key, so the value
        /// will not be found if any aliases are used or not all columns were selected.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
        public void ReadValues(IDataReader dataReader)
        {
            Acc = (Byte)dataReader.GetValue(dataReader.GetOrdinal("acc"));
            Agi = (Byte)dataReader.GetValue(dataReader.GetOrdinal("agi"));
            Ai = (String)dataReader.GetValue(dataReader.GetOrdinal("ai"));
            AllianceId = (Byte)dataReader.GetValue(dataReader.GetOrdinal("alliance_id"));
            Armor = (Byte)dataReader.GetValue(dataReader.GetOrdinal("armor"));
            Body = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("body"));
            Bra = (Byte)dataReader.GetValue(dataReader.GetOrdinal("bra"));
            Defence = (Byte)dataReader.GetValue(dataReader.GetOrdinal("defence"));
            Dex = (Byte)dataReader.GetValue(dataReader.GetOrdinal("dex"));
            Evade = (Byte)dataReader.GetValue(dataReader.GetOrdinal("evade"));
            Exp = (UInt32)dataReader.GetValue(dataReader.GetOrdinal("exp"));
            GiveCash = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("give_cash"));
            GiveExp = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("give_exp"));
            Id = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("id"));
            Imm = (Byte)dataReader.GetValue(dataReader.GetOrdinal("imm"));
            Int = (Byte)dataReader.GetValue(dataReader.GetOrdinal("int"));
            Level = (Byte)dataReader.GetValue(dataReader.GetOrdinal("level"));
            Maxhit = (Byte)dataReader.GetValue(dataReader.GetOrdinal("maxhit"));
            Maxhp = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("maxhp"));
            Maxmp = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("maxmp"));
            Minhit = (Byte)dataReader.GetValue(dataReader.GetOrdinal("minhit"));
            Name = (String)dataReader.GetValue(dataReader.GetOrdinal("name"));
            Perc = (Byte)dataReader.GetValue(dataReader.GetOrdinal("perc"));
            Recov = (Byte)dataReader.GetValue(dataReader.GetOrdinal("recov"));
            Regen = (Byte)dataReader.GetValue(dataReader.GetOrdinal("regen"));
            Respawn = (UInt16)dataReader.GetValue(dataReader.GetOrdinal("respawn"));
            Statpoints = (UInt32)dataReader.GetValue(dataReader.GetOrdinal("statpoints"));
            Str = (Byte)dataReader.GetValue(dataReader.GetOrdinal("str"));
            Tact = (Byte)dataReader.GetValue(dataReader.GetOrdinal("tact"));
            Ws = (Byte)dataReader.GetValue(dataReader.GetOrdinal("ws"));
        }

        #region ICharacterTemplateTable Members

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
        /// Gets or sets the value for the field that maps onto the database column `agi`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Agi
        {
            get { return _agi; }
            set { _agi = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `ai`.
        /// The underlying database type is `varchar(255)`.
        /// </summary>
        public String Ai
        {
            get { return _ai; }
            set { _ai = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `alliance_id`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public Byte AllianceId
        {
            get { return _allianceId; }
            set { _allianceId = value; }
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
        /// Gets or sets the value for the field that maps onto the database column `body`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
        /// </summary>
        public UInt16 Body
        {
            get { return _body; }
            set { _body = value; }
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
        /// The underlying database type is `int(10) unsigned`.
        /// </summary>
        public UInt32 Exp
        {
            get { return _exp; }
            set { _exp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `give_cash`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 GiveCash
        {
            get { return _giveCash; }
            set { _giveCash = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `give_exp`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 GiveExp
        {
            get { return _giveExp; }
            set { _giveExp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public UInt16 Id
        {
            get { return _id; }
            set { _id = value; }
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
        /// Gets or sets the value for the field that maps onto the database column `maxhit`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `2`.
        /// </summary>
        public Byte Maxhit
        {
            get { return _maxhit; }
            set { _maxhit = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `maxhp`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `50`.
        /// </summary>
        public UInt16 Maxhp
        {
            get { return _maxhp; }
            set { _maxhp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `maxmp`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `50`.
        /// </summary>
        public UInt16 Maxmp
        {
            get { return _maxmp; }
            set { _maxmp = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `minhit`.
        /// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
        /// </summary>
        public Byte Minhit
        {
            get { return _minhit; }
            set { _minhit = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `name`.
        /// The underlying database type is `varchar(50)` with the default value of `New NPC`.
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
        /// Gets or sets the value for the field that maps onto the database column `respawn`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `5`.
        /// </summary>
        public UInt16 Respawn
        {
            get { return _respawn; }
            set { _respawn = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `statpoints`.
        /// The underlying database type is `int(10) unsigned`.
        /// </summary>
        public UInt32 Statpoints
        {
            get { return _statpoints; }
            set { _statpoints = value; }
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
        public Byte Ws
        {
            get { return _ws; }
            set { _ws = value; }
        }

        #endregion
    }
}