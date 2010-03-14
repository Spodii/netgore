using System;
using System.Linq;
using NetGore;
using NetGore.IO;
using System.Collections.Generic;
using System.Collections;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Contains extension methods for class AllianceHostileTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class AllianceHostileTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IAllianceHostileTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@alliance_id"] = (System.Byte)source.AllianceID;
paramValues["@hostile_id"] = (System.Byte)source.HostileID;
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this AllianceHostileTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("alliance_id");

source.AllianceID = (DemoGame.AllianceID)(DemoGame.AllianceID)dataReader.GetByte(i);

i = dataReader.GetOrdinal("hostile_id");

source.HostileID = (DemoGame.AllianceID)(DemoGame.AllianceID)dataReader.GetByte(i);
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. Unlike ReadValues(), this method not only doesn't require
/// all values to be in the IDataReader, but also does not require the values in
/// the IDataReader to be a defined field for the table this class represents.
/// Because of this, you need to be careful when using this method because values
/// can easily be skipped without any indication.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void TryReadValues(this AllianceHostileTable source, System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "alliance_id":
source.AllianceID = (DemoGame.AllianceID)(DemoGame.AllianceID)dataReader.GetByte(i);
break;


case "hostile_id":
source.HostileID = (DemoGame.AllianceID)(DemoGame.AllianceID)dataReader.GetByte(i);
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
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void TryCopyValues(this IAllianceHostileTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@alliance_id":
paramValues[i] = (System.Byte)source.AllianceID;
break;


case "@hostile_id":
paramValues[i] = (System.Byte)source.HostileID;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="IAllianceHostileTable"/> contains the same values as an<paramref name="other"/> <see cref="IAllianceHostileTable"/>.
/// </summary>
/// <param name="other">The <see cref="IAllianceHostileTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="IAllianceHostileTable"/> contains the same values as the <paramref name="<paramref name="other"/>"/>; <paramref name="other"/>wise false.
/// </returns>
public static System.Boolean HasSameValues(this IAllianceHostileTable source, IAllianceHostileTable other)
{
return Equals(source.AllianceID, other.AllianceID) && 
Equals(source.HostileID, other.HostileID);
}

}

}
