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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.IO;

namespace DemoGame.Server.DbObjs
{
    /// <summary>
    /// Provides a strongly-typed structure for the database table `world_stats_network`.
    /// </summary>
    public class WorldStatsNetworkTable : IWorldStatsNetworkTable, IPersistable
    {
        /// <summary>
        /// The number of columns in the database table that this class represents.
        /// </summary>
        public const Int32 ColumnCount = 8;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "world_stats_network";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        { "connections", "connections_rejected", "id", "recv", "recvs", "sends", "sent", "when" };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        { "connections", "connections_rejected", "recv", "recvs", "sends", "sent", "when" };

        /// <summary>
        /// The field that maps onto the database column `connections`.
        /// </summary>
        UInt32 _connections;

        /// <summary>
        /// The field that maps onto the database column `connections_rejected`.
        /// </summary>
        UInt32 _connectionsRejected;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        UInt32 _iD;

        /// <summary>
        /// The field that maps onto the database column `recv`.
        /// </summary>
        UInt32 _recv;

        /// <summary>
        /// The field that maps onto the database column `recvs`.
        /// </summary>
        UInt32 _recvs;

        /// <summary>
        /// The field that maps onto the database column `sends`.
        /// </summary>
        UInt32 _sends;

        /// <summary>
        /// The field that maps onto the database column `sent`.
        /// </summary>
        UInt32 _sent;

