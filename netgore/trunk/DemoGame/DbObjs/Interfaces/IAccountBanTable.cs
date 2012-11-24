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
    http://www.netgore.com/wiki/DbClassCreator
********************************************************************/

using System;
using System.Linq;
namespace DemoGame.DbObjs
{
/// <summary>
/// Interface for a class that can be used to serialize values to the database table `account_ban`.
/// </summary>
public interface IAccountBanTable
{
/// <summary>
/// Creates a deep copy of this table. All the values will be the same
/// but they will be contained in a different object instance.
/// </summary>
/// <returns>
/// A deep copy of this table.
/// </returns>
IAccountBanTable DeepCopy();

/// <summary>
/// Gets the value of the database column `account_id`.
/// </summary>
DemoGame.AccountID AccountID
{
get;
}
/// <summary>
/// Gets the value of the database column `end_time`.
/// </summary>
System.DateTime EndTime
{
get;
}
/// <summary>
/// Gets the value of the database column `expired`.
/// </summary>
System.Boolean Expired
{
get;
}
/// <summary>
/// Gets the value of the database column `id`.
/// </summary>
System.Int32 ID
{
get;
}
/// <summary>
/// Gets the value of the database column `issued_by`.
/// </summary>
System.String IssuedBy
{
get;
}
/// <summary>
/// Gets the value of the database column `reason`.
/// </summary>
System.String Reason
{
get;
}
/// <summary>
/// Gets the value of the database column `start_time`.
/// </summary>
System.DateTime StartTime
{
get;
}
}

}
