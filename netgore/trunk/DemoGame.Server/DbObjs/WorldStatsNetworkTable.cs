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
        public const Int32 ColumnCount = 9;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "world_stats_network";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        {
            "connections", "id", "recv_bytes", "recv_messages", "recv_packets", "sent_bytes", "sent_messages", "sent_packets",
            "when"
        };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "id" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        { "connections", "recv_bytes", "recv_messages", "recv_packets", "sent_bytes", "sent_messages", "sent_packets", "when" };

        /// <summary>
        /// The field that maps onto the database column `connections`.
        /// </summary>
        UInt16 _connections;

        /// <summary>
        /// The field that maps onto the database column `id`.
        /// </summary>
        UInt32 _iD;

        /// <summary>
        /// The field that maps onto the database column `recv_bytes`.
        /// </summary>
        UInt32 _recvBytes;

        /// <summary>
        /// The field that maps onto the database column `recv_messages`.
        /// </summary>
        UInt32 _recvMessages;

        /// <summary>
        /// The field that maps onto the database column `recv_packets`.
        /// </summary>
        UInt32 _recvPackets;

        /// <summary>
        /// The field that maps onto the database column `sent_bytes`.
        /// </summary>
        UInt32 _sentBytes;

        /// <summary>
        /// The field that maps onto the database column `sent_messages`.
        /// </summary>
        UInt32 _sentMessages;

        /// <summary>
        /// The field that maps onto the database column `sent_packets`.
        /// </summary>
        UInt32 _sentPackets;

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
        /// <param name="iD">The initial value for the corresponding property.</param>
        /// <param name="recvBytes">The initial value for the corresponding property.</param>
        /// <param name="recvMessages">The initial value for the corresponding property.</param>
        /// <param name="recvPackets">The initial value for the corresponding property.</param>
        /// <param name="sentBytes">The initial value for the corresponding property.</param>
        /// <param name="sentMessages">The initial value for the corresponding property.</param>
        /// <param name="sentPackets">The initial value for the corresponding property.</param>
        /// <param name="when">The initial value for the corresponding property.</param>
        public WorldStatsNetworkTable(UInt16 @connections, UInt32 @iD, UInt32 @recvBytes, UInt32 @recvMessages,
                                      UInt32 @recvPackets, UInt32 @sentBytes, UInt32 @sentMessages, UInt32 @sentPackets,
                                      DateTime @when)
        {
            Connections = @connections;
            ID = @iD;
            RecvBytes = @recvBytes;
            RecvMessages = @recvMessages;
            RecvPackets = @recvPackets;
            SentBytes = @sentBytes;
            SentMessages = @sentMessages;
            SentPackets = @sentPackets;
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
            dic["id"] = source.ID;
            dic["recv_bytes"] = source.RecvBytes;
            dic["recv_messages"] = source.RecvMessages;
            dic["recv_packets"] = source.RecvPackets;
            dic["sent_bytes"] = source.SentBytes;
            dic["sent_messages"] = source.SentMessages;
            dic["sent_packets"] = source.SentPackets;
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
            ID = source.ID;
            RecvBytes = source.RecvBytes;
            RecvMessages = source.RecvMessages;
            RecvPackets = source.RecvPackets;
            SentBytes = source.SentBytes;
            SentMessages = source.SentMessages;
            SentPackets = source.SentPackets;
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
                    return new ColumnMetadata("connections", "Number of live connections to the server at this time.",
                                              "smallint(5) unsigned", null, typeof(UInt16), false, false, false);

                case "id":
                    return new ColumnMetadata("id", "", "mediumint(8) unsigned", null, typeof(UInt32), false, true, false);

                case "recv_bytes":
                    return new ColumnMetadata("recv_bytes",
                                              "The number of bytes that have been received (diff since last snapshot).",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "recv_messages":
                    return new ColumnMetadata("recv_messages",
                                              "The number of complete messages that have been received (diff since last snapshot).",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "recv_packets":
                    return new ColumnMetadata("recv_packets",
                                              "The number of individual packets received (diff since last snapshot).",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "sent_bytes":
                    return new ColumnMetadata("sent_bytes", "The number of bytes that have been sent (diff since last snapshot).",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "sent_messages":
                    return new ColumnMetadata("sent_messages",
                                              "The number of complete messages that have been sent (diff since last snapshot).",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "sent_packets":
                    return new ColumnMetadata("sent_packets", "The number of individual packets sent (diff since last snapshot).",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

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
                    Connections = (UInt16)value;
                    break;

                case "id":
                    ID = (UInt32)value;
                    break;

                case "recv_bytes":
                    RecvBytes = (UInt32)value;
                    break;

                case "recv_messages":
                    RecvMessages = (UInt32)value;
                    break;

                case "recv_packets":
                    RecvPackets = (UInt32)value;
                    break;

                case "sent_bytes":
                    SentBytes = (UInt32)value;
                    break;

                case "sent_messages":
                    SentMessages = (UInt32)value;
                    break;

                case "sent_packets":
                    SentPackets = (UInt32)value;
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
        /// The underlying database type is `smallint(5) unsigned`. The database column contains the comment: 
        /// "Number of live connections to the server at this time.".
        /// </summary>
        [Description("Number of live connections to the server at this time.")]
        [SyncValue]
        public UInt16 Connections
        {
            get { return _connections; }
            set { _connections = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `id`.
        /// The underlying database type is `mediumint(8) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt32 ID
        {
            get { return _iD; }
            set { _iD = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `recv_bytes`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of bytes that have been received (diff since last snapshot).".
        /// </summary>
        [Description("The number of bytes that have been received (diff since last snapshot).")]
        [SyncValue]
        public UInt32 RecvBytes
        {
            get { return _recvBytes; }
            set { _recvBytes = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `recv_messages`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of complete messages that have been received (diff since last snapshot).".
        /// </summary>
        [Description("The number of complete messages that have been received (diff since last snapshot).")]
        [SyncValue]
        public UInt32 RecvMessages
        {
            get { return _recvMessages; }
            set { _recvMessages = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `recv_packets`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of individual packets received (diff since last snapshot).".
        /// </summary>
        [Description("The number of individual packets received (diff since last snapshot).")]
        [SyncValue]
        public UInt32 RecvPackets
        {
            get { return _recvPackets; }
            set { _recvPackets = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `sent_bytes`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of bytes that have been sent (diff since last snapshot).".
        /// </summary>
        [Description("The number of bytes that have been sent (diff since last snapshot).")]
        [SyncValue]
        public UInt32 SentBytes
        {
            get { return _sentBytes; }
            set { _sentBytes = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `sent_messages`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of complete messages that have been sent (diff since last snapshot).".
        /// </summary>
        [Description("The number of complete messages that have been sent (diff since last snapshot).")]
        [SyncValue]
        public UInt32 SentMessages
        {
            get { return _sentMessages; }
            set { _sentMessages = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `sent_packets`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of individual packets sent (diff since last snapshot).".
        /// </summary>
        [Description("The number of individual packets sent (diff since last snapshot).")]
        [SyncValue]
        public UInt32 SentPackets
        {
            get { return _sentPackets; }
            set { _sentPackets = value; }
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