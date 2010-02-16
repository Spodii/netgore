using System;
using System.Linq;
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `account_ips`.
/// </summary>
public interface IAccountIpsTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IAccountIpsTable DeepCopy();

/// <summary>
/// Gets the value of the database column `account_id`.
/// </summary>
DemoGame.AccountID AccountID
{
get;
}
/// <summary>
/// Gets the value of the database column `ip`.
/// </summary>
System.UInt32 Ip
{
get;
}
/// <summary>
/// Gets the value of the database column `time`.
/// </summary>
System.DateTime Time
{
get;
}
}

}