        /// <summary>
        /// The field that maps onto the database column `when`.
        /// </summary>
        DateTime _when;

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
        public WorldStatsNetworkTable(UInt32 @connections, UInt32 @connectionsRejected, UInt32 @iD, UInt32 @recv, UInt32 @recvs,
                                      UInt32 @sends, UInt32 @sent, DateTime @when)
        {
            Connections = @connections;
            ConnectionsRejected = @connectionsRejected;
            ID = @iD;
            Recv = @recv;
            Recvs = @recvs;
            Sends = @sends;
            Sent = @sent;
            When = @when;
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
        /// Gets an IEnumerable of strings containing the names of the database columns for the table that this class represents.
        /// </summary>
        public static IEnumerable<String> DbColumns
        {
            get { return _dbColumns; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are primary keys.
        /// </summary>
        public static IEnumerable<String> DbKeyColumns
        {
            get { return _dbColumnsKeys; }
        }

        /// <summary>
        /// Gets an IEnumerable of strings containing the names of the database columns that are not primary keys.
        /// </summary>
        public static IEnumerable<String> DbNonKeyColumns
        {
            get { return _dbColumnsNonKey; }
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="source">The object to copy the values from.</param>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public static void CopyValues(IWorldStatsNetworkTable source, IDictionary<String, Object> dic)
        {
            dic["connections"] = source.Connections;
            dic["connections_rejected"] = source.ConnectionsRejected;
            dic["id"] = source.ID;
            dic["recv"] = source.Recv;
            dic["recvs"] = source.Recvs;
            dic["sends"] = source.Sends;
            dic["sent"] = source.Sent;
            dic["when"] = source.When;
        }

        /// <summary>
        /// Copies the column values into the given Dictionary using the database column name
        /// with a prefixed @ as the key. The keys must already exist in the Dictionary;
        /// this method will not create them if they are missing.
        /// </summary>
        /// <param name="dic">The Dictionary to copy the values into.</param>
        public void CopyValues(IDictionary<String, Object> dic)
        {
            CopyValues(this, dic);
        }

        /// <summary>
        /// Copies the values from the given <paramref name="source"/> into this WorldStatsNetworkTable.
        /// </summary>
        /// <param name="source">The IWorldStatsNetworkTable to copy the values from.</param>
        public void CopyValuesFrom(IWorldStatsNetworkTable source)
        {
            Connections = source.Connections;
            ConnectionsRejected = source.ConnectionsRejected;
            ID = source.ID;
            Recv = source.Recv;
            Recvs = source.Recvs;
            Sends = source.Sends;
            Sent = source.Sent;
            When = source.When;
        }

        /// <summary>
        /// Gets the data for the database column that this table represents.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the data for.</param>
        /// <returns>
        /// The data for the database column with the name <paramref name="columnName"/>.
        /// </returns>
        public static ColumnMetadata GetColumnData(String columnName)
        {
            switch (columnName)
            {
                case "connections":
                    return new ColumnMetadata("connections", "Connections made with the server (accepted connections).",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "connections_rejected":
                    return new ColumnMetadata("connections_rejected", "", "mediumint(8) unsigned", null, typeof(UInt32), false,
                                              false, false);

                case "id":
                    return new ColumnMetadata("id", "", "int(10) unsigned", null, typeof(UInt32), false, true, false);

                case "recv":
                    return new ColumnMetadata("recv", "The number of bytes that have been received.", "mediumint(8) unsigned",
                                              null, typeof(UInt32), false, false, false);

                case "recvs":
                    return new ColumnMetadata("recvs", "The number of messages that have been received.", "mediumint(8) unsigned",
                                              null, typeof(UInt32), false, false, false);

                case "sends":
                    return new ColumnMetadata("sends", "The number of messages that have been sent.", "mediumint(8) unsigned",
                                              null, typeof(UInt32), false, false, false);

                case "sent":
                    return new ColumnMetadata("sent", "The number of bytes that have been sent.", "mediumint(8) unsigned", null,
                                              typeof(UInt32), false, false, false);

                case "when":
                    return new ColumnMetadata("when",
                                              "When these network stats were logged. The values correspond to a time period defined in the WorldStatsTracker constructor. This timestamp marks the end of this period of time. So all stats correspond to the time frame range: [when - rate, when].",
                                              "timestamp", "CURRENT_TIMESTAMP", typeof(DateTime), false, false, false);

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Gets the value of a column by the database column's name.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the value for.</param>
        /// <returns>
        /// The value of the column with the name <paramref name="columnName"/>.
        /// </returns>
        public Object GetValue(String columnName)
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
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        /// <summary>
        /// Sets the <paramref name="value"/> of a column by the database column's name.
        /// </summary>
        /// <param name="columnName">The database name of the column to get the <paramref name="value"/> for.</param>
        /// <param name="value">Value to assign to the column.</param>
        public void SetValue(String columnName, Object value)
        {
            switch (columnName)
            {
                case "connections":
                    Connections = (UInt32)value;
                    break;

                case "connections_rejected":
                    ConnectionsRejected = (UInt32)value;
                    break;

                case "id":
                    ID = (UInt32)value;
                    break;

                case "recv":
                    Recv = (UInt32)value;
                    break;

                case "recvs":
                    Recvs = (UInt32)value;
                    break;

                case "sends":
                    Sends = (UInt32)value;
                    break;

                case "sent":
                    Sent = (UInt32)value;
                    break;

                case "when":
                    When = (DateTime)value;
                    break;

                default:
                    throw new ArgumentException("Field not found.", "columnName");
            }
        }

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public virtual void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public virtual void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion

        #region IWorldStatsNetworkTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `connections`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "Connections made with the server (accepted connections).".
        /// </summary>
        [Description("Connections made with the server (accepted connections).")]
        [SyncValue]
        public UInt32 Connections
        {
            get { return _connections; }
            set { _connections = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `connections_rejected`.
        /// The underlying database type is `mediumint(8) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt32 ConnectionsRejected
        {
            get { return _connectionsRejected; }
            set { _connectionsRejected = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `int(10) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt32 ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `recv`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of bytes that have been received.".
        /// </summary>
        [Description("The number of bytes that have been received.")]
        [SyncValue]
        public UInt32 Recv
        {
            get { return _recv; }
            set { _recv = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `recvs`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of messages that have been received.".
        /// </summary>
        [Description("The number of messages that have been received.")]
        [SyncValue]
        public UInt32 Recvs
        {
            get { return _recvs; }
            set { _recvs = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `sends`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of messages that have been sent.".
        /// </summary>
        [Description("The number of messages that have been sent.")]
        [SyncValue]
        public UInt32 Sends
        {
            get { return _sends; }
            set { _sends = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `sent`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of bytes that have been sent.".
        /// </summary>
        [Description("The number of bytes that have been sent.")]
        [SyncValue]
        public UInt32 Sent
        {
            get { return _sent; }
            set { _sent = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `when`.
        /// The underlying database type is `timestamp` with the default value of `CURRENT_TIMESTAMP`. The database column contains the comment: 
        /// "When these network stats were logged. The values correspond to a time period defined in the WorldStatsTracker constructor. This timestamp marks the end of this period of time. So all stats correspond to the time frame range: [when - rate, when].".
        /// </summary>
        [Description(
            "When these network stats were logged. The values correspond to a time period defined in the WorldStatsTracker constructor. This timestamp marks the end of this period of time. So all stats correspond to the time frame range: [when - rate, when]."
            )]
        [SyncValue]
        public DateTime When
        {
            get { return _when; }
            set { _when = value; }
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

        #endregion
    }
}