using System;
using System.Linq;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Provides a strongly-typed structure for the database table `character_template`.
/// </summary>
public class CharacterTemplateTable : ICharacterTemplateTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"acc", "agi", "ai", "alliance_id", "armor", "body_id", "bra", "defence", "dex", "evade", "exp", "give_cash", "give_exp", "id", "imm", "int", "level", "maxhit", "maxhp", "maxmp", "minhit", "name", "perc", "recov", "regen", "respawn", "statpoints", "str", "tact", "ws" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumns;
}
}
/// <summary>
/// Array of the database column names for columns that are primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsKeys = new string[] {"id" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsKeys;
}
}
/// <summary>
/// Array of the database column names for columns that are not primary keys.
/// </summary>
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"acc", "agi", "ai", "alliance_id", "armor", "body_id", "bra", "defence", "dex", "evade", "exp", "give_cash", "give_exp", "imm", "int", "level", "maxhit", "maxhp", "maxmp", "minhit", "name", "perc", "recov", "regen", "respawn", "statpoints", "str", "tact", "ws" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> DbNonKeyColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_dbColumnsNonKey;
}
}
/// <summary>
/// The fields that are used in the column collection `Stat`.
/// </summary>
 static  readonly System.String[] _statColumns = new string[] {"acc", "agi", "armor", "bra", "defence", "dex", "evade", "imm", "int", "maxhit", "maxhp", "maxmp", "minhit", "perc", "recov", "regen", "str", "tact", "ws" };
/// <summary>
/// Gets an IEnumerable of strings containing the name of the database
/// columns used in the column collection `Stat`.
/// </summary>
public static System.Collections.Generic.IEnumerable<System.String> StatColumns
{
get
{
return (System.Collections.Generic.IEnumerable<System.String>)_statColumns;
}
}
/// <summary>
/// Gets an IEnumerable of KeyValuePairs containing the values in the `Stat` collection. The
/// key is the collection's key and the value is the value for that corresponding key.
/// </summary>
public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> Stats
{
get
{
return (System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>>)_stat;
}
}
/// <summary>
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "character_template";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 30;
/// <summary>
/// Dictionary containing the values for the column collection `Stat`.
/// </summary>
 readonly StatConstDictionary _stat = new StatConstDictionary();
