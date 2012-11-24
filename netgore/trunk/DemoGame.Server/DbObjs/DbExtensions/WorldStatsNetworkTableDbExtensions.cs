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
/// Contains extension methods for class WorldStatsNetworkTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class WorldStatsNetworkTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IWorldStatsNetworkTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["connections"] = (System.UInt16)source.Connections;
paramValues["id"] = (System.UInt32)source.ID;
paramValues["recv_bytes"] = (System.UInt32)source.RecvBytes;
paramValues["recv_messages"] = (System.UInt32)source.RecvMessages;
paramValues["recv_packets"] = (System.UInt32)source.RecvPackets;
paramValues["sent_bytes"] = (System.UInt32)source.SentBytes;
paramValues["sent_messages"] = (System.UInt32)source.SentMessages;
paramValues["sent_packets"] = (System.UInt32)source.SentPackets;
paramValues["when"] = (System.DateTime)source.When;
}

/// <summary>
/// Reads the values from an <see cref="IDataRecord"/> and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataRecord"/> to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this WorldStatsNetworkTable source, System.Data.IDataRecord dataRecord)
{
System.Int32 i;

i = dataRecord.GetOrdinal("connections");

source.Connections = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);

i = dataRecord.GetOrdinal("id");

source.ID = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);

i = dataRecord.GetOrdinal("recv_bytes");

source.RecvBytes = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);

i = dataRecord.GetOrdinal("recv_messages");

source.RecvMessages = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);

i = dataRecord.GetOrdinal("recv_packets");

source.RecvPackets = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);

i = dataRecord.GetOrdinal("sent_bytes");

source.SentBytes = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);

i = dataRecord.GetOrdinal("sent_messages");

source.SentMessages = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);

i = dataRecord.GetOrdinal("sent_packets");

source.SentPackets = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);

i = dataRecord.GetOrdinal("when");

source.When = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);
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
public static void TryReadValues(this WorldStatsNetworkTable source, System.Data.IDataRecord dataRecord)
{
for (int i = 0; i < dataRecord.FieldCount; i++)
{
switch (dataRecord.GetName(i))
{
case "connections":
source.Connections = (System.UInt16)(System.UInt16)dataRecord.GetUInt16(i);
break;


case "id":
source.ID = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);
break;


case "recv_bytes":
source.RecvBytes = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);
break;


case "recv_messages":
source.RecvMessages = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);
break;


case "recv_packets":
source.RecvPackets = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);
break;


case "sent_bytes":
source.SentBytes = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);
break;


case "sent_messages":
source.SentMessages = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);
break;


case "sent_packets":
source.SentPackets = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);
break;


case "when":
source.When = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);
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
public static void TryCopyValues(this IWorldStatsNetworkTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "connections":
paramValues[i] = (System.UInt16)source.Connections;
break;


case "id":
paramValues[i] = (System.UInt32)source.ID;
break;


case "recv_bytes":
paramValues[i] = (System.UInt32)source.RecvBytes;
break;


case "recv_messages":
paramValues[i] = (System.UInt32)source.RecvMessages;
break;


case "recv_packets":
paramValues[i] = (System.UInt32)source.RecvPackets;
break;


case "sent_bytes":
paramValues[i] = (System.UInt32)source.SentBytes;
break;


case "sent_messages":
paramValues[i] = (System.UInt32)source.SentMessages;
break;


case "sent_packets":
paramValues[i] = (System.UInt32)source.SentPackets;
break;


case "when":
paramValues[i] = (System.DateTime)source.When;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="IWorldStatsNetworkTable"/> contains the same values as another <see cref="IWorldStatsNetworkTable"/>.
/// </summary>
/// <param name="source">The source <see cref="IWorldStatsNetworkTable"/>.</param>
/// <param name="otherItem">The <see cref="IWorldStatsNetworkTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="IWorldStatsNetworkTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
/// </returns>
public static System.Boolean HasSameValues(this IWorldStatsNetworkTable source, IWorldStatsNetworkTable otherItem)
{
return Equals(source.Connections, otherItem.Connections) && 
Equals(source.ID, otherItem.ID) && 
Equals(source.RecvBytes, otherItem.RecvBytes) && 
Equals(source.RecvMessages, otherItem.RecvMessages) && 
Equals(source.RecvPackets, otherItem.RecvPackets) && 
Equals(source.SentBytes, otherItem.SentBytes) && 
Equals(source.SentMessages, otherItem.SentMessages) && 
Equals(source.SentPackets, otherItem.SentPackets) && 
Equals(source.When, otherItem.When);
}

}

}
