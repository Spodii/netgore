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
/// Provides a strongly-typed structure for the database table `world_stats_network`.
/// </summary>
public class WorldStatsNetworkTable : IWorldStatsNetworkTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"connections", "id", "recv_bytes", "recv_messages", "recv_packets", "sent_bytes", "sent_messages", "sent_packets", "when" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"connections", "recv_bytes", "recv_messages", "recv_packets", "sent_bytes", "sent_messages", "sent_packets", "when" };
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
public const System.Int32 ColumnCount = 9;
/// <summary>
/// The field that maps onto the database column `connections`.
/// </summary>
System.UInt16 _connections;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt32 _iD;
/// <summary>
/// The field that maps onto the database column `recv_bytes`.
/// </summary>
System.UInt32 _recvBytes;
/// <summary>
/// The field that maps onto the database column `recv_messages`.
/// </summary>
System.UInt32 _recvMessages;
/// <summary>
/// The field that maps onto the database column `recv_packets`.
/// </summary>
System.UInt32 _recvPackets;
/// <summary>
/// The field that maps onto the database column `sent_bytes`.
/// </summary>
System.UInt32 _sentBytes;
/// <summary>
/// The field that maps onto the database column `sent_messages`.
/// </summary>
System.UInt32 _sentMessages;
/// <summary>
/// The field that maps onto the database column `sent_packets`.
/// </summary>
System.UInt32 _sentPackets;
/// <summary>
/// The field that maps onto the database column `when`.
/// </summary>
System.DateTime _when;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `connections`.
/// The underlying database type is `smallint(5) unsigned`.The database column contains the comment: 
/// "Number of connections to the server at the time of the snapshot.".
/// </summary>
[System.ComponentModel.Description("Number of connections to the server at the time of the snapshot.")]
[NetGore.SyncValueAttribute()]
public System.UInt16 Connections
{
get
{
return (System.UInt16)_connections;
}
set
{
this._connections = (System.UInt16)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `mediumint(8) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.UInt32 ID
{
get
{
return (System.UInt32)_iD;
}
set
{
this._iD = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `recv_bytes`.
/// The underlying database type is `mediumint(8) unsigned`.The database column contains the comment: 
/// "The average bytes received per second since the last snapshot.".
/// </summary>
[System.ComponentModel.Description("The average bytes received per second since the last snapshot.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 RecvBytes
{
get
{
return (System.UInt32)_recvBytes;
}
set
{
this._recvBytes = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `recv_messages`.
/// The underlying database type is `mediumint(8) unsigned`.The database column contains the comment: 
/// "The average messages received per second since the last snapshot.".
/// </summary>
[System.ComponentModel.Description("The average messages received per second since the last snapshot.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 RecvMessages
{
get
{
return (System.UInt32)_recvMessages;
}
set
{
this._recvMessages = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `recv_packets`.
/// The underlying database type is `mediumint(8) unsigned`.The database column contains the comment: 
/// "The average packets received per second since the last snapshot.".
/// </summary>
[System.ComponentModel.Description("The average packets received per second since the last snapshot.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 RecvPackets
{
get
{
return (System.UInt32)_recvPackets;
}
set
{
this._recvPackets = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `sent_bytes`.
/// The underlying database type is `mediumint(8) unsigned`.The database column contains the comment: 
/// "The average bytes sent per second since the last snapshot.".
/// </summary>
[System.ComponentModel.Description("The average bytes sent per second since the last snapshot.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 SentBytes
{
get
{
return (System.UInt32)_sentBytes;
}
set
{
this._sentBytes = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `sent_messages`.
/// The underlying database type is `mediumint(8) unsigned`.The database column contains the comment: 
/// "The average messages sent per second since the last snapshot.".
/// </summary>
[System.ComponentModel.Description("The average messages sent per second since the last snapshot.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 SentMessages
{
get
{
return (System.UInt32)_sentMessages;
}
set
{
this._sentMessages = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `sent_packets`.
/// The underlying database type is `mediumint(8) unsigned`.The database column contains the comment: 
/// "The average packets sent per second since the last snapshot.".
/// </summary>
[System.ComponentModel.Description("The average packets sent per second since the last snapshot.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 SentPackets
{
get
{
return (System.UInt32)_sentPackets;
}
set
{
this._sentPackets = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `when`.
/// The underlying database type is `timestamp` with the default value of `CURRENT_TIMESTAMP`.The database column contains the comment: 
/// "The time the snapshot took place.".
/// </summary>
[System.ComponentModel.Description("The time the snapshot took place.")]
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
public virtual IWorldStatsNetworkTable DeepCopy()
{
return new WorldStatsNetworkTable(this);
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsNetworkTable"/> class.
/// </summary>
public WorldStatsNetworkTable()
{
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsNetworkTable"/> class.
/// </summary>
/// <param name="connections">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="recvBytes">The initial value for the corresponding property.</param>
/// <param name="recvMessages">The initial value for the corresponding property.</param>
/// <param name="recvPackets">The initial value for the corresponding property.</param>
/// <param name="sentBytes">The initial value for the corresponding property.</param>
/// <param name="sentMessages">The initial value for the corresponding property.</param>
/// <param name="sentPackets">The initial value for the corresponding property.</param>
/// <param name="when">The initial value for the corresponding property.</param>
public WorldStatsNetworkTable(System.UInt16 @connections, System.UInt32 @iD, System.UInt32 @recvBytes, System.UInt32 @recvMessages, System.UInt32 @recvPackets, System.UInt32 @sentBytes, System.UInt32 @sentMessages, System.UInt32 @sentPackets, System.DateTime @when)
{
this.Connections = (System.UInt16)@connections;
this.ID = (System.UInt32)@iD;
this.RecvBytes = (System.UInt32)@recvBytes;
this.RecvMessages = (System.UInt32)@recvMessages;
this.RecvPackets = (System.UInt32)@recvPackets;
this.SentBytes = (System.UInt32)@sentBytes;
this.SentMessages = (System.UInt32)@sentMessages;
this.SentPackets = (System.UInt32)@sentPackets;
this.When = (System.DateTime)@when;
}
/// <summary>
/// Initializes a new instance of the <see cref="WorldStatsNetworkTable"/> class.
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
dic["connections"] = (System.UInt16)source.Connections;
dic["id"] = (System.UInt32)source.ID;
dic["recv_bytes"] = (System.UInt32)source.RecvBytes;
dic["recv_messages"] = (System.UInt32)source.RecvMessages;
dic["recv_packets"] = (System.UInt32)source.RecvPackets;
dic["sent_bytes"] = (System.UInt32)source.SentBytes;
dic["sent_messages"] = (System.UInt32)source.SentMessages;
dic["sent_packets"] = (System.UInt32)source.SentPackets;
dic["when"] = (System.DateTime)source.When;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this WorldStatsNetworkTable.
/// </summary>
/// <param name="source">The IWorldStatsNetworkTable to copy the values from.</param>
public void CopyValuesFrom(IWorldStatsNetworkTable source)
{
this.Connections = (System.UInt16)source.Connections;
this.ID = (System.UInt32)source.ID;
this.RecvBytes = (System.UInt32)source.RecvBytes;
this.RecvMessages = (System.UInt32)source.RecvMessages;
this.RecvPackets = (System.UInt32)source.RecvPackets;
this.SentBytes = (System.UInt32)source.SentBytes;
this.SentMessages = (System.UInt32)source.SentMessages;
this.SentPackets = (System.UInt32)source.SentPackets;
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

case "id":
return ID;

case "recv_bytes":
return RecvBytes;

case "recv_messages":
return RecvMessages;

case "recv_packets":
return RecvPackets;

case "sent_bytes":
return SentBytes;

case "sent_messages":
return SentMessages;

case "sent_packets":
return SentPackets;

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
this.Connections = (System.UInt16)value;
break;

case "id":
this.ID = (System.UInt32)value;
break;

case "recv_bytes":
this.RecvBytes = (System.UInt32)value;
break;

case "recv_messages":
this.RecvMessages = (System.UInt32)value;
break;

case "recv_packets":
this.RecvPackets = (System.UInt32)value;
break;

case "sent_bytes":
this.SentBytes = (System.UInt32)value;
break;

case "sent_messages":
this.SentMessages = (System.UInt32)value;
break;

case "sent_packets":
this.SentPackets = (System.UInt32)value;
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
return new ColumnMetadata("connections", "Number of connections to the server at the time of the snapshot.", "smallint(5) unsigned", null, typeof(System.UInt16), false, false, false);

case "id":
return new ColumnMetadata("id", "", "mediumint(8) unsigned", null, typeof(System.UInt32), false, true, false);

case "recv_bytes":
return new ColumnMetadata("recv_bytes", "The average bytes received per second since the last snapshot.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "recv_messages":
return new ColumnMetadata("recv_messages", "The average messages received per second since the last snapshot.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "recv_packets":
return new ColumnMetadata("recv_packets", "The average packets received per second since the last snapshot.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "sent_bytes":
return new ColumnMetadata("sent_bytes", "The average bytes sent per second since the last snapshot.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "sent_messages":
return new ColumnMetadata("sent_messages", "The average messages sent per second since the last snapshot.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "sent_packets":
return new ColumnMetadata("sent_packets", "The average packets sent per second since the last snapshot.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "when":
return new ColumnMetadata("when", "The time the snapshot took place.", "timestamp", "CURRENT_TIMESTAMP", typeof(System.DateTime), false, false, false);

default:
throw new ArgumentException("Field not found.","columnName");
}
}

/// <summary>
/// Reads the state of the object from an <see cref="IValueReader"/>.
/// </summary>
/// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
public virtual void ReadState(NetGore.IO.IValueReader reader)
{
NetGore.IO.PersistableHelper.Read(this, reader);
}

/// <summary>
/// Writes the state of the object to an <see cref="IValueWriter"/>.
/// </summary>
/// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
public virtual void WriteState(NetGore.IO.IValueWriter writer)
{
NetGore.IO.PersistableHelper.Write(this, writer);
}

}

}