/// <summary>
/// The field that maps onto the database column `ai`.
/// </summary>
System.String _aI;
/// <summary>
/// The field that maps onto the database column `alliance_id`.
/// </summary>
System.Byte _allianceID;
/// <summary>
/// The field that maps onto the database column `body_id`.
/// </summary>
System.UInt16 _bodyID;
/// <summary>
/// The field that maps onto the database column `exp`.
/// </summary>
System.UInt32 _exp;
/// <summary>
/// The field that maps onto the database column `give_cash`.
/// </summary>
System.UInt16 _giveCash;
/// <summary>
/// The field that maps onto the database column `give_exp`.
/// </summary>
System.UInt16 _giveExp;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt16 _iD;
/// <summary>
/// The field that maps onto the database column `level`.
/// </summary>
System.Byte _level;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// The field that maps onto the database column `respawn`.
/// </summary>
System.UInt16 _respawn;
/// <summary>
/// The field that maps onto the database column `statpoints`.
/// </summary>
System.UInt32 _statPoints;
/// <summary>
/// Gets the value of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <returns>
/// The value of the database column for the corresponding <paramref name="key"/>.
/// </returns>
public System.Int32 GetStat(DemoGame.StatType key)
{
return (System.Byte)_stat[(DemoGame.StatType)key];
}
/// <summary>
/// Gets the <paramref name="value"/> of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <param name="value">The value to assign to the column for the corresponding <paramref name="key"/>.</param>
public void SetStat(DemoGame.StatType key, System.Int32 value)
{
this._stat[(DemoGame.StatType)key] = (System.Byte)value;
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `ai`.
/// The underlying database type is `varchar(255)`.
/// </summary>
public System.String AI
{
get
{
return (System.String)_aI;
}
set
{
this._aI = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `alliance_id`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public DemoGame.Server.AllianceID AllianceID
{
get
{
return (DemoGame.Server.AllianceID)_allianceID;
}
set
{
this._allianceID = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `body_id`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `1`.
/// </summary>
public DemoGame.BodyIndex BodyID
{
get
{
return (DemoGame.BodyIndex)_bodyID;
}
set
{
this._bodyID = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `exp`.
/// The underlying database type is `int(10) unsigned`.
/// </summary>
public System.UInt32 Exp
{
get
{
return (System.UInt32)_exp;
}
set
{
this._exp = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `give_cash`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 GiveCash
{
get
{
return (System.UInt16)_giveCash;
}
set
{
this._giveCash = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `give_exp`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `0`.
/// </summary>
public System.UInt16 GiveExp
{
get
{
return (System.UInt16)_giveExp;
}
set
{
this._giveExp = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public DemoGame.Server.CharacterTemplateID ID
{
get
{
return (DemoGame.Server.CharacterTemplateID)_iD;
}
set
{
this._iD = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `level`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `1`.
/// </summary>
public System.Byte Level
{
get
{
return (System.Byte)_level;
}
set
{
this._level = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `name`.
/// The underlying database type is `varchar(50)` with the default value of `New NPC`.
/// </summary>
public System.String Name
{
get
{
return (System.String)_name;
}
set
{
this._name = (System.String)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `respawn`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `5`.
/// </summary>
public System.UInt16 Respawn
{
get
{
return (System.UInt16)_respawn;
}
set
{
this._respawn = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `statpoints`.
/// The underlying database type is `int(10) unsigned`.
/// </summary>
public System.UInt32 StatPoints
{
get
{
return (System.UInt32)_statPoints;
}
set
{
this._statPoints = (System.UInt32)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public ICharacterTemplateTable DeepCopy()
{
return new CharacterTemplateTable(this);
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
/// <param name="bodyID">The initial value for the corresponding property.</param>
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
public CharacterTemplateTable(System.Byte @acc, System.Byte @agi, System.String @aI, DemoGame.Server.AllianceID @allianceID, System.Byte @armor, DemoGame.BodyIndex @bodyID, System.Byte @bra, System.Byte @defence, System.Byte @dex, System.Byte @evade, System.UInt32 @exp, System.UInt16 @giveCash, System.UInt16 @giveExp, DemoGame.Server.CharacterTemplateID @iD, System.Byte @imm, System.Byte @int, System.Byte @level, System.Byte @maxHit, System.UInt16 @maxHP, System.UInt16 @maxMP, System.Byte @minHit, System.String @name, System.Byte @perc, System.Byte @recov, System.Byte @regen, System.UInt16 @respawn, System.UInt32 @statPoints, System.Byte @str, System.Byte @tact, System.Byte @wS)
{
this.SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)@acc);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@agi);
this.AI = (System.String)@aI;
this.AllianceID = (DemoGame.Server.AllianceID)@allianceID;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)@armor);
this.BodyID = (DemoGame.BodyIndex)@bodyID;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)@bra);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)@defence);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)@dex);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)@evade);
this.Exp = (System.UInt32)@exp;
this.GiveCash = (System.UInt16)@giveCash;
this.GiveExp = (System.UInt16)@giveExp;
this.ID = (DemoGame.Server.CharacterTemplateID)@iD;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)@imm);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@int);
this.Level = (System.Byte)@level;
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)@maxHit);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)@maxHP);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)@maxMP);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)@minHit);
this.Name = (System.String)@name;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)@perc);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)@recov);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)@regen);
this.Respawn = (System.UInt16)@respawn;
this.StatPoints = (System.UInt32)@statPoints;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)@str);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)@tact);
this.SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)@wS);
}
/// <summary>
/// CharacterTemplateTable constructor.
/// </summary>
/// <param name="source">ICharacterTemplateTable to copy the initial values from.</param>
public CharacterTemplateTable(ICharacterTemplateTable source)
{
CopyValuesFrom(source);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="dic">The Dictionary to copy the values into.</param>
public void CopyValues(System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
CopyValues(this, dic);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
/// this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="dic">The Dictionary to copy the values into.</param>
public static void CopyValues(ICharacterTemplateTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@acc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc);
dic["@agi"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@ai"] = (System.String)source.AI;
dic["@alliance_id"] = (DemoGame.Server.AllianceID)source.AllianceID;
dic["@armor"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor);
dic["@body_id"] = (DemoGame.BodyIndex)source.BodyID;
dic["@bra"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra);
dic["@defence"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
dic["@dex"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex);
dic["@evade"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade);
dic["@exp"] = (System.UInt32)source.Exp;
dic["@give_cash"] = (System.UInt16)source.GiveCash;
dic["@give_exp"] = (System.UInt16)source.GiveExp;
dic["@id"] = (DemoGame.Server.CharacterTemplateID)source.ID;
dic["@imm"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm);
dic["@int"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["@level"] = (System.Byte)source.Level;
dic["@maxhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
dic["@maxhp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
dic["@maxmp"] = (System.UInt16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
dic["@minhit"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
dic["@name"] = (System.String)source.Name;
dic["@perc"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc);
dic["@recov"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov);
dic["@regen"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen);
dic["@respawn"] = (System.UInt16)source.Respawn;
dic["@statpoints"] = (System.UInt32)source.StatPoints;
dic["@str"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
dic["@tact"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact);
dic["@ws"] = (System.Byte)source.GetStat((DemoGame.StatType)DemoGame.StatType.WS);
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this CharacterTemplateTable.
/// </summary>
/// <param name="source">The ICharacterTemplateTable to copy the values from.</param>
public void CopyValuesFrom(ICharacterTemplateTable source)
{
this.SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Acc));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi));
this.AI = (System.String)source.AI;
this.AllianceID = (DemoGame.Server.AllianceID)source.AllianceID;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Armor));
this.BodyID = (DemoGame.BodyIndex)source.BodyID;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Bra));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Dex));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Evade));
this.Exp = (System.UInt32)source.Exp;
this.GiveCash = (System.UInt16)source.GiveCash;
this.GiveExp = (System.UInt16)source.GiveExp;
this.ID = (DemoGame.Server.CharacterTemplateID)source.ID;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Imm));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int));
this.Level = (System.Byte)source.Level;
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit));
this.Name = (System.String)source.Name;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Perc));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Recov));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Regen));
this.Respawn = (System.UInt16)source.Respawn;
this.StatPoints = (System.UInt32)source.StatPoints;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Tact));
this.SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.WS));
}

