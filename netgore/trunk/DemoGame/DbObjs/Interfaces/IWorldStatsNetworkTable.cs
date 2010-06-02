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
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `world_stats_network`.
/// </summary>
public interface IWorldStatsNetworkTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IWorldStatsNetworkTable DeepCopy();

/// <summary>
/// Gets the value of the database column `connections`.
/// </summary>
System.UInt32 Connections
{
get;
}
/// <summary>
/// Gets the value of the database column `tcp_recv`.
/// </summary>
System.UInt32 TcpRecv
{
get;
}
/// <summary>
/// Gets the value of the database column `tcp_recvs`.
/// </summary>
System.UInt32 TcpRecvs
{
get;
}
/// <summary>
/// Gets the value of the database column `tcp_sends`.
/// </summary>
System.UInt32 TcpSends
{
get;
}
/// <summary>
/// Gets the value of the database column `tcp_sent`.
/// </summary>
System.UInt32 TcpSent
{
get;
}
/// <summary>
/// Gets the value of the database column `udp_recv`.
/// </summary>
System.UInt32 UdpRecv
{
get;
}
/// <summary>
/// Gets the value of the database column `udp_recvs`.
/// </summary>
System.UInt32 UdpRecvs
{
get;
}
/// <summary>
/// Gets the value of the database column `udp_sends`.
/// </summary>
System.UInt32 UdpSends
{
get;
}
/// <summary>
/// Gets the value of the database column `udp_sent`.
/// </summary>
System.UInt32 UdpSent
{
get;
}
/// <summary>
/// Gets the value of the database column `when`.
/// </summary>
System.DateTime When
{
get;
}
}

}
