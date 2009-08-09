using System;
using System.Linq;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `item_template`.
/// </summary>
public interface IItemTemplateTable
{
/// <summary>
/// Gets the value of the database column in the column collection `{0}`
/// that corresponds to the given <paramref name="key"/>.
/// </summary>
/// <param name="key">The key that represents the column in this column collection.</param>
/// <returns>
/// The value of the database column with the corresponding <paramref name="key"/>.
/// </returns>
System.Int32 GetStat(DemoGame.StatType key);

/// <summary>
/// Gets the <paramref name="value"/> of the database column in the column collection `{0}`
/// that corresponds to the given <paramref name="key"/>.
/// </summary>
/// <param name="key">The key that represents the column in this column collection.</param>
/// <param name="value">The value to assign to the column with the corresponding <paramref name="key"/>.</param>
void SetStat(DemoGame.StatType key, System.Int32 value);

System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> Stats
{
get;
}
/// <summary>
/// Gets the value of the database column `description`.
/// </summary>
System.String Description
{
get;
}
/// <summary>
/// Gets the value of the database column `graphic`.
/// </summary>
System.UInt16 Graphic
{
get;
}
/// <summary>
/// Gets the value of the database column `height`.
/// </summary>
System.Byte Height
{
get;
}
/// <summary>
/// Gets the value of the database column `hp`.
/// </summary>
System.UInt16 HP
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
DemoGame.Server.ItemTemplateID ID
{
get;
}
/// <summary>
/// Gets the value of the database column `mp`.
/// </summary>
System.UInt16 MP
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
/// Gets the value of the database column in the column collection `{0}`
/// that corresponds to the given <paramref name="key"/>.
/// </summary>
/// <param name="key">The key that represents the column in this column collection.</param>
/// <returns>
/// The value of the database column with the corresponding <paramref name="key"/>.
/// </returns>
System.Int32 GetReqStat(DemoGame.StatType key);

/// <summary>
/// Gets the <paramref name="value"/> of the database column in the column collection `{0}`
/// that corresponds to the given <paramref name="key"/>.
/// </summary>
/// <param name="key">The key that represents the column in this column collection.</param>
/// <param name="value">The value to assign to the column with the corresponding <paramref name="key"/>.</param>
void SetReqStat(DemoGame.StatType key, System.Int32 value);

System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<DemoGame.StatType, System.Int32>> ReqStats
{
get;
}
/// <summary>
/// Gets the value of the database column `type`.
/// </summary>
System.Byte Type
{
get;
}
/// <summary>
/// Gets the value of the database column `value`.
/// </summary>
System.Int32 Value
{
get;
}
/// <summary>
/// Gets the value of the database column `width`.
/// </summary>
System.Byte Width
{
get;
}
}

}