/// <summary>
/// Gets the value of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the value for.</param>
/// <returns>
/// The value of the column with the name <paramref name="columnName"/>.
/// </returns>
public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "acc":
return GetStat((DemoGame.StatType)DemoGame.StatType.Acc);

case "agi":
return GetStat((DemoGame.StatType)DemoGame.StatType.Agi);

case "ai":
return AI;

case "alliance_id":
return AllianceID;

case "armor":
return GetStat((DemoGame.StatType)DemoGame.StatType.Armor);

case "body_id":
return BodyID;

case "bra":
return GetStat((DemoGame.StatType)DemoGame.StatType.Bra);

case "defence":
return GetStat((DemoGame.StatType)DemoGame.StatType.Defence);

case "dex":
return GetStat((DemoGame.StatType)DemoGame.StatType.Dex);

case "evade":
return GetStat((DemoGame.StatType)DemoGame.StatType.Evade);

case "exp":
return Exp;

case "give_cash":
return GiveCash;

case "give_exp":
return GiveExp;

case "id":
return ID;

case "imm":
return GetStat((DemoGame.StatType)DemoGame.StatType.Imm);

case "int":
return GetStat((DemoGame.StatType)DemoGame.StatType.Int);

case "level":
return Level;

case "maxhit":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);

case "maxhp":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);

case "maxmp":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);

case "minhit":
return GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);

case "name":
return Name;

case "perc":
return GetStat((DemoGame.StatType)DemoGame.StatType.Perc);

case "recov":
return GetStat((DemoGame.StatType)DemoGame.StatType.Recov);

case "regen":
return GetStat((DemoGame.StatType)DemoGame.StatType.Regen);

case "respawn":
return Respawn;

case "statpoints":
return StatPoints;

case "str":
return GetStat((DemoGame.StatType)DemoGame.StatType.Str);

case "tact":
return GetStat((DemoGame.StatType)DemoGame.StatType.Tact);

case "ws":
return GetStat((DemoGame.StatType)DemoGame.StatType.WS);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Sets the <paramref name="value"/> of a column by the database column's name.
/// </summary>
/// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
/// <param name="value">Value to assign to the column.</param>
public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "acc":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Acc, (System.Int32)value);
break;

case "agi":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)value);
break;

case "ai":
this.AI = (System.String)value;
break;

case "alliance_id":
this.AllianceID = (DemoGame.Server.AllianceID)value;
break;

case "armor":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Armor, (System.Int32)value);
break;

case "body_id":
this.BodyID = (DemoGame.BodyIndex)value;
break;

case "bra":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Bra, (System.Int32)value);
break;

case "defence":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)value);
break;

case "dex":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Dex, (System.Int32)value);
break;

case "evade":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Evade, (System.Int32)value);
break;

case "exp":
this.Exp = (System.UInt32)value;
break;

case "give_cash":
this.GiveCash = (System.UInt16)value;
break;

case "give_exp":
this.GiveExp = (System.UInt16)value;
break;

case "id":
this.ID = (DemoGame.Server.CharacterTemplateID)value;
break;

case "imm":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Imm, (System.Int32)value);
break;

case "int":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)value);
break;

case "level":
this.Level = (System.Byte)value;
break;

case "maxhit":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)value);
break;

case "maxhp":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)value);
break;

case "maxmp":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)value);
break;

case "minhit":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)value);
break;

case "name":
this.Name = (System.String)value;
break;

case "perc":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Perc, (System.Int32)value);
break;

case "recov":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Recov, (System.Int32)value);
break;

case "regen":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Regen, (System.Int32)value);
break;

case "respawn":
this.Respawn = (System.UInt16)value;
break;

case "statpoints":
this.StatPoints = (System.UInt32)value;
break;

case "str":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)value);
break;

case "tact":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Tact, (System.Int32)value);
break;

