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
/// Provides a strongly-typed structure for the database table `world_stats_network`.
/// </summary>
public class WorldStatsNetworkTable : IWorldStatsNetworkTable, NetGore.IO.IPersistable
{
/// <summary>
/// Array of the database column names.
/// </summary>
 static  readonly System.String[] _dbColumns = new string[] {"connections", "connections_rejected", "id", "recv", "recvs", "sends", "sent", "when" };
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
 static  readonly System.String[] _dbColumnsNonKey = new string[] {"connections", "connections_rejected", "recv", "recvs", "sends", "sent", "when" };
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
public const System.Int32 ColumnCount = 8;
/// <summary>
/// The field that maps onto the database column `connections`.
/// </summary>
System.UInt32 _connections;
/// <summary>
/// The field that maps onto the database column `connections_rejected`.
/// </summary>
System.UInt32 _connectionsRejected;
/// <summary>
/// The field that maps onto the database column `id`.
/// </summary>
System.UInt32 _iD;
/// <summary>
/// The field that maps onto the database column `recv`.
/// </summary>
System.UInt32 _recv;
/// <summary>
/// The field that maps onto the database column `recvs`.
/// </summary>
System.UInt32 _recvs;
/// <summary>
/// The field that maps onto the database column `sends`.
/// </summary>
System.UInt32 _sends;
/// <summary>
/// The field that maps onto the database column `sent`.
/// </summary>
System.UInt32 _sent;
/// <summary>
/// The field that maps onto the database column `when`.
/// </summary>
System.DateTime _when;
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `connections`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "Connections made with the server (accepted connections).".
/// </summary>
[System.ComponentModel.Description("Connections made with the server (accepted connections).")]
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
/// Gets or sets the value for the field that maps onto the database column `connections_rejected`.
/// The underlying database type is `mediumint(8) unsigned`.
/// </summary>
[NetGore.SyncValueAttribute()]
public System.UInt32 ConnectionsRejected
{
get
{
return (System.UInt32)_connectionsRejected;
}
set
{
this._connectionsRejected = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `id`.
/// The underlying database type is `int(10) unsigned`.
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
/// Gets or sets the value for the field that maps onto the database column `recv`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of bytes that have been received.".
/// </summary>
[System.ComponentModel.Description("The number of bytes that have been received.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 Recv
{
get
{
return (System.UInt32)_recv;
}
set
{
this._recv = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `recvs`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of messages that have been received.".
/// </summary>
[System.ComponentModel.Description("The number of messages that have been received.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 Recvs
{
get
{
return (System.UInt32)_recvs;
}
set
{
this._recvs = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `sends`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of messages that have been sent.".
/// </summary>
[System.ComponentModel.Description("The number of messages that have been sent.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 Sends
{
get
{
return (System.UInt32)_sends;
}
set
{
this._sends = (System.UInt32)value;
}
}
/// <summary>
/// Gets or sets the value for the field that maps onto the database column `sent`.
/// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
/// "The number of bytes that have been sent.".
/// </summary>
[System.ComponentModel.Description("The number of bytes that have been sent.")]
[NetGore.SyncValueAttribute()]
public System.UInt32 Sent
{
get
{
return (System.UInt32)_sent;
}
set
{
this._sent = (System.UInt32)value;
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
/// <param name="connectionsRejected">The initial value for the corresponding property.</param>
/// <param name="iD">The initial value for the corresponding property.</param>
/// <param name="recv">The initial value for the corresponding property.</param>
/// <param name="recvs">The initial value for the corresponding property.</param>
/// <param name="sends">The initial value for the corresponding property.</param>
/// <param name="sent">The initial value for the corresponding property.</param>
/// <param name="when">The initial value for the corresponding property.</param>
public WorldStatsNetworkTable(System.UInt32 @connections, System.UInt32 @connectionsRejected, System.UInt32 @iD, System.UInt32 @recv, System.UInt32 @recvs, System.UInt32 @sends, System.UInt32 @sent, System.DateTime @when)
{
this.Connections = (System.UInt32)@connections;
this.ConnectionsRejected = (System.UInt32)@connectionsRejected;
this.ID = (System.UInt32)@iD;
this.Recv = (System.UInt32)@recv;
this.Recvs = (System.UInt32)@recvs;
this.Sends = (System.UInt32)@sends;
this.Sent = (System.UInt32)@sent;
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
dic["connections"] = (System.UInt32)source.Connections;
dic["connections_rejected"] = (System.UInt32)source.ConnectionsRejected;
dic["id"] = (System.UInt32)source.ID;
dic["recv"] = (System.UInt32)source.Recv;
dic["recvs"] = (System.UInt32)source.Recvs;
dic["sends"] = (System.UInt32)source.Sends;
dic["sent"] = (System.UInt32)source.Sent;
dic["when"] = (System.DateTime)source.When;
}

/// <summary>
/// Copies the values from the given <paramref name="source"/> into this WorldStatsNetworkTable.
/// </summary>
/// <param name="source">The IWorldStatsNetworkTable to copy the values from.</param>
public void CopyValuesFrom(IWorldStatsNetworkTable source)
{
this.Connections = (System.UInt32)source.Connections;
this.ConnectionsRejected = (System.UInt32)source.ConnectionsRejected;
this.ID = (System.UInt32)source.ID;
this.Recv = (System.UInt32)source.Recv;
this.Recvs = (System.UInt32)source.Recvs;
this.Sends = (System.UInt32)source.Sends;
this.Sent = (System.UInt32)source.Sent;
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

case "connections_rejected":
return ConnectionsRejected;

case "id":
return ID;

case "recv":
return Recv;

case "recvs":
return Recvs;

case "sends":
return Sends;

case "sent":
return Sent;

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

case "connections_rejected":
this.ConnectionsRejected = (System.UInt32)value;
break;

case "id":
this.ID = (System.UInt32)value;
break;

case "recv":
this.Recv = (System.UInt32)value;
break;

case "recvs":
this.Recvs = (System.UInt32)value;
break;

case "sends":
this.Sends = (System.UInt32)value;
break;

case "sent":
this.Sent = (System.UInt32)value;
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
return new ColumnMetadata("connections", "Connections made with the server (accepted connections).", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "connections_rejected":
return new ColumnMetadata("connections_rejected", "", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "id":
return new ColumnMetadata("id", "", "int(10) unsigned", null, typeof(System.UInt32), false, true, false);

case "recv":
return new ColumnMetadata("recv", "The number of bytes that have been received.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "recvs":
return new ColumnMetadata("recvs", "The number of messages that have been received.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "sends":
return new ColumnMetadata("sends", "The number of messages that have been sent.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "sent":
return new ColumnMetadata("sent", "The number of bytes that have been sent.", "mediumint(8) unsigned", null, typeof(System.UInt32), false, false, false);

case "when":
return new ColumnMetadata("when", "When these network stats were logged. The values correspond to a time period defined in the WorldStatsTracker constructor. This timestamp marks the end of this period of time. So all stats correspond to the time frame range: [when - rate, when].", "timestamp", "CURRENT_TIMESTAMP", typeof(System.DateTime), false, false, false);

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
