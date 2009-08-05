using System;
using System.Linq;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `alliance_hostile`.
/// </summary>
public interface IAllianceHostileTable
{
/// <summary>
/// Gets the value for the database column `alliance_id`.
/// </summary>
System.Byte AllianceId
{
get;
}
/// <summary>
/// Gets the value for the database column `hostile_id`.
/// </summary>
System.Byte HostileId
{
get;
}
/// <summary>
/// Gets the value for the database column `placeholder`.
/// </summary>
System.Byte Placeholder
{
get;
}
}
/// <summary>
/// Provides a strongly-typed structure for the database table `alliance_hostile`.
/// </summary>
public class AllianceHostileTable : IAllianceHostileTable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"alliance_id", "hostile_id", "placeholder" };
/// <summary>
/// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
/// </summary>
public System.Collections.Generic.IEnumerable<System.String> DbColumns
{
get
{
return _dbColumns;
}
}
/// <summary>
/// The name of the database table that this class represents.
/// </summary>
public const System.String TableName = "alliance_hostile";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 3;
/// <summary>
/// The field that maps onto the database column `alliance_id`.
/// </summary>
System.Byte _allianceId;
/// <summary>
/// The field that maps onto the database column `hostile_id`.
/// </summary>
System.Byte _hostileId;
/// <summary>
/// The field that maps onto the database column `placeholder`.
/// </summary>
System.Byte _placeholder;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `alliance_id`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public System.Byte AllianceId
{
get
{
return _allianceId;
}
set
{
this._allianceId = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `hostile_id`.
/// The underlying database type is `tinyint(3) unsigned`.
/// </summary>
public System.Byte HostileId
{
get
{
return _hostileId;
}
set
{
this._hostileId = value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `placeholder`.
/// The underlying database type is `tinyint(3) unsigned`. The database column contains the comment: 
/// "Unused placeholder column - please do not remove".
/// </summary>
public System.Byte Placeholder
{
get
{
return _placeholder;
}
set
{
this._placeholder = value;
}
}

/// <summary>
/// AllianceHostileTable constructor.
/// </summary>
public AllianceHostileTable()
{
}
/// <summary>
/// AllianceHostileTable constructor.
/// </summary>
/// <param name="allianceId">The initial value for the corresponding property.</param>
/// <param name="hostileId">The initial value for the corresponding property.</param>
/// <param name="placeholder">The initial value for the corresponding property.</param>
public AllianceHostileTable(System.Byte @allianceId, System.Byte @hostileId, System.Byte @placeholder)
{
this.AllianceId = @allianceId;
this.HostileId = @hostileId;
this.Placeholder = @placeholder;
}
/// <summary>
/// AllianceHostileTable constructor.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. See method ReadValues() for details.</param>
public AllianceHostileTable(System.Data.IDataReader dataReader)
{
ReadValues(dataReader);
}
/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public void ReadValues(System.Data.IDataReader dataReader)
{
this.AllianceId = (System.Byte)dataReader.GetValue(dataReader.GetOrdinal("alliance_id"));
this.HostileId = (System.Byte)dataReader.GetValue(dataReader.GetOrdinal("hostile_id"));
this.Placeholder = (System.Byte)dataReader.GetValue(dataReader.GetOrdinal("placeholder"));
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
public static void CopyValues(IAllianceHostileTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@alliance_id"] = (System.Byte)source.AllianceId;
dic["@hostile_id"] = (System.Byte)source.HostileId;
dic["@placeholder"] = (System.Byte)source.Placeholder;
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
public static void CopyValues(IAllianceHostileTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@alliance_id"] = (System.Byte)source.AllianceId;
paramValues["@hostile_id"] = (System.Byte)source.HostileId;
paramValues["@placeholder"] = (System.Byte)source.Placeholder;
}

}
}
