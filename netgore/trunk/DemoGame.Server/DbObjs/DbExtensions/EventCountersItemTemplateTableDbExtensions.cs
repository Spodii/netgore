/********************************************************************
                   DO NOT MANUALLY EDIT THIS FILE!

This file was automatically generated using the DbClassCreator
program. The only time you should ever alter this file is if you are
using an automated code formatter. The DbClassCreator will overwrite
this file every time it is run, so all manual changes will be lost.
If there is something in this file that you wish to change, you should
be able to do it through the DbClassCreator arguments.

Make sure that you re-run the DbClassCreator every time you alter your
game's database.

For more information on the DbClassCreator, please see:
    http://www.netgore.com/wiki/DbClassCreator
********************************************************************/

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
/// Contains extension methods for class EventCountersItemTemplateTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class EventCountersItemTemplateTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IEventCountersItemTemplateTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["counter"] = (System.Int64)source.Counter;
paramValues["item_template_event_counter_id"] = (System.Byte)source.ItemTemplateEventCounterId;
paramValues["item_template_id"] = (System.UInt16)source.ItemTemplateID;
}

/// <summary>
/// Reads the values from an <see cref="IDataRecord"/> and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataRecord"/> to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this EventCountersItemTemplateTable source, System.Data.IDataRecord dataRecord)
{
System.Int32 i;

i = dataRecord.GetOrdinal("counter");

source.Counter = (System.Int64)(System.Int64)dataRecord.GetInt64(i);

i = dataRecord.GetOrdinal("item_template_event_counter_id");

source.ItemTemplateEventCounterId = (System.Byte)(System.Byte)dataRecord.GetByte(i);

i = dataRecord.GetOrdinal("item_template_id");

source.ItemTemplateID = (DemoGame.ItemTemplateID)(DemoGame.ItemTemplateID)dataRecord.GetUInt16(i);
}

/// <summary>
/// Reads the values from an <see cref="IDataReader"/> and assigns the read values to this
/// object's properties. Unlike ReadValues(), this method not only doesn't require
/// all values to be in the <see cref="IDataReader"/>, but also does not require the values in
/// the <see cref="IDataReader"/> to be a defined field for the table this class represents.
/// Because of this, you need to be careful when using this method because values
/// can easily be skipped without any indication.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataReader"/> to read the values from. Must already be ready to be read from.</param>
public static void TryReadValues(this EventCountersItemTemplateTable source, System.Data.IDataRecord dataRecord)
{
for (int i = 0; i < dataRecord.FieldCount; i++)
{
switch (dataRecord.GetName(i))
{
case "counter":
source.Counter = (System.Int64)(System.Int64)dataRecord.GetInt64(i);
break;


case "item_template_event_counter_id":
source.ItemTemplateEventCounterId = (System.Byte)(System.Byte)dataRecord.GetByte(i);
break;


case "item_template_id":
source.ItemTemplateID = (DemoGame.ItemTemplateID)(DemoGame.ItemTemplateID)dataRecord.GetUInt16(i);
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
public static void TryCopyValues(this IEventCountersItemTemplateTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "counter":
paramValues[i] = (System.Int64)source.Counter;
break;


case "item_template_event_counter_id":
paramValues[i] = (System.Byte)source.ItemTemplateEventCounterId;
break;


case "item_template_id":
paramValues[i] = (System.UInt16)source.ItemTemplateID;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="IEventCountersItemTemplateTable"/> contains the same values as another <see cref="IEventCountersItemTemplateTable"/>.
/// </summary>
/// <param name="source">The source <see cref="IEventCountersItemTemplateTable"/>.</param>
/// <param name="otherItem">The <see cref="IEventCountersItemTemplateTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="IEventCountersItemTemplateTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
/// </returns>
public static System.Boolean HasSameValues(this IEventCountersItemTemplateTable source, IEventCountersItemTemplateTable otherItem)
{
return Equals(source.Counter, otherItem.Counter) && 
Equals(source.ItemTemplateEventCounterId, otherItem.ItemTemplateEventCounterId) && 
Equals(source.ItemTemplateID, otherItem.ItemTemplateID);
}

}

}
