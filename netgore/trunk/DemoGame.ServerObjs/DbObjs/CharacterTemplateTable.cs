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
        String Ai { get; }

        /// <summary>
        /// Gets the value of the database column `alliance_id`.
        /// </summary>
        AllianceID AllianceId { get; }

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
        UInt16 Id { get; }

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
        UInt32 Statpoints { get; }

        /// <summary>
        /// Gets the value of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The <paramref name="key"/> that represents the column in this column collection.</param>
        /// <returns>
        /// The value of the database column with the corresponding <paramref name="key"/>.
        /// </returns>
        Int32 GetStat(StatType key);

        /// <summary>
        /// Gets the <paramref name="value"/> of the database column in the column collection `{0}`
        /// that corresponds to the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The <paramref name="key"/> that represents the column in this column collection.</param>
        /// <param name="value">The <paramref name="value"/> to assign to the column with the corresponding <paramref name="key"/>.</param>
        Void SetStat(StatType key, Int32 value);
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

        readonly StatConstDictionary _stat = new StatConstDictionary();

        /// <summary>
        /// The field that maps onto the database column `ai`.
        /// </summary>
        String _ai;

        /// <summary>
        /// The field that maps onto the database column `alliance_id`.
        /// </summary>
        Byte _allianceId;

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
        UInt16 _id;

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
        UInt32 _statpoints;

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public IEnumerable<String> DbColumns
        {
            get { return (IEnumerable<String>)_dbColumns; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
        /// </summary>
        public IEnumerable<String> DbKeyColumns
        {
            get { return (IEnumerable<String>)_dbColumnsKeys; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
        /// </summary>
        public IEnumerable<String> DbNonKeyColumns
        {
            get { return (IEnumerable<String>)_dbColumnsNonKey; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the name of the database
        /// columns used in the column collection `Stat`.
        /// </summary>
        public IEnumerable<String> StatColumns
        {
            get { return (IEnumerable<String>)_statColumns; }
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
        public CharacterTemplateTable(Byte @acc, Byte @agi, String @ai, AllianceID @allianceId, Byte @armor, UInt16 @body,
                                      Byte @bra, Byte @defence, Byte @dex, Byte @evade, UInt32 @exp, UInt16 @giveCash,
                                      UInt16 @giveExp, UInt16 @id, Byte @imm, Byte @int, Byte @level, Byte @maxhit, UInt16 @maxhp,
                                      UInt16 @maxmp, Byte @minhit, String @name, Byte @perc, Byte @recov, Byte @regen,
                                      UInt16 @respawn, UInt32 @statpoints, Byte @str, Byte @tact, Byte @ws)
        {
            SetStat((StatType)StatType.Acc, (Int32)@acc);
            SetStat((StatType)StatType.Agi, (Int32)@agi);
            Ai = (String)@ai;
            AllianceId = (AllianceID)@allianceId;
            SetStat((StatType)StatType.Armor, (Int32)@armor);
            Body = (UInt16)@body;
            SetStat((StatType)StatType.Bra, (Int32)@bra);
            SetStat((StatType)StatType.Defence, (Int32)@defence);
            SetStat((StatType)StatType.Dex, (Int32)@dex);
            SetStat((StatType)StatType.Evade, (Int32)@evade);
            Exp = (UInt32)@exp;
            GiveCash = (UInt16)@giveCash;
            GiveExp = (UInt16)@giveExp;
            Id = (UInt16)@id;
            SetStat((StatType)StatType.Imm, (Int32)@imm);
            SetStat((StatType)StatType.Int, (Int32)@int);
            Level = (Byte)@level;
            SetStat((StatType)StatType.MaxHit, (Int32)@maxhit);
            SetStat((StatType)StatType.MaxHP, (Int32)@maxhp);
            SetStat((StatType)StatType.MaxMP, (Int32)@maxmp);
            SetStat((StatType)StatType.MinHit, (Int32)@minhit);
            Name = (String)@name;
            SetStat((StatType)StatType.Perc, (Int32)@perc);
            SetStat((StatType)StatType.Recov, (Int32)@recov);
            SetStat((StatType)StatType.Regen, (Int32)@regen);
            Respawn = (UInt16)@respawn;
            Statpoints = (UInt32)@statpoints;
            SetStat((StatType)StatType.Str, (Int32)@str);
            SetStat((StatType)StatType.Tact, (Int32)@tact);
            SetStat((StatType)StatType.WS, (Int32)@ws);
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
        ///  this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(ICharacterTemplateTable source, IDictionary<String, Object> dic)
        {
            dic["@acc"] = (Byte)source.GetStat((StatType)StatType.Acc);
            dic["@agi"] = (Byte)source.GetStat((StatType)StatType.Agi);
            dic["@ai"] = (String)source.Ai;
            dic["@alliance_id"] = (AllianceID)source.AllianceId;
            dic["@armor"] = (Byte)source.GetStat((StatType)StatType.Armor);
            dic["@body"] = (UInt16)source.Body;
            dic["@bra"] = (Byte)source.GetStat((StatType)StatType.Bra);
            dic["@defence"] = (Byte)source.GetStat((StatType)StatType.Defence);
            dic["@dex"] = (Byte)source.GetStat((StatType)StatType.Dex);
            dic["@evade"] = (Byte)source.GetStat((StatType)StatType.Evade);
            dic["@exp"] = (UInt32)source.Exp;
            dic["@give_cash"] = (UInt16)source.GiveCash;
            dic["@give_exp"] = (UInt16)source.GiveExp;
            dic["@id"] = (UInt16)source.Id;
            dic["@imm"] = (Byte)source.GetStat((StatType)StatType.Imm);
            dic["@int"] = (Byte)source.GetStat((StatType)StatType.Int);
            dic["@level"] = (Byte)source.Level;
            dic["@maxhit"] = (Byte)source.GetStat((StatType)StatType.MaxHit);
            dic["@maxhp"] = (UInt16)source.GetStat((StatType)StatType.MaxHP);
            dic["@maxmp"] = (UInt16)source.GetStat((StatType)StatType.MaxMP);
            dic["@minhit"] = (Byte)source.GetStat((StatType)StatType.MinHit);
            dic["@name"] = (String)source.Name;
            dic["@perc"] = (Byte)source.GetStat((StatType)StatType.Perc);
            dic["@recov"] = (Byte)source.GetStat((StatType)StatType.Recov);
            dic["@regen"] = (Byte)source.GetStat((StatType)StatType.Regen);
            dic["@respawn"] = (UInt16)source.Respawn;
            dic["@statpoints"] = (UInt32)source.Statpoints;
            dic["@str"] = (Byte)source.GetStat((StatType)StatType.Str);
            dic["@tact"] = (Byte)source.GetStat((StatType)StatType.Tact);
            dic["@ws"] = (Byte)source.GetStat((StatType)StatType.WS);
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
            paramValues["@acc"] = (Byte)source.GetStat((StatType)StatType.Acc);
            paramValues["@agi"] = (Byte)source.GetStat((StatType)StatType.Agi);
            paramValues["@ai"] = (String)source.Ai;
            paramValues["@alliance_id"] = (AllianceID)source.AllianceId;
            paramValues["@armor"] = (Byte)source.GetStat((StatType)StatType.Armor);
            paramValues["@body"] = (UInt16)source.Body;
            paramValues["@bra"] = (Byte)source.GetStat((StatType)StatType.Bra);
            paramValues["@defence"] = (Byte)source.GetStat((StatType)StatType.Defence);
            paramValues["@dex"] = (Byte)source.GetStat((StatType)StatType.Dex);
            paramValues["@evade"] = (Byte)source.GetStat((StatType)StatType.Evade);
            paramValues["@exp"] = (UInt32)source.Exp;
            paramValues["@give_cash"] = (UInt16)source.GiveCash;
            paramValues["@give_exp"] = (UInt16)source.GiveExp;
            paramValues["@id"] = (UInt16)source.Id;
            paramValues["@imm"] = (Byte)source.GetStat((StatType)StatType.Imm);
            paramValues["@int"] = (Byte)source.GetStat((StatType)StatType.Int);
            paramValues["@level"] = (Byte)source.Level;
            paramValues["@maxhit"] = (Byte)source.GetStat((StatType)StatType.MaxHit);
            paramValues["@maxhp"] = (UInt16)source.GetStat((StatType)StatType.MaxHP);
            paramValues["@maxmp"] = (UInt16)source.GetStat((StatType)StatType.MaxMP);
            paramValues["@minhit"] = (Byte)source.GetStat((StatType)StatType.MinHit);
            paramValues["@name"] = (String)source.Name;
            paramValues["@perc"] = (Byte)source.GetStat((StatType)StatType.Perc);
            paramValues["@recov"] = (Byte)source.GetStat((StatType)StatType.Recov);
            paramValues["@regen"] = (Byte)source.GetStat((StatType)StatType.Regen);
            paramValues["@respawn"] = (UInt16)source.Respawn;
            paramValues["@statpoints"] = (UInt32)source.Statpoints;
            paramValues["@str"] = (Byte)source.GetStat((StatType)StatType.Str);
            paramValues["@tact"] = (Byte)source.GetStat((StatType)StatType.Tact);
            paramValues["@ws"] = (Byte)source.GetStat((StatType)StatType.WS);
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

        public void CopyValuesFrom(ICharacterTemplateTable source)
        {
            SetStat((StatType)StatType.Acc, (Int32)source.GetStat((StatType)StatType.Acc));
            SetStat((StatType)StatType.Agi, (Int32)source.GetStat((StatType)StatType.Agi));
            Ai = (String)source.Ai;
            AllianceId = (AllianceID)source.AllianceId;
            SetStat((StatType)StatType.Armor, (Int32)source.GetStat((StatType)StatType.Armor));
            Body = (UInt16)source.Body;
            SetStat((StatType)StatType.Bra, (Int32)source.GetStat((StatType)StatType.Bra));
            SetStat((StatType)StatType.Defence, (Int32)source.GetStat((StatType)StatType.Defence));
            SetStat((StatType)StatType.Dex, (Int32)source.GetStat((StatType)StatType.Dex));
            SetStat((StatType)StatType.Evade, (Int32)source.GetStat((StatType)StatType.Evade));
            Exp = (UInt32)source.Exp;
            GiveCash = (UInt16)source.GiveCash;
            GiveExp = (UInt16)source.GiveExp;
            Id = (UInt16)source.Id;
            SetStat((StatType)StatType.Imm, (Int32)source.GetStat((StatType)StatType.Imm));
            SetStat((StatType)StatType.Int, (Int32)source.GetStat((StatType)StatType.Int));
            Level = (Byte)source.Level;
            SetStat((StatType)StatType.MaxHit, (Int32)source.GetStat((StatType)StatType.MaxHit));
            SetStat((StatType)StatType.MaxHP, (Int32)source.GetStat((StatType)StatType.MaxHP));
            SetStat((StatType)StatType.MaxMP, (Int32)source.GetStat((StatType)StatType.MaxMP));
            SetStat((StatType)StatType.MinHit, (Int32)source.GetStat((StatType)StatType.MinHit));
            Name = (String)source.Name;
            SetStat((StatType)StatType.Perc, (Int32)source.GetStat((StatType)StatType.Perc));
            SetStat((StatType)StatType.Recov, (Int32)source.GetStat((StatType)StatType.Recov));
            SetStat((StatType)StatType.Regen, (Int32)source.GetStat((StatType)StatType.Regen));
            Respawn = (UInt16)source.Respawn;
            Statpoints = (UInt32)source.Statpoints;
            SetStat((StatType)StatType.Str, (Int32)source.GetStat((StatType)StatType.Str));
            SetStat((StatType)StatType.Tact, (Int32)source.GetStat((StatType)StatType.Tact));
            SetStat((StatType)StatType.WS, (Int32)source.GetStat((StatType)StatType.WS));
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
                    return GetStat((StatType)StatType.Acc);

                case "agi":
                    return GetStat((StatType)StatType.Agi);

                case "ai":
                    return Ai;

                case "alliance_id":
                    return AllianceId;

                case "armor":
                    return GetStat((StatType)StatType.Armor);

                case "body":
                    return Body;

                case "bra":
                    return GetStat((StatType)StatType.Bra);

                case "defence":
                    return GetStat((StatType)StatType.Defence);

                case "dex":
                    return GetStat((StatType)StatType.Dex);

                case "evade":
                    return GetStat((StatType)StatType.Evade);

                case "exp":
                    return Exp;

                case "give_cash":
                    return GiveCash;

                case "give_exp":
                    return GiveExp;

                case "id":
                    return Id;

                case "imm":
                    return GetStat((StatType)StatType.Imm);

                case "int":
                    return GetStat((StatType)StatType.Int);

                case "level":
                    return Level;

                case "maxhit":
                    return GetStat((StatType)StatType.MaxHit);

                case "maxhp":
                    return GetStat((StatType)StatType.MaxHP);

                case "maxmp":
                    return GetStat((StatType)StatType.MaxMP);

                case "minhit":
                    return GetStat((StatType)StatType.MinHit);

                case "name":
                    return Name;

                case "perc":
                    return GetStat((StatType)StatType.Perc);

                case "recov":
                    return GetStat((StatType)StatType.Recov);

                case "regen":
                    return GetStat((StatType)StatType.Regen);

                case "respawn":
                    return Respawn;

                case "statpoints":
                    return Statpoints;

                case "str":
                    return GetStat((StatType)StatType.Str);

                case "tact":
                    return GetStat((StatType)StatType.Tact);

                case "ws":
                    return GetStat((StatType)StatType.WS);

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
            SetStat((StatType)StatType.Acc, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("acc")));
            SetStat((StatType)StatType.Agi, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("agi")));
            Ai = (String)(String)dataReader.GetString(dataReader.GetOrdinal("ai"));
            AllianceId = (AllianceID)(AllianceID)dataReader.GetByte(dataReader.GetOrdinal("alliance_id"));
            SetStat((StatType)StatType.Armor, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("armor")));
            Body = (UInt16)(UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("body"));
            SetStat((StatType)StatType.Bra, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("bra")));
            SetStat((StatType)StatType.Defence, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("defence")));
            SetStat((StatType)StatType.Dex, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("dex")));
            SetStat((StatType)StatType.Evade, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("evade")));
            Exp = (UInt32)(UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("exp"));
            GiveCash = (UInt16)(UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("give_cash"));
            GiveExp = (UInt16)(UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("give_exp"));
            Id = (UInt16)(UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("id"));
            SetStat((StatType)StatType.Imm, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("imm")));
            SetStat((StatType)StatType.Int, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("int")));
            Level = (Byte)(Byte)dataReader.GetByte(dataReader.GetOrdinal("level"));
            SetStat((StatType)StatType.MaxHit, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("maxhit")));
            SetStat((StatType)StatType.MaxHP, (Int32)(UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxhp")));
            SetStat((StatType)StatType.MaxMP, (Int32)(UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("maxmp")));
            SetStat((StatType)StatType.MinHit, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("minhit")));
            Name = (String)(String)dataReader.GetString(dataReader.GetOrdinal("name"));
            SetStat((StatType)StatType.Perc, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("perc")));
            SetStat((StatType)StatType.Recov, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("recov")));
            SetStat((StatType)StatType.Regen, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("regen")));
            Respawn = (UInt16)(UInt16)dataReader.GetUInt16(dataReader.GetOrdinal("respawn"));
            Statpoints = (UInt32)(UInt32)dataReader.GetUInt32(dataReader.GetOrdinal("statpoints"));
            SetStat((StatType)StatType.Str, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("str")));
            SetStat((StatType)StatType.Tact, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("tact")));
            SetStat((StatType)StatType.WS, (Int32)(Byte)dataReader.GetByte(dataReader.GetOrdinal("ws")));
        }

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

                case "ai":
                    Ai = (String)value;
                    break;

                case "alliance_id":
                    AllianceId = (AllianceID)value;
                    break;

                case "armor":
                    SetStat((StatType)StatType.Armor, (Int32)value);
                    break;

                case "body":
                    Body = (UInt16)value;
                    break;

                case "bra":
                    SetStat((StatType)StatType.Bra, (Int32)value);
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

                case "give_cash":
                    GiveCash = (UInt16)value;
                    break;

                case "give_exp":
                    GiveExp = (UInt16)value;
                    break;

                case "id":
                    Id = (UInt16)value;
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

                case "name":
                    Name = (String)value;
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

                case "respawn":
                    Respawn = (UInt16)value;
                    break;

                case "statpoints":
                    Statpoints = (UInt32)value;
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
                        paramValues[i] = source.GetStat((StatType)StatType.Acc);
                        break;

                    case "@agi":
                        paramValues[i] = source.GetStat((StatType)StatType.Agi);
                        break;

                    case "@ai":
                        paramValues[i] = source.Ai;
                        break;

                    case "@alliance_id":
                        paramValues[i] = source.AllianceId;
                        break;

                    case "@armor":
                        paramValues[i] = source.GetStat((StatType)StatType.Armor);
                        break;

                    case "@body":
                        paramValues[i] = source.Body;
                        break;

                    case "@bra":
                        paramValues[i] = source.GetStat((StatType)StatType.Bra);
                        break;

                    case "@defence":
                        paramValues[i] = source.GetStat((StatType)StatType.Defence);
                        break;

                    case "@dex":
                        paramValues[i] = source.GetStat((StatType)StatType.Dex);
                        break;

                    case "@evade":
                        paramValues[i] = source.GetStat((StatType)StatType.Evade);
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
                        paramValues[i] = source.Id;
                        break;

                    case "@imm":
                        paramValues[i] = source.GetStat((StatType)StatType.Imm);
                        break;

                    case "@int":
                        paramValues[i] = source.GetStat((StatType)StatType.Int);
                        break;

                    case "@level":
                        paramValues[i] = source.Level;
                        break;

                    case "@maxhit":
                        paramValues[i] = source.GetStat((StatType)StatType.MaxHit);
                        break;

                    case "@maxhp":
                        paramValues[i] = source.GetStat((StatType)StatType.MaxHP);
                        break;

                    case "@maxmp":
                        paramValues[i] = source.GetStat((StatType)StatType.MaxMP);
                        break;

                    case "@minhit":
                        paramValues[i] = source.GetStat((StatType)StatType.MinHit);
                        break;

                    case "@name":
                        paramValues[i] = source.Name;
                        break;

                    case "@perc":
                        paramValues[i] = source.GetStat((StatType)StatType.Perc);
                        break;

                    case "@recov":
                        paramValues[i] = source.GetStat((StatType)StatType.Recov);
                        break;

                    case "@regen":
                        paramValues[i] = source.GetStat((StatType)StatType.Regen);
                        break;

                    case "@respawn":
                        paramValues[i] = source.Respawn;
                        break;

                    case "@statpoints":
                        paramValues[i] = source.Statpoints;
                        break;

                    case "@str":
                        paramValues[i] = source.GetStat((StatType)StatType.Str);
                        break;

                    case "@tact":
                        paramValues[i] = source.GetStat((StatType)StatType.Tact);
                        break;

                    case "@ws":
                        paramValues[i] = source.GetStat((StatType)StatType.WS);
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
                        SetStat((StatType)StatType.Acc, (Int32)dataReader.GetByte(i));
                        break;

                    case "agi":
                        SetStat((StatType)StatType.Agi, (Int32)dataReader.GetByte(i));
                        break;

                    case "ai":
                        Ai = (String)dataReader.GetString(i);
                        break;

                    case "alliance_id":
                        AllianceId = (AllianceID)dataReader.GetByte(i);
                        break;

                    case "armor":
                        SetStat((StatType)StatType.Armor, (Int32)dataReader.GetByte(i));
                        break;

                    case "body":
                        Body = (UInt16)dataReader.GetUInt16(i);
                        break;

                    case "bra":
                        SetStat((StatType)StatType.Bra, (Int32)dataReader.GetByte(i));
                        break;

                    case "defence":
                        SetStat((StatType)StatType.Defence, (Int32)dataReader.GetByte(i));
                        break;

                    case "dex":
                        SetStat((StatType)StatType.Dex, (Int32)dataReader.GetByte(i));
                        break;

                    case "evade":
                        SetStat((StatType)StatType.Evade, (Int32)dataReader.GetByte(i));
                        break;

                    case "exp":
                        Exp = (UInt32)dataReader.GetUInt32(i);
                        break;

                    case "give_cash":
                        GiveCash = (UInt16)dataReader.GetUInt16(i);
                        break;

                    case "give_exp":
                        GiveExp = (UInt16)dataReader.GetUInt16(i);
                        break;

                    case "id":
                        Id = (UInt16)dataReader.GetUInt16(i);
                        break;

                    case "imm":
                        SetStat((StatType)StatType.Imm, (Int32)dataReader.GetByte(i));
                        break;

                    case "int":
                        SetStat((StatType)StatType.Int, (Int32)dataReader.GetByte(i));
                        break;

                    case "level":
                        Level = (Byte)dataReader.GetByte(i);
                        break;

                    case "maxhit":
                        SetStat((StatType)StatType.MaxHit, (Int32)dataReader.GetByte(i));
                        break;

                    case "maxhp":
                        SetStat((StatType)StatType.MaxHP, (Int32)dataReader.GetUInt16(i));
                        break;

                    case "maxmp":
                        SetStat((StatType)StatType.MaxMP, (Int32)dataReader.GetUInt16(i));
                        break;

                    case "minhit":
                        SetStat((StatType)StatType.MinHit, (Int32)dataReader.GetByte(i));
                        break;

                    case "name":
                        Name = (String)dataReader.GetString(i);
                        break;

                    case "perc":
                        SetStat((StatType)StatType.Perc, (Int32)dataReader.GetByte(i));
                        break;

                    case "recov":
                        SetStat((StatType)StatType.Recov, (Int32)dataReader.GetByte(i));
                        break;

                    case "regen":
                        SetStat((StatType)StatType.Regen, (Int32)dataReader.GetByte(i));
                        break;

                    case "respawn":
                        Respawn = (UInt16)dataReader.GetUInt16(i);
                        break;

                    case "statpoints":
                        Statpoints = (UInt32)dataReader.GetUInt32(i);
                        break;

                    case "str":
                        SetStat((StatType)StatType.Str, (Int32)dataReader.GetByte(i));
                        break;

                    case "tact":
                        SetStat((StatType)StatType.Tact, (Int32)dataReader.GetByte(i));
                        break;

                    case "ws":
                        SetStat((StatType)StatType.WS, (Int32)dataReader.GetByte(i));
                        break;
                }
            }
        }

        #region ICharacterTemplateTable Members

        public Int32 GetStat(StatType key)
        {
            return (Byte)_stat[(StatType)key];
        }

        public void SetStat(StatType key, Int32 value)
        {
            _stat[(StatType)key] = (Byte)value;
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `ai`.
        /// The underlying database type is `varchar(255)`.
        /// </summary>
        public String Ai
        {
            get { return (String)_ai; }
            set { _ai = (String)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `alliance_id`.
        /// The underlying database type is `tinyint(3) unsigned`.
        /// </summary>
        public AllianceID AllianceId
        {
            get { return (AllianceID)_allianceId; }
            set { _allianceId = (Byte)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `body`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
        /// </summary>
        public UInt16 Body
        {
            get { return (UInt16)_body; }
            set { _body = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `exp`.
        /// The underlying database type is `int(10) unsigned`.
        /// </summary>
        public UInt32 Exp
        {
            get { return (UInt32)_exp; }
            set { _exp = (UInt32)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `give_cash`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 GiveCash
        {
            get { return (UInt16)_giveCash; }
            set { _giveCash = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `give_exp`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
        /// </summary>
        public UInt16 GiveExp
        {
            get { return (UInt16)_giveExp; }
            set { _giveExp = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `smallint(5) unsigned`.
        /// </summary>
        public UInt16 Id
        {
            get { return (UInt16)_id; }
            set { _id = (UInt16)value; }
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
        /// Gets or sets the value for the field that maps onto the database column `name`.
        /// The underlying database type is `varchar(50)` with the default value of `New NPC`.
        /// </summary>
        public String Name
        {
            get { return (String)_name; }
            set { _name = (String)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `respawn`.
        /// The underlying database type is `smallint(5) unsigned` with the default value of `5`.
        /// </summary>
        public UInt16 Respawn
        {
            get { return (UInt16)_respawn; }
            set { _respawn = (UInt16)value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `statpoints`.
        /// The underlying database type is `int(10) unsigned`.
        /// </summary>
        public UInt32 Statpoints
        {
            get { return (UInt32)_statpoints; }
            set { _statpoints = (UInt32)value; }
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