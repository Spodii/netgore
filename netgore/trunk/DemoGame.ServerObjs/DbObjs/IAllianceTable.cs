using System;
using System.Linq;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `alliance`.
/// </summary>
public interface IAllianceTable
{
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
DemoGame.Server.AllianceID ID
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
}

}
