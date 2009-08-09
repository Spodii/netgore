using System;
using System.Linq;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `map_spawn`.
/// </summary>
public interface IMapSpawnTable
{
/// <summary>
/// Gets the value of the database column `amount`.
/// </summary>
System.Byte Amount
{
get;
}
/// <summary>
/// Gets the value of the database column `character_id`.
/// </summary>
DemoGame.Server.CharacterID CharacterID
{
get;
}
/// <summary>
/// Gets the value of the database column `height`.
/// </summary>
System.Nullable<System.UInt16> Height
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
DemoGame.Server.MapSpawnValuesID ID
{
get;
}
/// <summary>
/// Gets the value of the database column `map_id`.
/// </summary>
NetGore.MapIndex MapID
{
get;
}
/// <summary>
/// Gets the value of the database column `width`.
/// </summary>
System.Nullable<System.UInt16> Width
{
get;
}
/// <summary>
/// Gets the value of the database column `x`.
/// </summary>
System.Nullable<System.UInt16> X
{
get;
}
/// <summary>
/// Gets the value of the database column `y`.
/// </summary>
System.Nullable<System.UInt16> Y
{
get;
}
}

}
