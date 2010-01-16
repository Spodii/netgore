using System;
using System.Linq;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `account`.
/// </summary>
public interface IAccountTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IAccountTable DeepCopy();

/// <summary>
/// Gets the value of the database column `creator_ip`.
/// </summary>
System.UInt32 CreatorIp
{
get;
}
/// <summary>
/// Gets the value of the database column `current_ip`.
/// </summary>
System.Nullable<System.UInt32> CurrentIp
{
get;
}
/// <summary>
/// Gets the value of the database column `email`.
/// </summary>
System.String Email
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
DemoGame.Server.AccountID ID
{
get;
}
/// <summary>
/// Gets the value of the database column `name`.
/// </summary>
System.String Name
{
get;
}
/// <summary>
/// Gets the value of the database column `password`.
/// </summary>
System.String Password
{
get;
}
/// <summary>
/// Gets the value of the database column `time_created`.
/// </summary>
System.DateTime TimeCreated
{
get;
}
/// <summary>
/// Gets the value of the database column `time_last_login`.
/// </summary>
System.DateTime TimeLastLogin
{
get;
}
}

}
