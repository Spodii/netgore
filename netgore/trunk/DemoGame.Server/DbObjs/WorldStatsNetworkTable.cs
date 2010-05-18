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

This file was generated on (UTC): 5/17/2010 11:46:58 PM
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
/// Provides a strongly-typed structure for the database table `world_stats_network`.
/// </summary>
public class WorldStatsNetworkTable : IWorldStatsNetworkTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"connections", "tcp_recv", "tcp_recvs", "tcp_sends", "tcp_sent", "udp_recv", "udp_recvs", "udp_sends", "udp_sent", "when" };
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
 static  readonly System.String[] _dbColumnsKeys = new string[] {"when" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"connections", "tcp_recv", "tcp_recvs", "tcp_sends", "tcp_sent", "udp_recv", "udp_recvs", "udp_sends", "udp_sent" };
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
public const System.String TableName = "world_stats_network";
/// <summary>
/// The number of columns in the database table that this class represents.
/// </summary>
public const System.Int32 ColumnCount = 10;
/// <summary>
/// The field that maps onto the database column `connections`.
/// </summary>
System.UInt32 _connections;
/// <summary>
/// The field that maps onto the database column `tcp_recv`.
/// </summary>
System.UInt32 _tcpRecv;
/// <summary>
/// The field that maps onto the database column `tcp_recvs`.
/// </summary>
System.UInt32 _tcpRecvs;
/// <summary>
/// The field that maps onto the database column `tcp_sends`.
/// </summary>
System.UInt32 _tcpSends;
/// <summary>
/// The field that maps onto the database column `tcp_sent`.
/// </summary>
System.UInt32 _tcpSent;
/// <summary>
/// The field that maps onto the database column `udp_recv`.
/// </summary>
System.UInt32 _udpRecv;
/// <summary>
/// The field that maps onto the database column `udp_recvs`.
/// </summary>
System.UInt32 _udpRecvs;
/// <summary>
/// The field that maps onto the database column `udp_sends`.
/// </summary>
System.UInt32 _udpSends;
/// <summary>
/// The field that maps onto the database column `udp_sent`.
/// </summary>
System.UInt32 _udpSent;
/// <summary>
/// The field that maps onto the database column `when`.
/// </summary>
System.DateTime _when;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `connections`.
/// The underlying database type is `mediumint(8) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.UInt32 Connections
{
get
{
return (System.UInt32)_connections;
}
set
{
this._connections = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `tcp_recv`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of bytes that have been received over the TCP channel.".
/// </summary>
[System.ComponentModel.Description("The number of bytes that have been received over the TCP channel.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 TcpRecv
{
get
{
return (System.UInt32)_tcpRecv;
}
set
{
this._tcpRecv = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `tcp_recvs`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of messages that have been received over the TCP channel.".
/// </summary>
[System.ComponentModel.Description("The number of messages that have been received over the TCP channel.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 TcpRecvs
{
get
{
return (System.UInt32)_tcpRecvs;
}
set
{
this._tcpRecvs = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `tcp_sends`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of messages that have been sent over the TCP channel.".
/// </summary>
[System.ComponentModel.Description("The number of messages that have been sent over the TCP channel.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 TcpSends
{
get
{
return (System.UInt32)_tcpSends;
}
set
{
this._tcpSends = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `tcp_sent`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of bytes that have been sent over the TCP channel.".
/// </summary>
[System.ComponentModel.Description("The number of bytes that have been sent over the TCP channel.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 TcpSent
{
get
{
return (System.UInt32)_tcpSent;
}
set
{
this._tcpSent = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `udp_recv`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of bytes that have been received over the UDP channel.".
/// </summary>
[System.ComponentModel.Description("The number of bytes that have been received over the UDP channel.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 UdpRecv
{
get
{
return (System.UInt32)_udpRecv;
}
set
{
this._udpRecv = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `udp_recvs`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of messages that have been received over the UDP channel.".
/// </summary>
[System.ComponentModel.Description("The number of messages that have been received over the UDP channel.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 UdpRecvs
{
get
{
return (System.UInt32)_udpRecvs;
}
set
{
this._udpRecvs = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `udp_sends`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of messages that have been sent over the UDP channel.".
/// </summary>
[System.ComponentModel.Description("The number of messages that have been sent over the UDP channel.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 UdpSends
{
get
{
return (System.UInt32)_udpSends;
}
set
{
this._udpSends = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `udp_sent`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of bytes that have been sent over the UDP channel.".
/// </summary>
[System.ComponentModel.Description("The number of bytes that have been sent over the UDP channel.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 UdpSent
{
get
{
return (System.UInt32)_udpSent;
}
set
{
this._udpSent = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `when`.
/// The underlying database type is `timestamp` with the default value of `CURRENT_TIMESTAMP`. The database column contains the comment: 
/// "When these network stats were logged. The values correspond to a time period defined in the WorldStatsTracker constructor. This timestamp marks the end of this period of time. So all stats correspond to the time frame range: [when - rate, when].".
/// </summary>
[System.ComponentModel.Description("When these network stats were logged. The values correspond to a time period defined in the WorldStatsTracker constructor. This timestamp marks the end of this period of time. So all stats correspond to the time frame range: [when - rate, when].")]
[NetGore.SyncValueAttribute()]
public System.DateTime When
{
get
{
return (System.DateTime)_when;
}
set
{
this._when = (System.DateTime)value;
}
}

/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
public IWorldStatsNetworkTable DeepCopy()
{
return new WorldStatsNetworkTable(this);
}
/// <summary>
/// WorldStatsNetworkTable constructor.
/// </summary>
public WorldStatsNetworkTable()
{
}
/// <summary>
/// WorldStatsNetworkTable constructor.
/// </summary>
/// <param name="connections">The initial value for the corresponding property.</param>
/// <param name="tcpRecv">The initial value for the corresponding property.</param>
/// <param name="tcpRecvs">The initial value for the corresponding property.</param>
/// <param name="tcpSends">The initial value for the corresponding property.</param>
/// <param name="tcpSent">The initial value for the corresponding property.</param>
/// <param name="udpRecv">The initial value for the corresponding property.</param>
/// <param name="udpRecvs">The initial value for the corresponding property.</param>
/// <param name="udpSends">The initial value for the corresponding property.</param>
/// <param name="udpSent">The initial value for the corresponding property.</param>
/// <param name="when">The initial value for the corresponding property.</param>
public WorldStatsNetworkTable(System.UInt32 @connections, System.UInt32 @tcpRecv, System.UInt32 @tcpRecvs, System.UInt32 @tcpSends, System.UInt32 @tcpSent, System.UInt32 @udpRecv, System.UInt32 @udpRecvs, System.UInt32 @udpSends, System.UInt32 @udpSent, System.DateTime @when)
{
this.Connections = (System.UInt32)@connections;
this.TcpRecv = (System.UInt32)@tcpRecv;
this.TcpRecvs = (System.UInt32)@tcpRecvs;
this.TcpSends = (System.UInt32)@tcpSends;
this.TcpSent = (System.UInt32)@tcpSent;
this.UdpRecv = (System.UInt32)@udpRecv;
this.UdpRecvs = (System.UInt32)@udpRecvs;
this.UdpSends = (System.UInt32)@udpSends;
this.UdpSent = (System.UInt32)@udpSent;
this.When = (System.DateTime)@when;
}
/// <summary>
/// WorldStatsNetworkTable constructor.
/// </summary>
/// <param name="source">IWorldStatsNetworkTable to copy the initial values from.</param>
public WorldStatsNetworkTable(IWorldStatsNetworkTable source)
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
public static void CopyValues(IWorldStatsNetworkTable source, System.Collections.Generic.IDictionary<System.String,System.Object> dic)
{
dic["@connections"] = (System.UInt32)source.Connections;
dic["@tcp_recv"] = (System.UInt32)source.TcpRecv;
dic["@tcp_recvs"] = (System.UInt32)source.TcpRecvs;
dic["@tcp_sends"] = (System.UInt32)source.TcpSends;
dic["@tcp_sent"] = (System.UInt32)source.TcpSent;
dic["@udp_recv"] = (System.UInt32)source.UdpRecv;
dic["@udp_recvs"] = (System.UInt32)source.UdpRecvs;
dic["@udp_sends"] = (System.UInt32)source.UdpSends;
dic["@udp_sent"] = (System.UInt32)source.UdpSent;
dic["@when"] = (System.DateTime)source.When;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this WorldStatsNetworkTable.
/// </summary>
/// <param name="source">The IWorldStatsNetworkTable to copy the values from.</param>
public void CopyValuesFrom(IWorldStatsNetworkTable source)
{
this.Connections = (System.UInt32)source.Connections;
this.TcpRecv = (System.UInt32)source.TcpRecv;
this.TcpRecvs = (System.UInt32)source.TcpRecvs;
this.TcpSends = (System.UInt32)source.TcpSends;
this.TcpSent = (System.UInt32)source.TcpSent;
this.UdpRecv = (System.UInt32)source.UdpRecv;
this.UdpRecvs = (System.UInt32)source.UdpRecvs;
this.UdpSends = (System.UInt32)source.UdpSends;
this.UdpSent = (System.UInt32)source.UdpSent;
this.When = (System.DateTime)source.When;
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
case "connections":
return Connections;

case "tcp_recv":
return TcpRecv;

case "tcp_recvs":
return TcpRecvs;

case "tcp_sends":
return TcpSends;

case "tcp_sent":
return TcpSent;

case "udp_recv":
return UdpRecv;

case "udp_recvs":
return UdpRecvs;

case "udp_sends":
return UdpSends;

case "udp_sent":
return UdpSent;

case "when":
return When;

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
case "connections":
this.Connections = (System.UInt32)value;
break;

case "tcp_recv":
this.TcpRecv = (System.UInt32)value;
break;

case "tcp_recvs":
this.TcpRecvs = (System.UInt32)value;
break;

case "tcp_sends":
this.TcpSends = (System.UInt32)value;
break;

case "tcp_sent":
this.TcpSent = (System.UInt32)value;
break;

case "udp_recv":
this.UdpRecv = (System.UInt32)value;
break;

case "udp_recvs":
this.UdpRecvs = (System.UInt32)value;
break;

case "udp_sends":
this.UdpSends = (System.UInt32)value;
break;

case "udp_sent":
this.UdpSent = (System.UInt32)value;
break;

case "when":
this.When = (System.DateTime)value;
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
case "connections":
return new ColumnMetadata("connections", "", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "tcp_recv":
return new ColumnMetadata("tcp_recv", "The number of bytes that have been received over the TCP channel.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "tcp_recvs":
return new ColumnMetadata("tcp_recvs", "The number of messages that have been received over the TCP channel.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "tcp_sends":
return new ColumnMetadata("tcp_sends", "The number of messages that have been sent over the TCP channel.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "tcp_sent":
return new ColumnMetadata("tcp_sent", "The number of bytes that have been sent over the TCP channel.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "udp_recv":
return new ColumnMetadata("udp_recv", "The number of bytes that have been received over the UDP channel.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "udp_recvs":
return new ColumnMetadata("udp_recvs", "The number of messages that have been received over the UDP channel.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "udp_sends":
return new ColumnMetadata("udp_sends", "The number of messages that have been sent over the UDP channel.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "udp_sent":
return new ColumnMetadata("udp_sent", "The number of bytes that have been sent over the UDP channel.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "when":
return new ColumnMetadata("when", "When these network stats were logged. The values correspond to a time period defined in the WorldStatsTracker constructor. This timestamp marks the end of this period of time. So all stats correspond to the time frame range: [when - rate, when].", "timestamp", "CURRENT_TIMESTAMP", typeof(System.DateTime), false, true, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Reads the state of the object from an <see cref="IValueReader"/>.
/// </summary>
/// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
public void ReadState(NetGore.IO.IValueReader reader)
{
NetGore.IO.PersistableHelper.Read(this, reader);
}

/// <summary>
/// Writes the state of the object to an <see cref="IValueWriter"/>.
/// </summary>
/// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
public void WriteState(NetGore.IO.IValueWriter writer)
{
NetGore.IO.PersistableHelper.Write(this, writer);
}

}

}
