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
using NetGore;
using NetGore.IO;
using System.Collections.Generic;
using System.Collections;
using NetGore.Db;
using DemoGame.DbObjs;
namespace DemoGame.Server.DbObjs
{
/// <summary>
/// Contains extension methods for class AccountTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class AccountTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IAccountTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["creator_ip"] = (System.UInt32)source.CreatorIp;
paramValues["current_ip"] = (System.Nullable<System.UInt32>)source.CurrentIp;
paramValues["email"] = (System.String)source.Email;
paramValues["friends"] = (System.String)source.Friends;
paramValues["id"] = (System.Int32)source.ID;
paramValues["name"] = (System.String)source.Name;
paramValues["password"] = (System.String)source.Password;
paramValues["permissions"] = (System.Byte)source.Permissions;
paramValues["time_created"] = (System.DateTime)source.TimeCreated;
paramValues["time_last_login"] = (System.DateTime)source.TimeLastLogin;
}

/// <summary>
/// Reads the values from an <see cref="IDataRecord"/> and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataRecord"/> to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this AccountTable source, System.Data.IDataRecord dataRecord)
{
System.Int32 i;

i = dataRecord.GetOrdinal("creator_ip");

source.CreatorIp = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);

i = dataRecord.GetOrdinal("current_ip");

source.CurrentIp = (System.Nullable<System.UInt32>)(System.Nullable<System.UInt32>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt32>)null : dataRecord.GetUInt32(i));

i = dataRecord.GetOrdinal("email");

source.Email = (System.String)(System.String)dataRecord.GetString(i);

i = dataRecord.GetOrdinal("friends");

source.Friends = (System.String)(System.String)dataRecord.GetString(i);

i = dataRecord.GetOrdinal("id");

source.ID = (DemoGame.AccountID)(DemoGame.AccountID)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("name");

source.Name = (System.String)(System.String)dataRecord.GetString(i);

i = dataRecord.GetOrdinal("password");

source.Password = (System.String)(System.String)dataRecord.GetString(i);

i = dataRecord.GetOrdinal("permissions");

source.Permissions = (DemoGame.UserPermissions)(DemoGame.UserPermissions)dataRecord.GetByte(i);

i = dataRecord.GetOrdinal("time_created");

source.TimeCreated = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);

i = dataRecord.GetOrdinal("time_last_login");

source.TimeLastLogin = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);
}

/// <summary>
/// Reads the values from an <see cref="IDataReader"/> and assigns the read values to this
/// object's properties. Unlike ReadValues(), this method not only doesn't require
/// all values to be in the <see cref="IDataReader"/>, but also does not require the values in
/// the <see cref="IDataReader"/> to be a defined field for the table this class represents.
/// Because of this, you need to be careful when using this method because values
/// can easily be skipped without any indication.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataReader"/> to read the values from. Must already be ready to be read from.</param>
public static void TryReadValues(this AccountTable source, System.Data.IDataRecord dataRecord)
{
for (int i = 0; i < dataRecord.FieldCount; i++)
{
switch (dataRecord.GetName(i))
{
case "creator_ip":
source.CreatorIp = (System.UInt32)(System.UInt32)dataRecord.GetUInt32(i);
break;


case "current_ip":
source.CurrentIp = (System.Nullable<System.UInt32>)(System.Nullable<System.UInt32>)(dataRecord.IsDBNull(i) ? (System.Nullable<System.UInt32>)null : dataRecord.GetUInt32(i));
break;


case "email":
source.Email = (System.String)(System.String)dataRecord.GetString(i);
break;


case "friends":
source.Friends = (System.String)(System.String)dataRecord.GetString(i);
break;


case "id":
source.ID = (DemoGame.AccountID)(DemoGame.AccountID)dataRecord.GetInt32(i);
break;


case "name":
source.Name = (System.String)(System.String)dataRecord.GetString(i);
break;


case "password":
source.Password = (System.String)(System.String)dataRecord.GetString(i);
break;


case "permissions":
source.Permissions = (DemoGame.UserPermissions)(DemoGame.UserPermissions)dataRecord.GetByte(i);
break;


case "time_created":
source.TimeCreated = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);
break;


case "time_last_login":
source.TimeLastLogin = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);
break;


}

}
}

/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The key must already exist in the DbParameterValues
/// for the value to be copied over. If any of the keys in the DbParameterValues do not
/// match one of the column names, or if there is no field for a key, then it will be
/// ignored. Because of this, it is important to be careful when using this method
/// since columns or keys can be skipped without any indication.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void TryCopyValues(this IAccountTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "creator_ip":
paramValues[i] = (System.UInt32)source.CreatorIp;
break;


case "current_ip":
paramValues[i] = (System.Nullable<System.UInt32>)source.CurrentIp;
break;


case "email":
paramValues[i] = (System.String)source.Email;
break;


case "friends":
paramValues[i] = (System.String)source.Friends;
break;


case "id":
paramValues[i] = (System.Int32)source.ID;
break;


case "name":
paramValues[i] = (System.String)source.Name;
break;


case "password":
paramValues[i] = (System.String)source.Password;
break;


case "permissions":
paramValues[i] = (System.Byte)source.Permissions;
break;


case "time_created":
paramValues[i] = (System.DateTime)source.TimeCreated;
break;


case "time_last_login":
paramValues[i] = (System.DateTime)source.TimeLastLogin;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="IAccountTable"/> contains the same values as another <see cref="IAccountTable"/>.
/// </summary>
/// <param name="source">The source <see cref="IAccountTable"/>.</param>
/// <param name="otherItem">The <see cref="IAccountTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="IAccountTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
/// </returns>
public static System.Boolean HasSameValues(this IAccountTable source, IAccountTable otherItem)
{
return Equals(source.CreatorIp, otherItem.CreatorIp) && 
Equals(source.CurrentIp, otherItem.CurrentIp) && 
Equals(source.Email, otherItem.Email) && 
Equals(source.Friends, otherItem.Friends) && 
Equals(source.ID, otherItem.ID) && 
Equals(source.Name, otherItem.Name) && 
Equals(source.Password, otherItem.Password) && 
Equals(source.Permissions, otherItem.Permissions) && 
Equals(source.TimeCreated, otherItem.TimeCreated) && 
Equals(source.TimeLastLogin, otherItem.TimeLastLogin);
}

}

}
