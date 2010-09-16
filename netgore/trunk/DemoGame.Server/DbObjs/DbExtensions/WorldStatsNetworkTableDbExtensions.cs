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
    http://www.netgore.com/wiki/dbclasscreator.html
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
paramValues["connections"] = (System.UInt32)source.Connections;
paramValues["connections_rejected"] = (System.UInt32)source.ConnectionsRejected;
paramValues["id"] = (System.UInt32)source.ID;
paramValues["recv"] = (System.UInt32)source.Recv;
paramValues["recvs"] = (System.UInt32)source.Recvs;
paramValues["sends"] = (System.UInt32)source.Sends;
paramValues["sent"] = (System.UInt32)source.Sent;
paramValues["when"] = (System.DateTime)source.When;
}

/// <summary>
/// Reads the values from an IDataReader and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataReader">The IDataReader to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this WorldStatsNetworkTable source, System.Data.IDataReader dataReader)
{
System.Int32 i;

i = dataReader.GetOrdinal("connections");

source.Connections = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("connections_rejected");

source.ConnectionsRejected = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("id");

source.ID = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("recv");

source.Recv = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("recvs");

source.Recvs = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("sends");

source.Sends = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("sent");

source.Sent = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("when");

source.When = (System.DateTime)(System.DateTime)dataReader.GetDateTime(i);
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
public static void TryReadValues(this WorldStatsNetworkTable source, System.Data.IDataReader dataReader)
{
for (int i = 0; i < dataReader.FieldCount; i++)
{
switch (dataReader.GetName(i))
{
case "connections":
source.Connections = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "connections_rejected":
source.ConnectionsRejected = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "id":
source.ID = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "recv":
source.Recv = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "recvs":
source.Recvs = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "sends":
source.Sends = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "sent":
source.Sent = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "when":
source.When = (System.DateTime)(System.DateTime)dataReader.GetDateTime(i);
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
paramValues[i] = (System.UInt32)source.Connections;
break;


case "connections_rejected":
paramValues[i] = (System.UInt32)source.ConnectionsRejected;
break;


case "id":
paramValues[i] = (System.UInt32)source.ID;
break;


case "recv":
paramValues[i] = (System.UInt32)source.Recv;
break;


case "recvs":
paramValues[i] = (System.UInt32)source.Recvs;
break;


case "sends":
paramValues[i] = (System.UInt32)source.Sends;
break;


case "sent":
paramValues[i] = (System.UInt32)source.Sent;
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
Equals(source.ConnectionsRejected, otherItem.ConnectionsRejected) && 
Equals(source.ID, otherItem.ID) && 
Equals(source.Recv, otherItem.Recv) && 
Equals(source.Recvs, otherItem.Recvs) && 
Equals(source.Sends, otherItem.Sends) && 
Equals(source.Sent, otherItem.Sent) && 
Equals(source.When, otherItem.When);
}

}

}
