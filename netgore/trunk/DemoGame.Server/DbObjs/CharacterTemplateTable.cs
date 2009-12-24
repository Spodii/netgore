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
 static  readonly System.String[] _dbColumns = new string[] {"ai_id", "alliance_id", "body_id", "exp", "give_cash", "give_exp", "id", "level", "move_speed", "name", "respawn", "shop_id", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str", "statpoints" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"ai_id", "alliance_id", "body_id", "exp", "give_cash", "give_exp", "level", "move_speed", "name", "respawn", "shop_id", "stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str", "statpoints" };
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
 static  readonly System.String[] _statColumns = new string[] {"stat_agi", "stat_defence", "stat_int", "stat_maxhit", "stat_maxhp", "stat_maxmp", "stat_minhit", "stat_str" };
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
public const System.Int32 ColumnCount = 21;
/// <summary>
/// The field that maps onto the database column `ai_id`.
/// </summary>
System.Nullable<System.UInt16> _aIID;
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
System.Int32 _exp;
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
/// The field that maps onto the database column `move_speed`.
/// </summary>
System.UInt16 _moveSpeed;
/// <summary>
/// The field that maps onto the database column `name`.
/// </summary>
System.String _name;
/// <summary>
/// The field that maps onto the database column `respawn`.
/// </summary>
System.UInt16 _respawn;
/// <summary>
/// The field that maps onto the database column `shop_id`.
/// </summary>
System.Nullable<System.UInt16> _shopID;
/// <summary>
/// Dictionary containing the values for the column collection `Stat`.
/// </summary>
 readonly StatConstDictionary _stat = new StatConstDictionary();
/// <summary>
/// The field that maps onto the database column `statpoints`.
/// </summary>
System.Int32 _statPoints;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `ai_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.Nullable<NetGore.AI.AIID> AIID
{
get
{
return (System.Nullable<NetGore.AI.AIID>)_aIID;
}
set
{
this._aIID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `alliance_id`.
/// The underlying database type is `tinyint(3) unsigned` with the default value of `0`.
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
/// The underlying database type is `int(11)` with the default value of `0`.
/// </summary>
public System.Int32 Exp
{
get
{
return (System.Int32)_exp;
}
set
{
this._exp = (System.Int32)value;
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
/// Gets or sets the value for the field that maps onto the database column `move_speed`.
/// The underlying database type is `smallint(5) unsigned` with the default value of `1800`.
/// </summary>
public System.UInt16 MoveSpeed
{
get
{
return (System.UInt16)_moveSpeed;
}
set
{
this._moveSpeed = (System.UInt16)value;
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
/// Gets or sets the value for the field that maps onto the database column `shop_id`.
/// The underlying database type is `smallint(5) unsigned`.
/// </summary>
public System.Nullable<DemoGame.Server.ShopID> ShopID
{
get
{
return (System.Nullable<DemoGame.Server.ShopID>)_shopID;
}
set
{
this._shopID = (System.Nullable<System.UInt16>)value;
}
}
/// <summary>
/// Gets the value of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <returns>
/// The value of the database column for the corresponding <paramref name="key"/>.
/// </returns>
public System.Int32 GetStat(DemoGame.StatType key)
{
return (System.Int16)_stat[(DemoGame.StatType)key];
}
/// <summary>
/// Gets the <paramref name="value"/> of a database column for the corresponding <paramref name="key"/> for the column collection `Stat`.
/// </summary>
/// <param name="key">The key of the column to get.</param>
/// <param name="value">The value to assign to the column for the corresponding <paramref name="key"/>.</param>
public void SetStat(DemoGame.StatType key, System.Int32 value)
{
this._stat[(DemoGame.StatType)key] = (System.Int16)value;
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `statpoints`.
/// The underlying database type is `int(11)` with the default value of `0`.
/// </summary>
public System.Int32 StatPoints
{
get
{
return (System.Int32)_statPoints;
}
set
{
this._statPoints = (System.Int32)value;
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
/// <param name="aIID">The initial value for the corresponding property.</param>
/// <param name="allianceID">The initial value for the corresponding property.</param>
/// <param name="bodyID">The initial value for the corresponding property.</param>
/// <param name="exp">The initial value for the corresponding property.</param>
/// <param name="giveCash">The initial value for the corresponding property.</param>
/// <param name="giveExp">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="level">The initial value for the corresponding property.</param>
/// <param name="moveSpeed">The initial value for the corresponding property.</param>
/// <param name="name">The initial value for the corresponding property.</param>
/// <param name="respawn">The initial value for the corresponding property.</param>
/// <param name="shopID">The initial value for the corresponding property.</param>
/// <param name="statAgi">The initial value for the corresponding property.</param>
/// <param name="statDefence">The initial value for the corresponding property.</param>
/// <param name="statInt">The initial value for the corresponding property.</param>
/// <param name="statMaxhit">The initial value for the corresponding property.</param>
/// <param name="statMaxhp">The initial value for the corresponding property.</param>
/// <param name="statMaxmp">The initial value for the corresponding property.</param>
/// <param name="statMinhit">The initial value for the corresponding property.</param>
/// <param name="statStr">The initial value for the corresponding property.</param>
/// <param name="statPoints">The initial value for the corresponding property.</param>
public CharacterTemplateTable(System.Nullable<NetGore.AI.AIID> @aIID, DemoGame.Server.AllianceID @allianceID, DemoGame.BodyIndex @bodyID, System.Int32 @exp, System.UInt16 @giveCash, System.UInt16 @giveExp, DemoGame.Server.CharacterTemplateID @iD, System.Byte @level, System.UInt16 @moveSpeed, System.String @name, System.UInt16 @respawn, System.Nullable<DemoGame.Server.ShopID> @shopID, System.Int16 @statAgi, System.Int16 @statDefence, System.Int16 @statInt, System.Int16 @statMaxhit, System.Int16 @statMaxhp, System.Int16 @statMaxmp, System.Int16 @statMinhit, System.Int16 @statStr, System.Int32 @statPoints)
{
this.AIID = (System.Nullable<NetGore.AI.AIID>)@aIID;
this.AllianceID = (DemoGame.Server.AllianceID)@allianceID;
this.BodyID = (DemoGame.BodyIndex)@bodyID;
this.Exp = (System.Int32)@exp;
this.GiveCash = (System.UInt16)@giveCash;
this.GiveExp = (System.UInt16)@giveExp;
this.ID = (DemoGame.Server.CharacterTemplateID)@iD;
this.Level = (System.Byte)@level;
this.MoveSpeed = (System.UInt16)@moveSpeed;
this.Name = (System.String)@name;
this.Respawn = (System.UInt16)@respawn;
this.ShopID = (System.Nullable<DemoGame.Server.ShopID>)@shopID;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)@statAgi);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)@statDefence);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)@statInt);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)@statMaxhit);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)@statMaxhp);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)@statMaxmp);
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)@statMinhit);
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)@statStr);
this.StatPoints = (System.Int32)@statPoints;
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
dic["@ai_id"] = (System.Nullable<NetGore.AI.AIID>)source.AIID;
dic["@alliance_id"] = (DemoGame.Server.AllianceID)source.AllianceID;
dic["@body_id"] = (DemoGame.BodyIndex)source.BodyID;
dic["@exp"] = (System.Int32)source.Exp;
dic["@give_cash"] = (System.UInt16)source.GiveCash;
dic["@give_exp"] = (System.UInt16)source.GiveExp;
dic["@id"] = (DemoGame.Server.CharacterTemplateID)source.ID;
dic["@level"] = (System.Byte)source.Level;
dic["@move_speed"] = (System.UInt16)source.MoveSpeed;
dic["@name"] = (System.String)source.Name;
dic["@respawn"] = (System.UInt16)source.Respawn;
dic["@shop_id"] = (System.Nullable<DemoGame.Server.ShopID>)source.ShopID;
dic["@stat_agi"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi);
dic["@stat_defence"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence);
dic["@stat_int"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int);
dic["@stat_maxhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);
dic["@stat_maxhp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);
dic["@stat_maxmp"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);
dic["@stat_minhit"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);
dic["@stat_str"] = (System.Int16)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str);
dic["@statpoints"] = (System.Int32)source.StatPoints;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this CharacterTemplateTable.
/// </summary>
/// <param name="source">The ICharacterTemplateTable to copy the values from.</param>
public void CopyValuesFrom(ICharacterTemplateTable source)
{
this.AIID = (System.Nullable<NetGore.AI.AIID>)source.AIID;
this.AllianceID = (DemoGame.Server.AllianceID)source.AllianceID;
this.BodyID = (DemoGame.BodyIndex)source.BodyID;
this.Exp = (System.Int32)source.Exp;
this.GiveCash = (System.UInt16)source.GiveCash;
this.GiveExp = (System.UInt16)source.GiveExp;
this.ID = (DemoGame.Server.CharacterTemplateID)source.ID;
this.Level = (System.Byte)source.Level;
this.MoveSpeed = (System.UInt16)source.MoveSpeed;
this.Name = (System.String)source.Name;
this.Respawn = (System.UInt16)source.Respawn;
this.ShopID = (System.Nullable<DemoGame.Server.ShopID>)source.ShopID;
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Agi));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Defence));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Int));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP));
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.MinHit));
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)source.GetStat((DemoGame.StatType)DemoGame.StatType.Str));
this.StatPoints = (System.Int32)source.StatPoints;
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
case "ai_id":
return AIID;

case "alliance_id":
return AllianceID;

case "body_id":
return BodyID;

case "exp":
return Exp;

case "give_cash":
return GiveCash;

case "give_exp":
return GiveExp;

case "id":
return ID;

case "level":
return Level;

case "move_speed":
return MoveSpeed;

case "name":
return Name;

case "respawn":
return Respawn;

case "shop_id":
return ShopID;

case "stat_agi":
return GetStat((DemoGame.StatType)DemoGame.StatType.Agi);

case "stat_defence":
return GetStat((DemoGame.StatType)DemoGame.StatType.Defence);

case "stat_int":
return GetStat((DemoGame.StatType)DemoGame.StatType.Int);

case "stat_maxhit":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxHit);

case "stat_maxhp":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxHP);

case "stat_maxmp":
return GetStat((DemoGame.StatType)DemoGame.StatType.MaxMP);

case "stat_minhit":
return GetStat((DemoGame.StatType)DemoGame.StatType.MinHit);

case "stat_str":
return GetStat((DemoGame.StatType)DemoGame.StatType.Str);

case "statpoints":
return StatPoints;

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
case "ai_id":
this.AIID = (System.Nullable<NetGore.AI.AIID>)value;
break;

case "alliance_id":
this.AllianceID = (DemoGame.Server.AllianceID)value;
break;

case "body_id":
this.BodyID = (DemoGame.BodyIndex)value;
break;

case "exp":
this.Exp = (System.Int32)value;
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

case "level":
this.Level = (System.Byte)value;
break;

case "move_speed":
this.MoveSpeed = (System.UInt16)value;
break;

case "name":
this.Name = (System.String)value;
break;

case "respawn":
this.Respawn = (System.UInt16)value;
break;

case "shop_id":
this.ShopID = (System.Nullable<DemoGame.Server.ShopID>)value;
break;

case "stat_agi":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Agi, (System.Int32)value);
break;

case "stat_defence":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Defence, (System.Int32)value);
break;

case "stat_int":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Int, (System.Int32)value);
break;

case "stat_maxhit":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHit, (System.Int32)value);
break;

case "stat_maxhp":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxHP, (System.Int32)value);
break;

case "stat_maxmp":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MaxMP, (System.Int32)value);
break;

case "stat_minhit":
this.SetStat((DemoGame.StatType)DemoGame.StatType.MinHit, (System.Int32)value);
break;

case "stat_str":
this.SetStat((DemoGame.StatType)DemoGame.StatType.Str, (System.Int32)value);
break;

case "statpoints":
this.StatPoints = (System.Int32)value;
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
case "ai_id":
return new ColumnMetadata("ai_id", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, false);

case "alliance_id":
return new ColumnMetadata("alliance_id", "", "tinyint(3) unsigned", "0", typeof(System.Byte), false, false, true);

case "body_id":
return new ColumnMetadata("body_id", "", "smallint(5) unsigned", "1", typeof(System.UInt16), false, false, false);

case "exp":
return new ColumnMetadata("exp", "", "int(11)", "0", typeof(System.Int32), false, false, false);

case "give_cash":
return new ColumnMetadata("give_cash", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "give_exp":
return new ColumnMetadata("give_exp", "", "smallint(5) unsigned", "0", typeof(System.UInt16), false, false, false);

case "id":
return new ColumnMetadata("id", "", "smallint(5) unsigned", null, typeof(System.UInt16), false, true, false);

case "level":
return new ColumnMetadata("level", "", "tinyint(3) unsigned", "1", typeof(System.Byte), false, false, false);

case "move_speed":
return new ColumnMetadata("move_speed", "", "smallint(5) unsigned", "1800", typeof(System.UInt16), false, false, false);

case "name":
return new ColumnMetadata("name", "", "varchar(50)", "New NPC", typeof(System.String), false, false, false);

case "respawn":
return new ColumnMetadata("respawn", "", "smallint(5) unsigned", "5", typeof(System.UInt16), false, false, false);

case "shop_id":
return new ColumnMetadata("shop_id", "", "smallint(5) unsigned", null, typeof(System.Nullable<System.UInt16>), true, false, true);

case "stat_agi":
return new ColumnMetadata("stat_agi", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_defence":
return new ColumnMetadata("stat_defence", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_int":
return new ColumnMetadata("stat_int", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_maxhit":
return new ColumnMetadata("stat_maxhit", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_maxhp":
return new ColumnMetadata("stat_maxhp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "stat_maxmp":
return new ColumnMetadata("stat_maxmp", "", "smallint(6)", "50", typeof(System.Int16), false, false, false);

case "stat_minhit":
return new ColumnMetadata("stat_minhit", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "stat_str":
return new ColumnMetadata("stat_str", "", "smallint(6)", "1", typeof(System.Int16), false, false, false);

case "statpoints":
return new ColumnMetadata("statpoints", "", "int(11)", "0", typeof(System.Int32), false, false, false);

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
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
}

}
