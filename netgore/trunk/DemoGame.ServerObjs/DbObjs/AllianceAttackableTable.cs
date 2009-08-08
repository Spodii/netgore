using System;
using System.Linq;
using NetGore.Db;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `alliance_attackable`.
/// </summary>
public interface IAllianceAttackableTable
{
/// <summary>
/// Gets the value of the database column `alliance_id`.
/// </summary>
DemoGame.Server.AllianceID AllianceId
{
get;
}
/// <summary>
/// Gets the value of the database column `attackable_id`.
/// </summary>
DemoGame.Server.AllianceID AttackableId
{
get;
}
/// <summary>
/// Gets the value of the database column `placeholder`.
/// </summary>
System.Nullable<System.Byte> Placeholder
{
get;
}
}

/// <summary>
/// Provides a strongly-typed structure for the database table `alliance_attackable`.
/// </summary>
public class AllianceAttackableTable : IAllianceAttackableTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"alliance_id", "attackable_id", "placeholder" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"alliance_id", "attackable_id" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"placeholder" };
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
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "alliance_attackable";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 3;
/// <summary>
/// The field that maps onto the database column `alliance_id`.
/// </summary>
System.Byte _allianceId;
/// <summary>
/// The field that maps onto the database column `attackable_id`.
/// </summary>
System.Byte _attackableId;
/// <summary>
/// The field that maps onto the database column `placeholder`.
/// </summary>
System.Nullable<System.Byte> _placeholder;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `alliance_id`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public DemoGame.Server.AllianceID AllianceId
{
get
{
return (DemoGame.Server.AllianceID)_allianceId;
}
set
{
this._allianceId = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `attackable_id`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public DemoGame.Server.AllianceID AttackableId
{
get
{
return (DemoGame.Server.AllianceID)_attackableId;
}
set
{
this._attackableId = (System.Byte)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `placeholder`.
/// The underlying database type is `tinyint(3) unsigned`. The database column contains the comment: 
/// "Unused placeholder column - please do not remove".
/// </summary>
public System.Nullable<System.Byte> Placeholder
{
get
{
return (System.Nullable<System.Byte>)_placeholder;
}
set
{
this._placeholder = (System.Nullable<System.Byte>)value;
}
}

/// <summary>
/// AllianceAttackableTable constructor.
/// </summary>
public AllianceAttackableTable()
{
}
/// <summary>
/// AllianceAttackableTable constructor.
/// </summary>
/// <param name="allianceId">The initial value for the corresponding property.</param>
/// <param name="attackableId">The initial value for the corresponding property.</param>
/// <param name="placeholder">The initial value for the corresponding property.</param>
public AllianceAttackableTable(DemoGame.Server.AllianceID @allianceId, DemoGame.Server.AllianceID @attackableId, System.Nullable<System.Byte> @placeholder)
{
AllianceId = (DemoGame.Server.AllianceID)@allianceId;
AttackableId = (DemoGame.Server.AllianceID)@attackableId;
Placeholder = (System.Nullable<System.Byte>)@placeholder;
}
/// <summary>
/// AllianceAttackableTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public AllianceAttackableTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
public AllianceAttackableTable(IAllianceAttackableTable source)
{
CopyValuesFrom(source);
}
/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public void ReadValues(System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("alliance_id");
AllianceId = (DemoGame.Server.AllianceID)(DemoGame.Server.AllianceID)dataReader.GetByte(i);

i = dataReader.GetOrdinal("attackable_id");
AttackableId = (DemoGame.Server.AllianceID)(DemoGame.Server.AllianceID)dataReader.GetByte(i);

i = dataReader.GetOrdinal("placeholder");
Placeholder = (System.Nullable<System.Byte>)(System.Nullable<System.Byte>)(dataReader.IsDBNull(i) ? (System.Nullable<System.Byte>)null : dataReader.GetByte(i));
}

/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="dic">The Dictionary to copy the values into.</param>
public void CopyValues(System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
CopyValues(this, dic);
}
/// <summary>
/// Copies the column values into the given Dictionary using the database column name
/// with a prefixed @ as the key. The keys must already exist in the Dictionary;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="dic">The Dictionary to copy the values into.</param>
public static void CopyValues(IAllianceAttackableTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@alliance_id"] = (DemoGame.Server.AllianceID)source.AllianceId;
dic["@attackable_id"] = (DemoGame.Server.AllianceID)source.AttackableId;
dic["@placeholder"] = (System.Nullable<System.Byte>)source.Placeholder;
}

/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public void CopyValues(NetGore.Db.DbParameterValues paramValues)
{
CopyValues(this, paramValues);
}
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(IAllianceAttackableTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@alliance_id"] = (DemoGame.Server.AllianceID)source.AllianceId;
paramValues["@attackable_id"] = (DemoGame.Server.AllianceID)source.AttackableId;
paramValues["@placeholder"] = (System.Nullable<System.Byte>)source.Placeholder;
}

public void CopyValuesFrom(IAllianceAttackableTable source)
{
AllianceId = (DemoGame.Server.AllianceID)source.AllianceId;
AttackableId = (DemoGame.Server.AllianceID)source.AttackableId;
Placeholder = (System.Nullable<System.Byte>)source.Placeholder;
}

public System.Object GetValue(System.String columnName)
{
switch (columnName)
{
case "alliance_id":
return AllianceId;

case "attackable_id":
return AttackableId;

case "placeholder":
return Placeholder;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public void SetValue(System.String columnName, System.Object value)
{
switch (columnName)
{
case "alliance_id":
AllianceId = (DemoGame.Server.AllianceID)value;
break;

case "attackable_id":
AttackableId = (DemoGame.Server.AllianceID)value;
break;

case "placeholder":
Placeholder = (System.Nullable<System.Byte>)value;
break;

default:
throw new ArgumentException("Field not found.","columnName");
}
}

public static ColumnMetadata GetColumnData(System.String fieldName)
{
switch (fieldName)
{
case "alliance_id":
return new ColumnMetadata("alliance_id", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, true, false);

case "attackable_id":
return new ColumnMetadata("attackable_id", "", "tinyint(3) unsigned", null, typeof(System.Byte), false, true, false);

case "placeholder":
return new ColumnMetadata("placeholder", "Unused placeholder column - please do not remove", "tinyint(3) unsigned", null, typeof(System.Nullable<System.Byte>), true, false, false);

default:
throw new ArgumentException("Field not found.","fieldName");
}
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
public void TryReadValues(System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "alliance_id":
AllianceId = (DemoGame.Server.AllianceID)(DemoGame.Server.AllianceID)dataReader.GetByte(i);
break;


case "attackable_id":
AttackableId = (DemoGame.Server.AllianceID)(DemoGame.Server.AllianceID)dataReader.GetByte(i);
break;


case "placeholder":
Placeholder = (System.Nullable<System.Byte>)(System.Nullable<System.Byte>)(dataReader.IsDBNull(i) ? (System.Nullable<System.Byte>)null : dataReader.GetByte(i));
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
public void TryCopyValues(NetGore.Db.DbParameterValues paramValues)
{
TryCopyValues(this, paramValues);
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
public static void TryCopyValues(IAllianceAttackableTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@alliance_id":
paramValues[i] = source.AllianceId;
break;


case "@attackable_id":
paramValues[i] = source.AttackableId;
break;


case "@placeholder":
paramValues[i] = source.Placeholder;
break;


}

}
}

}

}
