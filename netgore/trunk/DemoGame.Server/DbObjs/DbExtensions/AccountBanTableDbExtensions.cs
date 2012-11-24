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
/// Contains extension methods for class AccountBanTable that assist in performing
/// reads and writes to and from a database.
/// </summary>
public static  class AccountBanTableDbExtensions
{
/// <summary>
/// Copies the column values into the given DbParameterValues using the database column name
/// with a prefixed @ as the key. The keys must already exist in the DbParameterValues;
///  this method will not create them if they are missing.
/// </summary>
/// <param name="source">The object to copy the values from.</param>
/// <param name="paramValues">The DbParameterValues to copy the values into.</param>
public static void CopyValues(this IAccountBanTable source, NetGore.Db.DbParameterValues paramValues)
{
paramValues["account_id"] = (System.Int32)source.AccountID;
paramValues["end_time"] = (System.DateTime)source.EndTime;
paramValues["expired"] = (System.Boolean)source.Expired;
paramValues["id"] = (System.Int32)source.ID;
paramValues["issued_by"] = (System.String)source.IssuedBy;
paramValues["reason"] = (System.String)source.Reason;
paramValues["start_time"] = (System.DateTime)source.StartTime;
}

/// <summary>
/// Reads the values from an <see cref="IDataRecord"/> and assigns the read values to this
/// object's properties. The database column's name is used to as the key, so the value
/// will not be found if any aliases are used or not all columns were selected.
/// </summary>
/// <param name="source">The object to add the extension method to.</param>
/// <param name="dataRecord">The <see cref="IDataRecord"/> to read the values from. Must already be ready to be read from.</param>
public static void ReadValues(this AccountBanTable source, System.Data.IDataRecord dataRecord)
{
System.Int32 i;

i = dataRecord.GetOrdinal("account_id");

source.AccountID = (DemoGame.AccountID)(DemoGame.AccountID)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("end_time");

source.EndTime = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);

i = dataRecord.GetOrdinal("expired");

source.Expired = (System.Boolean)(System.Boolean)dataRecord.GetBoolean(i);

i = dataRecord.GetOrdinal("id");

source.ID = (System.Int32)(System.Int32)dataRecord.GetInt32(i);

i = dataRecord.GetOrdinal("issued_by");

source.IssuedBy = (System.String)(System.String)(dataRecord.IsDBNull(i) ? (System.String)null : dataRecord.GetString(i));

i = dataRecord.GetOrdinal("reason");

source.Reason = (System.String)(System.String)dataRecord.GetString(i);

i = dataRecord.GetOrdinal("start_time");

source.StartTime = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);
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
public static void TryReadValues(this AccountBanTable source, System.Data.IDataRecord dataRecord)
{
for (int i = 0; i < dataRecord.FieldCount; i++)
{
switch (dataRecord.GetName(i))
{
case "account_id":
source.AccountID = (DemoGame.AccountID)(DemoGame.AccountID)dataRecord.GetInt32(i);
break;


case "end_time":
source.EndTime = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);
break;


case "expired":
source.Expired = (System.Boolean)(System.Boolean)dataRecord.GetBoolean(i);
break;


case "id":
source.ID = (System.Int32)(System.Int32)dataRecord.GetInt32(i);
break;


case "issued_by":
source.IssuedBy = (System.String)(System.String)(dataRecord.IsDBNull(i) ? (System.String)null : dataRecord.GetString(i));
break;


case "reason":
source.Reason = (System.String)(System.String)dataRecord.GetString(i);
break;


case "start_time":
source.StartTime = (System.DateTime)(System.DateTime)dataRecord.GetDateTime(i);
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
public static void TryCopyValues(this IAccountBanTable source, NetGore.Db.DbParameterValues paramValues)
{
for (int i = 0; i < paramValues.Count; i++)
{
switch (paramValues.GetParameterName(i))
{
case "account_id":
paramValues[i] = (System.Int32)source.AccountID;
break;


case "end_time":
paramValues[i] = (System.DateTime)source.EndTime;
break;


case "expired":
paramValues[i] = (System.Boolean)source.Expired;
break;


case "id":
paramValues[i] = (System.Int32)source.ID;
break;


case "issued_by":
paramValues[i] = (System.String)source.IssuedBy;
break;


case "reason":
paramValues[i] = (System.String)source.Reason;
break;


case "start_time":
paramValues[i] = (System.DateTime)source.StartTime;
break;


}

}
}

/// <summary>
/// Checks if this <see cref="IAccountBanTable"/> contains the same values as another <see cref="IAccountBanTable"/>.
/// </summary>
/// <param name="source">The source <see cref="IAccountBanTable"/>.</param>
/// <param name="otherItem">The <see cref="IAccountBanTable"/> to compare the values to.</param>
/// <returns>
/// True if this <see cref="IAccountBanTable"/> contains the same values as the <paramref name="otherItem"/>; otherwise false.
/// </returns>
public static System.Boolean HasSameValues(this IAccountBanTable source, IAccountBanTable otherItem)
{
return Equals(source.AccountID, otherItem.AccountID) && 
Equals(source.EndTime, otherItem.EndTime) && 
Equals(source.Expired, otherItem.Expired) && 
Equals(source.ID, otherItem.ID) && 
Equals(source.IssuedBy, otherItem.IssuedBy) && 
Equals(source.Reason, otherItem.Reason) && 
Equals(source.StartTime, otherItem.StartTime);
}

}

}