case "ws":
this.SetStat((DemoGame.StatType)DemoGame.StatType.WS, (System.Int32)value);
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Gets the data for the database column that this table represents.
/// </summary>
/// <param name="columnName">The database name of the column to get the data for.</param>
/// <returns>
/// The data for the database column with the name <paramref name="columnName"/>.
/// </returns>
public static ColumnMetadata GetColumnData(System.String columnName)
{
switch (columnName)
{
case "acc":
return new ColumnMetadata("acc", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "agi":
return new ColumnMetadata("agi", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "ai":
return new ColumnMetadata("ai", "", "varchar(255)", null, typeof(System.String), true, false, false);

case "alliance_id":
return new ColumnMetadata("alliance_id", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, false, true);

case "armor":
return new ColumnMetadata("armor", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "body_id":
return new ColumnMetadata("body_id", "", "smallint(5) unsigned", "1", typeof(System.UInt16), false, false, false);

case "bra":
return new ColumnMetadata("bra", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "defence":
return new ColumnMetadata("defence", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, false);

case "dex":
return new ColumnMetadata("dex", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "evade":
return new ColumnMetadata("evade", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "exp":
return new ColumnMetadata("exp", "", "int(10) unsigned", null, typeof(System.UInt32), false, false, false);

case "give_cash":
return new ColumnMetadata("give_cash", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "give_exp":
return new ColumnMetadata("give_exp", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "id":
return new ColumnMetadata("id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

case "imm":
return new ColumnMetadata("imm", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "int":
return new ColumnMetadata("int", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "level":
return new ColumnMetadata("level", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "maxhit":
return new ColumnMetadata("maxhit", "", "tinyint(3) unsigned", "2", typeof(System.Byte), false, false, false);

case "maxhp":
return new ColumnMetadata("maxhp", "", "smallint(5) unsigned", "50", typeof(System.UInt16), false, false, false);

case "maxmp":
return new ColumnMetadata("maxmp", "", "smallint(5) unsigned", "50", typeof(System.UInt16), false, false, false);

case "minhit":
return new ColumnMetadata("minhit", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "name":
return new ColumnMetadata("name", "", "varchar(50)", "New NPC", typeof(System.String), false, false, false);

case "perc":
return new ColumnMetadata("perc", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "recov":
return new ColumnMetadata("recov", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "regen":
return new ColumnMetadata("regen", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "respawn":
return new ColumnMetadata("respawn", "", "smallint(5) unsigned", "5", typeof(System.UInt16), false, false, false);

case "statpoints":
return new ColumnMetadata("statpoints", "", "int(10) unsigned", null, typeof(System.UInt32), false, false, false);

case "str":
return new ColumnMetadata("str", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "tact":
return new ColumnMetadata("tact", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "ws":
return new ColumnMetadata("ws", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// A Dictionary-like lookup table for the Enum values of the type collection `Stat` for the
/// table that this class represents. Majority of the code for this class was automatically generated and
/// only other automatically generated code should be using this class.
/// </summary>
private class StatConstDictionary : System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>>
{
    /// <summary>
    /// Array containing the actual values. The index of this array is found through the value returned
    /// from the _lookupTable.
    /// </summary>
    readonly System.Int32[] _values;
    
    /// <summary>
    /// Array that maps the integer value of key type to the index of the _values array.
    /// </summary>
    static readonly System.Int32[] _lookupTable;

    /// <summary>
    /// StatConstDictionary static constructor.
    /// </summary>
    static StatConstDictionary()
    {
        DemoGame.StatType[] asArray = Enum.GetValues(typeof(DemoGame.StatType)).Cast<DemoGame.StatType>().ToArray();
        _lookupTable = new System.Int32[asArray.Length];

        for (System.Int32 i = 0; i < _lookupTable.Length; i++)
            _lookupTable[i] = (System.Int32)asArray[i];
    }
    
    /// <summary>
    /// StatConstDictionary constructor.
    /// </summary>
    public StatConstDictionary()
    {
        _values = new System.Int32[_lookupTable.Length];
    }
    
	/// <summary>
	/// Gets or sets an item's value using the <paramref name="key"/>.
	/// </summary>
	/// <param name="key">The key for the value to get or set.</param>
	/// <returns>The item's value for the corresponding <paramref name="key"/>.</returns>
    public System.Int32 this[DemoGame.StatType key]
    {
        get
        {
            return _values[_lookupTable[(System.Int32)key]];
        }
        set
        {
            _values[_lookupTable[(System.Int32)key]] = value;
        }
    }

    #region IEnumerable<KeyValuePair<DemoGame.StatType,System.Int32>> Members

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> GetEnumerator()
    {
        for (int i = 0; i < _values.Length; i++)
        {
            yield return new System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>((DemoGame.StatType)i, _values[i]);
        }
    }

    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>2</filterpriority>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
}

}
