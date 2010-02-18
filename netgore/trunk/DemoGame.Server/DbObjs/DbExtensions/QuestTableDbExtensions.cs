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
/// Contains extension methods for class QuestTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class QuestTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IQuestTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["@description"] = (System.String)source.Description;
paramValues["@id"] = (System.Int32)source.ID;
paramValues["@name"] = (System.String)source.Name;
paramValues["@repeatable"] = (System.Boolean)source.Repeatable;
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this QuestTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("description");

source.Description = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("id");

source.ID = (System.Int32)(System.Int32)dataReader.GetInt32(i);

i = dataReader.GetOrdinal("name");

source.Name = (System.String)(System.String)dataReader.GetString(i);

i = dataReader.GetOrdinal("repeatable");

source.Repeatable = (System.Boolean)(System.Boolean)dataReader.GetBoolean(i);
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
public static void TryReadValues(this QuestTable source, System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "description":
source.Description = (System.String)(System.String)dataReader.GetString(i);
break;


case "id":
source.ID = (System.Int32)(System.Int32)dataReader.GetInt32(i);
break;


case "name":
source.Name = (System.String)(System.String)dataReader.GetString(i);
break;


case "repeatable":
source.Repeatable = (System.Boolean)(System.Boolean)dataReader.GetBoolean(i);
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
public static void TryCopyValues(this IQuestTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "@description":
paramValues[i] = (System.String)source.Description;
break;


case "@id":
paramValues[i] = (System.Int32)source.ID;
break;


case "@name":
paramValues[i] = (System.String)source.Name;
break;


case "@repeatable":
paramValues[i] = (System.Boolean)source.Repeatable;
break;


}

}
}

}

}
