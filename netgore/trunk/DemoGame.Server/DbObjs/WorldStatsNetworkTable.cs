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

This file was generated on (UTC): 5/16/2010 7:45:19 PM
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
        public const Int32 ColumnCount = 10;

        /// <summary>
        /// The name of the database table that this class represents.
        /// </summary>
        public const String TableName = "world_stats_network";

        /// <summary>
        /// Array of the database column names.
        /// </summary>
        static readonly String[] _dbColumns = new string[]
        {
            "connections", "tcp_recv", "tcp_recvs", "tcp_sends", "tcp_sent", "udp_recv", "udp_recvs", "udp_sends", "udp_sent",
            "when"
        };

        /// <summary>
        /// Array of the database column names for columns that are primary keys.
        /// </summary>
        static readonly String[] _dbColumnsKeys = new string[] { "when" };

        /// <summary>
        /// Array of the database column names for columns that are not primary keys.
        /// </summary>
        static readonly String[] _dbColumnsNonKey = new string[]
        { "connections", "tcp_recv", "tcp_recvs", "tcp_sends", "tcp_sent", "udp_recv", "udp_recvs", "udp_sends", "udp_sent" };

        /// <summary>
        /// The field that maps onto the database column `connections`.
        /// </summary>
        UInt32 _connections;

        /// <summary>
        /// The field that maps onto the database column `tcp_recv`.
        /// </summary>
        UInt32 _tcpRecv;

        /// <summary>
        /// The field that maps onto the database column `tcp_recvs`.
        /// </summary>
        UInt32 _tcpRecvs;

        /// <summary>
        /// The field that maps onto the database column `tcp_sends`.
        /// </summary>
        UInt32 _tcpSends;

        /// <summary>
        /// The field that maps onto the database column `tcp_sent`.
        /// </summary>
        UInt32 _tcpSent;

        /// <summary>
        /// The field that maps onto the database column `udp_recv`.
        /// </summary>
        UInt32 _udpRecv;

        /// <summary>
        /// The field that maps onto the database column `udp_recvs`.
        /// </summary>
        UInt32 _udpRecvs;

        /// <summary>
        /// The field that maps onto the database column `udp_sends`.
        /// </summary>
        UInt32 _udpSends;

        /// <summary>
        /// The field that maps onto the database column `udp_sent`.
        /// </summary>
        UInt32 _udpSent;

        /// <summary>
        /// The field that maps onto the database column `when`.
        /// </summary>
        DateTime _when;

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
        public WorldStatsNetworkTable(UInt32 @connections, UInt32 @tcpRecv, UInt32 @tcpRecvs, UInt32 @tcpSends, UInt32 @tcpSent,
                                      UInt32 @udpRecv, UInt32 @udpRecvs, UInt32 @udpSends, UInt32 @udpSent, DateTime @when)
        {
            Connections = @connections;
            TcpRecv = @tcpRecv;
            TcpRecvs = @tcpRecvs;
            TcpSends = @tcpSends;
            TcpSent = @tcpSent;
            UdpRecv = @udpRecv;
            UdpRecvs = @udpRecvs;
            UdpSends = @udpSends;
            UdpSent = @udpSent;
            When = @when;
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
            dic["@connections"] = source.Connections;
            dic["@tcp_recv"] = source.TcpRecv;
            dic["@tcp_recvs"] = source.TcpRecvs;
            dic["@tcp_sends"] = source.TcpSends;
            dic["@tcp_sent"] = source.TcpSent;
            dic["@udp_recv"] = source.UdpRecv;
            dic["@udp_recvs"] = source.UdpRecvs;
            dic["@udp_sends"] = source.UdpSends;
            dic["@udp_sent"] = source.UdpSent;
            dic["@when"] = source.When;
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
            TcpRecv = source.TcpRecv;
            TcpRecvs = source.TcpRecvs;
            TcpSends = source.TcpSends;
            TcpSent = source.TcpSent;
            UdpRecv = source.UdpRecv;
            UdpRecvs = source.UdpRecvs;
            UdpSends = source.UdpSends;
            UdpSent = source.UdpSent;
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
                    return new ColumnMetadata("connections", "", "mediumint(8) unsigned", null, typeof(UInt32), false, false,
                                              false);

                case "tcp_recv":
                    return new ColumnMetadata("tcp_recv", "The number of bytes that have been received over the TCP channel.",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "tcp_recvs":
                    return new ColumnMetadata("tcp_recvs", "The number of messages that have been received over the TCP channel.",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "tcp_sends":
                    return new ColumnMetadata("tcp_sends", "The number of messages that have been sent over the TCP channel.",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "tcp_sent":
                    return new ColumnMetadata("tcp_sent", "The number of bytes that have been sent over the TCP channel.",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "udp_recv":
                    return new ColumnMetadata("udp_recv", "The number of bytes that have been received over the UDP channel.",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "udp_recvs":
                    return new ColumnMetadata("udp_recvs", "The number of messages that have been received over the UDP channel.",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "udp_sends":
                    return new ColumnMetadata("udp_sends", "The number of messages that have been sent over the UDP channel.",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "udp_sent":
                    return new ColumnMetadata("udp_sent", "The number of bytes that have been sent over the UDP channel.",
                                              "mediumint(8) unsigned", null, typeof(UInt32), false, false, false);

                case "when":
                    return new ColumnMetadata("when",
                                              "When these network stats were logged. The values correspond to a time period defined in the WorldStatsTracker constructor. This timestamp marks the end of this period of time. So all stats correspond to the time frame range: [when - rate, when].",
                                              "timestamp", "CURRENT_TIMESTAMP", typeof(DateTime), false, true, false);

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

                case "tcp_recv":
                    TcpRecv = (UInt32)value;
                    break;

                case "tcp_recvs":
                    TcpRecvs = (UInt32)value;
                    break;

                case "tcp_sends":
                    TcpSends = (UInt32)value;
                    break;

                case "tcp_sent":
                    TcpSent = (UInt32)value;
                    break;

                case "udp_recv":
                    UdpRecv = (UInt32)value;
                    break;

                case "udp_recvs":
                    UdpRecvs = (UInt32)value;
                    break;

                case "udp_sends":
                    UdpSends = (UInt32)value;
                    break;

                case "udp_sent":
                    UdpSent = (UInt32)value;
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
        public void ReadState(IValueReader reader)
        {
            PersistableHelper.Read(this, reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            PersistableHelper.Write(this, writer);
        }

        #endregion

        #region IWorldStatsNetworkTable Members

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `connections`.
        /// The underlying database type is `mediumint(8) unsigned`.
        /// </summary>
        [SyncValue]
        public UInt32 Connections
        {
            get { return _connections; }
            set { _connections = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `tcp_recv`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of bytes that have been received over the TCP channel.".
        /// </summary>
        [Description("The number of bytes that have been received over the TCP channel.")]
        [SyncValue]
        public UInt32 TcpRecv
        {
            get { return _tcpRecv; }
            set { _tcpRecv = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `tcp_recvs`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of messages that have been received over the TCP channel.".
        /// </summary>
        [Description("The number of messages that have been received over the TCP channel.")]
        [SyncValue]
        public UInt32 TcpRecvs
        {
            get { return _tcpRecvs; }
            set { _tcpRecvs = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `tcp_sends`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of messages that have been sent over the TCP channel.".
        /// </summary>
        [Description("The number of messages that have been sent over the TCP channel.")]
        [SyncValue]
        public UInt32 TcpSends
        {
            get { return _tcpSends; }
            set { _tcpSends = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `tcp_sent`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of bytes that have been sent over the TCP channel.".
        /// </summary>
        [Description("The number of bytes that have been sent over the TCP channel.")]
        [SyncValue]
        public UInt32 TcpSent
        {
            get { return _tcpSent; }
            set { _tcpSent = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `udp_recv`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of bytes that have been received over the UDP channel.".
        /// </summary>
        [Description("The number of bytes that have been received over the UDP channel.")]
        [SyncValue]
        public UInt32 UdpRecv
        {
            get { return _udpRecv; }
            set { _udpRecv = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `udp_recvs`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of messages that have been received over the UDP channel.".
        /// </summary>
        [Description("The number of messages that have been received over the UDP channel.")]
        [SyncValue]
        public UInt32 UdpRecvs
        {
            get { return _udpRecvs; }
            set { _udpRecvs = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `udp_sends`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of messages that have been sent over the UDP channel.".
        /// </summary>
        [Description("The number of messages that have been sent over the UDP channel.")]
        [SyncValue]
        public UInt32 UdpSends
        {
            get { return _udpSends; }
            set { _udpSends = value; }
        }

        /// <summary>
        /// Gets or sets the value for the field that maps onto the database column `udp_sent`.
        /// The underlying database type is `mediumint(8) unsigned`. The database column contains the comment: 
        /// "The number of bytes that have been sent over the UDP channel.".
        /// </summary>
        [Description("The number of bytes that have been sent over the UDP channel.")]
        [SyncValue]
        public UInt32 UdpSent
        {
            get { return _udpSent; }
            set { _udpSent = value; }
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
        public IWorldStatsNetworkTable DeepCopy()
        {
            return new WorldStatsNetworkTable(this);
        }

        #endregion
    }
}