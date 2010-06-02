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

This file was generated on (UTC): 6/2/2010 10:29:24 PM
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
paramValues["tcp_recv"] = (System.UInt32)source.TcpRecv;
paramValues["tcp_recvs"] = (System.UInt32)source.TcpRecvs;
paramValues["tcp_sends"] = (System.UInt32)source.TcpSends;
paramValues["tcp_sent"] = (System.UInt32)source.TcpSent;
paramValues["udp_recv"] = (System.UInt32)source.UdpRecv;
paramValues["udp_recvs"] = (System.UInt32)source.UdpRecvs;
paramValues["udp_sends"] = (System.UInt32)source.UdpSends;
paramValues["udp_sent"] = (System.UInt32)source.UdpSent;
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

i = dataReader.GetOrdinal("tcp_recv");

source.TcpRecv = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("tcp_recvs");

source.TcpRecvs = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("tcp_sends");

source.TcpSends = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("tcp_sent");

source.TcpSent = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("udp_recv");

source.UdpRecv = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("udp_recvs");

source.UdpRecvs = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("udp_sends");

source.UdpSends = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

i = dataReader.GetOrdinal("udp_sent");

source.UdpSent = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);

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


case "tcp_recv":
source.TcpRecv = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "tcp_recvs":
source.TcpRecvs = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "tcp_sends":
source.TcpSends = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "tcp_sent":
source.TcpSent = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "udp_recv":
source.UdpRecv = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "udp_recvs":
source.UdpRecvs = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "udp_sends":
source.UdpSends = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
break;


case "udp_sent":
source.UdpSent = (System.UInt32)(System.UInt32)dataReader.GetUInt32(i);
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


case "tcp_recv":
paramValues[i] = (System.UInt32)source.TcpRecv;
break;


case "tcp_recvs":
paramValues[i] = (System.UInt32)source.TcpRecvs;
break;


case "tcp_sends":
paramValues[i] = (System.UInt32)source.TcpSends;
break;


case "tcp_sent":
paramValues[i] = (System.UInt32)source.TcpSent;
break;


case "udp_recv":
paramValues[i] = (System.UInt32)source.UdpRecv;
break;


case "udp_recvs":
paramValues[i] = (System.UInt32)source.UdpRecvs;
break;


case "udp_sends":
paramValues[i] = (System.UInt32)source.UdpSends;
break;


case "udp_sent":
paramValues[i] = (System.UInt32)source.UdpSent;
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
Equals(source.TcpRecv, otherItem.TcpRecv) && 
Equals(source.TcpRecvs, otherItem.TcpRecvs) && 
Equals(source.TcpSends, otherItem.TcpSends) && 
Equals(source.TcpSent, otherItem.TcpSent) && 
Equals(source.UdpRecv, otherItem.UdpRecv) && 
Equals(source.UdpRecvs, otherItem.UdpRecvs) && 
Equals(source.UdpSends, otherItem.UdpSends) && 
Equals(source.UdpSent, otherItem.UdpSent) && 
Equals(source.When, otherItem.When);
}

}

}
