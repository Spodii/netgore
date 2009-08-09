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
        /// Gets the value of the database column `ai`.
        /// </summary>
        String AI { get; }

        /// <summary>
        /// Gets the value of the database column `alliance_id`.
        /// </summary>
        AllianceID AllianceID { get; }

        /// <summary>
        /// Gets the value of the database column `body`.
        /// </summary>
        UInt16 Body { get; }

        /// <summary>
        /// Gets the value of the database column `exp`.
        /// </summary>
        UInt32 Exp { get; }

        /// <summary>
        /// Gets the value of the database column `give_cash`.
        /// </summary>
        UInt16 GiveCash { get; }

        /// <summary>
        /// Gets the value of the database column `give_exp`.
        /// </summary>
        UInt16 GiveExp { get; }

        /// <summary>
        /// Gets the value of the database column `id`.
        /// </summary>
        UInt16 ID { get; }

        /// <summary>
        /// Gets the value of the database column `level`.
        /// </summary>
        Byte Level { get; }

        /// <summary>
        /// Gets the value of the database column `name`.
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Gets the value of the database column `respawn`.
        /// </summary>
        UInt16 Respawn { get; }

        /// <summary>
        /// Gets the value of the database column `statpoints`.
        /// </summary>
        UInt32 StatPoints { get; }

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
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
                                                    {
                                                        "acc", "agi", "ai", "alliance_id", "armor", "body", "bra", "defence", "dex",
                                                        "evade", "exp", "give_cash", "give_exp", "imm", "int", "level", "maxhit",
                                                        "maxhp", "maxmp", "minhit", "name", "perc", "recov", "regen", "respawn",
                                                        "statpoints", "str", "tact", "ws"
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
        /// The field that maps onto the database column `ai`.
        /// </summary>
        String _aI;

        /// <summary>
        /// The field that maps onto the database column `alliance_id`.
        /// </summary>
        Byte _allianceID;

        /// <summary>
        /// The field that maps onto the database column `body`.
        /// </summary>
        UInt16 _body;

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
        UInt16 _iD;

        /// <summary>
        /// The field that maps onto the database column `level`.
        /// </summary>
        Byte _level;

        /// <summary>
        /// The field that maps onto the database column `name`.
        /// </summary>
        String _name;

        /// <summary>
        /// The field that maps onto the database column `respawn`.
        /// </summary>
        UInt16 _respawn;

        /// <summary>
        /// The field that maps onto the database column `statpoints`.
        /// </summary>
        UInt32 _statPoints;

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
        /// <param name="aI">The initial value for the corresponding property.</param>
        /// <param name="allianceID">The initial value for the corresponding property.</param>
        /// <param name="armor">The initial value for the corresponding property.</param>
        /// <param name="body">The initial value for the corresponding property.</param>
        /// <param name="bra">The initial value for the corresponding property.</param>
        /// <param name="defence">The initial value for the corresponding property.</param>
        /// <param name="dex">The initial value for the corresponding property.</param>
        /// <param name="evade">The initial value for the corresponding property.</param>
        /// <param name="exp">The initial value for the corresponding property.</param>
        /// <param name="giveCash">The initial value for the corresponding property.</param>
        /// <param name="giveExp">The initial value for the corresponding property.</param>
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="imm">The initial value for the corresponding property.</param>
        /// <param name="int">The initial value for the corresponding property.</param>
        /// <param name="level">The initial value for the corresponding property.</param>
        /// <param name="maxHit">The initial value for the corresponding property.</param>
        /// <param name="maxHP">The initial value for the corresponding property.</param>
        /// <param name="maxMP">The initial value for the corresponding property.</param>
        /// <param name="minHit">The initial value for the corresponding property.</param>
        /// <param name="name">The initial value for the corresponding property.</param>
        /// <param name="perc">The initial value for the corresponding property.</param>
        /// <param name="recov">The initial value for the corresponding property.</param>
        /// <param name="regen">The initial value for the corresponding property.</param>
        /// <param name="respawn">The initial value for the corresponding property.</param>
        /// <param name="statPoints">The initial value for the corresponding property.</param>
        /// <param name="str">The initial value for the corresponding property.</param>
        /// <param name="tact">The initial value for the corresponding property.</param>
        /// <param name="wS">The initial value for the corresponding property.</param>
        public CharacterTemplateTable(Byte @acc, Byte @agi, String @aI, AllianceID @allianceID, Byte @armor, UInt16 @body,
                                      Byte @bra, Byte @defence, Byte @dex, Byte @evade, UInt32 @exp, UInt16 @giveCash,
                                      UInt16 @giveExp, UInt16 @iD, Byte @imm, Byte @int, Byte @level, Byte @maxHit, UInt16 @maxHP,
                                      UInt16 @maxMP, Byte @minHit, String @name, Byte @perc, Byte @recov, Byte @regen,
                                      UInt16 @respawn, UInt32 @statPoints, Byte @str, Byte @tact, Byte @wS)
        {
            SetStat(StatType.Acc, @acc);
            SetStat(StatType.Agi, @agi);
            AI = @aI;
            AllianceID = @allianceID;
            SetStat(StatType.Armor, @armor);
            Body = @body;
            SetStat(StatType.Bra, @bra);
            SetStat(StatType.Defence, @defence);
            SetStat(StatType.Dex, @dex);
            SetStat(StatType.Evade, @evade);
            Exp = @exp;
            GiveCash = @giveCash;
            GiveExp = @giveExp;
            ID = @iD;
            SetStat(StatType.Imm, @imm);
            SetStat(StatType.Int, @int);
            Level = @level;
            SetStat(StatType.MaxHit, @maxHit);
            SetStat(StatType.MaxHP, @maxHP);
            SetStat(StatType.MaxMP, @maxMP);
            SetStat(StatType.MinHit, @minHit);
            Name = @name;
            SetStat(StatType.Perc, @perc);
            SetStat(StatType.Recov, @recov);
            SetStat(StatType.Regen, @regen);
            Respawn = @respawn;
            StatPoints = @statPoints;
            SetStat(StatType.Str, @str);
            SetStat(StatType.Tact, @tact);
            SetStat(StatType.WS, @wS);
        }

        /// <summary>
        /// CharacterTemplateTable constructor.
        /// </summary>
        /// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
        public CharacterTemplateTable(IDataReader dataReader)
        {
            ReadValues(dataReader);
        }

        public CharacterTemplateTable(ICharacterTemplateTable source)
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
        public static void CopyValues(ICharacterTemplateTable source, IDictionary<String, Object> dic)
        {
            dic["@acc"] = (Byte)source.GetStat(StatType.Acc);
            dic["@agi"] = (Byte)source.GetStat(StatType.Agi);
            dic["@ai"] = source.AI;
            dic["@alliance_id"] = source.AllianceID;
            dic["@armor"] = (Byte)source.GetStat(StatType.Armor);
            dic["@body"] = source.Body;
            dic["@bra"] = (Byte)source.GetStat(StatType.Bra);
            dic["@defence"] = (Byte)source.GetStat(StatType.Defence);
            dic["@dex"] = (Byte)source.GetStat(StatType.Dex);
            dic["@evade"] = (Byte)source.GetStat(StatType.Evade);
            dic["@exp"] = source.Exp;
            dic["@give_cash"] = source.GiveCash;
            dic["@give_exp"] = source.GiveExp;
            dic["@id"] = source.ID;
            dic["@imm"] = (Byte)source.GetStat(StatType.Imm);
            dic["@int"] = (Byte)source.GetStat(StatType.Int);
            dic["@level"] = source.Level;
            dic["@maxhit"] = (Byte)source.GetStat(StatType.MaxHit);
            dic["@maxhp"] = (UInt16)source.GetStat(StatType.MaxHP);
            dic["@maxmp"] = (UInt16)source.GetStat(StatType.MaxMP);
            dic["@minhit"] = (Byte)source.GetStat(StatType.MinHit);
            dic["@name"] = source.Name;
            dic["@perc"] = (Byte)source.GetStat(StatType.Perc);
            dic["@recov"] = (Byte)source.GetStat(StatType.Recov);
            dic["@regen"] = (Byte)source.GetStat(StatType.Regen);
            dic["@respawn"] = source.Respawn;
            dic["@statpoints"] = source.StatPoints;
            dic["@str"] = (Byte)source.GetStat(StatType.Str);
            dic["@tact"] = (Byte)source.GetStat(StatType.Tact);
            dic["@ws"] = (Byte)source.GetStat(StatType.WS);
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
            paramValues["@acc"] = (Byte)source.GetStat(StatType.Acc);
            paramValues["@agi"] = (Byte)source.GetStat(StatType.Agi);
            paramValues["@ai"] = source.AI;
            paramValues["@alliance_id"] = source.AllianceID;
            paramValues["@armor"] = (Byte)source.GetStat(StatType.Armor);
            paramValues["@body"] = source.Body;
            paramValues["@bra"] = (Byte)source.GetStat(StatType.Bra);
            paramValues["@defence"] = (Byte)source.GetStat(StatType.Defence);
            paramValues["@dex"] = (Byte)source.GetStat(StatType.Dex);
            paramValues["@evade"] = (Byte)source.GetStat(StatType.Evade);
            paramValues["@exp"] = source.Exp;
            paramValues["@give_cash"] = source.GiveCash;
            paramValues["@give_exp"] = source.GiveExp;
            paramValues["@id"] = source.ID;
            paramValues["@imm"] = (Byte)source.GetStat(StatType.Imm);
            paramValues["@int"] = (Byte)source.GetStat(StatType.Int);
            paramValues["@level"] = source.Level;
            paramValues["@maxhit"] = (Byte)source.GetStat(StatType.MaxHit);
            paramValues["@maxhp"] = (UInt16)source.GetStat(StatType.MaxHP);
            paramValues["@maxmp"] = (UInt16)source.GetStat(StatType.MaxMP);
            paramValues["@minhit"] = (Byte)source.GetStat(StatType.MinHit);
            paramValues["@name"] = source.Name;
            paramValues["@perc"] = (Byte)source.GetStat(StatType.Perc);
            paramValues["@recov"] = (Byte)source.GetStat(StatType.Recov);
            paramValues["@regen"] = (Byte)source.GetStat(StatType.Regen);
            paramValues["@respawn"] = source.Respawn;
            paramValues["@statpoints"] = source.StatPoints;
            paramValues["@str"] = (Byte)source.GetStat(StatType.Str);
            paramValues["@tact"] = (Byte)source.GetStat(StatType.Tact);
            paramValues["@ws"] = (Byte)source.GetStat(StatType.WS);
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

        public void CopyValuesFrom(ICharacterTemplateTable source)
        {
            SetStat(StatType.Acc, source.GetStat(StatType.Acc));
            SetStat(StatType.Agi, source.GetStat(StatType.Agi));
            AI = source.AI;
            AllianceID = source.AllianceID;
            SetStat(StatType.Armor, source.GetStat(StatType.Armor));
            Body = source.Body;
            SetStat(StatType.Bra, source.GetStat(StatType.Bra));
            SetStat(StatType.Defence, source.GetStat(StatType.Defence));
            SetStat(StatType.Dex, source.GetStat(StatType.Dex));
            SetStat(StatType.Evade, source.GetStat(StatType.Evade));
            Exp = source.Exp;
            GiveCash = source.GiveCash;
            GiveExp = source.GiveExp;
            ID = source.ID;
            SetStat(StatType.Imm, source.GetStat(StatType.Imm));
            SetStat(StatType.Int, source.GetStat(StatType.Int));
            Level = source.Level;
            SetStat(StatType.MaxHit, source.GetStat(StatType.MaxHit));
            SetStat(StatType.MaxHP, source.GetStat(StatType.MaxHP));
            SetStat(StatType.MaxMP, source.GetStat(StatType.MaxMP));
            SetStat(StatType.MinHit, source.GetStat(StatType.MinHit));
            Name = source.Name;
            SetStat(StatType.Perc, source.GetStat(StatType.Perc));
            SetStat(StatType.Recov, source.GetStat(StatType.Recov));
            SetStat(StatType.Regen, source.GetStat(StatType.Regen));
            Respawn = source.Respawn;
            StatPoints = source.StatPoints;
            SetStat(StatType.Str, source.GetStat(StatType.Str));
            SetStat(StatType.Tact, source.GetStat(StatType.Tact));
            SetStat(StatType.WS, source.GetStat(StatType.WS));
        }

        public static ColumnMetadata GetColumnData(String fieldName)
        {
            switch (fieldName)
            {
                case "acc":
                    return new ColumnMetadata("acc", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "agi":
                    return new ColumnMetadata("agi", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "ai":
                    return new ColumnMetadata("ai", "", "varchar(255)", null, typeof(String), false, false, false);

                case "alliance_id":
                    return new ColumnMetadata("alliance_id", "", "tinyint(3) unsigned", null, typeof(Byte), false, false, true);

                case "armor":
                    return new ColumnMetadata("armor", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "body":
                    return new ColumnMetadata("body", "", "smallint(5) unsigned", "1", typeof(UInt16), false, false, false);

                case "bra":
                    return new ColumnMetadata("bra", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "defence":
                    return new ColumnMetadata("defence", "", "tinyint(3) unsigned", "0", typeof(Byte), false, false, false);

                case "dex":
                    return new ColumnMetadata("dex", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "evade":
                    return new ColumnMetadata("evade", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "exp":
                    return new ColumnMetadata("exp", "", "int(10) unsigned", null, typeof(UInt32), false, false, false);

                case "give_cash":
                    return new ColumnMetadata("give_cash", "", "smallint(5) unsigned", "0", typeof(UInt16), false, false, false);

                case "give_exp":
                    return new ColumnMetadata("give_exp", "", "smallint(5) unsigned", "0", typeof(UInt16), false, false, false);

                case "id":
                    return new ColumnMetadata("id", "", "smallint(5) unsigned", null, typeof(UInt16), false, true, false);

                case "imm":
                    return new ColumnMetadata("imm", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "int":
                    return new ColumnMetadata("int", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "level":
                    return new ColumnMetadata("level", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "maxhit":
                    return new ColumnMetadata("maxhit", "", "tinyint(3) unsigned", "2", typeof(Byte), false, false, false);

                case "maxhp":
                    return new ColumnMetadata("maxhp", "", "smallint(5) unsigned", "50", typeof(UInt16), false, false, false);

                case "maxmp":
                    return new ColumnMetadata("maxmp", "", "smallint(5) unsigned", "50", typeof(UInt16), false, false, false);

                case "minhit":
                    return new ColumnMetadata("minhit", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "name":
                    return new ColumnMetadata("name", "", "varchar(50)", "New NPC", typeof(String), false, false, false);

                case "perc":
                    return new ColumnMetadata("perc", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "recov":
                    return new ColumnMetadata("recov", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "regen":
                    return new ColumnMetadata("regen", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "respawn":
                    return new ColumnMetadata("respawn", "", "smallint(5) unsigned", "5", typeof(UInt16), false, false, false);

                case "statpoints":
                    return new ColumnMetadata("statpoints", "", "int(10) unsigned", null, typeof(UInt32), false, false, false);

                case "str":
                    return new ColumnMetadata("str", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "tact":
                    return new ColumnMetadata("tact", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

                case "ws":
                    return new ColumnMetadata("ws", "", "tinyint(3) unsigned", "1", typeof(Byte), false, false, false);

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

                case "ai":
                    return AI;

                case "alliance_id":
                    return AllianceID;

                case "armor":
                    return GetStat(StatType.Armor);

                case "body":
                    return Body;

                case "bra":
                    return GetStat(StatType.Bra);

                case "defence":
                    return GetStat(StatType.Defence);

                case "dex":
                    return GetStat(StatType.Dex);

                case "evade":
                    return GetStat(StatType.Evade);

                case "exp":
                    return Exp;

                case "give_cash":
                    return GiveCash;

                case "give_exp":
                    return GiveExp;

                case "id":
                    return ID;

                case "imm":
                    return GetStat(StatType.Imm);

                case "int":
                    return GetStat(StatType.Int);

                case "level":
                    return Level;

                case "maxhit":
                    return GetStat(StatType.MaxHit);

                case "maxhp":
                    return GetStat(StatType.MaxHP);

                case "maxmp":
                    return GetStat(StatType.MaxMP);

                case "minhit":
                    return GetStat(StatType.MinHit);

                case "name":
                    return Name;

                case "perc":
                    return GetStat(StatType.Perc);

                case "recov":
                    return GetStat(StatType.Recov);

                case "regen":
                    return GetStat(StatType.Regen);

                case "respawn":
                    return Respawn;

                case "statpoints":
                    return StatPoints;

                case "str":
                    return GetStat(StatType.Str);

                case "tact":
                    return GetStat(StatType.Tact);

                case "ws":
                    return GetStat(StatType.WS);

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

            i = dataReader.GetOrdinal("ai");
            AI = dataReader.GetString(i);

            i = dataReader.GetOrdinal("alliance_id");
            AllianceID = (AllianceID)dataReader.GetByte(i);

            i = dataReader.GetOrdinal("armor");
            SetStat(StatType.Armor, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("body");
            Body = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("bra");
            SetStat(StatType.Bra, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("defence");
            SetStat(StatType.Defence, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("dex");
            SetStat(StatType.Dex, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("evade");
            SetStat(StatType.Evade, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("exp");
            Exp = dataReader.GetUInt32(i);

            i = dataReader.GetOrdinal("give_cash");
            GiveCash = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("give_exp");
            GiveExp = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("id");
            ID = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("imm");
            SetStat(StatType.Imm, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("int");
            SetStat(StatType.Int, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("level");
            Level = dataReader.GetByte(i);

            i = dataReader.GetOrdinal("maxhit");
            SetStat(StatType.MaxHit, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("maxhp");
            SetStat(StatType.MaxHP, dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("maxmp");
            SetStat(StatType.MaxMP, dataReader.GetUInt16(i));

            i = dataReader.GetOrdinal("minhit");
            SetStat(StatType.MinHit, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("name");
            Name = dataReader.GetString(i);

            i = dataReader.GetOrdinal("perc");
            SetStat(StatType.Perc, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("recov");
            SetStat(StatType.Recov, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("regen");
            SetStat(StatType.Regen, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("respawn");
            Respawn = dataReader.GetUInt16(i);

            i = dataReader.GetOrdinal("statpoints");
            StatPoints = dataReader.GetUInt32(i);

            i = dataReader.GetOrdinal("str");
            SetStat(StatType.Str, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("tact");
            SetStat(StatType.Tact, dataReader.GetByte(i));

            i = dataReader.GetOrdinal("ws");
            SetStat(StatType.WS, dataReader.GetByte(i));
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

                case "ai":
                    AI = (String)value;
                    break;

                case "alliance_id":
                    AllianceID = (AllianceID)value;
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

                case "give_cash":
                    GiveCash = (UInt16)value;
                    break;

                case "give_exp":
                    GiveExp = (UInt16)value;
                    break;

                case "id":
                    ID = (UInt16)value;
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

                case "respawn":
                    Respawn = (UInt16)value;
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

                case "ws":
                    SetStat(StatType.WS, (Int32)value);
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
        public static void TryCopyValues(ICharacterTemplateTable source, DbParameterValues paramValues)
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

                    case "@ai":
                        paramValues[i] = source.AI;
                        break;

                    case "@alliance_id":
                        paramValues[i] = source.AllianceID;
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

                    case "@give_cash":
                        paramValues[i] = source.GiveCash;
                        break;

                    case "@give_exp":
                        paramValues[i] = source.GiveExp;
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

                    case "@name":
                        paramValues[i] = source.Name;
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

                    case "@respawn":
                        paramValues[i] = source.Respawn;
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

                    case "@ws":
                        paramValues[i] = source.GetStat(StatType.WS);
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

                    case "ai":
                        AI = dataReader.GetString(i);
                        break;

                    case "alliance_id":
                        AllianceID = (AllianceID)dataReader.GetByte(i);
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

                    case "give_cash":
                        GiveCash = dataReader.GetUInt16(i);
                        break;

                    case "give_exp":
                        GiveExp = dataReader.GetUInt16(i);
                        break;

                    case "id":
                        ID = dataReader.GetUInt16(i);
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

                    case "name":
                        Name = dataReader.GetString(i);
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

                    case "respawn":
                        Respawn = dataReader.GetUInt16(i);
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

                    case "ws":
                        SetStat(StatType.WS, dataReader.GetByte(i));
                        break;
                }
            }
        }

        #region ICharacterTemplateTable Members

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
        /// Gets or sets the value for the field that maps onto the database column `ai`.
        /// The underlying database type is `varchar(255)`.
        /// </summary>
        public String AI
        {
            get { return _aI; }
            set { _aI = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `alliance_id`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public AllianceID AllianceID
        {
            get { return (AllianceID)_allianceID; }
            set { _allianceID = (Byte)value; }
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
        public UInt16 ID
        {
            get { return _iD; }
            set { _iD = value; }
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
        /// Gets or sets the value for the field that maps onto the database column `name`.
        /// The underlying database type is `varchar(50)` with the default value of `New NPC`.
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
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
        public UInt32 StatPoints
        {
            get { return _statPoints; }
            set { _statPoints = value; }
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